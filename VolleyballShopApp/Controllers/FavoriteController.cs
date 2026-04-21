using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using VolleyballShopApp.Core.Contracts;
using VolleyballShopApp.Infrastructure.Data;
using VolleyballShopApp.Infrastructure.Data.Entities;
using VolleyballShopApp.Models.Product;

namespace VolleyballShopApp.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly IFavoriteService favoritesService;

        public FavoritesController(IFavoriteService favoritesService)
        {
            this.favoritesService = favoritesService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            List<ProductIndexVM> products = favoritesService.GetFavorites(userId) 
                .Select(product => new ProductIndexVM
            {
                Id = product.ProductId,
                ProductName = product.Product.ProductName,               
                BrandName = product.Product.Brand.BrandName,
                CategoryName = product.Product.Category.CategoryName,
                Picture = product.Product.Picture,
                Quantity = product.Product.Quantity,
                Price = product.Product.Price,
                Discount = product.Product.Discount
                }).ToList();
            return View(products);
        }

        [HttpPost]
        public IActionResult Add(int productId)
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }


            bool isInFavorites = favoritesService.IsFavorite(userId, productId);

            if (!isInFavorites)
            {
               favoritesService.AddToFavorites(userId, productId);
            }

            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public IActionResult Remove(int productId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);



            bool isInFavorites = favoritesService.IsFavorite(userId, productId);
            if (isInFavorites)
            {
                favoritesService.RemoveFromFavorites(userId, productId);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}