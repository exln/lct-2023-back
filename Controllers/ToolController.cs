using System.Text.RegularExpressions;
using MediWingWebAPI.Data;
using MediWingWebAPI.Models;
using Util = MediWingWebAPI.Utils.Utilitas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ToolController: ControllerBase
{
    private readonly ILogger<ToolController> _logger;
    private readonly ApiDbContext _context;

    public ToolController(
        ILogger<ToolController> logger,
        ApiDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    // Mkb10 related tools
    [HttpPost("Mkb10", Name = "AddMkb10s")]
    public async Task<IActionResult> AddMkbs([FromBody] List<Mkb10Creation> mkb10Creations)
    {
        foreach (Mkb10Creation mkb10Creation in mkb10Creations)
        {
            Mkb10 mkb10 = Util.ParseMKBCode(mkb10Creation.Code);
            
            if (mkb10Creation.Name != null)
            {
                mkb10.Name = mkb10Creation.Name;   
            }
            if (mkb10.Subnumber != null)
            {
                _context.Mkb10s.AddAsync(mkb10);
            }
            else if (mkb10.Number != -1)
            {
                _context.Mkb10s.AddAsync(mkb10);
            }
        }
        
        await _context.SaveChangesAsync();
        return Ok();
    }
    
    [HttpPost("Mkb10/Standart/Rus", Name = "AddRus10Standarts")]
    public async Task<IActionResult> AdRus10dStandarts([FromBody] Rus10StandartCreation rus10StandartCreation)
    {
        foreach (string mkbcode in rus10StandartCreation.Mkb10Codes)
        {
            foreach (Rus10EsiliWithProb codeWProb in rus10StandartCreation.RusEsiliCodesWithProbs)
            {
                Rus10Standart rus10Standart = new()
                {
                    Id = Guid.NewGuid(),
                    Mkb10Code = mkbcode,
                    RusEsiliCode = codeWProb.RusEsiliCode,
                    Probability = codeWProb.Probability
                };
                await _context.Rus10Standarts.AddAsync(rus10Standart);
            }
        }
        
        await _context.SaveChangesAsync();

        return Ok();
    }
    
    [HttpPost("Mkb10/Standart/Msk", Name = "AddMsk10Standarts")]
    public async Task<IActionResult> AddMsk10Standarts([FromBody] Msk10StandartCreation msk10StandartCreation)
    {
        foreach (string mkbcode in msk10StandartCreation.Mkb10Codes)
        {
            foreach (Msk10EsiliWithBool codeWBool in msk10StandartCreation.Msk10EsiliWithBools)
            {
                Msk10Standart msk10Standart = new()
                {
                    Id = Guid.NewGuid(),
                    Mkb10Code = mkbcode,
                    MskEsiliCode = codeWBool.MskEsiliCode,
                    IsMandatory = codeWBool.IsMandatory
                };
                await _context.Msk10Standarts.AddAsync(msk10Standart);
            }
        }
        
        await _context.SaveChangesAsync();

        return Ok();
    }
    
    [HttpGet("Mkb10/Standart", Name = "GetStandartsByCode")]
    public async Task<IActionResult> GetStandarts([FromQuery] string code, string local = "Msk", int limit = 10)
    {
        if (local == "Msk")
        {
            List<Msk10Standart> query = await _context.Msk10Standarts
                .Where(s => s.Mkb10Code == code)
                .ToListAsync();

            if (query == null)
            {
                return NotFound();
            }

            List<Msk10EsiliWithBool> serviceWBools = new List<Msk10EsiliWithBool>();

            foreach (Msk10Standart standart in query)
            {
                Msk10EsiliWithBool service = new()
                {
                    MskEsiliCode = standart.MskEsiliCode,
                    IsMandatory = standart.IsMandatory
                };
                serviceWBools.Add(service);
            }

            Msk10StandartRead result = new Msk10StandartRead()
            {
                Mkb10Code = code,
                Msk10EsiliWithBools = serviceWBools
            };
            if (result != null) return Ok(result);
        }
        else if (local == "Rus") 
        {
            List<Rus10Standart> query = await _context.Rus10Standarts
                .Where(s => s.Mkb10Code == code)
                .ToListAsync();

            if (query == null)
            {
                return NotFound();
            }

            List<Rus10EsiliWithProb> serviceWProbs = new List<Rus10EsiliWithProb>();

            foreach (Rus10Standart standart in query)
            {
                Rus10EsiliWithProb service = new()
                {
                    RusEsiliCode = standart.RusEsiliCode,
                    Probability = standart.Probability
                };
                serviceWProbs.Add(service);
            }

            Rus10StandartRead result = new Rus10StandartRead()
            {
                Mkb10Code = code,
                RusEsiliCodesWithProbs = serviceWProbs
            };
            if (result != null) return Ok(result);
        }
        return NotFound();
    }

    [HttpGet("Mkb10/Standart/All", Name = "GetAllStandarts")]
    public async Task<IActionResult> GetAllStandarts([FromQuery] string local = "Msk", int limit = 50)
    {
        if (local == "Msk")
        {
            List<Msk10Standart> query = await _context.Msk10Standarts
                .ToListAsync();

            if (query == null)
            {
                return NotFound();
            }

            List<Msk10StandartRead> result = new List<Msk10StandartRead>();

            foreach (Msk10Standart standart in query)
            {
                Msk10StandartRead service = new Msk10StandartRead()
                {
                    Mkb10Code = standart.Mkb10Code,
                    Msk10EsiliWithBools = new List<Msk10EsiliWithBool>()
                    {
                        new Msk10EsiliWithBool()
                        {
                            MskEsiliCode = standart.MskEsiliCode,
                            IsMandatory = standart.IsMandatory
                        }
                    }
                };
                result.Add(service);
            }
            if (result != null) return Ok(result);
        }
        else if (local == "Rus") 
        {
            List<Rus10Standart> query = await _context.Rus10Standarts
                .ToListAsync();

            if (query == null)
            {
                return NotFound();
            }

            List<Rus10StandartRead> result = new List<Rus10StandartRead>();

            foreach (Rus10Standart standart in query)
            {
                Rus10StandartRead service = new Rus10StandartRead()
                {
                    Mkb10Code = standart.Mkb10Code,
                    RusEsiliCodesWithProbs = new List<Rus10EsiliWithProb>()
                    {
                        new Rus10EsiliWithProb()
                        {
                            RusEsiliCode = standart.RusEsiliCode,
                            Probability = standart.Probability
                        }
                    }
                };
                result.Add(service);
            }
            if (result != null) return Ok(result);
        }
        return NotFound();
    }
    
    
}