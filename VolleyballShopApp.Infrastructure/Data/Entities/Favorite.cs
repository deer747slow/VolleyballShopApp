using Microsoft.EntityFrameworkCore;

namespace VolleyballShopApp.Infrastructure.Data.Entities
{
    [PrimaryKey(nameof(UserId), nameof(ProductId))]
    public class Favorite
    {
        public string UserId { get; set; } = null!;

        public virtual ApplicationUser User { get; set; } = null!;

        public int ProductId { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}