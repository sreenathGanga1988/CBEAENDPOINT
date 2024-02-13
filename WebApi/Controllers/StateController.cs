using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApi.Controllers;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class StateController : AdminBaseController
    {
        private readonly ILogger<StateController> _logger;

        string apiUrl = "api/state" +
            "";
        public StateController(ILogger<StateController> logger, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _logger = logger;
            
        }
        public async Task<IActionResult> IndexAsync()
        {


            StateDTOList stateDTOList = new StateDTOList();
            stateDTOList.States = new List<StateDTO>();



            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var statelist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<StateDTO>>(data);
                stateDTOList.States = statelist;
            }

            return View(stateDTOList);
        }

        // Get: State/Create
        public IActionResult Create()
        {
            StateDTO stateDTO = new StateDTO();

            return View(stateDTO);
        }

        // POST: State/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID, Name, Abbreviation, CreatedByUserID, CreatedDate,AddedUser,IsActive")] StateDTO stateDTO)
        {
            try
            {

                stateDTO.CreatedDate = DateTime.UtcNow;
                stateDTO.ModifiedDate = DateTime.UtcNow;
                HttpResponseMessage response = await HttpTaskService.PostAsyncData(apiUrl, stateDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "State Added successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var statelist = Newtonsoft.Json.JsonConvert.DeserializeObject<StateDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Adding State : Try Again";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }







        // GET: State/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StateDTO stateDTO = new StateDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                stateDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<StateDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading State : Try Again";
            }
            if (stateDTO == null)
            {
                return NotFound();
            }
            return View(stateDTO);
        }


        // POST: State/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Abbreviation, CreatedByUserID, CreatedDate, AddedUser, IsActive")] StateDTO stateDTO)
        {
            if (id != stateDTO.Id)
            {
                return NotFound();
            }
           
            stateDTO.ModifiedByUserId = CurrentUserId;

            try
            {
                stateDTO.ModifiedDate = DateTime.UtcNow;


                apiUrl = apiUrl + "/" + id;
                HttpResponseMessage response = await HttpTaskService.PutAsyncData(apiUrl, stateDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "State Editted successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var statelist = Newtonsoft.Json.JsonConvert.DeserializeObject<StateDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Editing State : Try Again";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                return View(stateDTO);
            }

          
        
            
        }







        // GET: StateDTOes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StateDTO stateDTO = new StateDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                stateDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<StateDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading State : Try Again";
            }
            if (stateDTO == null)
            {
                return NotFound();
            }
            return View(stateDTO);

        }






        // GET: StateDTOes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StateDTO stateDTO = new StateDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                stateDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<StateDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading State : Try Again";
            }
            if (stateDTO == null)
            {
                return NotFound();
            }
            return View(stateDTO);

        }



        // POST: StateDTOes/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            apiUrl = apiUrl + "/" + id;
            HttpResponseMessage response = await HttpTaskService.DeleteAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                TempData["SucessAlert"] = "State Deleted successfully";
                string result = response.Content.ReadAsStringAsync().Result;
                var returndata = await response.Content.ReadAsStringAsync();
                var categorylist = Newtonsoft.Json.JsonConvert.DeserializeObject<StateDTO>(returndata);

            }
            else
            {
                TempData["ErrorAlert"] = "Error in Deleting State : Try Again";
            }

            return RedirectToAction(nameof(Index));
        }











    }
}