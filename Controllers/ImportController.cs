

using System.Collections.Immutable;
using System.Globalization;
using MediWingWebAPI.Data;
using MediWingWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Util = MediWingWebAPI.Utils.Utilitas;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Controls;

namespace MediWingWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ImportController : Controller
{
    private readonly ILogger<ImportController> _logger;
    private readonly ApiDbContext _context;

    public ImportController(
        ILogger<ImportController> logger,
        ApiDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpPost(Name = "ImportXLSX")]
    [ProducesResponseType(typeof(UserInputRelationRead), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IActionResult> ImportXLSX(IFormFile? file, int? name)
    {
        // Проверяем, что файл был успешно загружен
        if (file == null || file.Length == 0) return BadRequest("Файл не найден");

        string fileNameWithoutExtension;
        // Получаем имя файла
        if (name == null)
        {
            fileNameWithoutExtension =
                Path.GetFileNameWithoutExtension(file.FileName);
        }
        else
        {
            fileNameWithoutExtension = _context.Clinics.FirstOrDefault(c => c.Id == name).Name;
        }

        fileNameWithoutExtension += DateTime.Now.ToString("yyyyMMddHHmmss");

        // Определяем путь к директории, если она не существует - создаем ее
        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Сохраняем файл
        var filePath = Path.Combine(directoryPath, fileNameWithoutExtension + ".xlsx");
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        UserInputRelation userInputRelation = new UserInputRelation()
        {
            InputName = fileNameWithoutExtension,
            InputId = Guid.NewGuid(),
            UserId = Guid.Parse("00000000-0000-0000-0000-000000000000")
        };

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            var worksheet = package.Workbook.Worksheets.First();
            int rowCount = worksheet.Dimension.Rows;
            List<UserDiagnosticInput> list = new List<UserDiagnosticInput>();
            
            List<InputError> inputErrors = new List<InputError>();
            for (int row = 2; row <= rowCount; row++)
            {
                var recomendation = worksheet.Cells[row, 8].Value.ToString();
                List<string> recom = ProcessInputString(recomendation);
                  //  recomendation.Split(new[] { "\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

               var input = new UserDiagnosticInput()
                {
                    InputId = userInputRelation.InputId,
                    Sex = worksheet.Cells[row, 1].Value.ToString(),
                    BirthDate = DateOnly.ParseExact(worksheet.Cells[row, 2].Value.ToString(), "dd.MM.yyyy", CultureInfo.InvariantCulture),
                    PatientId = Convert.ToInt32(worksheet.Cells[row, 3].Value.ToString()),
                    MKBCode = worksheet.Cells[row, 4].Value.ToString(),
                    Diagnosis = worksheet.Cells[row, 5].Value.ToString(),
                    Date = DateOnly.ParseExact(worksheet.Cells[row, 6].Value.ToString(), "dd.MM.yyyy", CultureInfo.InvariantCulture),
                    DoctorPost = worksheet.Cells[row, 7].Value.ToString(),
                    Recommendations = recom
                };
                list.Add(input);
                foreach (string rec in input.Recommendations)
                {
                    if (!DiagnosisCheck(rec, _context))
                    {
                        var error = new InputError()
                        {
                            Id = Guid.NewGuid(),
                            InputId = input.InputId,
                            DiagnosisName = rec,
                        };
                        if (inputErrors.Where(e => e.DiagnosisName == error.DiagnosisName).ToList().IsNullOrEmpty())
                        {
                            inputErrors.Add(error);
                        }
                    }   
                }
            }
            if (!inputErrors.IsNullOrEmpty()) await _context.InputErrors.AddRangeAsync(inputErrors);
            await _context.UserDiagnosticInputs.AddRangeAsync(list);
        }
        
        await _context.UserInputRelations.AddAsync(userInputRelation);
        await _context.SaveChangesAsync();
        
        // Удаляем файл
        System.IO.File.Delete(filePath);
        List<InputError> ers = await _context.InputErrors
            .Where(e => e.InputId == userInputRelation.InputId)
            .ToListAsync();
        List<InputErrorRead> ersRead = new List<InputErrorRead>();
        foreach (InputError error in ers)
        {
            InputErrorRead inputErrorRead = new InputErrorRead()
            {
                Id = error.Id,
                DiagnosisName = error.DiagnosisName
            };
            ersRead.Add(inputErrorRead);
        }
        UserInputRelationRead userInputRelationRead = new UserInputRelationRead()
        {
            InputId = userInputRelation.InputId,
            InputName = userInputRelation.InputName,
            MissingNames = ersRead
        };
        userInputRelationRead.InputName = userInputRelationRead.InputName.Substring(0, userInputRelationRead.InputName.Length - 14);
        return Ok(userInputRelationRead);
    }
    
    [HttpGet("{guid}", Name="SearchInput")]
    [ProducesResponseType(typeof(UserDiagnosticInputGet), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> SearchInput(Guid guid, [FromQuery] string filter = "", int startElement = 0, int count = 0)
    {
        List<UserDiagnosticInput> inputs = await _context.UserDiagnosticInputs
            .Where(x => x.InputId == guid)
            .ToListAsync();
        if (inputs.IsNullOrEmpty())
        {
            return NotFound();
        }

        List<UserDiagnosticInputRead> userDiagnosticInputReads = new List<UserDiagnosticInputRead>();

        foreach (UserDiagnosticInput input in inputs)
        {
            UserDiagnosticInputRead userDiagnosticInputRead = new UserDiagnosticInputRead()
            {
                Id = input.Id,
                Sex = input.Sex,
                BirthDate = input.BirthDate,
                PatientId = input.PatientId,
                MKBCode = input.MKBCode,
                Diagnosis = input.Diagnosis,
                Date = input.Date,
                DoctorPost = input.DoctorPost,
                Recommendations = input.Recommendations
            };
            
            userDiagnosticInputReads.Add(userDiagnosticInputRead);
        }
        
        if (filter.IsNullOrEmpty())
        {
            userDiagnosticInputReads = userDiagnosticInputReads
                .OrderBy(x => x.Date)
                .ToList();
        }
        else
        {
            var propertyInfo = typeof(UserDiagnosticInputRead).GetProperty(filter);
            if (propertyInfo != null)
            {
                userDiagnosticInputReads = userDiagnosticInputReads
                    .OrderBy(x => propertyInfo.GetValue(x))
                    .ToList();
            }
        }

        UserInputRelation InputDb = _context.UserInputRelations
            .Where(u => u.InputId == guid)
            .FirstOrDefault();
        
        string InputName = InputDb.InputName;
        
        if (count == 0)
        {
            count = userDiagnosticInputReads.Count;
        }

        UserDiagnosticInputGet result = new UserDiagnosticInputGet()
        {
            Id = guid,
            Name = InputName.Substring(0, InputName.Length - 14),
            CreationDate = DateTime.ParseExact(InputName.Substring(InputName.Length - 14), "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None),
            InputDatas = userDiagnosticInputReads.Skip(startElement).Take(count).ToList()
        };
        
        return Ok(result);
    }
    
    [HttpDelete("{guid}", Name="DeleteInput")]
    public async Task<IActionResult> DeleteInput(Guid guid)
    {
        var inputs = await _context.UserDiagnosticInputs
            .Where(x => x.InputId == guid)
            .ToListAsync();
        if (inputs.IsNullOrEmpty())
        {
            return NotFound();
        }
        _context.UserDiagnosticInputs.RemoveRange(inputs);
        _context.UserInputRelations.RemoveRange(_context.UserInputRelations.Where(x => x.InputId == guid));
        await _context.SaveChangesAsync();
        return Ok();
    }
    
    [HttpGet("LastRequests")]
    [ProducesResponseType(typeof(List<Dictionary<Guid, string>>), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public IActionResult GetLastRequests([FromQuery] int limit = 10)
    {
        if(limit == 0)
        {
            limit = 10;
        }
        var lastRequests = _context.UserInputRelations
            .Take(limit)
            .ToList();
        if (lastRequests.IsNullOrEmpty())
        {
            return NotFound();
        }

        List<Dictionary<Guid, string>> result = new List<Dictionary<Guid, string>>();
        foreach (var requset in lastRequests)
        {
            result.Add(new Dictionary<Guid, string>()
            {
                {requset.InputId, requset.InputName}
            });
        }
        return Ok(lastRequests);
    }

    
    // TODO Поиск по данным
    /*[HttpGet("Data", Name = "SearchData")]
    public async Task<IActionResult> SearchData([FromQuery] string search, int limit = 10)
    {
        
        var inputs = await _context.UserDiagnosticInputs
            .ToListAsync();
        if (inputs.IsNullOrEmpty())
        {
            return NotFound();
        }
        return Ok(inputs);
    }*/
    
    [HttpDelete("Data/{id}", Name="DeleteData")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> DeleteData(int id)
    {
        var input = await _context.UserDiagnosticInputs
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        if (input == null)
        {
            return NotFound("Данные не найдены");
        }
        _context.UserDiagnosticInputs.Remove(input);
        await _context.SaveChangesAsync();
        return Ok("Данные удалены");
    }

    static List<string> ProcessInputString(string inputString)
    {
        List<string> outputList = new List<string>();
    
        string[] lines = inputString.Split('\n');
        foreach(string line in lines)
        {
            string trimmedLine = line.Trim();
            if(!string.IsNullOrEmpty(trimmedLine))
            {
                outputList.Add(trimmedLine);
            }
        }
    
        return outputList;
    }
    
    [HttpGet("Result/{guid}", Name = "GetResultByGuid")]
    [ProducesResponseType(typeof(UserOutputRead), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> GetResultByGuid(Guid guid)
    {
        List<UserDiagnosticInput> inputs = await _context.UserDiagnosticInputs
            .Where(x => x.InputId == guid)
            .ToListAsync();
        if (inputs.IsNullOrEmpty())
        {
            return NotFound();
        }
        UserInputRelation InputDb = _context.UserInputRelations
            .Where(u => u.InputId == guid)
            .FirstOrDefault();

        List<UserDiadnosticOutput> userDiadnosticOutputs = new List<UserDiadnosticOutput>();
        double totalAccuracy = 0;

        foreach (UserDiagnosticInput input in inputs)
        {
            bool standartExists = CheckIfStandartExist(input.MKBCode, _context);
            int totalRecommendations = 0;
            int totalNonStandardRecommendations = 0;

            UserDiadnosticOutput userDiadnosticOutput = new UserDiadnosticOutput()
            {
                Id = input.Id,
                Sex = input.Sex,
                BirthDate = input.BirthDate,
                PatientId = input.PatientId,
                MKBCode = input.MKBCode,
                Diagnosis = input.Diagnosis,
                Date = input.Date,
                DoctorPost = input.DoctorPost,
                StandartExists = standartExists
            };

            List<RecommendationsWStatus> recommendationStatuses = GetRecomendationStatused(input.Recommendations, input.MKBCode, _context);

            

            // Group recommendations by status
            List<RecommendationGroup> recommendationGroups = recommendationStatuses
                .GroupBy(r => r.Status)
                .Select(g => new RecommendationGroup
                {
                    GroupStatus = g.Key,
                    GroupStatusName = GetStatusName(g.Key), // Replace with the actual method that retrieves status name
                    GroupRecommendations = g.Select(r => r.Name).ToList()
                })
                .ToList();

            userDiadnosticOutput.RecommendationsGrouped = recommendationGroups;
            
            if (recommendationStatuses != null)
            {
                totalRecommendations = recommendationStatuses.Count;

                int totalCorrectRecommendations = recommendationGroups
                    .Where(g => g.GroupStatus == 1 || g.GroupStatus == 2)
                    .Sum(g => g.GroupRecommendations.Count);

                double accuracy = 0;

                if (standartExists)
                {
                    accuracy = totalRecommendations > 0 ? (totalCorrectRecommendations / (double)totalRecommendations) * 100 : 0;
                    totalAccuracy += accuracy;
                }
                else
                {
                    accuracy = 100;
                    totalAccuracy += accuracy * (totalRecommendations > 0 ? (totalCorrectRecommendations / (double)totalRecommendations) : 1);
                }

                userDiadnosticOutput.Accuracy = (int)accuracy;
            }

            userDiadnosticOutputs.Add(userDiadnosticOutput);
        }
        
        

        return Ok(new UserOutputRead()
        {
            Id = guid,
            Name = InputDb.InputName.Substring(0, InputDb.InputName.Length - 14),
            CreationDate = DateTime.ParseExact(InputDb.InputName.Substring(InputDb.InputName.Length - 14), "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None),
            TotalAccuracy = (int)totalAccuracy,
            OutputDatas = userDiadnosticOutputs
        });
    }

    //debug
    
    public static List<RecommendationsWStatus> GetRecomendationStatused(List<string> inputList, string mkbcode, ApiDbContext _context)
    {
        List<RecommendationsWStatus> recomendationWResults = new List<RecommendationsWStatus>();
        List<AvailableRecommendations> availableRecomendations = GetAllAvailableRecomendations(mkbcode, _context);

        foreach (string input in inputList)
        {
            int status = GetRecommendationValue(input, availableRecomendations);
                recomendationWResults.Add(new RecommendationsWStatus()
                {
                    Name = input,
                    Status = status
                });
        }
        foreach (var availableRecomendation in availableRecomendations)
        {
            string recomendationName = availableRecomendation.Name;
            if (!inputList.Contains(recomendationName) && !availableRecomendation.IsAnalog)
            {
                int statusValue = availableRecomendation.Status ? 3 : 4;
                RecommendationsWStatus recommendationsWStatus = new RecommendationsWStatus()
                {
                    Name = recomendationName,
                    Status = statusValue
                };
                recomendationWResults.Add(recommendationsWStatus);
            }
        }
        
        if (recomendationWResults.Count == 0)
        {
            return null;
        }
        return recomendationWResults;
    }   

    public static bool IsAnalog(string recommendation, List<AvailableRecommendations> availableRecomendations)
    {
        return availableRecomendations.Any(a => a.Name == recommendation && a.IsAnalog);
    }

    public static int GetRecommendationValue(string inputRecom, List<AvailableRecommendations> response)
    {
        var recommendation = response.FirstOrDefault(r => r.Name == inputRecom);
        if (recommendation != null)
        {
            return recommendation.Status ? 1 : 2;
        }
        return 0;
    }
    
    public static string GetStatusName(int status)
    {
        switch (status)
        {
            case 0:
                return "Назначен, но нет в стандарте";
            case 1:
                return "Назначено, обязательное назначение";
            case 2:
                return "Назначено, необязательное назначение";
            case 3:
                return "Не незначено, но есть в стандарте (обязательное назначение)";
            case 4:
                return "Не назначено, но есть в стандарте (необязательное назначение)";
            default:
                return "Неизвестный статус";
        }
    }
    public static List<RecomendationWResult> GetRecomendationStatus(string mkbcode, List<string> inputList, ApiDbContext _context)
    {
        List<RecomendationWResult> recomendationWResults = new List<RecomendationWResult>();
        foreach (string input in inputList)
        {
            recomendationWResults.Add(new RecomendationWResult()
            {
                Name = input,
                Status = CheckRecomendation(input, mkbcode, _context)
            });
        }
        if (recomendationWResults.Count == 0)
        {
            return null;
        }
        return recomendationWResults;
    }
    
    /*
    [HttpPost("Recommendations", Name = "GetAllRelatedRecommendations")]
    [ProducesResponseType(typeof(RecomendationWResult), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> GetAllRelated([FromBody] List<string>? inputRecomendations, [FromQuery] string mkbcode)
    {
        if (!CheckIfStandartExist(mkbcode, _context)) return NotFound("Стандарта назначений не найдено");
        if (inputRecomendations.IsNullOrEmpty()) return BadRequest();
        List<AvailableRecommendations>? availableRecomendations = GetAllAvailableRecomendations(mkbcode, _context);
        if (availableRecomendations == null) return NotFound("Стандарта назначений не найдено");
        List<RecommendationsWStatus> recommendationsWStatusList = new List<RecommendationsWStatus>();

        // Обработка inputRecomendations
        foreach (string inputRecomendation in inputRecomendations)
        {
            RecommendationsWStatus recommendationsWStatus = new RecommendationsWStatus()
            {
                Name = inputRecomendation,
                Status = GetRecommendationValue(inputRecomendation, availableRecomendations)
            };
            recommendationsWStatusList.Add(recommendationsWStatus);
        }

        // Обработка доступных рекомендаций, которые отсутствуют в inputRecomendations
        foreach (var availableRecomendation in availableRecomendations)
        {
            string recomendationName = availableRecomendation.Name;
            if (!inputRecomendations.Contains(recomendationName) && !availableRecomendation.IsAnalog)
            {
                int statusValue = availableRecomendation.Status ? 3 : 4;
                RecommendationsWStatus recommendationsWStatus = new RecommendationsWStatus()
                {
                    Name = recomendationName,
                    Status = statusValue
                };
                recommendationsWStatusList.Add(recommendationsWStatus);
            }
        }

        return Ok(recommendationsWStatusList);
    }
    */
    
    public static List<AvailableRecommendations> GetAllAvailableRecomendations(string mkbcode, ApiDbContext _context)
    {
        Mkb10 mkb10search = _context.Mkb10s
            .Where(m => m.Chapter == Util.ParseMkb10Code(mkbcode).Chapter)
            .Where(m => m.Litera == Util.ParseMkb10Code(mkbcode).Litera)
            .Where(m => m.Number == Util.ParseMkb10Code(mkbcode).Number)
            .FirstOrDefault(m => m.Subnumber == Util.ParseMkb10Code(mkbcode).Subnumber && m.Subnumber != -1);

        List<MkbStandart> availableStandartId = _context.Standarts
            .Where(s => s.Mkb10Id == mkb10search.Id)
            .ToList();

        var availableRecomendations = new List<AvailableRecommendations>();
        foreach (var standart in availableStandartId)
        {
            var analysis = _context.MskAnalyses
                .FirstOrDefault(a => a.Id == standart.EsiliId);
            if (analysis != null)
            {
                var recommendation = new AvailableRecommendations
                {
                    Name = analysis.Name,
                    Status = standart.IsMandatory,
                    IsAnalog = false
                };
                availableRecomendations.Add(recommendation);

                if (analysis.Analogs != null)
                {
                    foreach (var analog in analysis.Analogs)
                    {
                        var analogRecommendation = new AvailableRecommendations
                        {
                            Name = analog,
                            Status = standart.IsMandatory,
                            IsAnalog = true
                        };
                        availableRecomendations.Add(analogRecommendation);
                    }
                }
            }
        }

        return availableRecomendations;
    }


    public static bool? CheckRecomendation(string input, string mkbcode, ApiDbContext _context)
    {
        MskAnalysis? mskAnalysis = _context.MskAnalyses
            .Where(a => a.Name.Trim().ToLower() == input.Trim().ToLower() ||
                        a.Analogs.ToList().Contains(input))
            .FirstOrDefault();
        if (mskAnalysis == null) return false;
        Mkb10 mkb10search = _context.Mkb10s
            .Where(m => m.Chapter == Util.ParseMkb10Code(mkbcode).Chapter)
            .Where(m => m.Litera == Util.ParseMkb10Code(mkbcode).Litera)
            .Where(m => m.Number == Util.ParseMkb10Code(mkbcode).Number)
            .FirstOrDefault(m => m.Subnumber == Util.ParseMkb10Code(mkbcode).Subnumber && m.Subnumber != -1);
        if (mkb10search == null) return false;
        MkbStandart? standart = _context.Standarts
            .Where(s => s.Mkb10Id == mkb10search.Id)
            .FirstOrDefault(s => s.EsiliId == mskAnalysis.Id);
        if (standart == null) return null;
        return standart.IsMandatory;
    }
    
    public static bool CheckIfStandartExist(string mkbcode, ApiDbContext _context)
    {
        Mkb10 mkb10search = _context.Mkb10s
            .Where(m => m.Chapter == Util.ParseMkb10Code(mkbcode).Chapter)
            .Where(m => m.Litera == Util.ParseMkb10Code(mkbcode).Litera)
            .Where(m => m.Number == Util.ParseMkb10Code(mkbcode).Number)
            .FirstOrDefault(m => m.Subnumber == Util.ParseMkb10Code(mkbcode).Subnumber && m.Subnumber != -1);
        if (mkb10search == null) return false;
        List<MkbStandart> availableStandartId = _context.Standarts
            .Where(s => s.Mkb10Id == mkb10search.Id)
            .ToList();
        if (availableStandartId.IsNullOrEmpty()) return false;
        return true;
    }

    public static bool DiagnosisCheck(string checkstring, ApiDbContext _context)
    {
        MskAnalysis? mskAnalysis = _context.MskAnalyses
                .Where(a => a.Name.Trim().ToLower() == checkstring.Trim().ToLower() ||
                            a.Analogs.ToList().Contains(checkstring))
                .FirstOrDefault();
        if (mskAnalysis == null) return false;
        return true;
    }
    
    [HttpGet("Statistics/Doctor/{guid}")]
[ProducesResponseType(typeof(DoctorDepartmentStatisticsOutput), 200)]
public async Task<IActionResult> GetDoctorStatistics(Guid guid)
{
    Task<IActionResult> userOutputReadTask = GetResultByGuid(guid);
    IActionResult userOutputReadActionResult = await userOutputReadTask;
    UserOutputRead userOutputRead = new UserOutputRead();
    if (userOutputReadActionResult is OkObjectResult okObjectResult)
    {
        userOutputRead = (UserOutputRead)okObjectResult.Value;
    }
    else
    {
        Console.WriteLine("Error reading user output");
    }

    List<UserDiadnosticOutput> userDiadnosticOutputs = userOutputRead.OutputDatas;

    var doctors = userDiadnosticOutputs.Select(o => o.DoctorPost).Distinct();
    var doctorStatistics = new DoctorStatisticsOutput
    {
        TotalDoctors = doctors.Count()
    };

    foreach (var doctor in doctors)
    {
        var doctorOutputs = userDiadnosticOutputs.Where(o => o.DoctorPost == doctor);
        var totalRecommendations = doctorOutputs.Sum(o => o.RecommendationsGrouped.Sum(g => g.GroupRecommendations.Count));
        var correctRecommendations = doctorOutputs.Sum(o => o.RecommendationsGrouped
            .Where(g => g.GroupStatus == 1 || g.GroupStatus == 2)
            .Sum(g => g.GroupRecommendations.Count));
        // Статистика по отделениям
        var departmentCount = doctorOutputs.Count();
        var doctorStats = new DoctorStatistics
        {
            DoctorPost = doctor,
            TotalRecommendations = totalRecommendations,
            CorrectRecommendationRatio = totalRecommendations > 0 ? correctRecommendations / (double)totalRecommendations : 0,
            TotalPatients = departmentCount
        };

        doctorStatistics.DoctorStatistics.Add(doctorStats);
    }

    var doctorDepartmentStatistics = new DoctorDepartmentStatisticsOutput
    {
        TotalDoctors = doctorStatistics.TotalDoctors,
        DoctorStatistics = doctorStatistics.DoctorStatistics
    };

    return Ok(doctorDepartmentStatistics);
}


    [HttpGet("Statistics/Patient/{guid}")]
    [ProducesResponseType(typeof(PatientStatisticsOutput), 200)]
    public async Task<IActionResult> GetPatientStatistics(Guid guid)
    {
        Task<IActionResult> userOutputReadTask = GetResultByGuid(guid);
        IActionResult userOutputReadActionResult = await userOutputReadTask;
        UserOutputRead userOutputRead = new UserOutputRead();
        if (userOutputReadActionResult is OkObjectResult okObjectResult)
        {
            userOutputRead = (UserOutputRead)okObjectResult.Value;
        }
        else
        {
            Console.WriteLine("Error reading user output");
        }

        List<UserDiadnosticOutput> userDiadnosticOutputs = userOutputRead.OutputDatas;

        var patients = userDiadnosticOutputs.Select(o => o.PatientId).Distinct();
        var patientStatistics = new PatientStatisticsOutput
        {
            TotalPatients = patients.Count()
        };

        foreach (var diagnosis in userDiadnosticOutputs
            .Select(o => new { Diagnosis = o.Diagnosis, MKBCode = o.MKBCode })
            .Distinct()
            .ToList())
        {
            var patientsWithDiagnosis = userDiadnosticOutputs.Where(o => o.Diagnosis == diagnosis.Diagnosis);
            var ageSum = patientsWithDiagnosis.Sum(o =>
            {
                DateTime currentDate = DateTime.Now.Date;
                int age = currentDate.Year - o.BirthDate.Value.Year;
                if (currentDate.Month < o.BirthDate.Value.Month ||
                    (currentDate.Month == o.BirthDate.Value.Month && currentDate.Day < o.BirthDate.Value.Day))
                {
                    age--;
                }

                return age;
            });
            var averageAge = patientsWithDiagnosis.Any() ? ageSum / patientsWithDiagnosis.Count() : 0;

            var diagnosisStats = new DiagnosisStatistics
            {
                MkbCode = diagnosis.MKBCode,
                Diagnosis = diagnosis.Diagnosis,
                TotalPatients = patientsWithDiagnosis.Count(),
                AverageAge = averageAge,
                CorrectRecommendationRatio = (int)patientsWithDiagnosis.Average(o => o.Accuracy)
            };

            patientStatistics.PatientStatistics.Add(diagnosisStats);
        }

        return Ok(patientStatistics);
    }

    [HttpGet("Statistics/Recommendation/{guid}")]
    [ProducesResponseType(typeof(RecommendationStatisticsOutput), 200)]
    public async Task<IActionResult> GetRecommendationStatistics(Guid guid)
    {
        Task<IActionResult> userOutputReadTask = GetResultByGuid(guid);
        IActionResult userOutputReadActionResult = await userOutputReadTask;
        UserOutputRead userOutputRead = new UserOutputRead();
        if (userOutputReadActionResult is OkObjectResult okObjectResult)
        {
            userOutputRead = (UserOutputRead)okObjectResult.Value;
        }
        else
        {
            Console.WriteLine("Error reading user output");
        }

        List<UserDiadnosticOutput> userDiadnosticOutputs = userOutputRead.OutputDatas;

        var recommendationTypes = userDiadnosticOutputs
            .SelectMany(o => o.RecommendationsGrouped.SelectMany(g => g.GroupRecommendations))
            .Distinct();
        
        var recommendationStatistics = new RecommendationStatisticsOutput();

        foreach (var type in recommendationTypes)
        {
            var typeCount = userDiadnosticOutputs
                .SelectMany(o => o.RecommendationsGrouped)
                .SelectMany(g => g.GroupRecommendations)
                .Count(r => r == type);

            var typeStats = new RecommendationTypeStatistics
            {
                RecommendationType = type,
                TotalRecommendations = typeCount
            };

            recommendationStatistics.RecommendationTypeStatistics.Add(typeStats);
        }

        return Ok(recommendationStatistics);
    }

}