using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using DataAccess.EFCore;
using DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApi.Controllers;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class HomeController : BaseController
    {

        private readonly ApplicationContext _context;
        
        public HomeController(ApplicationContext context, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _context = context;

            var authenticatedUser = contextAccessor.HttpContext.User.Identity.Name;
        }
        ////////////////////////////////////////////////////


        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            ViewBag.isindex = "Y";
            return View(await GetMainPageModelAsync("api/PublicArea/GetMainPageLast"));
        }
        [AllowAnonymous]
        public async Task<IActionResult> Rules()
        {
            return View(await GetMainPageModelAsync("api/PublicArea/GetRules"));
        }
        [AllowAnonymous]
        public async Task<IActionResult> Contactus()
        {
            return View(await GetMainPageModelAsync("api/PublicArea/GetContact"));
        }

   


        public async Task<MainPageModel> GetMainPageModelAsync(string apiUrl)
        {
            MainPageModel mainPageModel = new MainPageModel();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                mainPageModel = Newtonsoft.Json.JsonConvert.DeserializeObject<MainPageModel>(data);

            }

            return mainPageModel;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Downloads()
        {
            FileUploadDetailDTOList fileUploadDetailDTOList = new FileUploadDetailDTOList();



            HttpResponseMessage response = await HttpTaskService.GetAsyncData("api/PublicArea/GetDownloads");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                fileUploadDetailDTOList = Newtonsoft.Json.JsonConvert.DeserializeObject<FileUploadDetailDTOList>(data);

            }



            return View(fileUploadDetailDTOList);
        }


        [AllowAnonymous]
        public async Task<IActionResult> ManagingCommittee()
        {
            ManagingComiteeDTOList managingComiteeDTOList = new ManagingComiteeDTOList();



            HttpResponseMessage response = await HttpTaskService.GetAsyncData("api/PublicArea/GetManagingComitee");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                managingComiteeDTOList = Newtonsoft.Json.JsonConvert.DeserializeObject<ManagingComiteeDTOList>(data);

            }


          
            return View(managingComiteeDTOList);
        }




        [AllowAnonymous]
        public IActionResult Login()
        {

            return View();
        }
        [AllowAnonymous]
        public IActionResult Register()
        {

            return View();
        }



        [AllowAnonymous]

        public IActionResult claims()
        {
            
            var q = _context.DataTable("exec GetStatewiseClaims ");
            var q1 = _context.DataTable("exec GetDesignationClaims ");
            claimdata claimdata = new claimdata();
            claimdata.dt1 = GenerateSummary(q);
            claimdata.dt2 = GenerateSummary(q1);
            return View(claimdata);
        }




        // POST: User/Create
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("ID, UserName, Password, UserTypeId, EmployeeId, EmailId, PhoneNum, IsActive")] UserDTO UserDTO)
        {
            string apiUrl = "api/User" +
              "";
            try
            {
                UserDTO.CreatedByUserId = CurrentUserId;
                UserDTO.CreatedDate = DateTime.UtcNow;
                UserDTO.ModifiedDate = null;
                UserDTO.IsActive = false;
                UserDTO.IsDeleted = false;
                UserDTO.UserStatus = "P";
                HttpResponseMessage response = await HttpTaskService.PostAsyncData(apiUrl, UserDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "Registration completed successfully,Please wait for administrator approval";                

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in User Registration : Try Again";
                }
                return RedirectToAction("Login");
            }
            catch
            {
                return View();
            }
        }

        public DataTable GenerateSummary(DataTable dataTable,int dtcolumnfrom=0 )
        {
            dataTable.Columns.Add("Total");
            
            foreach (DataRow drow in dataTable.Rows)
            {

                int sum = 0;

                for(int i = dtcolumnfrom; i < dataTable.Columns.Count-1; i++)
                {
                    try
                    {
                        sum = sum + int.Parse(drow[i].ToString());
                    }
                    catch (Exception)
                    {

                       
                    }
                }
                drow["Total"] = sum;
            }
            dataTable.Rows.Add();
            for (int i = dtcolumnfrom; i < dataTable.Columns.Count ; i++)
            {
                int sum = 0;
                for (int j = 0; j < dataTable.Rows.Count-1; j++)
                {
                    try
                    {
                        sum = sum + int.Parse(dataTable.Rows[j][i].ToString ());
                    }
                    catch (Exception)
                    {


                    }
                }

                dataTable.Rows[dataTable.Rows.Count - 1][i] = sum;
            }

            dataTable.Rows[dataTable.Rows.Count - 1][0] = "Total";
            return dataTable;
        }




        [AllowAnonymous]
        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginDTO userModel)
        {
            HttpResponseMessage response = await HttpTaskService.PostAsyncwithoutheader("/api/security", userModel);
            if (response.StatusCode != HttpStatusCode.Unauthorized)
            {
                string stringJWT = response.Content.ReadAsStringAsync().Result;

                JWT jwt = JsonConvert.DeserializeObject<JWT>(stringJWT);
                var tokenrev = new JwtSecurityTokenHandler().ReadJwtToken(jwt.Token);
                var staffno = tokenrev.Claims.First(c => c.Type == "nameid").Value; // staffno
                var unique_name = tokenrev.Claims.First(c => c.Type == "unique_name").Value; //username
                var typ = tokenrev.Claims.First(c => c.Type == "typ").Value; //usertype
                var userid = tokenrev.Claims.First(c => c.Type == "sid").Value; // UserID
                var employeename = tokenrev.Claims.First(c => c.Type == "given_name").Value; // employeename



                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, staffno),
                  new Claim(ClaimTypes.SerialNumber, unique_name),
                  new Claim(ClaimTypes.Role, typ),
                   new Claim(ClaimTypes.Sid, userid),
                     new Claim(ClaimTypes.GivenName, employeename)
            };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                

                var authProperties = new AuthenticationProperties
                {
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    //IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                HttpContext.Session.SetString("token", jwt.Token);
                HttpContext.Session.SetString("staffNo", staffno);
                HttpContext.Session.SetString("Userid", userid);
                HttpContext.Session.SetString("userName", unique_name);

                HttpContext.Session.SetString("UserRole", typ);
                TempData["SucessAlert"] = "User logged in successfully!";

                if (typ == "3")
                {
                    return RedirectToAction("Profile","MemberArea");
                }
                else
                {
                    return RedirectToAction("Profile", "Home", new { Area = "Admin" });
                }

            }
            else
            {
                TempData["ErrorAlert"] = "Invalid User Name & Password: Try Again";
                return RedirectToAction("Login");
            }








        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("token");
            HttpContext.Session.Clear();
            TempData["SucessAlert"] = "User logged out successfully!";
            return RedirectToAction("Login");
         
        }



     







        public IActionResult ShowData()
        {
            string baseUrl = "http://localhost:44396";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue
        ("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer",
        HttpContext.Session.GetString("token"));

            HttpResponseMessage response = client.GetAsync
        ("/api/employee").Result;
            string stringData = response.Content.
        ReadAsStringAsync().Result;
       
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Message = "Unauthorized!";
            }
            else
            {

                ViewBag.Message = "";
            }

            return View("Index");
        }


    }


    public class JWT
    {
        public string Token { get; set; }

        public string userName { get; set; }

        public string staffNo { get; set; }

        public string userTypeId { get; set; }
    }

    public class UserLoginDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
   
}