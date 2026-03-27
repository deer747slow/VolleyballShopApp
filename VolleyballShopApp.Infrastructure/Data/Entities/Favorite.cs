using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyballShopApp.Infrastructure.Data.Entities
{
    [PrimaryKey(nameof(UserId), nameof(ProductId))]
    public class Favorite
    {
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; } = null!;
        public int ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;
    }
}
