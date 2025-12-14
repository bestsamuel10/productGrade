using FluentValidation;
using ProductionGrade.DTOs;

namespace ProductionGrade.Validation
{
    public class ProductValidator : AbstractValidator<CreateProductDto>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name).NotEmpty().MaximumLength(100);
            RuleFor(p => p.Description).NotEmpty().MaximumLength(500);
            RuleFor(p => p.Price).GreaterThan(0);
            RuleFor(p => p.StockQuantity).GreaterThanOrEqualTo(0);
        }
    }
}
