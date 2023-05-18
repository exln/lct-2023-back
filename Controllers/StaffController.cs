using MediWingWebAPI.Data;
using MediWingWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediWingWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class StaffController : Controller
{
    private readonly ILogger<StaffController> _logger;
    private readonly ApiDbContext _context;

    public StaffController(
        ILogger<StaffController> logger,
        ApiDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    [HttpGet(Name="GetStaffs")]
    public async Task<IActionResult> GetStaffs()
    {
        var staffs = await _context.Staffs.ToListAsync();
        return Ok(staffs);
    }
    
    [HttpPost(Name="GetStaffById")]
    public async Task<IActionResult> GetStaffById([FromBody] Guid id)
    {
        var staff = await _context.Staffs.FindAsync(id);
        if (staff == null)
        {
            return NotFound();
        }
        return Ok(staff);
    }
    
    [HttpPost("Add", Name = "CreateStaff")]
    public async Task<IActionResult> Post([FromBody] Staff staff)
    {
        if (staff == null)
        {
            return BadRequest();
        }
        await _context.Staffs.AddAsync(staff);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetStaffById), new { id = staff.Id }, staff);
    }
}