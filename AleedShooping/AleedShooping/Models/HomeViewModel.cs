using AleedShooping.Common;
using AleedShooping.Data.Entities;

namespace AleedShooping.Models
{
    public class HomeViewModel
    {

        public PaginatedList<Product> Products { get; set; }

        public ICollection<Category> Categories { get; set; }

        public float Quantity { get; set; }
    }
}
