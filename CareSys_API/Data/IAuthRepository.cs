using CareSys_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CareSys_API.Data
{
    public interface IAuthRepository
    {
        Task<AppUser> SignIn(LoginUser userToCheck);
        Task<IEnumerable<string>> GetRoles(string username);
        Task<IEnumerable<Claim>> GetUserClaims(string username);
    }
}
