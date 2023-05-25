using Microsoft.EntityFrameworkCore;

namespace MediWingWebAPI.Models;

[PrimaryKey("Id")]
public class MkbStandart
{
    public Guid Id { get; set; }
    public int TagId { get; set; }
    public int SubTagId { get; set; }
    public int TypeId { get; set; }
    public bool IsCapital { get; set; }
    public int AnalysisId { get; set; }
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
    public List<Mkb10EsiliWithBool> Mkb10EsiliWithBools { get; set; }
}

public class Mkb10StandartCreation
{
    public List<Mkb10EsiliWithBool> Mkb10EsiliWithBools { get; set; }
    public List<string> Mkb10Codes { get; set; }
}

public class Mkb10EsiliWithBool
{
    public string EsiliName { get; set; }
    public bool IsMandatory { get; set; }
}

public class Mkb10StandartUpdate
{
    public List<Mkb10EsiliWithBool> Mkb10EsiliWithBools { get; set; }
    public List<string> Mkb10Codes { get; set; }
}