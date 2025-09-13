using apptrade.Models;

namespace apptrade.Services
{
    public interface IWatchlistService
    {
        Task<List<Watchlist>> GetUserWatchlistAsync(string userName);
        Task<bool> AddToWatchlistAsync(string userName, string ticker);
        Task<bool> RemoveFromWatchlistAsync(string userName, string ticker);
        Task InvalidateUserCacheAsync(string userName);
    }
}
