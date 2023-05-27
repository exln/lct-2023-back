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

public class Mkb10WCode
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Code { get; set; }
}


public class Mkb10WoChapter
{
    public int Id { get; set; }
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

[PrimaryKey("Id")]
public class MMkb10Group
{
    public int ChapterId { get; set; }
    public int Id { get; set; }
    public int Name { get; set; }
    public string Sub { get; set; }
}

public class Mkb10GroupRead
{
    public int Id { get; set; }
    public string ChapterName { get; set; }
    public string Name { get; set; }
    public string Sub { get; set; }
}

public class ChapterWMkb10Read
{
    public int ChapterId { get; set; }
    public string Chapter { get; set; }
    public string Name { get; set; }
    public List<Mkb10WoChapter> Mkb10s { get; set; }
}