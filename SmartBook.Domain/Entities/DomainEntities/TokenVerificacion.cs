using System.ComponentModel.DataAnnotations;

namespace SmartBook.Domain.Entities.DomainEntities
{
    public class TokenVerificacion
    {
        [Required]
        public string Email { get; init; }
        [Key]
        public string Token { get; init; }
        public DateTime Expiracion { get; init; }

    }
}