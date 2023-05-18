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
    public string HealthcareServiceCode { get; set; }
    public string HealthcareServiceName { get; set; }
    public string Mkb10Code { get; set; }
    public string Mkb10Name { get; set; }
    public bool IsMandatory { get; set; }
}

public class StandartCreation
{
    public List<string> HealthcareServiceCodes { get; set; }
    public List<string> Mkb10Codes { get; set; }
    public bool IsMandatory { get; set; }
}

public class StandartUpdate
{
    public string HealthcareServiceCode { get; set; }
    public string Mkb10Code { get; set; }
    public bool IsMandatory { get; set; }
}