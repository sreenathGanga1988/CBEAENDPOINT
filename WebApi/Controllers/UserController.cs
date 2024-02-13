using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApi.Controllers;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class UserController : AdminBaseController
    {
        private readonly ILogger<UserController> _logger;

        string apiUrl = "api/User" +
            "";
        public UserController(ILogger<UserController> logger, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _logger = logger;
           
        }
        public async Task<IActionResult> IndexAsync()
        {


            //UserDTOList UserDTOList = new UserDTOList();
            //UserDTOList.Users = new List<UserDTO>();



            //HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl);
            //if (response.IsSuccessStatusCode)
            //{
            //    var data = await response.Content.ReadAsStringAsync();
            //    var Userlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserDTO>>(data);
            //    UserDTOList.Users = Userlist;
            //}
            
            //return View(UserDTOList);

            return View();
        }

        // Get: User/Create
        public IActionResult Create()
        {
            UserDTO UserDTO = new UserDTO();

            return View(UserDTO);
        }
        
        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID, UserName, Password, UserTypeId, EmployeeId, EmailId, PhoneNum, IsActive")] UserDTO UserDTO)
        {
            try
            {
                UserDTO.CreatedByUserId = CurrentUserId ;
                UserDTO.CreatedDate = DateTime.UtcNow;
                UserDTO.ModifiedDate = null;
                UserDTO.IsActive = true;
                HttpResponseMessage response = await HttpTaskService.PostAsyncData(apiUrl, UserDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "User Added successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var Userlist = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Adding User : Try Again";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        [AllowAnonymous]
        // POST: User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("ID, UserName, Password, UserTypeId, EmployeeId, EmailId, PhoneNum, IsActive")] UserDTO UserDTO)
        {
            try
            {

                UserDTO.CreatedDate = DateTime.UtcNow;
                UserDTO.ModifiedDate = null;
                UserDTO.IsActive = false;
                UserDTO.UserTypeId = 3;
                HttpResponseMessage response = await HttpTaskService.PostAsyncData(apiUrl, UserDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "User Added successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var Userlist = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Adding User : Try Again";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }






        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserDTO UserDTO = new UserDTO();
          
            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                UserDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDTO>(data);                
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading User : Try Again";
            }
            if (UserDTO == null)
            {
                return NotFound();
            }
            return View(UserDTO);
        }


        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, UserName, Password, UserTypeId, EmployeeId,  EmailId, PhoneNum, CreatedByUserID, CreatedDate, AddedUser")] UserDTO UserDTO)
        {
            if (id != UserDTO.Id)
            {
                return NotFound();
            }
            UserDTO.IsActive = true;
            if (ModelState.IsValid)
            {
                UserDTO.ModifiedByUserId = CurrentUserId;
                UserDTO.ModifiedDate = DateTime.UtcNow;
             

                apiUrl = apiUrl + "/" + id;
                HttpResponseMessage response = await HttpTaskService.PutAsyncData(apiUrl, UserDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "User Editted successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var Userlist = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Editing User : Try Again";
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(UserDTO);
        }











        // GET: UserDTOes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserDTO UserDTO = new UserDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                UserDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading User : Try Again";
            }
            if (UserDTO == null)
            {
                return NotFound();
            }
            return View(UserDTO);
           
        }






        // GET: UserDTOes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserDTO UserDTO = new UserDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                UserDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading User : Try Again";
            }
            if (UserDTO == null)
            {
                return NotFound();
            }
            return View(UserDTO);
        
    }


        // POST: UserDTOes/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            apiUrl = apiUrl + "/" + id;
            HttpResponseMessage response = await HttpTaskService.DeleteAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                TempData["SucessAlert"] = "User Deleted successfully";
                string result = response.Content.ReadAsStringAsync().Result;
                var returndata = await response.Content.ReadAsStringAsync();
                var categorylist = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDTO>(returndata);

            }
            else
            {
                TempData["ErrorAlert"] = "Error in Deleting User : Try Again";
            }

            return RedirectToAction(nameof(Index));
        }




        // GET: User/Edit/5
        public async Task<IActionResult> Approve(int? id, Boolean isActive)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserDTO UserDTO = new UserDTO();
            apiUrl = apiUrl + "/" + id;
            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                UserDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDTO>(data);
                UserDTO.IsActive = isActive;
                HttpResponseMessage response1 = await HttpTaskService.PutAsyncData(apiUrl, UserDTO);
                if (response1.IsSuccessStatusCode)
                {
                    if (isActive == true)
                    {
                        TempData["SucessAlert"] = "User Activated successfully,Please Reload";
                    }
                    else
                    {
                        TempData["SucessAlert"] = "User DeActivated successfully,Please Reload";
                    }
                }
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Updating User : Try Again";
            }
            if (UserDTO == null)
            {
                return NotFound();
            }
            return View(UserDTO);
        }




    }
}