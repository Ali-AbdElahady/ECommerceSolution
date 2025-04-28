using Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Extentions
{
    public static class IdentityResultExtensions
    {
        public static Result ToApplicationResult(this IdentityResult identityResult)
        {
            if (identityResult.Succeeded)
            {
                return Result.Success();
            }

            var errors = identityResult.Errors.Select(e => e.Description);
            return Result.Failure(errors);
        }
    }
}
