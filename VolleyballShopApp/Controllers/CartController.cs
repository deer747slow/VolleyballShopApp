using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

using VolleyballShopApp.Infrastructure.Data;
using VolleyballShopApp.Models.Cart;

namespace VolleyballShopApp.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext context;

        public CartController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            string? userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return this.Unauthorized();
            }

            var model =  this.context.Cart
                .Where(c => c.UserId == userId)
                .Include(c => c.Product)
                .Select(c => new CartIndexVM
                {
                    Id = c.Id,
                    ProductId = c.ProductId,
                    ProductName = c.Product.ProductName,
                    Picture = c.Product.Picture,
                    Price = c.Product.Price,
                    Discount = c.Product.Discount,
                    Quantity = c.Quantity
                })
                .ToListAsync();

            return this.View(model);
        }

        [HttpPost]
        public IActionResult Add(int productId)
        {
            string? userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return this.Unauthorized();
            }

            var product =  this.context.Products
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                return this.NotFound();
            }

            var cartItem =  this.context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (cartItem != null)
            {
                if (cartItem.Quantity < product.Quantity)
                {
                    cartItem.Quantity++;
                }
            }
            else
            {
                cartItem = new Cart
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = 1
                };

                 this.context.Carts.AddAsync(cartItem);
            }

             this.context.SaveChangesAsync();

            return this.RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Increase(int productId)
        {
            string? userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return this.Unauthorized();
            }

            var cartItem =  this.context.Carts
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (cartItem == null)
            {
                return this.RedirectToAction(nameof(Index));
            }

            if (cartItem.Quantity < cartItem.Product.Quantity)
            {
                cartItem.Quantity++;
                 this.context.SaveChangesAsync();
            }

            return this.RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Decrease(int productId)
        {
            string? userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return this.Unauthorized();
            }

            var cartItem =  this.context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (cartItem == null)
            {
                return this.RedirectToAction(nameof(Index));
            }

            cartItem.Quantity--;

            if (cartItem.Quantity <= 0)
            {
                this.context.Carts.Remove(cartItem);
            }

             this.context.SaveChangesAsync();

            return this.RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Remove(int productId)
        {
            string? userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return this.Unauthorized();
            }

            var cartItem =  this.context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (cartItem != null)
            {
                this.context.Carts.Remove(cartItem);
                 this.context.SaveChangesAsync();
            }

            return this.RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Clear()
        {
            string? userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return this.Unauthorized();
            }

            var cartItems = this.context.Carts
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (cartItems.Any())
            {
                this.context.Carts.RemoveRange(cartItems);
                 this.context.SaveChangesAsync();
            }

            return this.RedirectToAction(nameof(Index));
        }
    }
}