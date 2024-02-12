using JobOpeningBackend.Data;
using JobOpeningBackend.DTOs;
using JobOpeningBackend.Exceptions;
using JobOpeningBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace JobOpeningBackend.Services;

public class LocationsService : ILocationsService
{
    private readonly AppDbContext _dbContext;

    public LocationsService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Location>> GetAllLocationsAsync()
    {
        return await _dbContext.Locations.ToListAsync();
    }

    public async Task<Location?> GetLocationByIdAsync(int id)
    {
        return await _dbContext.Locations.FindAsync(id);
    }

    public async Task CreateLocationAsync(Location location)
    {
        if (_dbContext.Locations.Any(j => j.Title == location.Title))
        {
            throw new DuplicateTitleException("A location with the same title already exists.");
        }

        _dbContext.Locations.Add(location);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateLocationAsync(int id, Location location)
    {
        var existingLocation =
            await _dbContext.Locations.FindAsync(id)
            ?? throw new NotFoundException("Location not found");

        existingLocation.Title = location.Title;
        existingLocation.City = location.City;
        existingLocation.State = location.State;
        existingLocation.Country = location.Country;
        existingLocation.Zip = location.Zip;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteLocationAsync(int id)
    {
        var location = await _dbContext.Locations.FindAsync(id);
        if (location == null)
        {
            return;
        }

        _dbContext.Locations.Remove(location);
        await _dbContext.SaveChangesAsync();
    }
}
