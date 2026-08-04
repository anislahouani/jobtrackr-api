using System.ComponentModel.DataAnnotations;

namespace JobTrackr.Api.DTOs.Auth;

public class RegisterRequest
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public required string FirstName { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public required string LastName { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [MinLength(8)]
    public required string Password { get; set; }
}