namespace MediWingWebAPI.Models;

public class Rus10Standart
{
    public Guid Id { get; set; }
    public string RusEsiliCode { get; set; }
    public string Mkb10Code { get; set; }
    public float Probability { get; set; }
}

public class Rus10StandartRead
{
    public string Mkb10Code { get; set; }
    public List<Rus10EsiliWithProb> RusEsiliCodesWithProbs { get; set; }
}

public class Rus10StandartCreation
{
    public List<Rus10EsiliWithProb> RusEsiliCodesWithProbs { get; set; }
    public List<string> Mkb10Codes { get; set; }
}

public class Rus10EsiliWithProb
{
    public string RusEsiliCode { get; set; }
    public float Probability { get; set; }
}

public class Rus10StandartUpdate
{
    public string RusEsiliCode { get; set; }
    public string Mkb10Code { get; set; }
    public float Probability { get; set; }
}
