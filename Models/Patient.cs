using Microsoft.EntityFrameworkCore;

namespace MediWingWebAPI.Models;

[PrimaryKey("Id")]
public class Patient
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Lastname { get; set; }
    public string? Email { get; set; }
    public string? Phonenumber { get; set; }
    public string? Address { get; set; }
    public long? Insurance { get; set; }
}