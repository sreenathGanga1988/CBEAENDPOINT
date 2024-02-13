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
    public class MainPageController : AdminBaseController
    {
        private readonly ILogger<MainPageController> _logger;

        string apiUrl = "api/MainPage" +
            "";
        public MainPageController(ILogger<MainPageController> logger, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _logger = logger;
            
        }
        public async Task<IActionResult> IndexAsync()
        {


            MainPageDTOList MainPageDTOList = new MainPageDTOList();
            MainPageDTOList.MainPages = new List<MainPageDTO>();



            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var MainPagelist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MainPageDTO>>(data);
                MainPageDTOList.MainPages = MainPagelist;
            }
            
            return View(MainPageDTOList);
        }

        // Get: MainPage/Create
        public IActionResult Create()
        {
            MainPageDTO MainPageDTO = new MainPageDTO();

            return View(MainPageDTO);
        }
        
        // POST: MainPage/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Slogan, DayQuote, MainText, RulesRegulation, CorouselImage1, CorouselImage2, CorouselImage3, LogoImage1, LogoImage2, ContactDesc1, ContactDesc2, ContactLine1, ContactLine2, ContactLine3, Phonenum, Faxnum, Website, Email")] MainPageDTO MainPageDTO)
        {
            try
            {

             
                HttpResponseMessage response = await HttpTaskService.PostAsyncData(apiUrl, MainPageDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "MainPage Added successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var MainPagelist = Newtonsoft.Json.JsonConvert.DeserializeObject<MainPageDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Adding MainPage : Try Again";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }







        // GET: MainPage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MainPageDTO MainPageDTO = new MainPageDTO();
           
            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                MainPageDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<MainPageDTO>(data);                
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading MainPage : Try Again";
            }
            if (MainPageDTO == null)
            {
                return NotFound();
            }
            return View(MainPageDTO);
        }


        // POST: MainPage/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Slogan, DayQuote, MainText,RulesRegulation, CorouselImage1, CorouselImage2, CorouselImage3, LogoImage1, LogoImage2, ContactDesc1, ContactDesc2, ContactLine1, ContactLine2, ContactLine3, Phonenum, Faxnum, Website, Email")] MainPageDTO MainPageDTO)
        {
            if (id != MainPageDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                  
         
             

                apiUrl = apiUrl + "/" + id;
                HttpResponseMessage response = await HttpTaskService.PutAsyncData(apiUrl, MainPageDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "MainPage Editted successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var MainPagelist = Newtonsoft.Json.JsonConvert.DeserializeObject<MainPageDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Editing MainPage : Try Again";
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(MainPageDTO);
        }







        // GET: MainPageDTOes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MainPageDTO MainPageDTO = new MainPageDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                MainPageDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<MainPageDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading MainPage : Try Again";
            }
            if (MainPageDTO == null)
            {
                return NotFound();
            }
            return View(MainPageDTO);
           
        }






        // GET: MainPageDTOes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MainPageDTO MainPageDTO = new MainPageDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                MainPageDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<MainPageDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading MainPage : Try Again";
            }
            if (MainPageDTO == null)
            {
                return NotFound();
            }
            return View(MainPageDTO);
        
    }


        // POST: MainPageDTOes/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            apiUrl = apiUrl + "/" + id;
            HttpResponseMessage response = await HttpTaskService.DeleteAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                TempData["SucessAlert"] = "MainPage Deleted successfully";
                string result = response.Content.ReadAsStringAsync().Result;
                var returndata = await response.Content.ReadAsStringAsync();
                var categorylist = Newtonsoft.Json.JsonConvert.DeserializeObject<MainPageDTO>(returndata);

            }
            else
            {
                TempData["ErrorAlert"] = "Error in Deleting MainPage : Try Again";
            }

            return RedirectToAction(nameof(Index));
        }










    }
}