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
using WebApi.Controllers;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class DesignationController : AdminBaseController
    {
        private readonly ILogger<DesignationController> _logger;

        string apiUrl = "api/designation" +
            "";
        public DesignationController(ILogger<DesignationController> logger, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _logger = logger;
            
        }
        public async Task<IActionResult> IndexAsync()
        {


            DesignationDTOList designationDTOList = new DesignationDTOList();
            designationDTOList.Designations = new List<DesignationDTO>();



            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var designationlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DesignationDTO>>(data);
                designationDTOList.Designations = designationlist;
            }
            
            return View(designationDTOList);
        }

        // Get: Designation/Create
        public IActionResult Create()
        {
            DesignationDTO designationDTO = new DesignationDTO();

            return View(designationDTO);
        }
        
        // POST: Designation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID, Name, Description,IsActive, IsDeleted")] DesignationDTO designationDTO)
        {
            try
            {
                designationDTO.CreatedByUserId = CurrentUserId;
                designationDTO.ModifiedByUserId = CurrentUserId;
                designationDTO.CreatedDate = DateTime.UtcNow;
                designationDTO.ModifiedDate = DateTime.UtcNow;
                HttpResponseMessage response = await HttpTaskService.PostAsyncData(apiUrl, designationDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "Designation Added successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var designationlist = Newtonsoft.Json.JsonConvert.DeserializeObject<DesignationDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Adding Designation : Try Again";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }







        // GET: Designation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DesignationDTO designationDTO = new DesignationDTO();
           
            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                designationDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<DesignationDTO>(data);                
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading Designation : Try Again";
            }
            if (designationDTO == null)
            {
                return NotFound();
            }
            return View(designationDTO);
        }


        // POST: Designation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Description, CreatedByUserId, CreatedDate, IsActive, IsDeleted")] DesignationDTO designationDTO)
        {
            if (id != designationDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                  
                designationDTO.ModifiedDate = DateTime.UtcNow;
                designationDTO.ModifiedByUserId = CurrentUserId;

                apiUrl = apiUrl + "/" + id;
                HttpResponseMessage response = await HttpTaskService.PutAsyncData(apiUrl, designationDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "Designation Editted successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var designationlist = Newtonsoft.Json.JsonConvert.DeserializeObject<DesignationDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Editing Designation : Try Again";
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(designationDTO);
        }







        // GET: DesignationDTOes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DesignationDTO designationDTO = new DesignationDTO();

            HttpResponseMessage response = await HttpTaskService .GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                designationDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<DesignationDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading Designation : Try Again";
            }
            if (designationDTO == null)
            {
                return NotFound();
            }
            return View(designationDTO);
           
        }






        // GET: DesignationDTOes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DesignationDTO designationDTO = new DesignationDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                designationDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<DesignationDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading Designation : Try Again";
            }
            if (designationDTO == null)
            {
                return NotFound();
            }
            return View(designationDTO);
        
    }




        // POST: DesignationDTOes/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            apiUrl = apiUrl + "/" + id;
            HttpResponseMessage response = await HttpTaskService.DeleteAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                TempData["SucessAlert"] = "Designation Deleted successfully";
                string result = response.Content.ReadAsStringAsync().Result;
                var returndata = await response.Content.ReadAsStringAsync();
                var categorylist = Newtonsoft.Json.JsonConvert.DeserializeObject<DesignationDTO>(returndata);

            }
            else
            {
                TempData["ErrorAlert"] = "Error in Deleting Designation : Try Again";
            }

            return RedirectToAction(nameof(Index));
        }








    }
}