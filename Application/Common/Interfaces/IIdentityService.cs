using Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
    {
    public interface IIdentityService
        {
        Task<string> GetUserNameAsync(string userId);
        Task<bool> IsInRoleAsync(string userId, string role);
        Task<bool> AuthorizeAsync(string userId, string policyName);

        Task<(Result Result, string UserId)> CreateUserAsync(string email, string password);
        Task<Result> DeleteUserAsync(string userId);
        Task<string> GenerateJwtTokenAsync(string email, string password);
        }
    }
