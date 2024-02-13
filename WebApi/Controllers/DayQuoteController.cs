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

    public class DayQuoteController : AdminBaseController
    {
        private readonly ILogger<DayQuoteController> _logger;

        string apiUrl = "api/DayQuote" +
            "";
        public DayQuoteController(ILogger<DayQuoteController> logger, IHttpContextAccessor contextAccessor): base (contextAccessor)
        {
           
            _logger = logger;
     
        }
        public async Task<IActionResult> IndexAsync()
        {


            DayQuoteDTOList DayQuoteDTOList = new DayQuoteDTOList();
            DayQuoteDTOList.DayQuotes = new List<DayQuoteDTO>();

            //HttpTaskService.token = HttpContext.Session.GetString("token");

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var DayQuotelist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DayQuoteDTO>>(data);
                DayQuoteDTOList.DayQuotes = DayQuotelist;
            }
            
            return View(DayQuoteDTOList);
        }

        // Get: DayQuote/Create
        public IActionResult Create()
        {
            DayQuoteDTO DayQuoteDTO = new DayQuoteDTO();

           
            return View(DayQuoteDTO);
        }
        
        // POST: DayQuote/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Day, MonthCode, ToDayQuote,  UnformatedContent")] DayQuoteDTO DayQuoteDTO)
        {
            try
            {
                //HttpTaskService.token = HttpContext.Session.GetString("token");
                DayQuoteDTO.CreatedByUserId = CurrentUserId;
                DayQuoteDTO.CreatedDate = DateTime.UtcNow;
                DayQuoteDTO.ModifiedDate = null;
                DayQuoteDTO.ModifiedByUserId = CurrentUserId;
                HttpResponseMessage response = await HttpTaskService.PostAsyncData(apiUrl, DayQuoteDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "DayQuote Added successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var DayQuotelist = Newtonsoft.Json.JsonConvert.DeserializeObject<DayQuoteDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Adding DayQuote : Try Again";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }







        // GET: DayQuote/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DayQuoteDTO DayQuoteDTO = new DayQuoteDTO();
           
            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                DayQuoteDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<DayQuoteDTO>(data);                
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading DayQuote : Try Again";
            }
            if (DayQuoteDTO == null)
            {
                return NotFound();
            }
            return View(DayQuoteDTO);
        }


        // POST: DayQuote/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Day, MonthCode, ToDayQuote, CreatedDate, UnformatedContent, CreatedByUserId, CreatedDate")] DayQuoteDTO DayQuoteDTO)
        {
            if (id != DayQuoteDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                  
                DayQuoteDTO.ModifiedDate = DateTime.UtcNow;
                DayQuoteDTO.ModifiedByUserId = CurrentUserId;

               

                apiUrl = apiUrl + "/" + id;
                HttpResponseMessage response = await HttpTaskService.PutAsyncData(apiUrl, DayQuoteDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "DayQuote Editted successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var DayQuotelist = Newtonsoft.Json.JsonConvert.DeserializeObject<DayQuoteDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Editing DayQuote : Try Again";
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
            return View(DayQuoteDTO);
        }







        // GET: DayQuoteDTOes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DayQuoteDTO DayQuoteDTO = new DayQuoteDTO();

            HttpResponseMessage response = await HttpTaskService .GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                DayQuoteDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<DayQuoteDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading DayQuote : Try Again";
            }
            if (DayQuoteDTO == null)
            {
                return NotFound();
            }
            return View(DayQuoteDTO);
           
        }






        // GET: DayQuoteDTOes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DayQuoteDTO DayQuoteDTO = new DayQuoteDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                DayQuoteDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<DayQuoteDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading DayQuote : Try Again";
            }
            if (DayQuoteDTO == null)
            {
                return NotFound();
            }
            return View(DayQuoteDTO);
        
    }




        // POST: DayQuoteDTOes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            apiUrl = apiUrl + "/" + id;
            HttpResponseMessage response = await HttpTaskService.DeleteAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                TempData["SucessAlert"] = "DayQuote Deleted successfully";
                string result = response.Content.ReadAsStringAsync().Result;
                var returndata = await response.Content.ReadAsStringAsync();
                var DayQuotelist = Newtonsoft.Json.JsonConvert.DeserializeObject<DayQuoteDTO>(returndata);

            }
            else
            {
                TempData["ErrorAlert"] = "Error in Deleted DayQuote : Try Again";
            }

            return RedirectToAction(nameof(Index));
        }


        // POST: Masters/Categories/Delete/5






    }
}