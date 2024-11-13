namespace Rise.Shared.Products
{
    public class ProductResponse
    {
        public IEnumerable<ProductDTO> Products { get; set; } = new List<ProductDTO>();
        public int TotalPages { get; set; }
    }
}
