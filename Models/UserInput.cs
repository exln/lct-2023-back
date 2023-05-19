using Microsoft.EntityFrameworkCore;

namespace MediWingWebAPI.Models;

[PrimaryKey("InputId")]
public class UserInputRelation
{
    public Guid UserId { get; set; } 
    public Guid InputId { get; set; }
    public string InputName { get; set; }
}

public class UserInputRead
{
    public Guid UserId { get; set; }
    public Guid InputId { get; set; }
    public string InputName { get; set; }
    public List<UserDiagnosticInput> DiagnosticInputs { get; set; }
}

[PrimaryKey("Id")]
public class UserDiagnosticInput
{
    public Guid Id { get; set; }
    public Guid InputId { get; set; }
    public string? Sex { get; set; }
    public DateOnly? BirthDate { get; set; }
    public int? PatientId { get; set; }
    public string? MKBCode { get; set; }
    public string? Diagnosis { get; set; }
    public DateOnly? Date { get; set; }
    public string? DoctorPost { get; set; }
    public string? Recomendation { get; set; }
}