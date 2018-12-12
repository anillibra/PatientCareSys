using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CareSys_API.Data;
using CareSys_API.Dtos;
using CareSys_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CareSys_API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _repo;
        private readonly IMapper _mapper;
        public AuthController(IAuthRepository authRepository, IMapper mapper)
        {
            _repo = authRepository;
            _mapper = mapper;
        }

        [HttpPost("loginOld")]
        public async Task<IActionResult> LoginOld([FromBody] LoginDots user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            var userToLogin = _mapper.Map<LoginUser>(user);

            var userObject = await _repo.SignIn(userToLogin);

            if (userObject == null)
            {
                return Unauthorized();
            }
            else
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("CareSys_API_Super_Key"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var Userclaims = await _repo.GetUserClaims(user.UserName);

                //var claims = new[]
                //{
                //    new Claim(JwtRegisteredClaimNames.GivenName, user.UserName),
                //    new Claim(JwtRegisteredClaimNames.Birthdate, "10/20/2018")
                //}.Union(Userclaims);
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userObject.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, userObject.Id.ToString()),
                };

                var tokeOptions = new JwtSecurityToken(
                    issuer: "http://localhost:51004",
                    audience: "http://localhost:4200",
                    claims: Userclaims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                return Ok(new
                {
                    userName = user.UserName,
                    bearerToken = tokenString,
                    isAuthenticated = true
                });

                //var claims = new List<Claim>
                //{
                //    new Claim(JwtRegisteredClaimNames.Sub, userObject.UserName),
                //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //    new Claim(ClaimTypes.NameIdentifier, userObject.Id)
                //};

                // ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "token");


                // Adding roles code
                // Roles property is string collection but you can modify Select code if it it's not

                // claims.AddRange(roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));
                // Get Current User Roles
                //var roles = _repo.GetRoles(userObject.UserName);
                //claims.Add(new Claim("Roles", "Admin"));

                // claims: new List<Claim>(),

                //var tokeOptions = new JwtSecurityToken(
                //    issuer: "http://localhost:51004",
                //    audience: "http://localhost:4200",

                //    expires: DateTime.Now.AddMinutes(50),
                //    signingCredentials: signinCredentials
                //);

                //var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                //// return Ok(new { Token = tokenString });
                //return Ok(new
                //{
                //    userName = user.UserName,
                //    bearerToken = tokenString,
                //    isAuthenticated = true
                //});
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDots user)
        {
            AppUserAuth appuserAuth = new AppUserAuth();
            IActionResult ret = null;

            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            var userToLogin = _mapper.Map<LoginUser>(user);

            var userObject = await _repo.SignIn(userToLogin);

            if (userObject == null)
            {
                ret = StatusCode(StatusCodes.Status404NotFound, "Invalid User Name/Password.");
            }
            else
            {
                // Build User Security Object
                appuserAuth = BuildUserAuthObject(userObject);
                ret = StatusCode(StatusCodes.Status200OK, appuserAuth);
            }
            return ret;
        }


         protected List<AppUserClaim> GetUserClaims(AppUser authUser)
        {
            List<AppUserClaim> list = new List<AppUserClaim>();

            try
            {
                var Userclaims = _repo.GetUserClaims(authUser.UserName);
                

                foreach(var cl in Userclaims.Result)
                {
                    AppUserClaim usrcl = new AppUserClaim();
                    usrcl.ClaimId = new Guid();
                    usrcl.UserId = new Guid(authUser.Id);
                    usrcl.ClaimType = cl.Type;
                    usrcl.ClaimValue = cl.Value;
                    list.Add(usrcl);

                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Exception trying to retrieve user claims.", ex);
            }

            return list;
        }

        protected AppUserAuth BuildUserAuthObject(AppUser authUser)
        {
            AppUserAuth ret = new AppUserAuth();
            List<AppUserClaim> claims = new List<AppUserClaim>();

            // Set User Properties
            ret.UserName = authUser.UserName;
            ret.IsAuthenticated = true;
            ret.BearerToken = new Guid().ToString();

            // Get all claims for this user
            ret.Claims = GetUserClaims(authUser);

            // Set JWT bearer token
            ret.BearerToken = BuildJwtToken(ret).Result;

            return ret;
        }

        protected async Task<string> BuildJwtToken(AppUserAuth authUser)
        {
            

            // Create a string representation of the Jwt token
           //// return new JwtSecurityTokenHandler().WriteToken(token); ;

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("CareSys_API_Super_Key"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var Userclaims = await _repo.GetUserClaims(authUser.UserName);

            var tokeOptions = new JwtSecurityToken(
                issuer: "http://localhost:51004",
                audience: "http://localhost:4200",
                claims: Userclaims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        }
    }
}