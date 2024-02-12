using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobOpeningBackend.Models;

public class Job
{
    [Key]
    public int Id { get; set; } // Primary key

    [Required]
    public string? Code { get; set; } // "JOB-01", // Autogenerated

    [Required]
    public string? Title { get; set; }

    [Required]
    public string? Description { get; set; }
    public int LocationId { get; set; } // Foreign key property

    [Required]
    [ForeignKey("LocationId")]
    public Location? Location { get; set; } // Navigation property
    public int DepartmentId { get; set; } // Foreign key property

    [Required]
    [ForeignKey("DepartmentId")]
    public Department? Department { get; set; } // Navigation property
    public DateTimeOffset PostedDate { get; set; } = DateTimeOffset.UtcNow; // Autogenerated
    public DateTimeOffset ClosingDate { get; set; }
}
