using apptrade.Data;
using apptrade.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;

namespace apptrade.Services
{
    public class MarketService : IMarketService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDatabase _redisDb;
        private readonly ILogger<MarketService> _logger;
        private readonly RedisConfiguration _redisConfig;
        private const string INSTRUMENTS_CACHE_KEY = "market:instruments:all";

        public MarketService(
            ApplicationDbContext context,
            IConnectionMultiplexer redis,
            ILogger<MarketService> logger,
            IOptions<RedisConfiguration> redisConfig)
        {
            _context = context;
            _redisDb = redis.GetDatabase();
            _logger = logger;
            _redisConfig = redisConfig.Value;
        }

        public async Task<List<Assest>> GetAllInstrumentsAsync()
        {
            try
            {
                // Intentar obtener desde cache
                var cachedData = await _redisDb.StringGetAsync(INSTRUMENTS_CACHE_KEY);
                if (cachedData.HasValue)
                {
                    _logger.LogInformation("Returning instruments from Redis cache");
                    var instruments = JsonSerializer.Deserialize<List<Assest>>(cachedData!);
                    return instruments ?? new List<Assest>();
                }

                // Si no está en cache, obtener de la base de datos
                _logger.LogInformation("Cache miss - fetching instruments from database");
                var instrumentsFromDb = await _context.DbAssest.ToListAsync();

                // Guardar en cache con TTL de 2 minutos
                var serializedData = JsonSerializer.Serialize(instrumentsFromDb);
                var expiry = TimeSpan.FromMinutes(_redisConfig.CacheTtlMinutes);
                await _redisDb.StringSetAsync(INSTRUMENTS_CACHE_KEY, serializedData, expiry);

                _logger.LogInformation($"Cached {instrumentsFromDb.Count} instruments in Redis with TTL of {_redisConfig.CacheTtlMinutes} minutes");
                return instrumentsFromDb;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error accessing Redis cache for instruments. Falling back to database.");
                // Fallback a base de datos si Redis falla
                return await _context.DbAssest.ToListAsync();
            }
        }

        public async Task<Assest?> GetInstrumentByTickerAsync(string ticker)
        {
            try
            {
                var cacheKey = $"market:instrument:{ticker}";
                var cachedData = await _redisDb.StringGetAsync(cacheKey);
                
                if (cachedData.HasValue)
                {
                    _logger.LogInformation($"Returning instrument {ticker} from Redis cache");
                    return JsonSerializer.Deserialize<Assest>(cachedData!);
                }

                // Si no está en cache, obtener de la base de datos
                _logger.LogInformation($"Cache miss - fetching instrument {ticker} from database");
                var instrument = await _context.DbAssest.FirstOrDefaultAsync(a => a.Ticker == ticker);

                if (instrument != null)
                {
                    // Guardar en cache con TTL de 2 minutos
                    var serializedData = JsonSerializer.Serialize(instrument);
                    var expiry = TimeSpan.FromMinutes(_redisConfig.CacheTtlMinutes);
                    await _redisDb.StringSetAsync(cacheKey, serializedData, expiry);
                    _logger.LogInformation($"Cached instrument {ticker} in Redis");
                }

                return instrument;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error accessing Redis cache for instrument {ticker}. Falling back to database.");
                // Fallback a base de datos si Redis falla
                return await _context.DbAssest.FirstOrDefaultAsync(a => a.Ticker == ticker);
            }
        }

        public async Task InvalidateInstrumentsCacheAsync()
        {
            try
            {
                await _redisDb.KeyDeleteAsync(INSTRUMENTS_CACHE_KEY);
                _logger.LogInformation("Invalidated instruments cache");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error invalidating instruments cache");
            }
        }
    }
}
