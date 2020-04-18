using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using reservacion.Api.JwtCustomModels;
using reservacion.Areas.Identity.Data;

namespace reservacion.Api.Controllers
{
    [Route("api/[controller]")]
    public class JwtLoginController: Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public JwtLoginController(UserManager<User> userManager, SignInManager<User> signInManager){
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        [HttpPost]
        public async Task <IActionResult> CreateToken([FromBody]JwtCustomUser model){
            if(ModelState.IsValid){
                User user = await _userManager.FindByNameAsync(model.UserName);
                if(user == null){
                    return NotFound("Credenciales no validas");
                }
                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if(signInResult.Succeeded){
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtCustomConstants.secretKey));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var claims = new List<Claim> {
                        new Claim(JwtRegisteredClaimNames.Sub, model.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.UniqueName, model.UserName)
                    };
                    var userRoles = await _userManager.GetRolesAsync(user);
                    foreach(var userRole in userRoles){
                        claims.Add(new Claim(ClaimTypes.Role, userRole));
                    }
                    var token = new JwtSecurityToken(
                        JwtCustomConstants.Issuer,
                        JwtCustomConstants.Audience,
                        claims,
                        expires: DateTime.Now.AddHours(1),
                        signingCredentials: creds
                    );
                    var resultados = new {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    };
                    return Created("", resultados);
                }
                else{
                    return NotFound("Credenciales no validas");
                }
            }

            return BadRequest();
        }
    }
}