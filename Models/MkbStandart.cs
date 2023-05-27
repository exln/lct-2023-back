using Microsoft.EntityFrameworkCore;

namespace MediWingWebAPI.Models;

[PrimaryKey("Id")]
public class MkbStandart
{
    public Guid Id { get; set; }
    //public int TagId { get; set; }
    //public int SubTagId { get; set; }
    public int TypeId { get; set; }
    public bool IsCapital { get; set; }
    public int EsiliId { get; set; }
    public int Mkb10Id { get; set; }
    public bool IsMandatory { get; set; }
}

[PrimaryKey("Id")]
public class StandartTag 
{
    public int Id { get; set; }
    public string Name { get; set; }
}

[PrimaryKey("Id")]
public class StandartSubTag
{
    public int Id { get; set; }
    public int TagId { get; set; }
    public string Name { get; set; }
}

public class Mkb10StandartRead
{
    public string Mkb10Code { get; set; }
    public List<AnalysisWithBool> Mkb10EsiliWithBools { get; set; }
}

public class Mkb10StandartCreation
{
    //public string Tag { get; set; }
    //public string SubTag { get; set; }
    public List<AnalysisIdWithBool> AnalysesWBools { get; set; }
    public List<string> Mkb10Codes { get; set; }
}

public class AnalysisWithBool
{
    public string AnalysisName { get; set; }
    public bool IsMandatory { get; set; }
}

public class AnalysisIdWithBool
{
    public int AnalysisId { get; set; }
    public bool IsMandatory { get; set; }
}

public class Mkb10StandartUpdate
{
    public List<AnalysisWithBool> Mkb10EsiliWithBools { get; set; }
    public List<string> Mkb10Codes { get; set; }
}