using Microsoft.EntityFrameworkCore;

namespace MediWingWebAPI.Models;

[PrimaryKey("InputId")]
public class UserInputRelation
{
    public Guid UserId { get; set; } 
    public Guid InputId { get; set; }
    public string InputName { get; set; }
}

public class UserInputRelationRead
{
    public Guid UserId { get; set; }
    public Guid InputId { get; set; }
    public string InputName { get; set; }
    public List<InputErrorRead>? MissingNames { get; set; }
}

[PrimaryKey("Id")]
public class UserDiagnosticInput
{
    public int? PatientId { get; set; }
    public Guid InputId { get; set; }
    public string? Sex { get; set; }
    public DateOnly? BirthDate { get; set; }
    public int Id { get; set; }
    public string? MKBCode { get; set; }
    public string? Diagnosis { get; set; }
    public DateOnly? Date { get; set; }
    public string? DoctorPost { get; set; }
    public List<string> Recommendations { get; set; }
}

public class UserDiagnosticInputRead
{
    public int Id { get; set; }
    public string? Sex { get; set; }
    public DateOnly? BirthDate { get; set; }
    public int? PatientId { get; set; }
    public string? MKBCode { get; set; }
    public string? Diagnosis { get; set; }
    public DateOnly? Date { get; set; }
    public string? DoctorPost { get; set; }
    public List<string>? Recommendations { get; set; }
}

[PrimaryKey("Id")]
public class InputError
{
    public Guid Id { get; set; }
    public Guid InputId { get; set; }
    public string DiagnosisName { get; set; }
}
public class InputErrorRead
{
    public Guid Id { get; set; }
    public string DiagnosisName { get; set; }
}

public class UserInputReadWResult
{
    public int Id { get; set; }
    public string? Sex { get; set; }
    public DateOnly? BirthDate { get; set; }
    public int? PatientId { get; set; }
    public string? MKBCode { get; set; }
    public string? Diagnosis { get; set; }
    public DateOnly? Date { get; set; }
    public string? DoctorPost { get; set; }
    public List<RecomendationWResult>? RecommendationsWResults { get; set; }
}

public class RecomendationWResult
{
    public string Name { get; set; }
    public bool? Status { get; set; }
}

[PrimaryKey("Id")]
public class Recommendation
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class UserDiagnosticInputGet
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreationDate { get; set; }
    public List<UserDiagnosticInputRead> InputDatas { get; set; }
}

public class UserDiagnosticInputCreation
{
    public Guid InputId { get; set; }
    public string? Sex { get; set; }
    public DateOnly? BirthDate { get; set; }
    public int? PatientId { get; set; }
    public string? MKBCode { get; set; }
    public string? Diagnosis { get; set; }
    public DateOnly? Date { get; set; }
    public string? DoctorPost { get; set; }
    public List<string>? Recommendations { get; set; }
}

public class UserDiagnosticInputUpdate
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
    public List<string>? Recommendations { get; set; }
}

public class UserDiagnosticInputDelete
{
    public Guid Id { get; set; }
    public Guid InputId { get; set; }
}

public class UserInputCheckResult
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreationDate { get; set; }
    public List<UserDiagnosticInputRead> InputDatas { get; set; }
}

public class UserDiadnosticOutput
{
    public int Id { get; set; }
    public string? Sex { get; set; }
    public DateOnly? BirthDate { get; set; }
    public int? PatientId { get; set; }
    public string? MKBCode { get; set; }
    public string? Diagnosis { get; set; }
    public DateOnly? Date { get; set; }
    public string? DoctorPost { get; set; }
    public bool StandartExists { get; set; }
    public double? Accuracy { get; set; }
    public List<RecommendationGroup>? RecommendationsGrouped { get; set; }
}

public class RecommendationGroup
{
    public int GroupStatus { get; set; }
    public string GroupStatusName { get; set; }
    public List<string> GroupRecommendations { get; set; }
}

public class UserOutputRead
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreationDate { get; set; }
    public double TotalAccuracy { get; set; }
    public List<UserDiadnosticOutput> OutputDatas { get; set; }
}

[PrimaryKey("Id")]
public class Status
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class RecommendationsWStatus
{
    public string Name { get; set; }
    public int Status { get; set; }
}

public class AvailableRecommendations
{
    public string Name { get; set; }
    public bool Status { get; set; }
    public bool IsAnalog { get; set; }
}
