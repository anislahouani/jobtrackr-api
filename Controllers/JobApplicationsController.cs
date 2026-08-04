using JobTrackr.Api.DTOs.JobApplications;
using JobTrackr.Api.Extensions;
using JobTrackr.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobTrackr.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/job-applications")]
public class JobApplicationsController : ControllerBase
{
    private readonly JobApplicationService _service;

    public JobApplicationsController(JobApplicationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<JobApplicationResponse>>> GetAll()
    {
        var userId = User.GetUserId();
        return Ok(await _service.GetAllAsync(userId));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<JobApplicationResponse>> GetById(Guid id)
    {
        var userId = User.GetUserId();
        var application = await _service.GetByIdAsync(userId, id);

        return application is null ? NotFound() : Ok(application);
    }

    [HttpPost]
    public async Task<ActionResult<JobApplicationResponse>> Create(
        CreateJobApplicationRequest request)
    {
        var userId = User.GetUserId();
        var application = await _service.CreateAsync(userId, request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = application.Id },
            application);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<JobApplicationResponse>> Update(
        Guid id,
        UpdateJobApplicationRequest request)
    {
        var userId = User.GetUserId();
        var application = await _service.UpdateAsync(userId, id, request);

        return application is null ? NotFound() : Ok(application);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.GetUserId();
        var deleted = await _service.DeleteAsync(userId, id);

        return deleted ? NoContent() : NotFound();
    }
}