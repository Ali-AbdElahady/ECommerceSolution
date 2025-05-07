using Microsoft.AspNetCore.Identity;

namespace ECommerceSolution.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string? FCMToken { get; set; }
}
