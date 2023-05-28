using Microsoft.EntityFrameworkCore;

namespace MediWingWebAPI.Models;

public class StatisticsInput
{
    public Guid InputId { get; set; }
}

public class DoctorDepartmentStatisticsOutput
{
    public int TotalDoctors { get; set; }
    public List<DoctorStatistics> DoctorStatistics { get; set; } = new List<DoctorStatistics>();
}

public class DoctorStatisticsOutput
{
    public int TotalDoctors { get; set; }
    public List<DoctorStatistics> DoctorStatistics { get; set; } = new List<DoctorStatistics>();
}

public class DoctorStatistics
{
    public string DoctorPost { get; set; }
    public int TotalRecommendations { get; set; }
    public double CorrectRecommendationRatio { get; set; }
    public int TotalPatients { get; set; }
}

public class PatientStatisticsOutput
{
    public int TotalPatients { get; set; }
    public List<DiagnosisStatistics> PatientStatistics { get; set; } = new List<DiagnosisStatistics>();
}

public class DiagnosisStatistics
{
    public string MkbCode { get; set; }
    public string Diagnosis { get; set; }
    public int TotalPatients { get; set; }
    public int AverageAge { get; set; }
    public int CorrectRecommendationRatio { get; set; }
}

public class RecommendationStatisticsOutput
{
    public List<RecommendationTypeStatistics> RecommendationTypeStatistics { get; set; } = new List<RecommendationTypeStatistics>();
}

public class RecommendationTypeStatistics
{
    public string RecommendationType { get; set; }
    public int TotalRecommendations { get; set; }
}

