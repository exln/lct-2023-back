using Microsoft.EntityFrameworkCore;

namespace MediWingWebAPI.Models;

[PrimaryKey("Id")]
public class HealthcareService
{
    public int Id { get; set; }
    public char Section { get; set; }
    public int Block { get; set; }
    public int Number { get; set; }
    public int Subnumber { get; set; }
    public int? Subsubnumber { get; set; }
    public string Name { get; set; }
}

public class HealthcareServiceRead
{
    public string Code { get; set; }
    public string Name { get; set; }
}

public class HealthcareServiceCreation
{
    public string Code { get; set; }
    public string Name { get; set; }
}

public class HealthcareServiceUpdate
{
    public string Code { get; set; }
    public string Name { get; set; }
}

[PrimaryKey("Id")]
public class ServiceSectionAccordance
{
    public int Id { get; set; }
    public char Section { get; set; }
    public string Name { get; set; }
}

[PrimaryKey("Id")]
public class ServiceBlockAccordance
{
    public int Id { get; set; }
    public char Section { get; set; }
    public int Block { get; set; }
    public string Name { get; set; }
}

[PrimaryKey("Id")]
public class ServiceNumberAccordance
{
    public int Id { get; set; }
    public char Section { get; set; }
    public int Block { get; set; }
    public int Number { get; set; }
    public string Name { get; set; }
}