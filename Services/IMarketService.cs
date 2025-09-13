using apptrade.Models;

namespace apptrade.Services
{
    public interface IMarketService
    {
        Task<List<Assest>> GetAllInstrumentsAsync();
        Task<Assest?> GetInstrumentByTickerAsync(string ticker);
        Task InvalidateInstrumentsCacheAsync();
    }
}
