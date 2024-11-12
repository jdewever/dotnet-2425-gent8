namespace Rise.Domain.DomainClasses
{
    public class Booking : Entity
    {
        private int productId = default!;
        private string userId = default!;
        private DateTime startDate = default!;
        private DateTime endDate = default!;

        public Booking(int productId, string userId, DateTime startDate, DateTime endDate)
        {
            ProductId = productId;
            UserId = userId;
            StartDate = startDate;
            EndDate = endDate;
        }

        public int ProductId {
            get => productId;
            set => productId = Guard.Against.Negative(value, nameof(productId));
        }

        public string UserId
        {
          get => userId;
          set => userId = Guard.Against.NullOrWhiteSpace(value);
        }

        public DateTime StartDate 
        {
          get => startDate;
          set => startDate = value;
        }

        public DateTime EndDate 
        {
          get => endDate;
          set => endDate = value;
        }


    }
}