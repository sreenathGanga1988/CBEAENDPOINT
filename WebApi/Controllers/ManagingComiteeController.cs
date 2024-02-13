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
    public class ManagingComiteeController : AdminBaseController
    {
        private readonly ILogger<ManagingComiteeController> _logger;

        string apiUrl = "api/ManagingComitee" +
            "";
        public ManagingComiteeController(ILogger<ManagingComiteeController> logger, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _logger = logger;
    
        }
        public async Task<IActionResult> IndexAsync()
        {


            ManagingComiteeDTOList ManagingComiteeDTOList = new ManagingComiteeDTOList();
            ManagingComiteeDTOList.ManagingComitees = new List<ManagingComiteeDTO>();



            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var ManagingComiteelist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ManagingComiteeDTO>>(data);
                ManagingComiteeDTOList.ManagingComitees = ManagingComiteelist;
            }
            
            return View(ManagingComiteeDTOList);
        }

        // Get: ManagingComitee/Create
        public IActionResult Create()
        {
            ManagingComiteeDTO ManagingComiteeDTO = new ManagingComiteeDTO();

            return View(ManagingComiteeDTO);
        }
        
        // POST: ManagingComitee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Name, Position, Description1, Description2, imageLocation, order")] ManagingComiteeDTO ManagingComiteeDTO)
        {
            try
            {

                ManagingComiteeDTO.CreatedDate = DateTime.UtcNow;
                ManagingComiteeDTO.ModifiedDate = DateTime.UtcNow;
                ManagingComiteeDTO.CreatedByUserId = CurrentUserId;
                ManagingComiteeDTO.ModifiedByUserId = CurrentUserId;
                HttpResponseMessage response = await HttpTaskService.PostAsyncData(apiUrl, ManagingComiteeDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "ManagingComitee Added successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var ManagingComiteelist = Newtonsoft.Json.JsonConvert.DeserializeObject<ManagingComiteeDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Adding ManagingComitee : Try Again";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }







        // GET: ManagingComitee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ManagingComiteeDTO ManagingComiteeDTO = new ManagingComiteeDTO();
           
            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                ManagingComiteeDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<ManagingComiteeDTO>(data);                
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading ManagingComitee : Try Again";
            }
            if (ManagingComiteeDTO == null)
            {
                return NotFound();
            }
            return View(ManagingComiteeDTO);
        }


        // POST: ManagingComitee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Position, Description1, Description2, imageLocation, order")] ManagingComiteeDTO ManagingComiteeDTO)
        {
            if (id != ManagingComiteeDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                ManagingComiteeDTO.ModifiedDate = DateTime.UtcNow;
                ManagingComiteeDTO.ModifiedByUserId = CurrentUserId;
             

                apiUrl = apiUrl + "/" + id;
                HttpResponseMessage response = await HttpTaskService.PutAsyncData(apiUrl, ManagingComiteeDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "ManagingComitee Editted successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var ManagingComiteelist = Newtonsoft.Json.JsonConvert.DeserializeObject<ManagingComiteeDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Editing ManagingComitee : Try Again";
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(ManagingComiteeDTO);
        }







        // GET: ManagingComiteeDTOes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ManagingComiteeDTO ManagingComiteeDTO = new ManagingComiteeDTO();

            HttpResponseMessage response = await HttpTaskService .GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                ManagingComiteeDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<ManagingComiteeDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading ManagingComitee : Try Again";
            }
            if (ManagingComiteeDTO == null)
            {
                return NotFound();
            }
            return View(ManagingComiteeDTO);
           
        }






        // GET: ManagingComiteeDTOes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ManagingComiteeDTO ManagingComiteeDTO = new ManagingComiteeDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                ManagingComiteeDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<ManagingComiteeDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading ManagingComitee : Try Again";
            }
            if (ManagingComiteeDTO == null)
            {
                return NotFound();
            }
            return View(ManagingComiteeDTO);

        }


        // POST: ManagingComiteeDTOes/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            apiUrl = apiUrl + "/" + id;
            HttpResponseMessage response = await HttpTaskService.DeleteAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                TempData["SucessAlert"] = "ManagingComitee Deleted successfully";
                string result = response.Content.ReadAsStringAsync().Result;
                var returndata = await response.Content.ReadAsStringAsync();
                var categorylist = Newtonsoft.Json.JsonConvert.DeserializeObject<ManagingComiteeDTO>(returndata);

            }
            else
            {
                TempData["ErrorAlert"] = "Error in Deleting ManagingComitee : Try Again";
            }

            return RedirectToAction(nameof(Index));
        }











    }
}