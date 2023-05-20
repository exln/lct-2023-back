using System.Text.RegularExpressions;
using MediWingWebAPI.Data;
using MediWingWebAPI.Models;
using MediWingWebAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MediWingWebAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
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
        Mkb10 mkb10 = SearchMKB10(_context, ParseMKB10(code));
        
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
        Mkb10 mkb10 = Utilitas.SearchMKB10(_context, Utilitas.ParseMKBCode(code));
        
        if (mkb10 == null)
        {
            return NotFound();
        }
        
        _context.Mkb10s.Remove(mkb10);
        await _context.SaveChangesAsync();
        
        return Ok();
    }*/

    [HttpGet(Name = "GetInfoByMkb10Code")]
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

        return !results.IsNullOrEmpty() ? Ok(results.Take((int)limit).ToList()) : NotFound();
    }
    
    [HttpGet("Diagnosis", Name="SearchInMkb10Diagnosis")]
    //public async Task<IActionResult> SearchMkb10([FromQuery] char? litera, string? chapter, int? number, int? subnumber, string? name, int limit = 10)
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
        return NotFound();
    }

}