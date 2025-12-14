using FluentValidation;
using ProductionGrade.DTOs;

namespace ProductionGrade.Validation
{
    public class OrderValidator : AbstractValidator<CreateOrderDto>
    {
        public OrderValidator()
        {
            RuleFor(o => o.UserId).GreaterThan(0);
            RuleForEach(o => o.OrderLines).SetValidator(new OrderLineValidator());
        }
    }

    public class OrderLineValidator : AbstractValidator<CreateOrderLineDto>
    {
        public OrderLineValidator()
        {
            RuleFor(ol => ol.ProductId).GreaterThan(0);
            RuleFor(ol => ol.Quantity).GreaterThan(0);
        }
    }
}
