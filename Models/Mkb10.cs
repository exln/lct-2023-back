using Microsoft.EntityFrameworkCore;

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

public class Mkb10Read
{
    public string Code { get; set; }
    public string Name { get; set; }
}

public class Mkb10Creation
{
    public string Code { get; set; }
    public string Name { get; set; }
}

public class Mkb10Update
{
    public string Code { get; set; }
    public string Name { get; set; }
}

[PrimaryKey("Id")]
public class Mkb10Chapter
{
    public int Id { get; set; }
    public string Chapter { get; set; }
    public string Sub { get; set; }
    public string Name { get; set; }
}