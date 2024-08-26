using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Microservice.Authentication.Api.Helpers.Enums;

namespace Microservice.Authentication.Api.Domain;

[Table("MSOS_User")]
public class User
{
    public User() { }

    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public Role Role { get; set; }

    public DateTime? Verified { get; set; }

    public bool IsAuthenticated => Verified.HasValue;

    [NotMapped]
    public string Password { get; set; } = string.Empty;
}