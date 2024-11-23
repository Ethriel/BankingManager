using System.Text.Json;
using System.Text;
using BankingManager.Services.Model;

namespace BankingManager.Server.Middlewares
{
    public class AssignNewAccountNumberMiddleware
    {
        private readonly RequestDelegate _next;

        public AssignNewAccountNumberMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Post &&
                context.Request.Path.StartsWithSegments("/bank-account/add"))
            {
                context.Request.EnableBuffering();
                var bodyAsText = await new StreamReader(context.Request.Body).ReadToEndAsync();
                context.Request.Body.Position = 0;

                if (!string.IsNullOrEmpty(bodyAsText))
                {
                    var jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var account = JsonSerializer.Deserialize<BankAccountDTO>(bodyAsText, jsonSerializerOptions);

                    if (string.IsNullOrWhiteSpace(account?.Number))
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
