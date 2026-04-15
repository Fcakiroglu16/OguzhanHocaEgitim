using FluentValidation;

namespace Applications.Products.Update
{
    public record UpdateProductRequest(int Id, string Name, decimal Price);


    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id alanı sıfırdan büyük olmalıdır.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("boş olmamalıdır").MaximumLength(100)
                .WithMessage("isim alanı maksimum 100 karakter olmalıdır");
            RuleFor(x => x.Price).GreaterThan(0);
        }
    }
}