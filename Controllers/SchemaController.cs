using MediWingWebAPI.Data;
using MediWingWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace MediWingWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchemaController : Controller
{
    private readonly ILogger<SchemaController> _logger;
    private readonly ApiDbContext _context;

    public SchemaController(
        ILogger<SchemaController> logger,
        ApiDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    [HttpPost("MKB10", Name="GetMKB10Schema")]
    public async Task<IActionResult> AddMKB10([FromBody] Mkb10 mkb10)
    {
        if (mkb10 == null)
        {
            return BadRequest();
        }
        await _context.MKB10s.AddAsync(mkb10);
        await _context.SaveChangesAsync();
        return Ok("MK10 added successfully!");
    }
    
    [HttpPost("ChapterAccordance", Name = "GetChapterAccordanceSchema")]
    public async Task<IActionResult> AddChapterAccordance([FromBody] ChapterAccordance chapterAccordance)
    {
        if (chapterAccordance == null)
        {
            return BadRequest();
        }
        await _context.Chapters.AddAsync(chapterAccordance);
        await _context.SaveChangesAsync();
        return Ok("ChapterAccordance added successfully!");
    }
    
    [HttpPost("HealthcareService", Name="GetHealthcareServiceSchema")]
    public async Task<IActionResult> AddHealthcareService([FromBody] HealthcareService healthcareService)
    {
        if (healthcareService == null)
        {
            return BadRequest();
        }
        await _context.HealthcareServices.AddAsync(healthcareService);
        await _context.SaveChangesAsync();
        return Ok("HealthcareService added successfully!");
    }
    
    [HttpPost("ServiceSectionAccordance", Name = "GetServiceSectionAccordanceSchema")]
    public async Task<IActionResult> AddServiceSectionAccordance([FromBody] ServiceSectionAccordance healthcareServiceSectionAccordance)
    {
        if (healthcareServiceSectionAccordance == null)
        {
            return BadRequest();
        }
        await _context.ServiceSections.AddAsync(healthcareServiceSectionAccordance);
        await _context.SaveChangesAsync();
        return Ok("HealthcareServiceSectionAccordance added successfully!");
    }
    
    [HttpPost("ServiceBlockAccordance", Name = "GetServiceBlockAccordanceSchema")]
    public async Task<IActionResult> AddServiceBlockAccordance([FromBody] ServiceBlockAccordance healthcareServiceBlockAccordance)
    {
        if (healthcareServiceBlockAccordance == null)
        {
            return BadRequest();
        }
        await _context.ServiceBlocks.AddAsync(healthcareServiceBlockAccordance);
        await _context.SaveChangesAsync();
        return Ok("HealthcareServiceBlockAccordance added successfully!");
    }
    
    [HttpPost("ServiceNumberAccordance", Name = "GetServiceNumberAccordanceSchema")]
    public async Task<IActionResult> AddServiceNumberAccordance([FromBody] ServiceNumberAccordance healthcareServiceNumberAccordance)
    {
        if (healthcareServiceNumberAccordance == null)
        {
            return BadRequest();
        }
        await _context.ServiceNumbers.AddAsync(healthcareServiceNumberAccordance);
        await _context.SaveChangesAsync();
        return Ok("HealthcareServiceNumberAccordance added successfully!");
    }
    
    [HttpPost("Standart", Name = "GetStandartSchema")]
    public async Task<IActionResult> AddStandart([FromBody] Standart standart)
    {
        if (standart == null)
        {
            return BadRequest();
        }
        await _context.Standarts.AddAsync(standart);
        await _context.SaveChangesAsync();
        return Ok("Standart added successfully!");
    }
    
    [HttpPost("UserInputRelations", Name = "GetUserInputRelationsSchema")]
    public async Task<IActionResult> AddUserInputRelations([FromBody] UserInputRelation userInputRelation)
    {
        if (userInputRelation == null)
        {
            return BadRequest();
        }
        await _context.UserInputRelations.AddAsync(userInputRelation);
        await _context.SaveChangesAsync();
        return Ok("UserInputRelations added successfully!");
    }

}