using CulinaryCloud.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace CulinaryCloud
{
    public class LoginController : Controller
    {
        public LoginController()
        {
            using(var db = new CloudContext())
            {
                db.Database.EnsureCreated();
            }
        }

        [HttpPost("/user")]
        public IActionResult Register(string login, string password)
        {
            var user = new User() { Login = login.Trim(), Password = password };
            if(string.IsNullOrEmpty(user.Login) || string.IsNullOrEmpty(user.Password))
            {
                return Content("empty user name or password");
            }
            using (var db = new CloudContext())
            {
                if (db.Users.Any(u => u.Login == user.Login)) return Content($"login \"{user.Login}\" already exists");

                db.Users.Add(user);
                db.SaveChanges();
            }
            return new OkResult();
        }

        [HttpGet("/token")]
        public IActionResult Login(string login, string password)
        {
            var identity = GetIdentity(login, password);

            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFE_TIME_MINUTES)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return Content(encodedJwt);
        }

        private ClaimsIdentity GetIdentity(string login, string password)
        {
            using (var db = new CloudContext())
            {
                var user = db.Users.FirstOrDefault(x => x.Login == login && x.Password == password);
                if (user == null) return null;
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
                return claimsIdentity;
            }
        }
    }
}
