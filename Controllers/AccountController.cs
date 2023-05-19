using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MediWingWebAPI.Data;
using MediWingWebAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MediWingWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController: ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly ApiDbContext _context;
    private readonly IConfiguration _configuration;

    public AccountController(
        ILogger<AccountController> logger, 
        ApiDbContext context, 
        IConfiguration configuration)
    {
        _logger = logger;
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("SignIn", Name = "SignIn")]
    public async Task<IActionResult> SignIn([FromBody] UserCreation userCreation)
    {
        if (userCreation == null)
        {
            return BadRequest();
        }
        
        User user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == userCreation.Email);
        
        if (user == null) return NotFound();
        
        PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
        PasswordVerificationResult passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userCreation.Password+user.PasswordSalt);
        
        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            return Unauthorized();
        }

        var token = GenerateJwtToken(user);

        return Ok(new { token });
    }
    
    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    [HttpPost("SignUp", Name = "SignUp")]
    public async Task<IActionResult> SignUp([FromBody] UserCreation userCreation)
    {
        if (userCreation == null)
        {
            return BadRequest();
        }

        PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
        
        byte[] salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        string saltString = Convert.ToBase64String(salt);
        
        User user = new User
        {
            Id = Guid.NewGuid(),
            Email = userCreation.Email,
            PasswordSalt = saltString,
            PasswordHash = passwordHasher.HashPassword(null, userCreation.Password + saltString),
            Role = "Unassigned"
        };

        Staff staff = new Staff
        {
            Id = Guid.NewGuid(),
            UserId = user.Id
        };
        
        user.StaffId = staff.Id;
        
        await _context.Users.AddAsync(user);
        await _context.Staffs.AddAsync(staff);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(SignUp), new {id = user.Id}, user);
    }
}