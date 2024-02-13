using Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using WebApi.Controllers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using WebApp.Models;

namespace WebApi.API_Controllers.WebApis.Security
{

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    [AllowAnonymous]
    public class Api_SecurityController : Api_BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        public Api_SecurityController(IUnitOfWork unitOfWork, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
        }

        [HttpPost]
        [Route("Login")]
        public CustomApiResponse Login( UserLoginDTO loginDetails)
        {
            CustomApiResponse response = new CustomApiResponse();
            var result = ValidateUser(loginDetails);
            if (result != null)
            {
                var claims = new[]
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

                var item = new { token = stringToken, userName = result.UserName, staffNo = result.EmployeeId, userTypeId = result.UserTypeId };
                _resp.SuccessReponse(item, ref response);
            }
            else
            {
                _resp.ActionNotAuthorizedResponse(ref response);
            }

            return response;
        }

        [HttpPost]
        [Route("LoginTemp")]
        public CustomApiResponse LoginTemp(String UserName,String Password )
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                UserLoginDTO loginDetails = new UserLoginDTO();
                loginDetails.UserName = UserName;
                loginDetails.Password = Password;

                var result = ValidateUser(loginDetails);
                if (result != null)
                {
                    var claims = new[]
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

                    var item = new { token = stringToken, userName = result.UserName, staffNo = result.EmployeeId, userTypeId = result.UserTypeId };
                    _resp.SuccessReponse(item, ref response);
                }
                else
                {
                    _resp.ActionNotAuthorizedResponse(ref response);
                }
            }
            catch (Exception ex)
            {

                _resp.ExceptionResponse(ex, ref response);
            }

            return response;
        }

        private User ValidateUser(UserLoginDTO loginDetails)
        {


            try
            {
                string message = "";
                var k = _unitOfWork.User.GetvalidUser(loginDetails.UserName, loginDetails.Password, out message);

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

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    [AllowAnonymous]
    public class Api_PDController : Api_BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        public Api_PDController(IUnitOfWork unitOfWork, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
        }

        [AllowAnonymous]
        [HttpGet()]
        [Route("GetRules")]
        public ActionResult<MainPageModel> GetRules()
        {
            MainPageModel mainPageModel = new MainPageModel();
            var k = _unitOfWork.MainPage.GetAll().FirstOrDefault();
            mainPageModel.RulesRegulation = k.RulesRegulation;
            return mainPageModel;
        }

        [AllowAnonymous]
        // GET: api/UserProfile/5
        [HttpGet()]
        [Route("GetUserProfile/{id}")]
        public CustomApiResponse GetUserProfile(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                var item = _unitOfWork.Member.GetAllMemberasync(u => u.StaffNo == id).FirstOrDefault();
                _resp.SuccessReponse(item, ref response);
            }
            catch (Exception ex)
            {

                _resp.ExceptionResponse(ex,ref response);
            }

            return response;
        }

        [HttpGet()]
        [Route("GetUserContribution/{id}")]
        public CustomApiResponse GetUserContribution(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                var item = _unitOfWork.AccountsRepository.GetContribution(id);
                _resp.SuccessReponse(item, ref response);
            }
            catch (Exception ex)
            {

                _resp.ExceptionResponse(ex, ref response);
            }

            return response;
            

        }

    }


}

