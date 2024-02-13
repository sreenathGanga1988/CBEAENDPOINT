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
using WebApi.API_Controllers;
using WebApi.Controllers;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class BranchController : AdminBaseController
    {
        private readonly ILogger<BranchController> _logger;

        string apiUrl = "api/branch" +            "";
        public BranchController(ILogger<BranchController> logger, IHttpContextAccessor contextAccessor):base (contextAccessor)
        {
            _logger = logger;
           
        }
        public async Task<IActionResult> IndexAsync()
        {


            //BranchDTOList branchDTOList = new BranchDTOList();
            //branchDTOList.Branchs = new List<BranchDTO>();



            //HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl);
            //if (response.IsSuccessStatusCode)
            //{
            //    var data = await response.Content.ReadAsStringAsync();
            //    var branchlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BranchDTO>>(data);
            //    branchDTOList.Branchs = branchlist;
            //}
            
            //return View(branchDTOList);

            return View();
        }

        // Get: Branch/Create
        public IActionResult Create()
        {
            BranchDTO branchDTO = new BranchDTO();

            return View(branchDTO);
        }
        
        // POST: Branch/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Name, DpCode, CircleId, StateId, Address1, Address2, Address3, District, IsActive, IsRegCompleted")] BranchDTO branchDTO)
        {
            try
            {
                branchDTO.CreatedByUserId = CurrentUserId;
                branchDTO.CreatedDate = DateTime.UtcNow;
                branchDTO.ModifiedDate = DateTime.UtcNow;
                branchDTO.ModifiedByUserId = CurrentUserId;
                HttpResponseMessage response = await HttpTaskService.PostAsyncData(apiUrl, branchDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "Branch Added successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var branchlist = Newtonsoft.Json.JsonConvert.DeserializeObject<BranchDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Adding Branch : Try Again";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }







        // GET: Branch/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BranchDTO branchDTO = new BranchDTO();
           
            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                branchDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<BranchDTO>(data);                
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading Branch : Try Again";
            }
            if (branchDTO == null)
            {
                return NotFound();
            }
            return View(branchDTO);
        }


        // POST: Branch/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int DpCode, [Bind("Id, Name, DpCode, CircleId, StateId, Address1, Address2, Address3, District, IsActive, IsRegCompleted, CreatedByUserID, CreatedDate")] BranchDTO branchDTO)
        {
            if (DpCode != branchDTO.DpCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                branchDTO.ModifiedByUserId = CurrentUserId;
                branchDTO.ModifiedDate = DateTime.UtcNow;
                
                apiUrl = apiUrl + "/" + DpCode;
                HttpResponseMessage response = await HttpTaskService.PutAsyncData(apiUrl, branchDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "Branch Editted successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var branchlist = Newtonsoft.Json.JsonConvert.DeserializeObject<BranchDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Editing Branch : Try Again";
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(branchDTO);
        }







        // GET: BranchDTOes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BranchDTO branchDTO = new BranchDTO();

            HttpResponseMessage response = await HttpTaskService .GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                branchDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<BranchDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading Branch : Try Again";
            }
            if (branchDTO == null)
            {
                return NotFound();
            }
            return View(branchDTO);
           
        }






        // GET: BranchDTOes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BranchDTO branchDTO = new BranchDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                branchDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<BranchDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading Branch : Try Again";
            }
            if (branchDTO == null)
            {
                return NotFound();
            }
            return View(branchDTO);
        
    }


        // POST: BranchDTOes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            apiUrl = apiUrl + "/" + id;
            HttpResponseMessage response = await HttpTaskService.DeleteAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                TempData["SucessAlert"] = "Branch Deleted successfully";
                string result = response.Content.ReadAsStringAsync().Result;
                var returndata = await response.Content.ReadAsStringAsync();
                var categorylist = Newtonsoft.Json.JsonConvert.DeserializeObject<BranchDTO>(returndata);

            }
            else
            {
                TempData["ErrorAlert"] = "Error in Deleting Branch : Try Again";
            }

            return RedirectToAction(nameof(Index));
        }











    }
}