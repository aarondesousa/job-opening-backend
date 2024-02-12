using System.ComponentModel.DataAnnotations;

namespace JobOpeningBackend.Models;

public class Location
{
    [Key]
    public int Id { get; set; } // Primary key
    public string? Title { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Zip { get; set; }
}
