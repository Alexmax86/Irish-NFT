using System.ComponentModel.DataAnnotations;

namespace OrdersApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int OrderedBy { get; set; }

        public DateTime DateOrdered { get; set; }
    }
}