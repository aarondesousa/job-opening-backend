using JobOpeningBackend.Data;
using JobOpeningBackend.DTOs;
using JobOpeningBackend.Exceptions;
using JobOpeningBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace JobOpeningBackend.Services;

public class JobsService : IJobsService
{
    private readonly AppDbContext _dbContext;

    public JobsService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<object>> GetAllJobsAsync()
    {
        return await _dbContext
            .Jobs.Include(j => j.Location)
            .Include(j => j.Department)
            .Select(j => new
            {
                Id = j.Id,
                Code = j.Code,
                Title = j.Title,
                Location = j.Location != null ? j.Location.Title : null,
                Department = j.Department != null ? j.Department.Title : null,
                PostedDate = j.PostedDate,
                ClosingDate = j.ClosingDate
            })
            .ToListAsync();
    }

    public async Task<object?> GetJobByIdAsync(int id)
    {
        return await _dbContext
            .Jobs.Include(j => j.Location)
            .Include(j => j.Department)
            .Where(j => j.Id == id)
            .Select(j => new
            {
                Id = j.Id,
                Code = j.Code,
                Title = j.Title,
                Description = j.Description,
                Location = j.Location,
                Department = j.Department,
                PostedDate = j.PostedDate,
                ClosingDate = j.ClosingDate
            })
            .FirstOrDefaultAsync();
    }

    public async Task<Job> CreateJobAsync(JobDTO jobDTO)
    {
        if (_dbContext.Jobs.Any(j => j.Title == jobDTO.Title))
        {
            throw new DuplicateTitleException("A job with the same title already exists.");
        }

        string jobCode = await GenerateJobCodeAsync();

        var job = new Job
        {
            Title = jobDTO.Title,
            Description = jobDTO.Description,
            LocationId = jobDTO.LocationId,
            DepartmentId = jobDTO.DepartmentId,
            ClosingDate = jobDTO.ClosingDate,
            Code = jobCode
        };

        _dbContext.Jobs.Add(job);
        await _dbContext.SaveChangesAsync();

        return job;
    }

    public async Task UpdateJobAsync(int id, JobDTO jobDTO)
    {
        var job =
            await _dbContext.Jobs.FindAsync(id) ?? throw new NotFoundException("Job not found");
        job.Title = jobDTO.Title;
        job.Description = jobDTO.Description;
        job.LocationId = jobDTO.LocationId;
        job.DepartmentId = jobDTO.DepartmentId;
        job.ClosingDate = jobDTO.ClosingDate;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteJobAsync(int id)
    {
        var job = await _dbContext.Jobs.FindAsync(id);
        if (job == null)
        {
            return;
        }

        _dbContext.Jobs.Remove(job);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<string> GenerateJobCodeAsync()
    { // Does not account for deleted records
        // Get the latest job ID from the database
        int latestJobId = await _dbContext.Jobs.MaxAsync(j => (int?)j.Id) ?? 0;

        // Add a constant offset to the latest ID to generate the job code
        int jobCodeId = latestJobId + 1;

        // Remove the first digit from the ID
        string jobIdWithoutFirstDigit = jobCodeId.ToString()[1..];

        // Generate the job code based on the modified ID
        string jobCode = $"JOB-{jobIdWithoutFirstDigit}";

        return jobCode;
    }
}
