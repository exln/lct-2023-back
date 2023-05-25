using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MediWingWebAPI.Data;
using MediWingWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MediWingWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
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
    [ProducesResponseType(typeof(UserGet), 200)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> SignIn([FromBody] UserLogin userLogin)
    {
        if (userLogin == null)
        {
            return BadRequest("User is null");
        }
        
        User user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == userLogin.Email);
        
        if (user == null) return NotFound();
        
        PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
        PasswordVerificationResult passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userLogin.Password+user.PasswordSalt);
        
        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            return Conflict("Wrong password");
        }
        
        var token = GenerateJwtToken(user);

        UserGet userGet = new UserGet()
        {
            Login = user.Email,
            Role = user.Role,
            Name = "Тест",
            Surname = "Тестович"
        };

        // set the JWT token in a cookie
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTimeOffset.UtcNow.AddDays(30)
        };
        Response.Cookies.Append("jwt", token, cookieOptions);

        return Ok(userGet);
    }
    
    private string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ex_ilon.me_dte_chnik"));  // Replace with your own secret key

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(30),  // Set the expiration time for the token
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
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
            UserId = user.Id,
            Name = userCreation.Name ?? "Лена",
            Surname = userCreation.Surname
        };
        
        user.StaffId = staff.Id;
            
        await _context.Users.AddAsync(user);
        await _context.Staffs.AddAsync(staff);
        await _context.SaveChangesAsync();
        
        UserGet userGet = new UserGet()
        {
            Login = user.Email,
            Role = user.Role,
            Name = "Тест",
            Surname = "Тестович"
        };

        return Ok(userGet);
    }

    [HttpPost("SignOut", Name = "SignOut")]
    [ProducesResponseType(typeof(string), 200)]
    public async Task<IActionResult> SignOut()
    {
        Response.Cookies.Delete("jwt");

        return Ok("Signed out");
    }
    
    
    [HttpGet("User", Name = "GetUser")]
    [ProducesResponseType(typeof(UserGet), 200)]
    [ProducesResponseType(typeof(string), 401)]
    public async Task<IActionResult> GetUser()
    {
        try
        {
            // get the JWT token from the cookie
            var jwt = HttpContext.Request.Cookies["jwt"];
            jwt = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiZGYzOTdmNS1mYzRkLTQwY2ItOWFhZi0wMDk5MzY5MjIwYTIiLCJleHAiOjE2ODcyNzM4MzF9.1xEnbFCH3im-IRx7ivdlAX_Y6DNQ6FiViC6A8MrD524";

            if (jwt == null)
            {
                return Unauthorized("You are not authorized");
            }

            // validate the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = "ex_ilon.me_dte_chnik"u8.ToArray();

            tokenHandler.ValidateToken(jwt, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            // get the user ID from the token
            var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "sub").Value);

            // get the user from the database
            User user = await _context.Users.FindAsync(userId);
            Guid staffId = user.StaffId;
            Staff staff = await _context.Staffs.FindAsync(staffId);

            if (user == null || staff == null)
            {
                return NotFound("User not found");
            }

            UserGet userGet = new UserGet()
            {
                Login = user.Email,
                Role = user.Role,
                Name = "Тест",
                Surname = "Тестович"
            };

            return Ok(userGet);
        }
        catch(SecurityTokenException e)
        {
            return Unauthorized("Invalid token: " + e.Message);
        }
    }   
}