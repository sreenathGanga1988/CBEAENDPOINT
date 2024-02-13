using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebApi.Controllers;
using Microsoft.AspNetCore.Http;


namespace WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "1")]
    public class AdminBaseController : BaseController
    {
        private IHttpContextAccessor contextAccessor;

        public AdminBaseController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            this.contextAccessor = contextAccessor;

            ViewBag.UserName = "Sreenath";
        }
        //good
    }
}
