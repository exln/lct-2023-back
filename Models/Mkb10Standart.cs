using Microsoft.EntityFrameworkCore;

namespace MediWingWebAPI.Models;

[PrimaryKey("Id")]
public class Mkb10Standart
{
    public Guid Id { get; set; }
    public string EsiliName { get; set; }
    public string Mkb10Code { get; set; }
    public bool IsMandatory { get; set; }
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