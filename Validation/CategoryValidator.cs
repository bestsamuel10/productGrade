using FluentValidation;
using ProductionGrade.DTOs;

namespace ProductionGrade.Validation
{

    public class CategoryValidator : AbstractValidator<CreateCategoryDto>
    {
        public CategoryValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MaximumLength(100);
        }
    }

}