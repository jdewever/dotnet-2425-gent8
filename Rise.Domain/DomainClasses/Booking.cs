namespace Rise.Domain.DomainClasses
{
    public class Booking : Entity
    {
        public Product Product { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Booking() { }
        
        public Booking(Product product, string userId, DateTime startDate, DateTime endDate)
        {
            Product = product;
            UserId = userId;
            StartDate = startDate;
            EndDate = endDate;
        }
    }   
}