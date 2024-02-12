using JobOpeningBackend.Models;

namespace JobOpeningBackend.Services;

public interface ILocationsService
{
    Task<IEnumerable<Location>> GetAllLocationsAsync();
    Task<Location?> GetLocationByIdAsync(int id);
    Task CreateLocationAsync(Location location);
    Task UpdateLocationAsync(int id, Location location);
    Task DeleteLocationAsync(int id);
}
