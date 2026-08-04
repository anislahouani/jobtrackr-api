using JobTrackr.Api.Enums;

namespace JobTrackr.Api.Models;

public class JobApplication
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public required string Company { get; set; }

    public required string Position { get; set; }

    public string? Location { get; set; }

    public string? JobUrl { get; set; }

    public ApplicationStatus Status { get; set; } = ApplicationStatus.Wishlist;

    public DateTime? AppliedAt { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Guid UserId { get; set; }

    public User? User { get; set; }
}