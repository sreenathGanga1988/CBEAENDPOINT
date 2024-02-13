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
   
    public class DeathClaimController : AdminBaseController
    {
        private readonly ILogger<DeathClaimController> _logger;

        string apiUrl = "api/DeathClaim" +
            "";
        public DeathClaimController(ILogger<DeathClaimController> logger, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _logger = logger;
    
        }
        public async Task<IActionResult> IndexAsync()
        {


            DeathClaimDTOList DeathClaimDTOList = new DeathClaimDTOList();
            DeathClaimDTOList.DeathClaimDTOs = new List<DeathClaimDTO>();



            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var DeathClaimlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DeathClaimDTO>>(data);
                DeathClaimDTOList.DeathClaimDTOs = DeathClaimlist;
            }
            
            return View(DeathClaimDTOList);
        }

        // Get: DeathClaim/Create
        public IActionResult Create()
        {
            DeathClaimDTO DeathClaimDTO = new DeathClaimDTO();

            return View(DeathClaimDTO);
        }
        
        // POST: DeathClaim/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, StaffNo,StateId,DesignationId,DeathDate,Nominee,NomineeRelation,NomineeIDentity,DDNO,DDDATE,LastContribution,Amount")] DeathClaimDTO DeathClaimDTO)
        {
            try
            {

                DeathClaimDTO.CreatedDate = DateTime.UtcNow;
                DeathClaimDTO.ModifiedDate = DateTime.UtcNow;
                DeathClaimDTO.CreatedByUserId = CurrentUserId;
                DeathClaimDTO.ModifiedByUserId = CurrentUserId;
                DeathClaimDTO.YearOF = DateTime.Parse ( DeathClaimDTO.DDDATE.ToString ()).Year;
                HttpResponseMessage response = await HttpTaskService.PostAsyncData(apiUrl, DeathClaimDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "DeathClaim Added successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var DeathClaimlist = Newtonsoft.Json.JsonConvert.DeserializeObject<DeathClaimDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Adding DeathClaim : Try Again";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }







        // GET: DeathClaim/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DeathClaimDTO DeathClaimDTO = new DeathClaimDTO();
           
            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                DeathClaimDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<DeathClaimDTO>(data);                
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading DeathClaim : Try Again";
            }
            if (DeathClaimDTO == null)
            {
                return NotFound();
            }
            return View(DeathClaimDTO);
        }


        // POST: DeathClaim/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StaffNo,StateId,DesignationId,DeathDate,Nominee,NomineeRelation,NomineeIDentity,DDNO,DDDATE,LastContribution,Amount")] DeathClaimDTO DeathClaimDTO)
        {
            if (id != DeathClaimDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                  
                DeathClaimDTO.ModifiedDate = DateTime.UtcNow;
                DeathClaimDTO.ModifiedByUserId = CurrentUserId;
                DeathClaimDTO.YearOF = DateTime.Parse(DeathClaimDTO.DDDATE.ToString()).Year;

                apiUrl = apiUrl + "/" + id;
                HttpResponseMessage response = await HttpTaskService.PutAsyncData(apiUrl, DeathClaimDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "DeathClaim Editted successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var DeathClaimlist = Newtonsoft.Json.JsonConvert.DeserializeObject<DeathClaimDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Editing DeathClaim : Try Again";
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(DeathClaimDTO);
        }







        // GET: DeathClaimDTOes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DeathClaimDTO DeathClaimDTO = new DeathClaimDTO();

            HttpResponseMessage response = await HttpTaskService .GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                DeathClaimDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<DeathClaimDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading DeathClaim : Try Again";
            }
            if (DeathClaimDTO == null)
            {
                return NotFound();
            }
            return View(DeathClaimDTO);
           
        }






        // GET: DeathClaimDTOes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DeathClaimDTO DeathClaimDTO = new DeathClaimDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                DeathClaimDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<DeathClaimDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading DeathClaim : Try Again";
            }
            if (DeathClaimDTO == null)
            {
                return NotFound();
            }
            return View(DeathClaimDTO);

        }


        // POST: DeathClaimDTOes/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            apiUrl = apiUrl + "/" + id;
            HttpResponseMessage response = await HttpTaskService.DeleteAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                TempData["SucessAlert"] = "DeathClaim Deleted successfully";
                string result = response.Content.ReadAsStringAsync().Result;
                var returndata = await response.Content.ReadAsStringAsync();
                var categorylist = Newtonsoft.Json.JsonConvert.DeserializeObject<DeathClaimDTO>(returndata);

            }
            else
            {
                TempData["ErrorAlert"] = "Error in Deleting DeathClaim : Try Again";
            }

            return RedirectToAction(nameof(Index));
        }











    }
}