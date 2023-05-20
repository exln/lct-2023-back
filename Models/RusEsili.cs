using Microsoft.EntityFrameworkCore;

namespace MediWingWebAPI.Models;

[PrimaryKey("Id")]
public class RusEsili
{
    public int Id { get; set; }
    public char Section { get; set; }
    public int Block { get; set; }
    public int Number { get; set; }
    public int Subnumber { get; set; }
    public int? Subsubnumber { get; set; }
    public string Name { get; set; }
}

public class RusEsiliRead
{
    public string Code { get; set; }
    public string Name { get; set; }
}

public class RusEsiliCreation
{
    public string Code { get; set; }
    public string Name { get; set; }
}

public class RusEsiliUpdate
{
    public string Code { get; set; }
    public string Name { get; set; }
}

[PrimaryKey("Id")]
public class RusEsiliSection
{
    public int Id { get; set; }
    public char Section { get; set; }
    public string Name { get; set; }
}

[PrimaryKey("Id")]
public class RusEsiliBlock
{
    public int Id { get; set; }
    public char Section { get; set; }
    public int Block { get; set; }
    public string Name { get; set; }
}

// TODO Заполинть базу данных
[PrimaryKey("Id")]
public class RusEsiliNumber
{
    public int Id { get; set; }
    public char Section { get; set; }
    public int Block { get; set; }
    public int Number { get; set; }
    public string Name { get; set; }
}