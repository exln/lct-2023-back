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


    public DbSet<User> Users { get; set; }
    public DbSet<Staff> Staffs { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<Mkb10> MKB10s { get; set; }
    public DbSet<HealthcareService> HealthcareServices { get; set; }
    
    //Services
    public DbSet<Standart> Standarts { get; set; }
    public DbSet<ChapterAccordance> Chapters { get; set; }
    public DbSet<ServiceSectionAccordance> ServiceSections { get; set; }
    public DbSet<ServiceBlockAccordance> ServiceBlocks { get; set; }
    public DbSet<ServiceNumberAccordance> ServiceNumbers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(e => e.Staff)
            .WithOne(e => e.User)
            .HasForeignKey<Staff>(e => e.UserId);
    }
}