using VolleyballShopApp.Infrastructure.Data.Entities;

namespace VolleyballShopApp.Core.Contracts
{
    public interface IFavoriteService
    {
        List<Favorite> GetFavorites(string userId);
        bool AddToFavorites(string userId, int productId);
        bool RemoveFromFavorites(string userId, int productId);
        bool IsFavorite(string userId, int productId);
    }
}