using FluentValidation;

namespace Applications.Products.Create;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(10).WithMessage("name alanı maksimum 10 karakter olacaktır")
            .MinimumLength(4).WithMessage("name alanı minimum 4 karakter olacaktır")
            .Must(name => name.StartsWith("A")).WithMessage("ürün ismi büyük A harfi ile başlamalıdır");
        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than zero.");
    }
}