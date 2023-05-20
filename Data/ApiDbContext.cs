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
    public DbSet<MskEsiliType> MskEsiliTypes { get; set; }

    //MKB10
    public DbSet<Mkb10> Mkb10s { get; set; }
    public DbSet<Mkb10Chapter> Mkb10Chapters { get; set; }
    
    //MKB10 Standarts
    public DbSet<Msk10Standart> Msk10Standarts { get; set; }
    public DbSet<Rus10Standart> Rus10Standarts { get; set; }

    //MKB11 TODO
    
    //Inputs
    public DbSet<UserInputRelation> UserInputRelations { get; set; }
    public DbSet<UserDiagnosticInput> UserDiagnosticInputs { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(e => e.Staff)
            .WithOne(e => e.User)
            .HasForeignKey<Staff>(e => e.UserId);
    }
}