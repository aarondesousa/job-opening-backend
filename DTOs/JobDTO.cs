namespace JobOpeningBackend.DTOs;

public class JobDTO
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int LocationId { get; set; } // Foreign key property
    public int DepartmentId { get; set; } // Foreign key property
    public DateTimeOffset ClosingDate { get; set; }
}
