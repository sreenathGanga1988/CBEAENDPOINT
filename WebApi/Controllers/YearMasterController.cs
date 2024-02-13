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
using WebApi.Controllers;
namespace WebApp.Controllers
{
    public class YearMasterController : AdminBaseController
    {
        private readonly ILogger<YearMasterController> _logger;

        string apiUrl = "api/YearMaster" +
            "";
      

        public YearMasterController(ILogger<YearMasterController> logger, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _logger = logger;

        }
        public async Task<IActionResult> IndexAsync()
        {


            YearMasterDTOList YearMasterDTOList = new YearMasterDTOList();
            YearMasterDTOList.YearMasters = new List<YearMasterDTO>();



            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var YearMasterlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<YearMasterDTO>>(data);
                YearMasterDTOList.YearMasters = YearMasterlist;
            }
            
            return View(YearMasterDTOList);
        }

        // Get: YearMaster/Create
        public IActionResult Create()
        {
            YearMasterDTO YearMasterDTO = new YearMasterDTO();

            return View(YearMasterDTO);
        }
        
        // POST: YearMaster/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID, YearMasterCode , Name, Abbreviation, IsActive, DateFrom, DateTo")] YearMasterDTO YearMasterDTO)
        {
            try
            {

          
                HttpResponseMessage response = await HttpTaskService.PostAsyncData(apiUrl, YearMasterDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "Year  Added successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var YearMasterlist = Newtonsoft.Json.JsonConvert.DeserializeObject<YearMasterDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Adding YearMaster : Try Again";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }







        // GET: YearMaster/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            YearMasterDTO YearMasterDTO = new YearMasterDTO();
           
            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                YearMasterDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<YearMasterDTO>(data);                
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading YearMaster : Try Again";
            }
            if (YearMasterDTO == null)
            {
                return NotFound();
            }
            return View(YearMasterDTO);
        }


        // POST: YearMaster/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,YearMasterCode, Name,Abbreviation ,CreatedByUserId ,CreatedDate, IsActive,DateFrom,DateTo")] YearMasterDTO YearMasterDTO)
        {
            if (id != YearMasterDTO.YearOf)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                  
             
             

                apiUrl = apiUrl + "/" + id;
                HttpResponseMessage response = await HttpTaskService.PutAsyncData(apiUrl, YearMasterDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "YearMaster Editted successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var YearMasterlist = Newtonsoft.Json.JsonConvert.DeserializeObject<YearMasterDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Editing YearMaster : Try Again";
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(YearMasterDTO);
        }







        // GET: YearMasterDTOes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            YearMasterDTO YearMasterDTO = new YearMasterDTO();

            HttpResponseMessage response = await HttpTaskService .GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                YearMasterDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<YearMasterDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading YearMaster : Try Again";
            }
            if (YearMasterDTO == null)
            {
                return NotFound();
            }
            return View(YearMasterDTO);
           
        }






        // GET: YearMasterDTOes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            YearMasterDTO YearMasterDTO = new YearMasterDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                YearMasterDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<YearMasterDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading YearMaster : Try Again";
            }
            if (YearMasterDTO == null)
            {
                return NotFound();
            }
            return View(YearMasterDTO);

        }


        // POST: YearMasterDTOes/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            apiUrl = apiUrl + "/" + id;
            HttpResponseMessage response = await HttpTaskService.DeleteAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                TempData["SucessAlert"] = "YearMaster Deleted successfully";
                string result = response.Content.ReadAsStringAsync().Result;
                var returndata = await response.Content.ReadAsStringAsync();
                var categorylist = Newtonsoft.Json.JsonConvert.DeserializeObject<YearMasterDTO>(returndata);

            }
            else
            {
                TempData["ErrorAlert"] = "Error in Deleting YearMaster : Try Again";
            }

            return RedirectToAction(nameof(Index));
        }











    }
}