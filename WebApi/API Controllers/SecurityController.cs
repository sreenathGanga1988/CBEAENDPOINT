using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class SecurityController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        public SecurityController(IUnitOfWork unitOfWork , IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
        }


        [HttpPost]
        public IActionResult Login([FromBody]UserLoginDTO loginDetails)
        {
            var  result = ValidateUser(loginDetails);
            if (result!=null)
            {
                var claims = new []
                {
                    new Claim(JwtRegisteredClaimNames.Sid,result.Id.ToString ()) ,
                   new Claim(JwtRegisteredClaimNames.NameId,result.EmployeeId.ToString ()) ,
                    new Claim(JwtRegisteredClaimNames.UniqueName,result.UserName.ToString ()) ,
                     new Claim(JwtRegisteredClaimNames.Typ,result.UserTypeId.ToString ()) ,
                       new Claim(JwtRegisteredClaimNames.GivenName,result.EmployeeName.ToString ()) ,
                     new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString ()),


                };
                var issuer = _config["Jwt:Issuer"];
                var audience = _config["Jwt:Audience"];
                var expiry = DateTime.Now.AddMinutes(120);
                var securityKey = new SymmetricSecurityKey
               (Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials
            (securityKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    claims: claims,
            audience: audience,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

                var tokenHandler = new JwtSecurityTokenHandler();
                var stringToken = tokenHandler.WriteToken(token);
              
                return Ok(new { token = stringToken, userName=result.UserName, staffNo=result.EmployeeId, userTypeId= result.UserTypeId });
            }
            else
            {
                return Unauthorized();
            }
        }
       



        private User ValidateUser(UserLoginDTO loginDetails)
        {

            
            try
            {
                string message = "";
                var k = _unitOfWork.User.GetvalidUser(loginDetails.UserName, loginDetails.Password,out message);

                if (k != null)
                {
                    return k;




                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

               
                return null;
            }


        }




    }

    public class UserLoginDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}