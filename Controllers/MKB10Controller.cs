using System.Text.RegularExpressions;
using MediWingWebAPI.Data;
using MediWingWebAPI.Models;
using MediWingWebAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediWingWebAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class MKB10Controller: ControllerBase
{
    private readonly ILogger<MKB10Controller> _logger;
    private readonly ApiDbContext _context;

    public MKB10Controller(
        ILogger<MKB10Controller> logger,
        ApiDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    [HttpGet(Name = "GetMKB10s")]
    public async Task<IActionResult> GetMKB10s()
    {
        List<Mkb10> mkb10s = await _context.MKB10s.ToListAsync();
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
    }

    /*[HttpPut("{code}", Name = "UpdateMKB10")]
    public async Task<IActionResult> UpdateMKB10(string code, [FromBody] MKB10Update mkb10Update)
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
    
    [HttpDelete("{code}", Name = "DeleteMKB10")]
    public async Task<IActionResult> DeleteMKB10(string code)
    {
        Mkb10 mkb10 = Utilitas.SearchMKB10(_context, Utilitas.ParseMKBCode(code));
        
        if (mkb10 == null)
        {
            return NotFound();
        }
        
        _context.MKB10s.Remove(mkb10);
        await _context.SaveChangesAsync();
        
        return Ok();
    }
    
    [HttpGet("{search}")]
    public async Task<IActionResult> Get(string search)
    {
        int codeType = -1;
        Regex pattern = new Regex(@"^([A-Za-z])(?:\d+)?(?:\.(\d+))?$");
        Match match = pattern.Match(search);
        
        char litera = match.Groups[1].Value.ToUpper()[0];
        int? number = match.Groups[2].Success ? int.Parse(match.Groups[2].Value) : null;
        int? subnumber = match.Groups[3].Success ? int.Parse(match.Groups[3].Value) : null;

        if (subnumber != null) codeType = 2;
        else if (number == -1) codeType = 1;
        else codeType = 0;

        List<Mkb10> results = null;
        
        if (codeType == 0)
        {
            results = await _context.MKB10s
                .Where(m => m.Litera == litera)
                .ToListAsync();
        }
        else if (codeType == 1 | codeType == 2) 
        { 
            results = await _context.MKB10s
            .Where(m => m.Litera == litera)
            .Where(m => number == null || m.Number == number)
            .Where(m => subnumber == null|| m.Subnumber == subnumber)
            .ToListAsync();
        }
        return Ok(results);
    }
}