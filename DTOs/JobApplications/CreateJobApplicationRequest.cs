using JobTrackr.Api.Enums;

namespace JobTrackr.Api.DTOs.JobApplications;

public class CreateJobApplicationRequest
{
    public required string Company { get; set; }

    public required string Position { get; set; }

    public string? Location { get; set; }

    public string? JobUrl { get; set; }

    public ApplicationStatus Status { get; set; } = ApplicationStatus.Wishlist;

    public DateTime? AppliedAt { get; set; }

    public string? Notes { get; set; }
}