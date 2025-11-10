

namespace SmartBook.Domain.Entities;
public class EmailSettings
{
    public string Host { get; init; }
    public int Port { get; init; }
    public bool EnableSsl { get; init; }
    public string User { get; init; }
    public string Password { get; init; }
}
