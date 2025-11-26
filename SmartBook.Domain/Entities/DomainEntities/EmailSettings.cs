using System.ComponentModel.DataAnnotations;

namespace SmartBook.Domain.Entities.DomainEntities;
public class EmailSettings
{
    [Key]
    public string Host { get; set; }
    [Required]
    public int Port { get; set; }
    [Required]
    public bool EnableSsl { get; set; }
    [Required]
    public string User { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string BaseUrl { get; set; }
}
