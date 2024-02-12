using System.ComponentModel.DataAnnotations;

namespace JobOpeningBackend.Models;

public class Department
{
    [Key]
    public int Id { get; set; } // Primary key
    public string? Title { get; set; }
}
