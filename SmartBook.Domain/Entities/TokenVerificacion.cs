



namespace SmartBook.Domain.Entities
{
    public class TokenVerificacion
    {
        public string Email { get; init; }
        public string Token { get; init; }
        public DateTime Expiracion { get; init; }

    }
}