using MediWingWebAPI.Data;
using MediWingWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Util = MediWingWebAPI.Utils.Utilitas;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MediWingWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class EsiliController: Controller
{
    private ILogger<EsiliController> _logger;
    private ApiDbContext _context;
    
    public EsiliController(
        ILogger<EsiliController> logger,
        ApiDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    // Esili related tools
    [HttpPost("Rus", Name = "AddRusEsilis")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> AddSections([FromBody] List<RusEsiliCreation> rusEsiliCreations)
    {
        foreach (RusEsiliCreation rusEsiliCreation in rusEsiliCreations)
        {
            RusEsili rusEsili = Util.ParseRusEsiliCode(rusEsiliCreation.Code);

            if (rusEsiliCreation.Name != null)
            {
                rusEsili.Name = rusEsiliCreation.Name;
            }

            if (rusEsili.Subsubnumber != null)
            {
                RusEsili newRusEsili = new()
                {
                    Section = rusEsili.Section,
                    Block = rusEsili.Block,
                    Number = rusEsili.Number,
                    Subnumber = rusEsili.Subnumber,
                    Subsubnumber = rusEsili.Subsubnumber,
                    Name = rusEsili.Name
                };

                await _context.RusEsilis.AddAsync(newRusEsili);
            }
            else if (rusEsili.Subnumber != -1)
            {
                RusEsili newRusEsili = new()
                {
                    Section = rusEsili.Section,
                    Block = rusEsili.Block,
                    Number = rusEsili.Number,
                    Subnumber = rusEsili.Subnumber,
                    Name = rusEsili.Name
                };

                await _context.RusEsilis.AddAsync(newRusEsili);
            }
            else if (rusEsili.Number != -1)
            {
                RusEsiliNumber newRusEsiliNumber = new()
                {
                    Section = rusEsili.Section,
                    Block = rusEsili.Block,
                    Number = rusEsili.Number,
                    Name = rusEsili.Name
                };

                await _context.RusEsiliNumbers.AddAsync(newRusEsiliNumber);
            }
            else if (rusEsili.Block != -1)
            {
                RusEsiliBlock newRusEsiliBlock = new()
                {
                    Section = rusEsili.Section,
                    Block = rusEsili.Block,
                    Name = rusEsili.Name
                };

                await _context.RusEsiliBlocks.AddAsync(newRusEsiliBlock);
            }
            else if (rusEsili.Section != -1)
            {
                RusEsiliSection newRusEsiliSection = new()
                {
                    Section = rusEsili.Section,
                    Name = rusEsili.Name
                };

                await _context.RusEsiliSections.AddAsync(newRusEsiliSection);
            }
        }

        await _context.SaveChangesAsync();
        return Ok("Successfully added new RusEsilis");
    }

    [HttpPost(Name = "AddEsilis")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> AddMskEsilis([FromBody] List<MskEsiliCreation> mskEsiliCreations)
    {
        foreach (MskEsiliCreation mskEsiliCreation in mskEsiliCreations)
        {
            MskEsili mskEsil = new MskEsili()
            {
                MskEsiliCode = !mskEsiliCreation.MskEsiliCode.IsNullOrEmpty() ? mskEsiliCreation.MskEsiliCode : null,
                Type = mskEsiliCreation.Type,
                IsChild = mskEsiliCreation.IsChild,
                IdCode = mskEsiliCreation.IdCode.HasValue ? mskEsiliCreation.IdCode.Value : null,
                LdpCode = mskEsiliCreation.LdpCode.HasValue ? mskEsiliCreation.LdpCode.Value : null,
                Name = mskEsiliCreation.Name,
                Ambulatory = mskEsiliCreation.Ambulatory.HasValue ? mskEsiliCreation.Ambulatory.Value : null,
                Stationary = mskEsiliCreation.Stationary.HasValue ? mskEsiliCreation.Stationary.Value : null,
                Modalities = mskEsiliCreation.Modalities.IsNullOrEmpty() ? null : mskEsiliCreation.Modalities,
                Analogs = mskEsiliCreation.Analogs.IsNullOrEmpty() ? null : mskEsiliCreation.Analogs,
            };

            await _context.MskEsilis.AddAsync(mskEsil);
        }

        _context.SaveChangesAsync();

        return Ok("Esilis added successfully");
    }
    
    [HttpPost("Analog", Name = "AddEsiliAnalogs")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> AddEsiliAnalogs([FromBody] List<MskEsiliAnalogCreation> mskEsiliAnalogCreations)
    {
        foreach (MskEsiliAnalogCreation mskEsiliAnalogCreation in mskEsiliAnalogCreations)
        {
            try
            {
                MskEsili mskEsiliByName = _context.MskEsilis
                    .FirstOrDefault(m => m.Name == mskEsiliAnalogCreation.EsiliName);
                
                if (mskEsiliByName == null) return NotFound("Esili not found");
                mskEsiliByName.Analogs.AddRange(mskEsiliAnalogCreation.AnalogNames);
                _context.MskEsilis.Update(mskEsiliByName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        await _context.SaveChangesAsync();

        return Ok("Success");
    }
    
    [HttpGet("Analog", Name = "GetEsiliAnalogs")]
    [ProducesResponseType(typeof(List<MskEsiliAnalogRead>), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IActionResult> GetEsiliAnalogs([FromQuery] string? EsiliName, int limit = 100)
    {
        var mskEsilis = await _context.MskEsilis
            .Where(m => m.Name == EsiliName)
            .ToListAsync();

        if (mskEsilis == null) return NotFound("Esili not found");

        List<MskEsiliAnalogRead> result = new List<MskEsiliAnalogRead>();

        foreach (MskEsili mskEsili in mskEsilis)
        {
            MskEsiliAnalogRead mskEsiliAnalogRead = new MskEsiliAnalogRead()
            {
                EsiliName = mskEsili.Name,
                AnalogNames = mskEsili.Analogs
            };
            result.Add(mskEsiliAnalogRead);
        }

        return Ok(result);
    }
}