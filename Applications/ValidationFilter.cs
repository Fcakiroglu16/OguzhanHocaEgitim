using FluentValidation;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Applications
{
    public class ValidationFilter<T> : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();


            if (validator is null)
            {
                throw new Exception("Validation not found");
            }


            var requestModel = context.Arguments.OfType<T>().FirstOrDefault();


            if (requestModel is null) return next(context);


            var validationResult = validator.Validate(requestModel);


            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            return next(context);
        }
    }
}
