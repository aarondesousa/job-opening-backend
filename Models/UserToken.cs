using System.ComponentModel.DataAnnotations;

namespace JobOpeningBackend.Models;

public class UserToken
{
    [Key]
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public string Role { get; set; } = "Department";
}
