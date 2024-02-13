using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DTO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApi.Controllers;
using WebApi.Services;
using WebApp.Services;

namespace WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class BaseController : Controller 
    {
      

       
        public int CurrentUserId { get; set; }
        public int CurrentStaffno { get; set; }
        public String CurrentUserName { get; set; }
        public String CurrentUserRole{ get; set; }

        public String CurrentEmployeeName { get; set; }

        public String JWTTokenstring { get; set; }


        public BaseController(IHttpContextAccessor contextAccessor)
        {


           
            CurrentUserRole = contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            var currentstaff = contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (currentstaff != null)
            {
                CurrentStaffno = int.Parse(currentstaff.ToString());
            }

            var userid = contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid)?.Value;
            if (userid != null)
            {
                CurrentUserId = int.Parse(userid.ToString());
            }

            CurrentUserName = contextAccessor.HttpContext.User.FindFirst(ClaimTypes.SerialNumber)?.Value;
            CurrentEmployeeName= contextAccessor.HttpContext.User.FindFirst(ClaimTypes.GivenName)?.Value;

            JWTTokenstring= contextAccessor.HttpContext.Session.GetString("token");
            HttpTaskService.token = JWTTokenstring;

 
        }


    }
}
