using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            throw new NotImplementedException();
        }

        public List<Favorite> GetFavorites(string userId)
        {
            throw new NotImplementedException();
        }

        public bool IsFavorite(string userId, int productId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveFromFavorites(string userId, int productId)
        {
            throw new NotImplementedException();
        }
    }
}
