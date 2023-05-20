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
    
    // TODO: Add all schemas
    // Mkb10 related schemas
    [HttpPost("Mkb/Mkb10", Name="GetMkb10Schema")]
    public async Task<IActionResult> AddMkb10([FromBody] Mkb10 mkb10)
    {
        if (mkb10 == null)
        {
            return BadRequest();
        }
        await _context.Mkb10s.AddAsync(mkb10);
        await _context.SaveChangesAsync();
        return Ok("Mkb10 added successfully!");
    }
    
    [HttpPost("Mkb10/Chapter", Name = "GetMkb10ChapterSchema")]
    public async Task<IActionResult> AddMkb10Chapter([FromBody] Mkb10Chapter mkb10Chapter)
    {
        if (mkb10Chapter == null)
        {
            return BadRequest();
        }
        await _context.Mkb10Chapters.AddAsync(mkb10Chapter);
        await _context.SaveChangesAsync();
        return Ok("Mkb10Chapter added successfully!");
    }

    [HttpPost("Mkb10/Standart/Msk", Name = "GetMsk10StandartSchema")]
    public async Task<IActionResult> AddMsk10Standart([FromBody] Msk10Standart msk10Standart)
    {
        if (msk10Standart == null)
        {
            return BadRequest();
        }
        await _context.Msk10Standarts.AddAsync(msk10Standart);
        await _context.SaveChangesAsync();
        return Ok("Msk10Standart added successfully!");
    }
    
    [HttpPost("Mkb10/Standart/Rus", Name = "GetRus10StandartSchema")]
    public async Task<IActionResult> AddRus10Standart([FromBody] Rus10Standart rus10Standart)
    {
        if (rus10Standart == null)
        {
            return BadRequest();
        }
        await _context.Rus10Standarts.AddAsync(rus10Standart);
        await _context.SaveChangesAsync();
        return Ok("Rus10Standart added successfully!");
    }
    
    // Rus Esili related schemas
    [HttpPost("Esili/Rus", Name="GetRusEsiliSchema")]
    public async Task<IActionResult> AddRusEsili([FromBody] RusEsili rusEsili)
    {
        if (rusEsili == null)
        {
            return BadRequest();
        }
        await _context.RusEsilis.AddAsync(rusEsili);
        await _context.SaveChangesAsync();
        return Ok("RusEsili added successfully!");
    }
    
    [HttpPost("Esili/Rus/Section", Name = "GetRussianEsiliSectionSchema")]
    public async Task<IActionResult> AddRusEsiliSection([FromBody] RusEsiliSection rusEsiliSection)
    {
        if (rusEsiliSection == null)
        {
            return BadRequest();
        }
        await _context.RusEsiliSections.AddAsync(rusEsiliSection);
        await _context.SaveChangesAsync();
        return Ok("RusEsiliSection added successfully!");
    }
    
    [HttpPost("Esili/Rus/Block", Name = "GetRusEsiliBlockSchema")]
    public async Task<IActionResult> AddRusEsiliBlock([FromBody] RusEsiliBlock rusEsiliBlock)
    {
        if (rusEsiliBlock == null)
        {
            return BadRequest();
        }
        await _context.RusEsiliBlocks.AddAsync(rusEsiliBlock);
        await _context.SaveChangesAsync();
        return Ok("RussianEsiliBlock added successfully!");
    }
    
    [HttpPost("Esili/Rus/Number", Name = "GetRusEsiliNumberSchema")]
    public async Task<IActionResult> AddRusEsiliNumber([FromBody] RusEsiliNumber rusEsiliNumber)
    {
        if (rusEsiliNumber == null)
        {
            return BadRequest();
        }
        await _context.RusEsiliNumbers.AddAsync(rusEsiliNumber);
        await _context.SaveChangesAsync();
        return Ok("RusEsiliNumber added successfully!");
    }
    
    // Msk Esili related schemas
    [HttpPost("Esili/Msk", Name="GetMskEsiliSchema")]
    public async Task<IActionResult> AddMskEsili([FromBody] MskEsili mskEsili)
    {
        if (mskEsili == null)
        {
            return BadRequest();
        }
        await _context.MskEsilis.AddAsync(mskEsili);
        await _context.SaveChangesAsync();
        return Ok("MskEsili added successfully!");
    }
    
    [HttpPost("Esili/Msk/Type", Name = "GetMskEsiliTypeSchema")]
    public async Task<IActionResult> AddMskEsiliType([FromBody] MskEsiliType mskEsiliType)
    {
        if (mskEsiliType == null)
        {
            return BadRequest();
        }
        await _context.MskEsiliTypes.AddAsync(mskEsiliType);
        await _context.SaveChangesAsync();
        return Ok("MskEsiliType added successfully!");
    }
    
    // Input related schemas
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