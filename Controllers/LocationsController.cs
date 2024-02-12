using JobOpeningBackend.Exceptions;
using JobOpeningBackend.Models;
using JobOpeningBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobOpeningBackend.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Location")]
public class LocationsController : ControllerBase
{
    private readonly ILocationsService _locationsService;
    private readonly ILogger<LocationsController> _logger;

    public LocationsController(
        ILocationsService locationsService,
        ILogger<LocationsController> logger
    )
    {
        _locationsService = locationsService;
        _logger = logger;
    }

    // GET: api/v1/locations
    [HttpGet]
    public async Task<IActionResult> GetAllLocations()
    {
        var locations = await _locationsService.GetAllLocationsAsync();
        return Ok(locations);
    }

    // GET: api/v1/locations/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLocationById(int id)
    {
        var location = await _locationsService.GetLocationByIdAsync(id);
        if (location == null)
        {
            return NotFound();
        }
        return Ok(location);
    }

    // POST: api/v1/locations
    [HttpPost]
    public async Task<IActionResult> CreateLocation(Location location)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _locationsService.CreateLocationAsync(location);
            return CreatedAtAction(nameof(GetLocationById), new { id = location.Id }, location);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating the location.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    // PUT: api/v1/locations/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLocation(int id, Location location)
    {
        try
        {
            await _locationsService.UpdateLocationAsync(id, location);
            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    // DELETE: api/v1/locations/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLocation(int id)
    {
        await _locationsService.DeleteLocationAsync(id);
        return NoContent();
    }
}
