using System.ComponentModel;
using MediWingWebAPI.Data;
using MediWingWebAPI.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Util = MediWingWebAPI.Utils.Utilitas;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MediWingWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AnalysisController: Controller
{
    private ILogger<AnalysisController> _logger;
    private ApiDbContext _context;
    
    public AnalysisController(
        ILogger<AnalysisController> logger,
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

    [HttpPost(Name = "AddAnalysis")]
    [Description("Добавить новый анализ")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> AddAnalysis([FromBody] List<MskAnalysisCreation> mskAnalysisCreations)
    {
        foreach (MskAnalysisCreation mskAnalysisCreation in mskAnalysisCreations)
        {
            MskAnalysis mskAnalysis = new()
            {
                Name = mskAnalysisCreation.Name,
                ClassId = _context.MskAnalysisClasses
                    .Where(c => c.Name == mskAnalysisCreation.Class)
                    .Select(c => c.Id).FirstOrDefault(),
                CategoryId =  _context.MskAnalysisCategories
                    .Where(c => c.Name == mskAnalysisCreation.Category)
                    .Select(c => c.Id).FirstOrDefault(),
            };

            await _context.MskAnalyses.AddAsync(mskAnalysis);
        }
        
        await _context.SaveChangesAsync();
        return Ok("Успешно добавили новый анализ!");
    }
    
    [HttpPost("Classes", Name = "AddClasses")]
    [Description("Добавить новый класс анализа")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> AddClasses([FromBody] List<MskAnalysisClassCreation> mskAnalysisClassCreations)
    {
        foreach (MskAnalysisClassCreation mskAnalysisClassCreation in mskAnalysisClassCreations)
        {
            MskAnalysisClass mskAnalysisClass = new()
            {
                Name = mskAnalysisClassCreation.Name,
                AnalysisTypeId = mskAnalysisClassCreation.AnalysisTypeId
            };

            await _context.MskAnalysisClasses.AddAsync(mskAnalysisClass);
        }
        
        await _context.SaveChangesAsync();
        return Ok("Successfully added new MskAnalysisClass");
    }
    
    [HttpPost("Categories", Name = "AddCategories")]
    [Description("Добавить новую категорию анализа")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> AddCategories([FromBody] List<MskAnalysisCategoryCreation> mskAnalysisCategoryCreations)
    {
        foreach (MskAnalysisCategoryCreation mskAnalysisCategoryCreation in mskAnalysisCategoryCreations)
        {
            MskAnalysisCategory mskAnalysisCategory = new()
            {
                Name = mskAnalysisCategoryCreation.Name
            };

            await _context.MskAnalysisCategories.AddAsync(mskAnalysisCategory);
        }
        
        await _context.SaveChangesAsync();
        return Ok("Successfully added new MskAnalysisCategory");
    }

    [HttpGet(Name = "GetAllAnalysesByType")]
    [Description("Получить все анализы по типу анализа")]
    [ProducesResponseType(typeof(List<MskAnalysisTypeGet>), 200)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> GetAllAnalysesByType([FromQuery] string? type)
    {
        List<MskAnalysisTypeGet> result = new List<MskAnalysisTypeGet>();
        if (type == null)
        {
            for (int typeCountId = 1; typeCountId <= _context.MskAnalysisTypes.Count(); typeCountId++)
            {
                string TypeName = _context.MskAnalysisTypes
                    .Where(t => t.Id == typeCountId)
                    .Select(t => t.Name)
                    .FirstOrDefault()
                    .ToString();

                List<MskAnalysisClassGet> mskAnalysisClassGets = new List<MskAnalysisClassGet>();

                List<MskAnalysisClass> Classes = _context.MskAnalysisClasses
                    .Where(c => c.AnalysisTypeId == typeCountId)
                    .ToList();

                foreach (MskAnalysisClass Class in Classes)
                {
                    List<MskAnalysisCategoryGet> mskAnalysisCategoryGets = new List<MskAnalysisCategoryGet>();

                    List<MskAnalysisCategory> Categories = _context.MskAnalysisCategories
                        .ToList();

                    foreach (MskAnalysisCategory Category in Categories)
                    {
                        List<MskAnalysisGet> mskAnalysisGets = new List<MskAnalysisGet>();

                        List<MskAnalysis> Analyses = _context.MskAnalyses
                            .Where(a => a.ClassId == Class.Id)
                            .Where(a => a.CategoryId == Category.Id)
                            .ToList();

                        foreach (MskAnalysis Analysis in Analyses)
                        {
                            MskAnalysisGet mskAnalysisGet = new MskAnalysisGet()
                            {
                                Id = Analysis.Id,
                                Name = Analysis.Name,
                                Analogs = Analysis.Analogs
                            };

                            mskAnalysisGets.Add(mskAnalysisGet);
                        }

                        MskAnalysisCategoryGet mskAnalysisCategoryGet = new MskAnalysisCategoryGet()
                        {
                            Id = Category.Id,
                            Name = Category.Name,
                            Analyses = mskAnalysisGets
                        };

                        if (!mskAnalysisCategoryGet.Analyses.IsNullOrEmpty())
                        {
                            mskAnalysisCategoryGets.Add(mskAnalysisCategoryGet);
                        }
                    }

                    MskAnalysisClassGet mskAnalysisClassGet = new MskAnalysisClassGet()
                    {
                        Id = Class.Id,
                        Name = Class.Name,
                        Categories = mskAnalysisCategoryGets
                    };
                    mskAnalysisClassGets.Add(mskAnalysisClassGet);
                }

                MskAnalysisTypeGet mskAnalysisTypeGet = new MskAnalysisTypeGet()
                {
                    Id = typeCountId,
                    Name = TypeName,
                    Classes = mskAnalysisClassGets
                };

                result.Add(mskAnalysisTypeGet);
            }
        }
        else
        {
            int typeCountId = 0;
            if (type == "laboratory") typeCountId = 1;
            else if (type == "instrumental") typeCountId = 2;

            string TypeName = _context.MskAnalysisTypes
                .Where(t => t.Id == typeCountId)
                .Select(t => t.Name)
                .FirstOrDefault()
                .ToString();

                List<MskAnalysisClassGet> mskAnalysisClassGets = new List<MskAnalysisClassGet>();
            
                List<MskAnalysisClass> Classes = _context.MskAnalysisClasses
                    .Where(c => c.AnalysisTypeId == typeCountId)
                    .ToList();

                foreach (MskAnalysisClass Class in Classes)
                {
                    List<MskAnalysisCategoryGet> mskAnalysisCategoryGets = new List<MskAnalysisCategoryGet>();
                    
                    List<MskAnalysisCategory> Categories = _context.MskAnalysisCategories
                        .ToList();
                    
                    foreach (MskAnalysisCategory Category in Categories)
                    {
                        List<MskAnalysisGet> mskAnalysisGets = new List<MskAnalysisGet>();
                        
                        List<MskAnalysis> Analyses = _context.MskAnalyses
                            .Where(a => a.ClassId == Class.Id)
                            .Where(a => a.CategoryId == Category.Id)
                            .ToList();
                        
                        foreach (MskAnalysis Analysis in Analyses)
                        {
                            MskAnalysisGet mskAnalysisGet = new MskAnalysisGet()
                            {
                                Id = Analysis.Id,
                                Name = Analysis.Name,
                                Analogs = Analysis.Analogs
                            };

                            mskAnalysisGets.Add(mskAnalysisGet);
                        }

                        MskAnalysisCategoryGet mskAnalysisCategoryGet = new MskAnalysisCategoryGet()
                        {
                            Id = Category.Id,
                            Name = Category.Name,
                            Analyses = mskAnalysisGets
                        };
                        
                        if (!mskAnalysisCategoryGet.Analyses.IsNullOrEmpty())
                        {
                            mskAnalysisCategoryGets.Add(mskAnalysisCategoryGet);
                        }
                    }

                    MskAnalysisClassGet mskAnalysisClassGet = new MskAnalysisClassGet()
                    {
                        Id = Class.Id,
                        Name = Class.Name,
                        Categories = mskAnalysisCategoryGets
                    };
                    mskAnalysisClassGets.Add(mskAnalysisClassGet);
                }

                MskAnalysisTypeGet mskAnalysisTypeGet = new MskAnalysisTypeGet()
                {
                    Id = typeCountId,
                    Name = TypeName,
                    Classes = mskAnalysisClassGets
                };

                result.Add(mskAnalysisTypeGet);
        }
            
        return Ok(result);
            
    }
    
    [HttpGet("Search", Name = "SearchAnalyses")]
    [Description("Поиск анализов по названию/части названия или Id")]
    [ProducesResponseType(typeof(List<MskAnalysisGet>), 200)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> SearchAnalyses([FromQuery] string? search, int? id)
    {
        List<MskAnalysisGet> result = new List<MskAnalysisGet>();

        List<MskAnalysis> Analyses = _context.MskAnalyses
            .Where(a => a.Name.Contains(search) || a.Id == id)
            .ToList();
        
        if (Analyses.IsNullOrEmpty())
        {
            return NotFound("Анализы не найдены");
        }

        foreach (MskAnalysis Analysis in Analyses)
        {
            MskAnalysisGet mskAnalysisGet = new MskAnalysisGet()
            {
                Id = Analysis.Id,
                Name = Analysis.Name,
                Analogs = Analysis.Analogs
            };

            result.Add(mskAnalysisGet);
        }

        return Ok(result);
    }

    [HttpPost("Analog", Name = "AddAnalog")]
    public async Task<IActionResult> AddAnalog([FromBody] MskAnalysisAnalogCreation mskAnalysisAnalogCreation)
    {
        MskAnalysis mskAnalysis = _context.MskAnalyses
            .Where(a => a.Id == mskAnalysisAnalogCreation.AnalysisId)
            .FirstOrDefault();

        string analogNew = _context.InputErrors
            .Where(e => e.Id == mskAnalysisAnalogCreation.AnalogGuid)
            .FirstOrDefault().DiagnosisName;
        
        if (mskAnalysis == null) return NotFound("Анализ не найден");

        if (mskAnalysis.Analogs != null)
        {
            foreach (string analogExist in mskAnalysis.Analogs)
            {
                if (analogExist == analogNew) return BadRequest("Аналог уже существует");
                mskAnalysis.Analogs.Add(analogNew);
            }
        }
        if (mskAnalysis.Analogs == null)
        {
            mskAnalysis.Analogs = new List<string>();
            mskAnalysis.Analogs.Add(analogNew);
        }

        string errDef = _context.InputErrors
            .Where(e => e.Id == mskAnalysisAnalogCreation.AnalogGuid)
            .FirstOrDefault().DiagnosisName;
        List<InputError> inputError = _context.InputErrors
            .Where(e => e.DiagnosisName == errDef)
            .ToList();
        
        
        _context.MskAnalyses.Update(mskAnalysis);
        _context.InputErrors.RemoveRange(inputError);
        
        await _context.SaveChangesAsync();
        return Ok("Аналоги добавлены");
    }

    /*[HttpPost(Name = "AddEsilis")]
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
    }*/
    
    /*[HttpPost("Analog", Name = "AddEsiliAnalogs")]
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
                    .FirstOrDefault(m => m.Name == mskEsiliAnalogCreation.AnalysisName);
                
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
    }*/
    
    /*[HttpGet("Analog", Name = "GetEsiliAnalogs")]
    [ProducesResponseType(typeof(List<MskEsiliAnalogRead>), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IActionResult> GetEsiliAnalogs([FromQuery] string? AnalysisName, int limit = 100)
    {
        var mskEsilis = await _context.MskEsilis
            .Where(m => m.Name == AnalysisName)
            .ToListAsync();

        if (mskEsilis == null) return NotFound("Esili not found");

        List<MskEsiliAnalogRead> result = new List<MskEsiliAnalogRead>();

        foreach (MskEsili mskEsili in mskEsilis)
        {
            MskEsiliAnalogRead mskEsiliAnalogRead = new MskEsiliAnalogRead()
            {
                AnalysisName = mskEsili.Name,
                AnalogNames = mskEsili.Analogs
            };
            result.Add(mskEsiliAnalogRead);
        }

        return Ok(result);
    }*/
}