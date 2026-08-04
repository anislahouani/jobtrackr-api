using JobTrackr.Api.Data;
using JobTrackr.Api.DTOs.JobApplications;
using JobTrackr.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace JobTrackr.Api.Services;

public class JobApplicationService
{
    private readonly AppDbContext _dbContext;

    public JobApplicationService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<JobApplicationResponse>> GetAllAsync(Guid userId)
    {
        return await _dbContext.JobApplications
            .AsNoTracking()
            .Where(application => application.UserId == userId)
            .OrderByDescending(application => application.CreatedAt)
            .Select(application => ToResponse(application))
            .ToListAsync();
    }

    public async Task<JobApplicationResponse?> GetByIdAsync(
        Guid userId,
        Guid applicationId)
    {
        var application = await _dbContext.JobApplications
            .AsNoTracking()
            .SingleOrDefaultAsync(application =>
                application.Id == applicationId &&
                application.UserId == userId);

        return application is null ? null : ToResponse(application);
    }

    public async Task<JobApplicationResponse> CreateAsync(
        Guid userId,
        CreateJobApplicationRequest request)
    {
        var application = new JobApplication
        {
            Company = request.Company.Trim(),
            Position = request.Position.Trim(),
            Location = request.Location?.Trim(),
            JobUrl = request.JobUrl?.Trim(),
            Status = request.Status,
            AppliedAt = request.AppliedAt,
            Notes = request.Notes?.Trim(),
            UserId = userId
        };

        _dbContext.JobApplications.Add(application);
        await _dbContext.SaveChangesAsync();

        return ToResponse(application);
    }

    public async Task<JobApplicationResponse?> UpdateAsync(
        Guid userId,
        Guid applicationId,
        UpdateJobApplicationRequest request)
    {
        var application = await _dbContext.JobApplications
            .SingleOrDefaultAsync(application =>
                application.Id == applicationId &&
                application.UserId == userId);

        if (application is null)
        {
            return null;
        }

        application.Company = request.Company.Trim();
        application.Position = request.Position.Trim();
        application.Location = request.Location?.Trim();
        application.JobUrl = request.JobUrl?.Trim();
        application.Status = request.Status;
        application.AppliedAt = request.AppliedAt;
        application.Notes = request.Notes?.Trim();
        application.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return ToResponse(application);
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid applicationId)
    {
        var application = await _dbContext.JobApplications
            .SingleOrDefaultAsync(application =>
                application.Id == applicationId &&
                application.UserId == userId);

        if (application is null)
        {
            return false;
        }

        _dbContext.JobApplications.Remove(application);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    private static JobApplicationResponse ToResponse(
        JobApplication application)
    {
        return new JobApplicationResponse
        {
            Id = application.Id,
            Company = application.Company,
            Position = application.Position,
            Location = application.Location,
            JobUrl = application.JobUrl,
            Status = application.Status,
            AppliedAt = application.AppliedAt,
            Notes = application.Notes,
            CreatedAt = application.CreatedAt,
            UpdatedAt = application.UpdatedAt
        };
    }
}