namespace ProductionGrade.DTOs
{
    public class CreateOrderDto
    {
        public int UserId { get; set; }
        public List<CreateOrderLineDto> OrderLines { get; set; } = new();
    }

    public class CreateOrderLineDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
