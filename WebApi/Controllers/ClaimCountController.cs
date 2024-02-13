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
    public class ClaimCountController : BaseController
    {
        private readonly ILogger<ClaimCountController> _logger;

        string apiUrl = "api/ClaimCount" +
            "";
        public ClaimCountController(ILogger<ClaimCountController> logger, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _logger = logger;
    
        }
        public async Task<IActionResult> IndexAsync()
        {


            ClaimCountDTOList ClaimCountDTOList = new ClaimCountDTOList();
            ClaimCountDTOList.ClaimCountDTOS = new List<ClaimCountDTO>();



            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var ClaimCountlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ClaimCountDTO>>(data);
                ClaimCountDTOList.ClaimCountDTOS = ClaimCountlist;
            }
            
            return View(ClaimCountDTOList);
        }

        // Get: ClaimCount/Create
        public IActionResult Create()
        {
            ClaimCountDTO ClaimCountDTO = new ClaimCountDTO();

            return View(ClaimCountDTO);
        }
        
        // POST: ClaimCount/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, StateId, YearOF, DesignationId, Count")] ClaimCountDTO ClaimCountDTO)
        {
            try
            {

                ClaimCountDTO.CreatedDate = DateTime.UtcNow;
                ClaimCountDTO.ModifiedDate = DateTime.UtcNow;
                ClaimCountDTO.CreatedByUserId = CurrentUserId;
                ClaimCountDTO.ModifiedByUserId = CurrentUserId;
                HttpResponseMessage response = await HttpTaskService.PostAsyncData(apiUrl, ClaimCountDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "ClaimCount Added successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var ClaimCountlist = Newtonsoft.Json.JsonConvert.DeserializeObject<ClaimCountDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Adding ClaimCount : Try Again";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }







        // GET: ClaimCount/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ClaimCountDTO ClaimCountDTO = new ClaimCountDTO();
           
            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                ClaimCountDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<ClaimCountDTO>(data);                
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading ClaimCount : Try Again";
            }
            if (ClaimCountDTO == null)
            {
                return NotFound();
            }
            return View(ClaimCountDTO);
        }


        // POST: ClaimCount/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, StateId, YearOF, DesignationId, Count")] ClaimCountDTO ClaimCountDTO)
        {
            if (id != ClaimCountDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                  
                ClaimCountDTO.ModifiedDate = DateTime.UtcNow;
                ClaimCountDTO.ModifiedByUserId = CurrentUserId;
             

                apiUrl = apiUrl + "/" + id;
                HttpResponseMessage response = await HttpTaskService.PutAsyncData(apiUrl, ClaimCountDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "ClaimCount Editted successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var ClaimCountlist = Newtonsoft.Json.JsonConvert.DeserializeObject<ClaimCountDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Editing ClaimCount : Try Again";
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(ClaimCountDTO);
        }







        // GET: ClaimCountDTOes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ClaimCountDTO ClaimCountDTO = new ClaimCountDTO();

            HttpResponseMessage response = await HttpTaskService .GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                ClaimCountDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<ClaimCountDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading ClaimCount : Try Again";
            }
            if (ClaimCountDTO == null)
            {
                return NotFound();
            }
            return View(ClaimCountDTO);
           
        }






        // GET: ClaimCountDTOes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ClaimCountDTO ClaimCountDTO = new ClaimCountDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                ClaimCountDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<ClaimCountDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading ClaimCount : Try Again";
            }
            if (ClaimCountDTO == null)
            {
                return NotFound();
            }
            return View(ClaimCountDTO);

        }


        // POST: ClaimCountDTOes/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            apiUrl = apiUrl + "/" + id;
            HttpResponseMessage response = await HttpTaskService.DeleteAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                TempData["SucessAlert"] = "ClaimCount Deleted successfully";
                string result = response.Content.ReadAsStringAsync().Result;
                var returndata = await response.Content.ReadAsStringAsync();
                var categorylist = Newtonsoft.Json.JsonConvert.DeserializeObject<ClaimCountDTO>(returndata);

            }
            else
            {
                TempData["ErrorAlert"] = "Error in Deleting ClaimCount : Try Again";
            }

            return RedirectToAction(nameof(Index));
        }











    }
}