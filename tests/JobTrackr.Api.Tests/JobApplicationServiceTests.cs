using JobTrackr.Api.Data;
using JobTrackr.Api.DTOs.JobApplications;
using JobTrackr.Api.Enums;
using JobTrackr.Api.Models;
using JobTrackr.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace JobTrackr.Api.Tests;

public class JobApplicationServiceTests
{
    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_PersistsApplication_AndTrimsTextFields()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        await using var db = CreateDbContext();
        var service = new JobApplicationService(db);
        var userId = Guid.NewGuid();

        var result = await service.CreateAsync(userId, new CreateJobApplicationRequest
        {
            Company = "  OpenAI  ",
            Position = "  Software Engineer Intern  ",
            Location = "  Paris  ",
            JobUrl = "  https://example.com/job  ",
            Status = ApplicationStatus.Applied,
            Notes = "  Follow up next week  "
        });

        var stored = await db.JobApplications.SingleAsync(cancellationToken);

        Assert.Equal(userId, stored.UserId);
        Assert.Equal("OpenAI", stored.Company);
        Assert.Equal("Software Engineer Intern", stored.Position);
        Assert.Equal("Paris", stored.Location);
        Assert.Equal("https://example.com/job", stored.JobUrl);
        Assert.Equal("Follow up next week", stored.Notes);
        Assert.Equal(ApplicationStatus.Applied, stored.Status);
        Assert.Equal(stored.Id, result.Id);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOnlyCurrentUsersApplications_InNewestFirstOrder()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        await using var db = CreateDbContext();
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var older = new JobApplication
        {
            Company = "Older Corp",
            Position = "Engineer",
            UserId = userId,
            CreatedAt = DateTime.UtcNow.AddDays(-2)
        };
        var newer = new JobApplication
        {
            Company = "Newer Corp",
            Position = "Engineer",
            UserId = userId,
            CreatedAt = DateTime.UtcNow.AddDays(-1)
        };
        var otherUsersApplication = new JobApplication
        {
            Company = "Hidden Corp",
            Position = "Engineer",
            UserId = otherUserId,
            CreatedAt = DateTime.UtcNow
        };

        db.JobApplications.AddRange(older, newer, otherUsersApplication);
        await db.SaveChangesAsync(cancellationToken);

        var service = new JobApplicationService(db);
        var result = await service.GetAllAsync(userId);

        Assert.Equal(2, result.Count);
        Assert.Equal(newer.Id, result[0].Id);
        Assert.Equal(older.Id, result[1].Id);
        Assert.DoesNotContain(result, item => item.Id == otherUsersApplication.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenApplicationBelongsToAnotherUser()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        await using var db = CreateDbContext();
        var ownerId = Guid.NewGuid();
        var requesterId = Guid.NewGuid();
        var application = new JobApplication
        {
            Company = "Private Corp",
            Position = "Engineer",
            UserId = ownerId
        };

        db.JobApplications.Add(application);
        await db.SaveChangesAsync(cancellationToken);

        var service = new JobApplicationService(db);
        var result = await service.GetByIdAsync(requesterId, application.Id);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesOwnedApplication_AndTrimsFields()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        await using var db = CreateDbContext();
        var userId = Guid.NewGuid();
        var application = new JobApplication
        {
            Company = "Old Corp",
            Position = "Old Role",
            UserId = userId
        };

        db.JobApplications.Add(application);
        await db.SaveChangesAsync(cancellationToken);

        var service = new JobApplicationService(db);
        var result = await service.UpdateAsync(userId, application.Id, new UpdateJobApplicationRequest
        {
            Company = "  Updated Corp  ",
            Position = "  Updated Role  ",
            Location = "  Lyon  ",
            Status = ApplicationStatus.Interview,
            Notes = "  Technical interview  "
        });

        Assert.NotNull(result);
        Assert.Equal("Updated Corp", result.Company);
        Assert.Equal("Updated Role", result.Position);
        Assert.Equal("Lyon", result.Location);
        Assert.Equal(ApplicationStatus.Interview, result.Status);
        Assert.Equal("Technical interview", result.Notes);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_AndDoesNotModify_WhenApplicationBelongsToAnotherUser()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        await using var db = CreateDbContext();
        var ownerId = Guid.NewGuid();
        var requesterId = Guid.NewGuid();
        var application = new JobApplication
        {
            Company = "Original Corp",
            Position = "Original Role",
            UserId = ownerId
        };

        db.JobApplications.Add(application);
        await db.SaveChangesAsync(cancellationToken);

        var service = new JobApplicationService(db);
        var result = await service.UpdateAsync(requesterId, application.Id, new UpdateJobApplicationRequest
        {
            Company = "Tampered Corp",
            Position = "Tampered Role",
            Status = ApplicationStatus.Offer
        });

        var stored = await db.JobApplications.SingleAsync(cancellationToken);
        Assert.Null(result);
        Assert.Equal("Original Corp", stored.Company);
        Assert.Equal("Original Role", stored.Position);
    }

    [Fact]
    public async Task DeleteAsync_DeletesOwnedApplication()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        await using var db = CreateDbContext();
        var userId = Guid.NewGuid();
        var application = new JobApplication
        {
            Company = "Delete Me",
            Position = "Engineer",
            UserId = userId
        };

        db.JobApplications.Add(application);
        await db.SaveChangesAsync(cancellationToken);

        var service = new JobApplicationService(db);
        var deleted = await service.DeleteAsync(userId, application.Id);

        Assert.True(deleted);
        Assert.Empty(db.JobApplications);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_AndKeepsApplication_WhenApplicationBelongsToAnotherUser()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        await using var db = CreateDbContext();
        var ownerId = Guid.NewGuid();
        var requesterId = Guid.NewGuid();
        var application = new JobApplication
        {
            Company = "Keep Me",
            Position = "Engineer",
            UserId = ownerId
        };

        db.JobApplications.Add(application);
        await db.SaveChangesAsync(cancellationToken);

        var service = new JobApplicationService(db);
        var deleted = await service.DeleteAsync(requesterId, application.Id);

        Assert.False(deleted);
        Assert.Single(db.JobApplications);
    }
}
