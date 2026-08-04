using JobTrackr.Api.Enums;

namespace JobTrackr.Api.DTOs.JobApplications;

public class JobApplicationResponse
{
    public Guid Id { get; set; }

    public required string Company { get; set; }

    public required string Position { get; set; }

    public string? Location { get; set; }

    public string? JobUrl { get; set; }

    public ApplicationStatus Status { get; set; }

    public DateTime? AppliedAt { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}