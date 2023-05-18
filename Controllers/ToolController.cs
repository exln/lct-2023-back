using System.Text.RegularExpressions;
using MediWingWebAPI.Data;
using MediWingWebAPI.Models;
using Util = MediWingWebAPI.Utils.Utilitas;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
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
    
    [HttpPost("AddServices", Name = "AddServices")]
    public async Task<IActionResult> AddSections([FromBody] List<HealthcareServiceCreation> healthcareServiceCreations)
    {
        foreach (HealthcareServiceCreation healthcareServiceCreation in healthcareServiceCreations)
        {
            HealthcareService healthcareService = Util.ParseServiceCode(healthcareServiceCreation.Code);
            
            if (healthcareServiceCreation.Name != null)
            {
                healthcareService.Name = healthcareServiceCreation.Name;
            }

            if (healthcareService.Subsubnumber != null)
            {
                HealthcareService serviceSubsubnumberAccordance = new()
                {
                    Section = healthcareService.Section,
                    Block = healthcareService.Block,
                    Number = healthcareService.Number,
                    Subnumber = healthcareService.Subnumber,
                    Subsubnumber = healthcareService.Subsubnumber,
                    Name = healthcareService.Name
                };
                
                await _context.HealthcareServices.AddAsync(serviceSubsubnumberAccordance);
            }
            else if (healthcareService.Subnumber != -1)
            {
                HealthcareService serviceSubnumberAccordance = new()
                {
                    Section = healthcareService.Section,
                    Block = healthcareService.Block,
                    Number = healthcareService.Number,
                    Subnumber = healthcareService.Subnumber,
                    Name = healthcareService.Name
                };
                
                await _context.HealthcareServices.AddAsync(serviceSubnumberAccordance);
            }
            else if (healthcareService.Number != -1)
            {
                ServiceNumberAccordance serviceNumberAccordance = new()
                {
                    Section = healthcareService.Section,
                    Block = healthcareService.Block,
                    Number = healthcareService.Number,
                    Name = healthcareService.Name
                };
                
                await _context.ServiceNumbers.AddAsync(serviceNumberAccordance);
            }
            else if (healthcareService.Block != -1)
            {
                ServiceBlockAccordance serviceBlockAccordance = new()
                {
                    Section = healthcareService.Section,
                    Block = healthcareService.Block,
                    Name = healthcareService.Name
                };
                
                await _context.ServiceBlocks.AddAsync(serviceBlockAccordance);
            }
            else
            {
                ServiceSectionAccordance serviceSectionAccordance = new()
                {
                    Section = healthcareService.Section,
                    Name = healthcareService.Name
                };
                
                await _context.ServiceSections.AddAsync(serviceSectionAccordance);
            }
        }
        
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("AddService", Name = "AddService")]
    public async Task<IActionResult> AddSection([FromBody] HealthcareServiceCreation healthcareServiceCreation)
    {
        HealthcareService healthcareService = Util.ParseServiceCode(healthcareServiceCreation.Code);
            
        if (healthcareServiceCreation.Name != null)
        {
            healthcareService.Name = healthcareServiceCreation.Name;
        }

        if (healthcareService.Subsubnumber != null)
        {
            HealthcareService serviceSubsubnumberAccordance = new()
            {
                Section = healthcareService.Section,
                Block = healthcareService.Block,
                Number = healthcareService.Number,
                Subnumber = healthcareService.Subnumber,
                Subsubnumber = healthcareService.Subsubnumber,
                Name = healthcareService.Name
            };
                
            await _context.HealthcareServices.AddAsync(serviceSubsubnumberAccordance);
        }
        else if (healthcareService.Subnumber != null)
        {
            HealthcareService serviceSubnumberAccordance = new()
            {
                Section = healthcareService.Section,
                Block = healthcareService.Block,
                Number = healthcareService.Number,
                Subnumber = healthcareService.Subnumber,
                Name = healthcareService.Name
            };
                
            await _context.HealthcareServices.AddAsync(serviceSubnumberAccordance);
        }
        else if (healthcareService.Number != -1)
        {
            ServiceNumberAccordance serviceNumberAccordance = new()
            {
                Section = healthcareService.Section,
                Block = healthcareService.Block,
                Number = healthcareService.Number,
                Name = healthcareService.Name
            };
                
            await _context.ServiceNumbers.AddAsync(serviceNumberAccordance);
        }
        else if (healthcareService.Block != -1)
        {
            ServiceBlockAccordance serviceBlockAccordance = new()
            {
                Section = healthcareService.Section,
                Block = healthcareService.Block,
                Name = healthcareService.Name
            };
                
            await _context.ServiceBlocks.AddAsync(serviceBlockAccordance);
        }
        else
        {
            ServiceSectionAccordance serviceSectionAccordance = new()
            {
                Section = healthcareService.Section,
                Name = healthcareService.Name
            };
                
            await _context.ServiceSections.AddAsync(serviceSectionAccordance);
        }
        
        await _context.SaveChangesAsync();
        return Ok();
    }
    
    [HttpPost("AddMKBs", Name = "AddMKBs")]
    public async Task<IActionResult> AddMKBs([FromBody] List<MKB10Creation> mkb10Creations)
    {
        foreach (MKB10Creation mkb10creation in mkb10Creations)
        {
            Mkb10 mkb10 = Util.ParseMKBCode(mkb10creation.Code);
            
            if (mkb10creation.Name != null)
            {
                mkb10.Name = mkb10creation.Name;   
            }
            if (mkb10.Subnumber != null)
            {
                _context.MKB10s.AddAsync(mkb10);
            }
            else if (mkb10.Number != -1)
            {
                _context.MKB10s.AddAsync(mkb10);
            }
        }
        
        await _context.SaveChangesAsync();
        return Ok();
    }
    
    [HttpPost("AddMKB", Name = "AddMKB")]
    public async Task<IActionResult> AddMKB([FromBody] MKB10Creation mkb10Creation)
    {
        Mkb10 mkb10 = Util.ParseMKBCode(mkb10Creation.Code);
        if (mkb10Creation.Name != null)
        {
            mkb10.Name = mkb10Creation.Name;   
        }
        if (mkb10.Subnumber != null)
        {
            _context.MKB10s.AddAsync(mkb10);
        }
        else if (mkb10.Number != -1)
        {
            _context.MKB10s.AddAsync(mkb10);
        }

        await _context.SaveChangesAsync();
        return Ok();
    }
    
    
    [HttpPost("AddStandart", Name = "AddStandart")]
    public async Task<IActionResult> AddStandart([FromBody] StandartCreation standartCreation)
    {
        foreach (string mkbcode in standartCreation.Mkb10Codes)
        {
            foreach (string servicecode in standartCreation.HealthcareServiceCodes)
            {
                Standart standart = new()
                {
                    Id = Guid.NewGuid(),
                    Mkb10Code = mkbcode,
                    HealthcareServiceCode = servicecode,
                    IsMandatory = standartCreation.IsMandatory
                };
                await _context.Standarts.AddAsync(standart);
            }
        }
        
        await _context.SaveChangesAsync();

        return Ok();
    }
}