namespace Rise.Shared.Products
{
    public class DashboardDTO
    {
        public required IEnumerable<ProductDTO> LowStockProducts { get; set; }
        public required int ProductsReturning { get; set; }
        public required int ProductsReserved { get; set; }
    }
}