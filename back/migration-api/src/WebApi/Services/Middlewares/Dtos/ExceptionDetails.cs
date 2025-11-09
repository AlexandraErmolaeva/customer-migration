using System.Text.Json;

namespace CustomerMigrationApi.Services.Middlewares.Dtos;

public class ExceptionDetails
{
    public Guid Id { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public override string ToString() => JsonSerializer.Serialize(this);
}
