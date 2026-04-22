using Domains;

namespace Presentation.API.Endpoints.ErrorHandlerExample
{
    public static class ExceptionHandlerEndpoints
    {
        public static void AddExceptionHandlerEndpoints(this WebApplication app)
        {
            app.MapGet("api/exception-handler-example", () =>
            {
                throw new Exception("db hatası");
                //var product = new Product()
                //{
                //    Id = 1,
                //    Name = "100"
                //};
                //product.UpdatePrice(-100);

                return Results.Ok();
            });
        }
    }
}
