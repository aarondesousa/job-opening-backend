using JobOpeningBackend.DTOs;
using JobOpeningBackend.Models;

namespace JobOpeningBackend.Services;

public interface IJobsService
{
    Task<IEnumerable<object>> GetAllJobsAsync();
    Task<object?> GetJobByIdAsync(int id);
    Task<Job> CreateJobAsync(JobDTO jobDTO);
    Task UpdateJobAsync(int id, JobDTO jobDTO);
    Task DeleteJobAsync(int id);
    Task<string> GenerateJobCodeAsync();
}
