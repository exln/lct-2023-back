using System.ComponentModel.DataAnnotations;
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

[PrimaryKey("Id")]
public class Modality
{
    public int Id { get; set; }
    public string Name { get; set; }
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

public class MskAnalysisAnalogCreation
{
    public int AnalysisId { get; set; }
    public Guid AnalogGuid { get; set; }
}

public class MskEsiliAnalogRead
{
    public string EsiliName { get; set; }
    public List<MskEsiliAnalog> AnalogNames { get; set; }   
}

[PrimaryKey("Id")]
public class MskAnalysisType
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
}

[PrimaryKey("Id")]
public class MskAnalysisClass
{
    public int Id { get; set; }
    [Required]
    public int AnalysisTypeId { get; set; }
    [Required]
    public string Name { get; set; }
}

public class MskAnalysisClassCreation
{
    public int AnalysisTypeId { get; set; }
    public string Name { get; set; }
}

[PrimaryKey("Id")]
public class MskAnalysisCategory
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
}

public class MskAnalysisCategoryCreation
{
    public string Name { get; set; }
}

[PrimaryKey("Id")]
public class MskAnalysis
{
    public int Id { get; set; }
    [Required]
    public int ClassId { get; set; }
    [Required]
    public int CategoryId { get; set; }
    [Required]
    public string Name { get; set; }
    public List<string>? Analogs { get; set; }
}

public class MskAnalysisCreation
{
    public string Type { get; set; }
    public string Class { get; set; }
    public string Category { get; set; }
    public string Name { get; set; }
}

public class MskAnalysisTypeGet
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<MskAnalysisClassGet> Classes { get; set; }
}

public class MskAnalysisClassGet
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<MskAnalysisCategoryGet> Categories { get; set; }
}

public class MskAnalysisCategoryGet
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<MskAnalysisGet> Analyses { get; set; }
}

public class MskAnalysisGet
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public List<string>? Analogs { get; set; }
}

