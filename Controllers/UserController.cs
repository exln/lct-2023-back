using System.Security.Claims;
using System.Security.Cryptography;
using MediWingWebAPI.Data;
using MediWingWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediWingWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly ApiDbContext _context;

    public UserController(
        ILogger<UserController> logger,
        ApiDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet(Name = "GetUsers")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }
    
    [HttpPost(Name = "GetUserById")]
    public async Task<IActionResult> GetUserById([FromBody] Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }
    
    [HttpPost("Add", Name = "AddUser")]
    public async Task<IActionResult> GetUserById([FromBody] User user)
    {
        return Ok(user);
    }
    
    /*
    [HttpGet("Me", Name = "GetAuthenticatedUser")]
    [Authorize]
    public async Task<IActionResult> GetAuthenticatedUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _context.Users.FindAsync(Guid.Parse(userId));
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }*/
}