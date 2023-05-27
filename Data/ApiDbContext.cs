using MediWingWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MediWingWebAPI.Data;

public class ApiDbContext: DbContext
{
    public ApiDbContext (DbContextOptions<ApiDbContext> options) : base(options)
    {
        
    }

    public ApiDbContext()
    {
        throw new NotImplementedException();
    }

    // Account related
    public DbSet<User> Users { get; set; }
    public DbSet<Staff> Staffs { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Assignment> Assignments { get; set; }

    //RusEsili
    public DbSet<RusEsili> RusEsilis { get; set; }
    public DbSet<RusEsiliSection> RusEsiliSections { get; set; }
    public DbSet<RusEsiliBlock> RusEsiliBlocks { get; set; }
    public DbSet<RusEsiliNumber> RusEsiliNumbers { get; set; }
    
    //MskEsili
    public DbSet<MskEsili> MskEsilis { get; set; }
    
    //MskAnalysis
    public DbSet<MskAnalysis> MskAnalyses { get; set; }
    public DbSet<MskAnalysisType> MskAnalysisTypes { get; set; }
    public DbSet<MskAnalysisClass> MskAnalysisClasses { get; set; }
    public DbSet<MskAnalysisCategory> MskAnalysisCategories { get; set; }

    //MKB10
    public DbSet<Mkb10> Mkb10s { get; set; }
    public DbSet<Mkb10Chapter> Mkb10Chapters { get; set; }
    
    //MKB10 Standarts
    public DbSet<MkbStandart> Standarts { get; set; }
    public DbSet<StandartTag> StandartTags { get; set; }
    public DbSet<StandartSubTag> StandartSubTags { get; set; }


    //MKB11 TODO
    
    //Inputs
    public DbSet<UserInputRelation> UserInputRelations { get; set; }
    public DbSet<UserDiagnosticInput> UserDiagnosticInputs { get; set; }
    public DbSet<InputError> InputErrors { get; set; }
    
    
    public DbSet<Clinic> Clinics { get; set; }
    public DbSet<Modality> Modalities { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Staff>()
            .HasOne<User>(s => s.User)
            .WithOne(u => u.Staff)
            .HasForeignKey<User>(u => u.StaffId);
        
    }
}