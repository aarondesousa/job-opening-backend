using JobOpeningBackend.DTOs;
using JobOpeningBackend.Exceptions;
using JobOpeningBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobOpeningBackend.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class JobsController : ControllerBase
{
    private readonly IJobsService _jobsService;
    private readonly ILogger<JobsController> _logger;

    public JobsController(IJobsService jobsService, ILogger<JobsController> logger)
    {
        _jobsService = jobsService;
        _logger = logger;
    }

    // GET: api/v1/jobs
    [HttpGet]
    public async Task<IActionResult> GetAllJobs()
    {
        var jobs = await _jobsService.GetAllJobsAsync();
        return Ok(jobs);
    }

    // GET: api/v1/jobs/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetJobById(int id)
    {
        var job = await _jobsService.GetJobByIdAsync(id);
        if (job == null)
        {
            return NotFound();
        }
        return Ok(job);
    }

    // POST: api/v1/jobs
    [HttpPost]
    public async Task<IActionResult> CreateJob(JobDTO jobDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var createdJob = await _jobsService.CreateJobAsync(jobDTO);
            return CreatedAtAction(nameof(GetJobById), new { id = createdJob.Id }, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating the job.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    // PUT: api/v1/jobs/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateJob(int id, JobDTO jobDTO)
    {
        try
        {
            await _jobsService.UpdateJobAsync(id, jobDTO);
            // return NoContent();
            return Ok();
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    // DELETE: api/v1/jobs/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJob(int id)
    {
        await _jobsService.DeleteJobAsync(id);
        return NoContent();
    }
}
