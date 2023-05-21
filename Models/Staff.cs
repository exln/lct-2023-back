using Microsoft.EntityFrameworkCore;

namespace MediWingWebAPI.Models;

[PrimaryKey("Id")]
public class Staff
{
    public Guid Id;
    public string Name = "Вася";
    public string Surname;
    public string? Lastname;
    public string? Position;
    public string? Department;
    public DateOnly? DateOfBirth;
    public DateOnly? DateOfEmployment;
    public DateOnly? DateOfDismissal;
    
    public Guid UserId;
    internal User? User;
}