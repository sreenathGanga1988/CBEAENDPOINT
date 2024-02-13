using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DTO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApi.Controllers;
using WebApp.Services;

namespace WebApp.Controllers
{
    
    public class CategoryController : AdminBaseController
    {
        private readonly ILogger<CategoryController> _logger;

        string apiUrl = "api/category" +
            "";
        public CategoryController(ILogger<CategoryController> logger, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
           
            _logger = logger;
     
        }
        public async Task<IActionResult> IndexAsync()
        {


            CategoryDTOList categoryDTOList = new CategoryDTOList();
            categoryDTOList.Categorys = new List<CategoryDTO>();

          //  HttpTaskService.token = HttpContext.Session.GetString("token");
            
            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var categorylist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CategoryDTO>>(data);
                categoryDTOList.Categorys = categorylist;
            }
            
            return View(categoryDTOList);
        }

        // Get: Category/Create
        public IActionResult Create()
        {
            CategoryDTO categoryDTO = new CategoryDTO();

           
            return View(categoryDTO);
        }
        
        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID, Name, Abbreviation, CreatedByUserID, CreatedDate,AddedUser,IsActive, IsDeleted")] CategoryDTO categoryDTO)
        {
            try
            {
                categoryDTO.CreatedByUserId = CurrentUserId;
                categoryDTO.CreatedDate = DateTime.UtcNow;
                categoryDTO.ModifiedDate = null;
              
                HttpResponseMessage response = await HttpTaskService.PostAsyncData(apiUrl, categoryDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "Category Added successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var categorylist = Newtonsoft.Json.JsonConvert.DeserializeObject<CategoryDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Adding Category : Try Again";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }







        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CategoryDTO categoryDTO = new CategoryDTO();
           
            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                categoryDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<CategoryDTO>(data);                
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading Category : Try Again";
            }
            if (categoryDTO == null)
            {
                return NotFound();
            }
            return View(categoryDTO);
        }


        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Abbreviation, CreatedByUserID, CreatedDate, AddedUser")] CategoryDTO categoryDTO)
        {
            if (id != categoryDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                  
                categoryDTO.ModifiedDate = DateTime.UtcNow;
                categoryDTO.ModifiedByUserId = CurrentUserId;

                categoryDTO.IsActive = true;
                categoryDTO.IsDeleted = false;

                apiUrl = apiUrl + "/" + id;
                HttpResponseMessage response = await HttpTaskService.PutAsyncData(apiUrl, categoryDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "Category Editted successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var categorylist = Newtonsoft.Json.JsonConvert.DeserializeObject<CategoryDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Editing Category : Try Again";
                }
                
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                                       .Where(y => y.Count > 0)
                                       .ToList();

                int k = 6;
            }
            return View(categoryDTO);
        }







        // GET: CategoryDTOes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CategoryDTO categoryDTO = new CategoryDTO();

            HttpResponseMessage response = await HttpTaskService .GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                categoryDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<CategoryDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading Category : Try Again";
            }
            if (categoryDTO == null)
            {
                return NotFound();
            }
            return View(categoryDTO);
           
        }






        // GET: CategoryDTOes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CategoryDTO categoryDTO = new CategoryDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                categoryDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<CategoryDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading Category : Try Again";
            }
            if (categoryDTO == null)
            {
                return NotFound();
            }
            return View(categoryDTO);
        
    }




        // POST: CategoryDTOes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            apiUrl = apiUrl + "/" + id;
            HttpResponseMessage response = await HttpTaskService.DeleteAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                TempData["SucessAlert"] = "Category Deleted successfully";
                string result = response.Content.ReadAsStringAsync().Result;
                var returndata = await response.Content.ReadAsStringAsync();
                var categorylist = Newtonsoft.Json.JsonConvert.DeserializeObject<CategoryDTO>(returndata);

            }
            else
            {
                TempData["ErrorAlert"] = "Error in Deleted Category : Try Again";
            }

            return RedirectToAction(nameof(Index));
        }


        // POST: Masters/Categories/Delete/5






    }
}