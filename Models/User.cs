using Microsoft.EntityFrameworkCore;

namespace MediWingWebAPI.Models;

[PrimaryKey("Id")]
public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }    
    public string PasswordSalt { get; set; }
    public string Role { get; set; } = "Unassigned";
    public Guid StaffId { get; set; }
    internal Staff? Staff { get; set; }
}

public class UserLogin
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UserGet
{
    public string Login { get; set; }
    public string Role { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
}

public class UserCreation
{
    public string Email { get; set; }
    public string Password { get; set; }
    
    public string? Name { get; set; }
    public string? Surname { get; set; }
}
