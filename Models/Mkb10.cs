using Microsoft.EntityFrameworkCore;
// TODO: Переименовать все в Mkb10+что-то
namespace MediWingWebAPI.Models;
[PrimaryKey("Id")]
public class Mkb10
{
    public int Id { get; set; }
    public string Chapter { get; set; } 
    public char Litera { get; set; }
    public int Number { get; set; }
    public int? Subnumber { get; set; } 
    public string Name { get; set; }
}

public class MKB10Read
{
    public string Code { get; set; }
    public string Name { get; set; }
}

public class MKB10Creation
{
    public string Code { get; set; }
    public string Name { get; set; }
}

public class MKB10Update
{
    public string Code { get; set; }
    public string Name { get; set; }
}

[PrimaryKey("Name")]
public class ChapterAccordance
{
    public string Chapter { get; set; }
    public string Sub { get; set; }
    public string Name { get; set; }
}