using Microsoft.EntityFrameworkCore;

namespace MediWingWebAPI.Models;

[PrimaryKey("Id")]
public class MskEsili
{
    public int Id { get; set; }
    public string? MskEsiliCode { get; set; }
    public int Type { get; set; }
    public bool? IsChild { get; set; }
    public int? IdCode { get; set; }
    public int? LdpCode { get; set; }
    public string Name { get; set; }
    public bool? Ambulatory { get; set; }
    public bool? Stationary { get; set; }
    public string? Modalities { get; set; }
    public List<MskEsiliAnalog>? Analogs { get; set; }
}

public class MskEsiliCreation
{
    public string? MskEsiliCode { get; set; }
    public int Type { get; set; }
    public bool? IsChild { get; set; }
    public int? IdCode { get; set; }
    public int? LdpCode { get; set; }
    public string Name { get; set; }
    public bool? Ambulatory { get; set; }
    public bool? Stationary { get; set; }
    public string? Modalities { get; set; }
    public List<MskEsiliAnalog>? Analogs { get; set; }
}

public class MskEsiliUpdate
{
    public string? MskEsiliCode { get; set; }
    public int Type { get; set; }
    public bool? IsChild { get; set; }
    public int? IdCode { get; set; }
    public int? LdpCode { get; set; }
    public string Name { get; set; }
    public bool? Ambulatory { get; set; }
    public bool? Stationary { get; set; }
    public string? Modalities { get; set; }
    public List<MskEsiliAnalog>? Analogs { get; set; }
}

public class MskEsiliRead
{
    public string? MskEsiliCode { get; set; }
    public int Type { get; set; }
    public bool? IsChild { get; set; }
    public int? IdCode { get; set; }
    public int? LdpCode { get; set; }
    public string Name { get; set; }
    public bool? Ambulatory { get; set; }
    public bool? Stationary { get; set; }
    public string? Modalities { get; set; }
    public List<MskEsiliAnalog> Analogs { get; set; } = new();
}

[PrimaryKey("Name")]
public class MskEsiliAnalog
{
    public string Name { get; set; }
}

public class MskEsiliAnalogCreation
{
    public string EsiliName { get; set; }
    public List<MskEsiliAnalog> AnalogNames { get; set; }
}

public class MskEsiliAnalogRead
{
    public string EsiliName { get; set; }
    public List<MskEsiliAnalog> AnalogNames { get; set; }   
}

[PrimaryKey("Id")]
public class MskEsiliType
{
    public int Id { get; set; }
    public string Name { get; set; }
}
