using Microsoft.EntityFrameworkCore;

namespace MediWingWebAPI.Models;

/*
 * diagnosis = {
                "Name": match.group(1) + " " + match.group(2),
                "Adress": match.group(6),
                "Filial": match.group(3) if match.group(3) else None,
                "DiffName": match.group(4) + match.group(5) if match.group(4) else None,
                "RateGeneral": row["Общий"],
                "RateProfes": row["Профессионализм"],
                "RateKind": row["Доброжелательность"],
                "RateTeam": row["Командная работа"],
                "RateTrust": row["Доверие"],
                "RatePatient": row["Пациентоориентированность"],
                "RateRespect": row["Уважение"]
            }
 */
[PrimaryKey("Id")]
public class Clinic
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string? Filial { get; set; }
    public string? DiffName { get; set; }
    public float RateGeneral { get; set; }
    public float RateProfes { get; set; }
    public float RateKind { get; set; }
    public float RateTeam { get; set; }
    public float RateTrust { get; set; }
    public float RatePatient { get; set; }
    public float RateRespect { get; set; }
}

public class ClininicReadShort
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string? Filial { get; set; }
    public string? DiffName { get; set; }
}
public class ClinicCreation {
    public string Name { get; set; }
    public string Adress { get; set; }
    public string? Filial { get; set; }
    public string? DiffName { get; set; }
    public float RateGeneral { get; set; }
    public float RateProfes { get; set; }
    public float RateKind { get; set; }
    public float RateTeam { get; set; }
    public float RateTrust { get; set; }
    public float RatePatient { get; set; }
    public float RateRespect { get; set; }
}

public class ClinicUpdate {
    public string Name { get; set; }
    public string Adress { get; set; }
    public string? Filial { get; set; }
    public string? DiffName { get; set; }
    public float RateGeneral { get; set; }
    public float RateProfes { get; set; }
    public float RateKind { get; set; }
    public float RateTeam { get; set; }
    public float RateTrust { get; set; }
    public float RatePatient { get; set; }
    public float RateRespect { get; set; }
}

public class ClinicRead {
    public string Name { get; set; }
    public string Adress { get; set; }
    public string? Filial { get; set; }
    public string? DiffName { get; set; }
    public float RateGeneral { get; set; }
    public float RateProfes { get; set; }
    public float RateKind { get; set; }
    public float RateTeam { get; set; }
    public float RateTrust { get; set; }
    public float RatePatient { get; set; }
    public float RateRespect { get; set; }
}

public class ClinicGet {
    public string Name { get; set; }
    public string Adress { get; set; }
    public string? Filial { get; set; }
    public string? DiffName { get; set; }
    public float RateGeneral { get; set; }
    public float RateProfes { get; set; }
    public float RateKind { get; set; }
    public float RateTeam { get; set; }
    public float RateTrust { get; set; }
    public float RatePatient { get; set; }
    public float RateRespect { get; set; }
}
