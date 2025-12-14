namespace ProductionGrade.Models
{
    public class Product : BaseEntity
    {
        public string SKU { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
    }

}
