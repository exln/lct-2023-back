using Microsoft.EntityFrameworkCore;

namespace MediWingWebAPI.Models;

public class StatisticsInput
{
    public Guid InputId { get; set; }
}

public class StatisticsOutput
{
    public Guid InputId { get; set; }
    public int TotalPatients { get; set; }
    public List<DiagnosisStatistics> PatientStatistics { get; set; } = new List<DiagnosisStatistics>();
    public int TotalDoctors { get; set; }
    public List<DoctorStatistics> DoctorStatistics { get; set; } = new List<DoctorStatistics>();
    public List<RecommendationTypeStatistics> RecommendationTypeStatistics { get; set; } = new List<RecommendationTypeStatistics>();
    public List<DepartmentStatistics> DepartmentStatistics { get; set; } = new List<DepartmentStatistics>();
}

public class DiagnosisStatistics
{
    public string MkbCode { get; set; }
    public string Diagnosis { get; set; }
    public int TotalPatients { get; set; }
    public double AverageAge { get; set; }
    public double? CorrectRecommendationRatio { get; set; }
}

public class DoctorStatistics
{
    public string DoctorPost { get; set; }
    public int TotalRecommendations { get; set; }
    public double CorrectRecommendationRatio { get; set; }
}

public class RecommendationTypeStatistics
{
    public string RecommendationType { get; set; }
    public int TotalRecommendations { get; set; }
}

public class DepartmentStatistics
{
    public string Department { get; set; }
    public int TotalPatients { get; set; }
}
