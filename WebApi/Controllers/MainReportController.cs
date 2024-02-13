using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DTO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApi.Controllers;
using WebApp.Services;

namespace WebApp.Controllers
{

    public class MainReportController : AdminBaseController
    {
        private readonly ILogger<MainReportController> _logger;

        string apiUrl = "api/MainReport" +
            "";
        public MainReportController(ILogger<MainReportController> logger, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
           
            _logger = logger;
     
        }
        public async Task<IActionResult> IndexAsync()
        {


            MainReportDTOList MainReportDTOList = new MainReportDTOList();
            MainReportDTOList.MainReports = new List<MainReportDTO>();

            //HttpTaskService.token = HttpContext.Session.GetString("token");
            
            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var MainReportlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MainReportDTO>>(data);
                MainReportDTOList.MainReports = MainReportlist;
            }
            
            return View(MainReportDTOList);
        }

        // Get: MainReport/Create
        public IActionResult Create()
        {
            MainReportDTO mainReportDTO = new MainReportDTO();

           
            return View(mainReportDTO);
        }

        // POST: MainReport/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Name, Description, SQLString,CreatedByUserID, CreatedDate")] MainReportDTO mainReportDTO)
        {
            try
            {
                mainReportDTO.CreatedByUserId = CurrentUserId;
                mainReportDTO.CreatedDate = DateTime.UtcNow;
                
              
                HttpResponseMessage response = await HttpTaskService.PostAsyncData(apiUrl, mainReportDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "MainReport Added successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var MainReportlist = Newtonsoft.Json.JsonConvert.DeserializeObject<MainReportDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Adding MainReport : Try Again";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }







        // GET: MainReport/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            MainReportDTO mainReportDTO = new MainReportDTO();


            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                mainReportDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<MainReportDTO>(data);                
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading MainReport : Try Again";
            }
            if (mainReportDTO == null)
            {
                return NotFound();
            }
            return View(mainReportDTO);
        }


        // POST: MainReport/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Description, SQLString,CreatedByUserID, CreatedDate, AddedUser")] MainReportDTO mainReportDTO)
        {
            if (id != mainReportDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                mainReportDTO.ModifiedDate = DateTime.UtcNow;
                mainReportDTO.ModifiedByUserId = CurrentUserId;

                

                apiUrl = apiUrl + "/" + id;
                HttpResponseMessage response = await HttpTaskService.PutAsyncData(apiUrl, mainReportDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "MainReport Editted successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var mainReportlist = Newtonsoft.Json.JsonConvert.DeserializeObject<MainReportDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Editing MainReport : Try Again";
                }
                
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                                       .Where(y => y.Count > 0)
                                       .ToList();

                int k = 6;
            }
            return View(mainReportDTO);
        }







        // GET: MainReportDTO/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MainReportDTO mainReportDTO = new MainReportDTO();

            HttpResponseMessage response = await HttpTaskService .GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                mainReportDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<MainReportDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading MainReport : Try Again";
            }
            if (mainReportDTO == null)
            {
                return NotFound();
            }
            return View(mainReportDTO);
           
        }






        // GET: MainReportDTO/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MainReportDTO mainReportDTO = new MainReportDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                mainReportDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<MainReportDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading MainReport : Try Again";
            }
            if (mainReportDTO == null)
            {
                return NotFound();
            }
            return View(mainReportDTO);
        
    }




        // POST: mainReportDTO/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            apiUrl = apiUrl + "/" + id;
            HttpResponseMessage response = await HttpTaskService.DeleteAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                TempData["SucessAlert"] = "MainReport Deleted successfully";
                string result = response.Content.ReadAsStringAsync().Result;
                var returndata = await response.Content.ReadAsStringAsync();
                var MainReportlist = Newtonsoft.Json.JsonConvert.DeserializeObject<MainReportDTO>(returndata);

            }
            else
            {
                TempData["ErrorAlert"] = "Error in Deleted MainReport : Try Again";
            }

            return RedirectToAction(nameof(Index));
        }


        // POST: Masters/MainReport/Delete/5






    }
}