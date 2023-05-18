using Microsoft.EntityFrameworkCore;

namespace MediWingWebAPI.Models;

[PrimaryKey("Id")]
public class Assignment
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsCompleted { get; set; } 
}

public class AssignmentCreation
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
}
