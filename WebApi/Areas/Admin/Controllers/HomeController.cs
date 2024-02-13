using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApp.Services;

namespace WebApi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {


        private readonly ILogger<HomeController> _logger;

        string apiUrl = "api/UserProfile" + "";
        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor contextAccessor) 
        {
            _logger = logger;
         
        }

        // GET: HomeController
        public async Task<IActionResult> IndexAsync()
        {
            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var branchlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProfileDTO>>(data);
              
            }

            return View();


            
        }

        public IActionResult DashBoard()
        {

            return View();
        }

        public async Task<IActionResult> Profile()
        {
            int id = int.Parse(HttpContext.Session.GetString("staffNo"));


            ProfileDTO profileDTO = new ProfileDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData("api/UserProfile" + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                profileDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<ProfileDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading Branch : Try Again";
            }
            if (profileDTO == null)
            {
                return NotFound();
            }
            return View(profileDTO);



        }

        public ActionResult Contribution()
        {
            return View();
        }


        // GET: HomeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HomeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
