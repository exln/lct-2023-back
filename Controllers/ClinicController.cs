using MediWingWebAPI.Data;
using MediWingWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MediWingWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ClinicController : Controller
{
    private ILogger<ClinicController> _logger;
    private ApiDbContext _context;

    public ClinicController(
        ILogger<ClinicController> logger,
        ApiDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpPost(Name = "AddClinics")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> AddClinics([FromBody] List<ClinicCreation> clinicCreations)
    {
        foreach (ClinicCreation clinicCreation in clinicCreations)
        {

            Clinic clinic = new()
            {
                Name = clinicCreation.Name,
                Address = clinicCreation.Adress,
                Filial = clinicCreation.Filial,
                DiffName = clinicCreation.DiffName,
                RateGeneral = clinicCreation.RateGeneral,
                RateProfes = clinicCreation.RateProfes,
                RateKind = clinicCreation.RateKind,
                RateTeam = clinicCreation.RateTeam,
                RateTrust = clinicCreation.RateTrust,
                RatePatient = clinicCreation.RatePatient,
                RateRespect = clinicCreation.RateRespect
            };

            await _context.Clinics.AddAsync(clinic);
        }

        _context.SaveChanges();

        return Ok();
    }
    
    [HttpGet(Name = "GetClinicsShort")]
    [ProducesResponseType(typeof(List<ClininicReadShort>), 200)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> GetClinics([FromQuery] int limit = 30)
    {
        List<Clinic> clinics = await _context.Clinics
            .OrderByDescending(clinic => clinic.RateGeneral)
            .ToListAsync();
        List<ClininicReadShort> result = new();
        
        foreach (Clinic clinic in clinics)
        {
            ClininicReadShort clinicReadShort = new()
            {
                Id = clinic.Id,
                Name = clinic.Name,
                Address = clinic.Address,
                Filial = clinic.Filial,
                DiffName = clinic.DiffName
            };
            
            result.Add(clinicReadShort);
        }

        return Ok(result.Take(limit));
    }
    
    [HttpGet("Search", Name = "SearchClinicsByName")]
    [ProducesResponseType(typeof(List<ClininicReadShort>), 200)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> SearchClinicsByName([FromQuery] string? name, int limit = 10)
    {
        List<Clinic> clinics;
        if (name.IsNullOrEmpty())
        {
            clinics = await _context.Clinics
                .OrderByDescending(clinic => clinic.RateGeneral)
                .ToListAsync();
        }
        else
        {
            clinics = await _context.Clinics
                .Where(clinic => clinic.Name.Contains(name))
                .OrderByDescending(clinic => clinic.RateGeneral)
                .ToListAsync();
        }

        List<ClininicReadShort> result = new();
        foreach (Clinic clinic in clinics)
        {
            ClininicReadShort clinicReadShort = new()
            {
                Id = clinic.Id,
                Name = clinic.Name,
                Address = clinic.Address,
                Filial = clinic.Filial,
                DiffName = clinic.DiffName
            };
            
            result.Add(clinicReadShort);
        }

        return Ok(result.Take(limit));
    }
}