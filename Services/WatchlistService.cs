using apptrade.Data;
using apptrade.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace apptrade.Services
{
    public class WatchlistService : IWatchlistService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _cache;
        private readonly ILogger<WatchlistService> _logger;
        private readonly TimeSpan _cacheTtl;

        public WatchlistService(
            ApplicationDbContext context, 
            IDistributedCache cache, 
            ILogger<WatchlistService> logger,
            IConfiguration configuration)
        {
            _context = context;
            _cache = cache;
            _logger = logger;
            
            // TTL máximo de 2 minutos como solicitado
            var ttlMinutes = configuration.GetValue<int>("Redis:CacheTtlMinutes", 2);
            _cacheTtl = TimeSpan.FromMinutes(Math.Min(ttlMinutes, 2));
        }

        public async Task<List<Watchlist>> GetUserWatchlistAsync(string userName)
        {
            var cacheKey = $"watchlist:user:{userName}";
            
            try
            {
                // Intentar obtener desde cache
                var cachedData = await _cache.GetStringAsync(cacheKey);
                if (!string.IsNullOrEmpty(cachedData))
                {
                    _logger.LogInformation($"Cache hit para usuario: {userName}");
                    var cachedWatchlist = JsonSerializer.Deserialize<List<Watchlist>>(cachedData);
                    return cachedWatchlist ?? new List<Watchlist>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Error al acceder al cache para usuario: {userName}");
            }

            // Cache miss - obtener desde base de datos
            _logger.LogInformation($"Cache miss para usuario: {userName}, consultando base de datos");
            var userWatchlist = await _context.DbWatchlist
                .Include(w => w.Assest)
                .Where(w => w.UserName == userName)
                .ToListAsync();

            // Guardar en cache
            try
            {
                var serializedData = JsonSerializer.Serialize(userWatchlist);
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    SlidingExpiration = _cacheTtl
                };
                await _cache.SetStringAsync(cacheKey, serializedData, cacheOptions);
                _logger.LogInformation($"Datos guardados en cache para usuario: {userName}");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Error al guardar en cache para usuario: {userName}");
            }

            return userWatchlist;
        }

        public async Task<bool> AddToWatchlistAsync(string userName, string ticker)
        {
            try
            {
                // Buscar el asset
                var asset = await _context.DbAssest.FirstOrDefaultAsync(a => a.Ticker == ticker);
                if (asset == null)
                {
                    _logger.LogWarning($"Asset no encontrado: {ticker}");
                    return false;
                }

                // Verificar si ya existe
                var existingItem = await _context.DbWatchlist
                    .Include(w => w.Assest)
                    .FirstOrDefaultAsync(w => w.UserName == userName
                        && w.Assest != null && w.Assest.Id == asset.Id);

                if (existingItem != null)
                {
                    _logger.LogInformation($"Item ya existe en watchlist: {ticker} para usuario {userName}");
                    return true; // Ya existe, consideramos como éxito
                }

                // Agregar a la watchlist
                var watchlistItem = new Watchlist
                {
                    UserName = userName,
                    Assest = asset,
                    CreatedAt = DateTime.Now
                };

                _context.DbWatchlist.Add(watchlistItem);
                await _context.SaveChangesAsync();

                // Invalidar cache
                await InvalidateUserCacheAsync(userName);

                _logger.LogInformation($"Item agregado a watchlist: {ticker} para usuario {userName}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al agregar {ticker} a watchlist para usuario {userName}");
                return false;
            }
        }

        public async Task<bool> RemoveFromWatchlistAsync(string userName, string ticker)
        {
            try
            {
                // Buscar el asset
                var asset = await _context.DbAssest.FirstOrDefaultAsync(a => a.Ticker == ticker);
                if (asset == null)
                {
                    _logger.LogWarning($"Asset no encontrado: {ticker}");
                    return false;
                }

                // Buscar el item en la watchlist
                var existingItem = await _context.DbWatchlist
                    .Include(w => w.Assest)
                    .FirstOrDefaultAsync(w => w.UserName == userName
                        && w.Assest != null && w.Assest.Id == asset.Id);

                if (existingItem == null)
                {
                    _logger.LogInformation($"Item no encontrado en watchlist: {ticker} para usuario {userName}");
                    return true; // No existe, consideramos como éxito
                }

                // Remover de la watchlist
                _context.DbWatchlist.Remove(existingItem);
                await _context.SaveChangesAsync();

                // Invalidar cache
                await InvalidateUserCacheAsync(userName);

                _logger.LogInformation($"Item removido de watchlist: {ticker} para usuario {userName}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al remover {ticker} de watchlist para usuario {userName}");
                return false;
            }
        }

        public async Task InvalidateUserCacheAsync(string userName)
        {
            var cacheKey = $"watchlist:user:{userName}";
            try
            {
                await _cache.RemoveAsync(cacheKey);
                _logger.LogInformation($"Cache invalidado para usuario: {userName}");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Error al invalidar cache para usuario: {userName}");
            }
        }
    }
}
