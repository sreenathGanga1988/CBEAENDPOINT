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
    public class UserTypeController : AdminBaseController
    {
        private readonly ILogger<UserTypeController> _logger;

        string apiUrl = "api/UserType" +
            "";
        public UserTypeController(ILogger<UserTypeController> logger, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _logger = logger;
            
        }
        public async Task<IActionResult> IndexAsync()
        {


            UserTypeDTOList UserTypeDTOList = new UserTypeDTOList();
            UserTypeDTOList.UserTypes = new List<UserTypeDTO>();



            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var UserTypelist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserTypeDTO>>(data);
                UserTypeDTOList.UserTypes = UserTypelist;
            }
            
            return View(UserTypeDTOList);
        }

        // Get: UserType/Create
        public IActionResult Create()
        {
            UserTypeDTO UserTypeDTO = new UserTypeDTO();

            return View(UserTypeDTO);
        }
        
        // POST: UserType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id", "Abbreviation", "Description")] UserTypeDTO UserTypeDTO)
        {
            try
            {

             
                HttpResponseMessage response = await HttpTaskService.PostAsyncData(apiUrl, UserTypeDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "UserType Added successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var UserTypelist = Newtonsoft.Json.JsonConvert.DeserializeObject<UserTypeDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Adding UserType : Try Again";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }







        // GET: UserType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserTypeDTO UserTypeDTO = new UserTypeDTO();
           
            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                UserTypeDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<UserTypeDTO>(data);                
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading UserType : Try Again";
            }
            if (UserTypeDTO == null)
            {
                return NotFound();
            }
            return View(UserTypeDTO);
        }


        // POST: UserType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id", "Abbreviation", "Description")] UserTypeDTO UserTypeDTO)
        {
            if (id != UserTypeDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                  
         
             

                apiUrl = apiUrl + "/" + id;
                HttpResponseMessage response = await HttpTaskService.PutAsyncData(apiUrl, UserTypeDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "UserType Editted successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var UserTypelist = Newtonsoft.Json.JsonConvert.DeserializeObject<UserTypeDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Editing UserType : Try Again";
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(UserTypeDTO);
        }







        // GET: UserTypeDTOes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserTypeDTO UserTypeDTO = new UserTypeDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                UserTypeDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<UserTypeDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading UserType : Try Again";
            }
            if (UserTypeDTO == null)
            {
                return NotFound();
            }
            return View(UserTypeDTO);
           
        }






        // GET: UserTypeDTOes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserTypeDTO UserTypeDTO = new UserTypeDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                UserTypeDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<UserTypeDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading UserType : Try Again";
            }
            if (UserTypeDTO == null)
            {
                return NotFound();
            }
            return View(UserTypeDTO);
        
    }



        // POST: UserTypeDTOes/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            apiUrl = apiUrl + "/" + id;
            HttpResponseMessage response = await HttpTaskService.DeleteAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                TempData["SucessAlert"] = "UserType Deleted successfully";
                string result = response.Content.ReadAsStringAsync().Result;
                var returndata = await response.Content.ReadAsStringAsync();
                var categorylist = Newtonsoft.Json.JsonConvert.DeserializeObject<UserTypeDTO>(returndata);

            }
            else
            {
                TempData["ErrorAlert"] = "Error in Deleting UserType : Try Again";
            }

            return RedirectToAction(nameof(Index));
        }










    }
}