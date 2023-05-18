using System.Text.RegularExpressions;
using MediWingWebAPI.Data;
using MediWingWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediWingWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthcareServiceController: Controller
{
    private readonly ILogger<HealthcareServiceController> _logger;
    private readonly ApiDbContext _context;

    public HealthcareServiceController(
        ILogger<HealthcareServiceController> logger,
        ApiDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("{search}", Name = "SearchHealthcareServices")]
    public async Task<IActionResult> SearchServices(string search)
    {
        int typeCode = -1;
        Regex pattern = new Regex(@"^([A-Za-z]+)(?:(\d{1,2}))?(?:\.(\d{1,3}))?(?:\.(\d{1,3}))?(?:\.(\d{1,3}))?$");
        Match match = pattern.Match(search);
        
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
            List<ServiceSectionAccordance> accordances = await _context.ServiceSections
                .Where(a => a.Section == section)
                .ToListAsync();
            return Ok(accordances);
        }
        if (typeCode == 1)
        {
            List<ServiceBlockAccordance> accordances = await _context.ServiceBlocks
                .Where(a => a.Section == section)
                .Where(a => a.Block == block)
                .ToListAsync();
            return Ok(accordances);
        }
        if (typeCode == 2)
        {
            List<ServiceNumberAccordance> accordances = await _context.ServiceNumbers
                .Where(a => a.Section == section)
                .Where(a => a.Block == block)
                .Where(a => a.Number == number)
                .ToListAsync();
            return Ok(accordances);
        }
        if (typeCode == 3 | typeCode == 4)
        {
            List<HealthcareService> accordances = await _context.HealthcareServices
                .Where(a => a.Section == section)
                .Where(a => a.Block == block)
                .Where(a => a.Number == number)
                .Where(a => a.Subnumber == subnumber)
                .Where(a => a.Subsubnumber == subsubnumber)
                .ToListAsync();
            return Ok(accordances);
        }

        return BadRequest();
    }
}