using System.Text.Json;
using System.Text;
using BankingManager.Services.Model;

namespace BankingManager.Server.Middlewares
{
    public class AssignNewAccountNumberMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Post &&
                context.Request.Path.StartsWithSegments("/bank-account/create"))
            {
                context.Request.EnableBuffering();
                var bodyAsText = await new StreamReader(context.Request.Body).ReadToEndAsync();
                context.Request.Body.Position = 0;

                if (!string.IsNullOrEmpty(bodyAsText))
                {
                    var jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var account = JsonSerializer.Deserialize<BankAccountDTO>(bodyAsText, jsonSerializerOptions);

                    // The "string" check is for Swagger if a person forgets to send an empty account number and sends a default for Swagger one
                    if (string.IsNullOrWhiteSpace(account?.Number) || account?.Number == "string")
                    {
                        account.Number = Guid.NewGuid().ToString();
                        var updatedBody = JsonSerializer.Serialize(account);
                        var updatedBodyAsBytes = Encoding.UTF8.GetBytes(updatedBody);
                        context.Request.Body = new MemoryStream(updatedBodyAsBytes);
                        context.Request.ContentLength = updatedBodyAsBytes.Length;
                    }
                }
            }

            await _next(context);
        }
    }
}
