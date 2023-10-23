namespace OrdersApp.Models
{
    public class Orders
    {
        public Guid Id { get; set; }
        public string OrderName { get; set; }
        public double Price { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
