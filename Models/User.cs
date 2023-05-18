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

[PrimaryKey("Email")]
public class UserCreation
{
    public string Email { get; set; }
    public string Password { get; set; }
}