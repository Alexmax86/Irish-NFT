namespace NFTmvc.Models

{    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string OrderedBy { get; set; }
        public DateTime DateOrdered { get; set; }
    }
}