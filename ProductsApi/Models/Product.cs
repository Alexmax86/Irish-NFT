using System.ComponentModel.DataAnnotations;

namespace ProductsApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public string? CreatedBy { get; set; }
        public string? Category { get; set; }
        public int Cost { get; set; }

        public string? ImgLink { get; set; }

        public bool? Sold { get; set;}
    }
}
