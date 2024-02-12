using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JobOpeningBackend.Data;
using JobOpeningBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JobOpeningBackend.Controllers;

public class UserTokenController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;
    private readonly ILogger<UserTokenController> _logger;

    public UserTokenController(
        IConfiguration configuration,
        AppDbContext context,
        ILogger<UserTokenController> logger
    )
    {
        _configuration = configuration;
        _context = context;
        _logger = logger;
    }

    [HttpPost("/token")]
    public async Task<IActionResult> Post(UserToken userToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingUserToken = await GetUser(userToken.Username);

        if (existingUserToken == null)
        {
            return NotFound();
        }

        if (!BCrypt.Net.BCrypt.Verify(userToken.Password, existingUserToken.Password))
        {
            return BadRequest();
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]!),
            // new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            // new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim("id", existingUserToken.Id.ToString()),
            new Claim("UserName", existingUserToken.Username),
            new Claim(ClaimTypes.Role, existingUserToken.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddYears(5),
            signingCredentials: signIn
        );

        return Ok(new JwtSecurityTokenHandler().WriteToken(token));
    }

    [HttpPost("/User")]
    public async Task<IActionResult> CreateUserToken(UserToken userToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (userToken.Role != "Department")
        {
            userToken.Role = "Location";
        }

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(userToken.Password);

        userToken.Password = passwordHash;

        try
        {
            _context.UserToken.Add(userToken);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserTokenById), new { id = userToken.Id }, userToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating the UserToken.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserTokenById(int id)
    {
        var userToken = await _context.UserToken.FindAsync(id);
        if (userToken == null)
        {
            return NotFound();
        }
        return Ok(userToken);
    }

    [HttpGet("/GetAll")]
    public async Task<IActionResult> GetAllUserTokens()
    {
        try
        {
            var userTokens = await _context.UserToken.ToListAsync();
            return Ok(userTokens);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while fetching the UserToken.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<UserToken>> DeleteUserInfo(int id)
    {
        var userToken = await _context.UserToken.FindAsync(id);
        if (userToken == null)
        {
            return NotFound();
        }

        _context.UserToken.Remove(userToken);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<UserToken?> GetUser(string Username)
    {
        return await _context.UserToken.FirstOrDefaultAsync(u => u.Username == Username);
    }
}
