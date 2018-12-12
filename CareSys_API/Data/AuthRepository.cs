using CareSys_API.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CareSys_API.Data
{
    public class AuthRepository : IAuthRepository
    {

        private readonly UserManager<AppUser> _userManager;

        public AuthRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppUser> SignIn(LoginUser userToCheck)
        {
            var user = await _userManager.FindByNameAsync(userToCheck.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, userToCheck.Password))
            {
                return user;
            }
            return null;
        }

        public async Task<IEnumerable<string>> GetRoles(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var roles = await _userManager.GetRolesAsync(user);
            return roles;
        }

        public async Task<IEnumerable<Claim>> GetUserClaims(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var userClaims = await _userManager.GetClaimsAsync(user);
            return userClaims;
        }
    }
}
