using System.ComponentModel.DataAnnotations;
using JobTrackr.Api.Enums;

namespace JobTrackr.Api.DTOs.JobApplications;

public class CreateJobApplicationRequest
{
    [Required]
    [StringLength(100)]
    public required string Company { get; set; }

    [Required]
    [StringLength(100)]
    public required string Position { get; set; }

    [StringLength(100)]
    public string? Location { get; set; }

    [Url]
    public string? JobUrl { get; set; }

    public ApplicationStatus Status { get; set; } = ApplicationStatus.Wishlist;

    public DateTime? AppliedAt { get; set; }

    [StringLength(2000)]
    public string? Notes { get; set; }
}