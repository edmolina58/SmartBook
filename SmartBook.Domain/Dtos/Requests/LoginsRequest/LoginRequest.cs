using System.ComponentModel.DataAnnotations;

namespace SmartBook.Domain.Dtos.Requests.LoginRequest;

public record LoginRequest
(
    [EmailAddress]
    string Email,
    [Required]
    string PassWord
    );
