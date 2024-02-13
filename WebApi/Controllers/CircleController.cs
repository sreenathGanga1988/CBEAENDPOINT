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
   
    public class CircleController : AdminBaseController
    {
        private readonly ILogger<CircleController> _logger;

        string apiUrl = "api/circle" +
            "";
        public CircleController(ILogger<CircleController> logger, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _logger = logger;
    
        }
        public async Task<IActionResult> IndexAsync()
        {


            CircleDTOList circleDTOList = new CircleDTOList();
            circleDTOList.Circles = new List<CircleDTO>();



            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var circlelist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CircleDTO>>(data);
                circleDTOList.Circles = circlelist;
            }
            
            return View(circleDTOList);
        }

        // Get: Circle/Create
        public IActionResult Create()
        {
            CircleDTO circleDTO = new CircleDTO();

            return View(circleDTO);
        }
        
        // POST: Circle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID, CircleCode , Name, Abbreviation, IsActive, DateFrom, DateTo")] CircleDTO circleDTO)
        {
            try
            {

                circleDTO.CreatedDate = DateTime.UtcNow;
                circleDTO.ModifiedDate = DateTime.UtcNow;
                circleDTO.CreatedByUserId = CurrentUserId;
                circleDTO.ModifiedByUserId = CurrentUserId;
                HttpResponseMessage response = await HttpTaskService.PostAsyncData(apiUrl, circleDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "Circle Added successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var circlelist = Newtonsoft.Json.JsonConvert.DeserializeObject<CircleDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Adding Circle : Try Again";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }







        // GET: Circle/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CircleDTO circleDTO = new CircleDTO();
           
            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                circleDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<CircleDTO>(data);                
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading Circle : Try Again";
            }
            if (circleDTO == null)
            {
                return NotFound();
            }
            return View(circleDTO);
        }


        // POST: Circle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CircleCode, Name,Abbreviation ,CreatedByUserId ,CreatedDate, IsActive,DateFrom,DateTo")] CircleDTO circleDTO)
        {
            if (id != circleDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                  
                circleDTO.ModifiedDate = DateTime.UtcNow;
                circleDTO.ModifiedByUserId = CurrentUserId;
             

                apiUrl = apiUrl + "/" + id;
                HttpResponseMessage response = await HttpTaskService.PutAsyncData(apiUrl, circleDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "Circle Editted successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var circlelist = Newtonsoft.Json.JsonConvert.DeserializeObject<CircleDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Editing Circle : Try Again";
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(circleDTO);
        }







        // GET: CircleDTOes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CircleDTO circleDTO = new CircleDTO();

            HttpResponseMessage response = await HttpTaskService .GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                circleDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<CircleDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading Circle : Try Again";
            }
            if (circleDTO == null)
            {
                return NotFound();
            }
            return View(circleDTO);
           
        }






        // GET: CircleDTOes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CircleDTO circleDTO = new CircleDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                circleDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<CircleDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading Circle : Try Again";
            }
            if (circleDTO == null)
            {
                return NotFound();
            }
            return View(circleDTO);

        }


        // POST: CircleDTOes/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            apiUrl = apiUrl + "/" + id;
            HttpResponseMessage response = await HttpTaskService.DeleteAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                TempData["SucessAlert"] = "Circle Deleted successfully";
                string result = response.Content.ReadAsStringAsync().Result;
                var returndata = await response.Content.ReadAsStringAsync();
                var categorylist = Newtonsoft.Json.JsonConvert.DeserializeObject<CircleDTO>(returndata);

            }
            else
            {
                TempData["ErrorAlert"] = "Error in Deleting Circle : Try Again";
            }

            return RedirectToAction(nameof(Index));
        }











    }
}