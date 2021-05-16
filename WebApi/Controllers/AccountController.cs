using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    public class AccountController : Controller
    {
        private ServerContext _dbContext;

        public AccountController(ServerContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("/token")]
        public IActionResult Token(string login, string password)
        {
            var identity = GetIdentity(login, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid login or password" });
            }

            // Создание токена
            var curTime = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: curTime,
                    claims: identity.Claims,
                    expires: curTime.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                login = identity.Name
            };

            return Json(response);
        }

        private ClaimsIdentity GetIdentity(string login, string password)
        {
            var user = _dbContext.Users.Include(x => x.UserGroup)
                .FirstOrDefault(x => x.Login == login && x.Password == password && x.UserStateId != 2);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.UserGroup.Code)
                };
                var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // Неверный логин или пароль
            return null;
        }
    }
}