namespace Rise.Shared.Products;

public class BookingDTO
{
      public int Id { get; set; }
      public required ProductDTO Product { get; set; }
      public string UserId { get; set; } = string.Empty;
      public DateTime StartDate { get; set; }
      public DateTime EndDate { get; set; }
}