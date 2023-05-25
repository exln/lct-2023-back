

using System.Globalization;
using MediWingWebAPI.Data;
using MediWingWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;

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
    [ProducesResponseType(typeof(UserInputRelation), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IActionResult> ImportXLSX(IFormFile? file, string? name)
    {
        // Проверяем, что файл был успешно загружен
        if (file == null || file.Length == 0) return BadRequest("Файл не найден");

        string fileNameWithoutExtension;
        // Получаем имя файла
        if (name.IsNullOrEmpty())
        {
            fileNameWithoutExtension =
                Path.GetFileNameWithoutExtension(file.FileName);
        }
        else
        {
            fileNameWithoutExtension = name;
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
                
            }
            await _context.UserDiagnosticInputs.AddRangeAsync(list);
        }
        
        await _context.UserInputRelations.AddAsync(userInputRelation);
        await _context.SaveChangesAsync();
        
        // Удаляем файл
        System.IO.File.Delete(filePath);
        userInputRelation.InputName = userInputRelation.InputName.Substring(0, userInputRelation.InputName.Length - 14);
        return Ok(userInputRelation);
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
}