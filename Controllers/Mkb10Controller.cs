using System.Text.RegularExpressions;
using MediWingWebAPI.Data;
using MediWingWebAPI.Models;
using Util = MediWingWebAPI.Utils.Utilitas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MediWingWebAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class Mkb10Controller: ControllerBase
{
    private readonly ILogger<Mkb10Controller> _logger;
    private readonly ApiDbContext _context;

    public Mkb10Controller(
        ILogger<Mkb10Controller> logger,
        ApiDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    // Mkb10 related tools
    [HttpPost(Name = "AddMkb10s")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IActionResult> AddMkbs([FromBody] List<Mkb10Creation> mkb10Creations)
    {
        foreach (Mkb10Creation mkb10Creation in mkb10Creations)
        {
            Mkb10 mkb10 = Util.ParseMkb10Code(mkb10Creation.Code);

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
        return Ok("Mkb10s added successfully");
    }
    
    [HttpPost("Standart", Name = "AddStandarts")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IActionResult> AddMsk10Standarts([FromBody] Mkb10StandartCreation mkb10StandartCreation)
    {
        foreach (string mkbcode in mkb10StandartCreation.Mkb10Codes)
        {
            Mkb10 mkb10 = Util.ParseMkb10Code(mkbcode);
            
            foreach (Mkb10EsiliWithBool nameWBool in mkb10StandartCreation.Mkb10EsiliWithBools)
            {
                MkbStandart mkbStandart = new()
                {
                    Mkb10Id = _context.Mkb10s
                        .Where(m => m.Chapter == mkb10.Chapter)
                        .Where(m => m.Litera == mkb10.Litera)
                        .Where(m => m.Number == mkb10.Number)
                        .FirstOrDefault(m => m.Subnumber == mkb10.Subnumber && m.Subnumber != -1)!.Id,
                    AnalysisId = _context.MskEsilis.FirstOrDefault(e => e.Name == nameWBool.EsiliName)!.Id,
                    IsMandatory = nameWBool.IsMandatory
                };
                await _context.Standarts.AddAsync(mkbStandart);
            }
        }

        await _context.SaveChangesAsync();

        return Ok("Standarts added successfully");
    }

    [HttpGet("Standart", Name = "GetStandartsByCode")]
    [ProducesResponseType(typeof(Mkb10StandartRead), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> GetStandarts([FromQuery] string code, int limit = 10)
    {
        int mkb10Id = _context.Mkb10s
            .Where(m => m.Chapter == Util.ParseMkb10Code(code).Chapter)
            .Where(m => m.Litera == Util.ParseMkb10Code(code).Litera)
            .Where(m => m.Number == Util.ParseMkb10Code(code).Number)
            .FirstOrDefault(m => m.Subnumber == Util.ParseMkb10Code(code).Subnumber && m.Subnumber != -1)!.Id;
        
        if (mkb10Id == null) return NotFound("Mkb10 not found");
        
        List<MkbStandart> query = await _context.Standarts
            .Where(m => m.Mkb10Id == mkb10Id)
            .ToListAsync();

        if (query == null)
        {
            return NotFound("Standart not found");
        }

        List<Mkb10EsiliWithBool> serviceWBools = new List<Mkb10EsiliWithBool>();

        foreach (MkbStandart standart in query)
        {
            MskEsili esili = await _context.MskEsilis.FirstOrDefaultAsync(e => e.Id == standart.AnalysisId);
            Mkb10EsiliWithBool service = new()
            {
                EsiliName = esili.Name,
                IsMandatory = standart.IsMandatory
            };
            serviceWBools.Add(service);
        }

        Mkb10StandartRead result = new Mkb10StandartRead()
        {
            Mkb10Code = code,
            Mkb10EsiliWithBools = serviceWBools
        };
        if (result != null) return Ok(result);
        return NotFound("Standart not found");
    }

    [HttpGet("Standart/All", Name = "GetAllStandarts")]
    [ProducesResponseType(typeof(List<Mkb10StandartRead>), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> GetAllStandarts([FromQuery] int limit = 50)
    {
       List<MkbStandart> query = await _context.Standarts
                .ToListAsync();

           if (query == null)
           {
               return NotFound("Standart not found");
           }

           List<Mkb10StandartRead> result = new List<Mkb10StandartRead>();

            foreach (MkbStandart standart in query)
            {
                Mkb10 mkb10 = await _context.Mkb10s.FirstOrDefaultAsync(m => m.Id == standart.Mkb10Id);
                MskEsili esili = await _context.MskEsilis.FirstOrDefaultAsync(e => e.Id == standart.AnalysisId);
                Mkb10StandartRead service = new Mkb10StandartRead()
                {
                    Mkb10Code = Util.GetMkb10FullCode(mkb10),
                    Mkb10EsiliWithBools = new List<Mkb10EsiliWithBool>()
                    {
                        new Mkb10EsiliWithBool()
                        {
                            EsiliName = esili.Name,
                            IsMandatory = standart.IsMandatory
                        }
                    }
                };
                result.Add(service);
            }

            if (result != null) return Ok(result);
        
        return NotFound("Standart not found");
    }
    
    [HttpGet("Chapters", Name = "ChapterList")]
    [ProducesResponseType(typeof(List<Mkb10Chapter>), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> GetChapters()
    {
        List<Mkb10Chapter> chapters = await _context.Mkb10Chapters.ToListAsync();
         if (chapters != null) return Ok(chapters);
         return NotFound("Chapters not found");
        
    }
    
    /*[HttpGet(Name = "GetMKB10s")]
    public async Task<IActionResult> GetMKB10s()
    {
        List<Mkb10> mkb10s = await _context.Mkb10s.ToListAsync();
        List<MKB10Read> mkb10Reads = new();
        foreach (Mkb10 mkb10 in mkb10s)
        {
            if (mkb10.Subnumber != null)
            {
                MKB10Read mkb10Read = new()
                {
                    Code = $"{mkb10.Litera}{mkb10.Number.ToString("D2")}.{mkb10.Subnumber.ToString()}",
                    Name = mkb10.Name,
                };
                mkb10Reads.Add(mkb10Read);
            }
            else
            {
                MKB10Read mkb10Read = new()
                {
                    Code = $"{mkb10.Litera}{mkb10.Number.ToString("D2")}",
                    Name = mkb10.Name,
                };
                mkb10Reads.Add(mkb10Read);
            }
        }

        return Ok(mkb10Reads);
    }*

    /*[HttpPut("{code}", Name = "UpdateMKB10")]
    public async Task<IActionResult> UpdateMKB10(string code, [FromBody] Mkb10Update mkb10Update)
    {
        Mkb10 mkb10 = SearchMkb10(_context, ParseMKB10(code));
        
        if (mkb10 == null)
        {
            return NotFound();
        }
        
        mkb10.Name = mkb10Update.Name;
        await _context.SaveChangesAsync();
        
        return Ok(mkb10);
    }*/
    
    /*[HttpDelete("{code}", Name = "DeleteMKB10")]
    public async Task<IActionResult> DeleteMKB10(string code)
    {
        Mkb10 mkb10 = Utilitas.SearchMkb10(_context, Utilitas.ParseMkb10Code(code));
        
        if (mkb10 == null)
        {
            return NotFound();
        }
        
        _context.Mkb10s.Remove(mkb10);
        await _context.SaveChangesAsync();
        
        return Ok();
    }*/

    [HttpGet(Name = "GetInfoByFullMkb10Code")]
    [ProducesResponseType(typeof(Mkb10), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> GetCodeInfo([FromQuery] string code, int limit = 10)
    {
        int codeType;
        Regex pattern = new Regex(@"^([A-Za-z])(\d{1,2})?(?:\.(\d+))?$");
        Match match = pattern.Match(code);
        
        char litera = match.Groups[1].Value.ToUpper()[0];
        int number = match.Groups[2].Success ? int.Parse(match.Groups[2].Value) : -1;
        int? subnumber = match.Groups[3].Success ? int.Parse(match.Groups[3].Value) : null;

        if (subnumber != null) codeType = 2;
        else if (number != -1) codeType = 1;
        else codeType = 0;

        List<Mkb10> results = null;

        if (codeType == 2)
        {
            results = await _context.Mkb10s
                .Where(m => m.Litera == litera)
                .Where(m => m.Number == number)
                .Where(m => m.Subnumber == subnumber)
                .ToListAsync();
        }
        else if (codeType == 1) 
        { 
            results = await _context.Mkb10s
            .Where(m => m.Litera == litera)
            .Where(m => m.Number == number)
            .Where(m => m.Subnumber == null)
            .ToListAsync();
        }
        else if (codeType == 0)
        {
            results = await _context.Mkb10s
                .Where(m => m.Litera == litera)
                .ToListAsync();
        }

        return !results.IsNullOrEmpty() ? Ok(results.Take((int)limit).ToList()) : NotFound("Mkb10 not found");
    }
        
    [HttpGet("Search", Name="SearchInMkb10")] 
    [ProducesResponseType(typeof(Mkb10), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> SearchMkb10([FromQuery] string search, int limit = 10)
    {
        IQueryable<Mkb10> query = _context.Mkb10s;
        Regex pattern = new Regex(@"^([\p{L}]{3,})?([A-Za-z])?(\d+)?(?:\.(\d+))?$");
        Match match = pattern.Match(search);
        
        string? name  = match.Groups[1].Success ? match.Groups[1].Value : null;
        char? litera = match.Groups[2].Success ? match.Groups[2].Value.ToUpper()[0] : null;
        int? number = match.Groups[3].Success ? int.Parse(match.Groups[3].Value) : null;
        int? subnumber = match.Groups[4].Success ? int.Parse(match.Groups[4].Value) : null;
      
        if (name != null) query = query.Where(m => m.Name.Contains(name));
        if (litera != null) query = query.Where(m => m.Litera == litera);
        if (number != null) query = query.Where(m => m.Number == number);
        if (subnumber != null) query = query.Where(m => m.Subnumber == subnumber);

        if (query.Any()) return Ok(query.Take(limit).ToList());
        return NotFound("Mkb10 not found");
    }

}