using Microsoft.EntityFrameworkCore;

namespace MediWingWebAPI.Models;

[PrimaryKey("Id")]
public class Msk10Standart
{
    public Guid Id { get; set; }
    public string MskEsiliCode { get; set; }
    public string Mkb10Code { get; set; }
    public bool IsMandatory { get; set; }
}

public class Msk10StandartRead
{
    public string Mkb10Code { get; set; }
    public List<Msk10EsiliWithBool> Msk10EsiliWithBools { get; set; }
}

public class Msk10StandartCreation
{
    public List<Msk10EsiliWithBool> Msk10EsiliWithBools { get; set; }
    public List<string> Mkb10Codes { get; set; }
}

public class Msk10EsiliWithBool
{
    public string MskEsiliCode { get; set; }
    public bool IsMandatory { get; set; }
}

public class StandartUpdate
{
    public string Msk10EsiliCode { get; set; }
    public string Mkb10Code { get; set; }
    public bool IsMandatory { get; set; }
}