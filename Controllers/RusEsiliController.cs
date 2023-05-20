using System.Text.RegularExpressions;
using MediWingWebAPI.Data;
using MediWingWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MediWingWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RusEsiliController: Controller
{
    private readonly ILogger<RusEsiliController> _logger;
    private readonly ApiDbContext _context;

    public RusEsiliController(
        ILogger<RusEsiliController> logger,
        ApiDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet(Name = "GetInfoByRusEsiliCode")]
    public async Task<IActionResult> SearchServices([FromQuery] string code, int limit = 10)
    {
        int typeCode;
        Regex pattern = new Regex(@"^([A-Za-z]+)(?:(\d{1,2}))?(?:\.(\d{1,3}))?(?:\.(\d{1,3}))?(?:\.(\d{1,3}))?$");
        Match match = pattern.Match(code);
        
        char section = match.Groups[1].Value[0];
        int? block = match.Groups[2].Success ? int.Parse(match.Groups[2].Value) : null;
        int? number = match.Groups[3].Success ? int.Parse(match.Groups[3].Value) : null;
        int? subnumber = match.Groups[4].Success ? int.Parse(match.Groups[4].Value) : null;
        int? subsubnumber = match.Groups[5].Success ? int.Parse(match.Groups[5].Value) : null;
        
        if (subsubnumber != null) typeCode = 4;
        else if (subnumber != null) typeCode = 3;
        else if (number != null) typeCode = 2;
        else if (block != null) typeCode = 1;
        else typeCode = 0;

        if (typeCode == 0)
        {
            List<RusEsiliSection> accordances = await _context.RusEsiliSections
                .Where(a => a.Section == section)
                .ToListAsync();
            return !accordances.IsNullOrEmpty() ? Ok(accordances.Take(limit).ToList()) : NotFound();
        }
        if (typeCode == 1)
        {
            List<RusEsiliBlock> accordances = await _context.RusEsiliBlocks
                .Where(a => a.Section == section)
                .Where(a => a.Block == block)
                .ToListAsync();
            return !accordances.IsNullOrEmpty() ? Ok(accordances.Take(limit).ToList()) : NotFound();
        }
        if (typeCode == 2)
        {
            List<RusEsiliNumber> accordances = await _context.RusEsiliNumbers
                .Where(a => a.Section == section)
                .Where(a => a.Block == block)
                .Where(a => a.Number == number)
                .ToListAsync();
            return !accordances.IsNullOrEmpty() ? Ok(accordances.Take(limit).ToList()) : NotFound();
        }
        if (typeCode == 3 | typeCode == 4)
        {
            List<RusEsili> accordances = await _context.RusEsilis
                .Where(a => a.Section == section)
                .Where(a => a.Block == block)
                .Where(a => a.Number == number)
                .Where(a => a.Subnumber == subnumber)
                .Where(a => a.Subsubnumber == subsubnumber)
                .ToListAsync();
            return !accordances.IsNullOrEmpty() ? Ok(accordances.Take(limit).ToList()) : NotFound();
        }

        return BadRequest();
    }

    [HttpGet("RusEsili", Name = "SearchInRusEsili")]
    public async Task<IActionResult> SearchHealthcareServices([FromQuery] string search, int limit = 10)
    {
        IQueryable <RusEsili> query = _context.RusEsilis;
        Regex pattern =
            new Regex(@"^([\p{L}]{3,})?([A-Za-z]+)(?:(\d{1,2}))?(?:\.(\d{1,3}))?(?:\.(\d{1,3}))?(?:\.(\d{1,3}))?$");
        Match match = pattern.Match(search);

        string? name = match.Groups[1].Success ? match.Groups[1].Value : null;
        char? section = match.Groups[2].Success ? match.Groups[2].Value[0] : null;
        int? block = match.Groups[3].Success ? int.Parse(match.Groups[2].Value) : null;
        int? number = match.Groups[4].Success ? int.Parse(match.Groups[3].Value) : null;
        int? subnumber = match.Groups[5].Success ? int.Parse(match.Groups[4].Value) : null;
        int? subsubnumber = match.Groups[6].Success ? int.Parse(match.Groups[5].Value) : null;
        
        if (name != null) query = query.Where(a => a.Name.Contains(name));
        if (section != null) query = query.Where(a => a.Section == section);
        if (block != null) query = query.Where(a => a.Block == block);
        if (number != null) query = query.Where(a => a.Number == number);
        if (subnumber != null) query = query.Where(a => a.Subnumber == subnumber);
        if (subsubnumber != null) query = query.Where(a => a.Subsubnumber == subsubnumber);
        
        if (query.Any()) return Ok(query.Take(limit).ToList());
        return NotFound();

    }
}