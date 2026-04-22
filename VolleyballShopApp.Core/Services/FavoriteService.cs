using Microsoft.EntityFrameworkCore;
using VolleyballShopApp.Core.Contracts;
using VolleyballShopApp.Infrastructure.Data;
using VolleyballShopApp.Infrastructure.Data.Entities;

namespace VolleyballShopApp.Core.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly ApplicationDbContext _context;

        public FavoriteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool AddToFavorites(string userId, int productId)
        {
            if (IsFavorite(userId, productId))
            {
                return false;
            }

            var product = _context.Products.Find(productId);
            if (product == null)
            {
                return false;
            }

            _context.Favorites.Add(new Favorite
            {
                ProductId = productId,
                UserId = userId
            });

            return _context.SaveChanges() > 0;
        }

        public List<Favorite> GetFavorites(string userId)
        {
            return _context.Favorites
                .Include(f => f.Product)
                .ThenInclude(p => p.Brand)
                .Include(f => f.Product)
                .ThenInclude(p => p.Category)
                .Where(f => f.UserId == userId)
                .ToList();
        }

        public bool IsFavorite(string userId, int productId)
        {
            return _context.Favorites
                .Any(f => f.UserId == userId && f.ProductId == productId);
        }

        public bool RemoveFromFavorites(string userId, int productId)
        {
            var item = _context.Favorites
                .FirstOrDefault(f => f.ProductId == productId && f.UserId == userId);

            if (item == null)
            {
                return false;
            }

            _context.Favorites.Remove(item);
            return _context.SaveChanges() > 0;
        }
    }
}