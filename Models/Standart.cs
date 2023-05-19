using Microsoft.EntityFrameworkCore;

namespace MediWingWebAPI.Models;

[PrimaryKey("Id")]
public class Standart
{
    public Guid Id { get; set; }
    public string HealthcareServiceCode { get; set; }
    public string Mkb10Code { get; set; }
    public bool IsMandatory { get; set; }
}

public class StandartRead
{
    public string Mkb10Code { get; set; }
    public List<HealthcareServiceWithBool> HealthcareServiceCodesWithBools { get; set; }
}

public class StandartCreation
{
    public List<HealthcareServiceWithBool> ServiceCodesWithBools { get; set; }
    public List<string> Mkb10Codes { get; set; }
}

public class HealthcareServiceWithBool
{
    public string HealthcareServiceCode { get; set; }
    public bool IsMandatory { get; set; }
}

public class StandartUpdate
{
    public string HealthcareServiceCode { get; set; }
    public string Mkb10Code { get; set; }
    public bool IsMandatory { get; set; }
}