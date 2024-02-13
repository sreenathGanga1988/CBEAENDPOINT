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
    public class NewsItemController : AdminBaseController
    {
        private readonly ILogger<NewsItemController> _logger;

        string apiUrl = "api/NewsItem" +
            "";
        public NewsItemController(ILogger<NewsItemController> logger, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _logger = logger;
            
        }
        public async Task<IActionResult> IndexAsync()
        {


            NewsItemDTOList NewsItemDTOList = new NewsItemDTOList();
            NewsItemDTOList.newsitems = new List<NewsItemDTO>();



            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var NewsItemlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NewsItemDTO>>(data);
                NewsItemDTOList.newsitems = NewsItemlist;
            }
            
            return View(NewsItemDTOList);
        }

        // Get: NewsItem/Create
        public IActionResult Create()
        {
            NewsItemDTO NewsItemDTO = new NewsItemDTO();

            return View(NewsItemDTO);
        }
        
        // POST: NewsItem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, DateofAction, NewsText, NewsLink")] NewsItemDTO NewsItemDTO)
        {
            try
            {

             
                HttpResponseMessage response = await HttpTaskService.PostAsyncData(apiUrl, NewsItemDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "NewsItem Added successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var NewsItemlist = Newtonsoft.Json.JsonConvert.DeserializeObject<NewsItemDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Adding NewsItem : Try Again";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }







        // GET: NewsItem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NewsItemDTO NewsItemDTO = new NewsItemDTO();
           
            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                NewsItemDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<NewsItemDTO>(data);                
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading NewsItem : Try Again";
            }
            if (NewsItemDTO == null)
            {
                return NotFound();
            }
            return View(NewsItemDTO);
        }


        // POST: NewsItem/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, DateofAction, NewsText, NewsLink")] NewsItemDTO NewsItemDTO)
        {
            if (id != NewsItemDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                  
         
             

                apiUrl = apiUrl + "/" + id;
                HttpResponseMessage response = await HttpTaskService.PutAsyncData(apiUrl, NewsItemDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "NewsItem Editted successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var NewsItemlist = Newtonsoft.Json.JsonConvert.DeserializeObject<NewsItemDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Editing NewsItem : Try Again";
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(NewsItemDTO);
        }







        // GET: NewsItemDTOes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NewsItemDTO NewsItemDTO = new NewsItemDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                NewsItemDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<NewsItemDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading NewsItem : Try Again";
            }
            if (NewsItemDTO == null)
            {
                return NotFound();
            }
            return View(NewsItemDTO);
           
        }






        // GET: NewsItemDTOes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NewsItemDTO NewsItemDTO = new NewsItemDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                NewsItemDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<NewsItemDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading NewsItem : Try Again";
            }
            if (NewsItemDTO == null)
            {
                return NotFound();
            }
            return View(NewsItemDTO);
        
    }



        // POST: NewsItemDTOes/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            apiUrl = apiUrl + "/" + id;
            HttpResponseMessage response = await HttpTaskService.DeleteAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                TempData["SucessAlert"] = "NewsItem Deleted successfully";
                string result = response.Content.ReadAsStringAsync().Result;
                var returndata = await response.Content.ReadAsStringAsync();
                var categorylist = Newtonsoft.Json.JsonConvert.DeserializeObject<NewsItemDTO>(returndata);

            }
            else
            {
                TempData["ErrorAlert"] = "Error in Deleting NewsItem : Try Again";
            }

            return RedirectToAction(nameof(Index));
        }



         [HttpPost, ActionName("DeleteAll")]
        public async Task<IActionResult> DeleteAll(DateTime deldate)
        {
            apiUrl = apiUrl + "/RemoveNewsItem";
            HttpResponseMessage response = await HttpTaskService.PostAsyncData(apiUrl, deldate.ToString("dd MMMM yyyy"));
            if (response.IsSuccessStatusCode)
            {
                TempData["SucessAlert"] = "NewsItem Deleted successfully";
                string result = response.Content.ReadAsStringAsync().Result;
                var returndata = await response.Content.ReadAsStringAsync();
                var categorylist = Newtonsoft.Json.JsonConvert.DeserializeObject<NewsItemDTO>(returndata);

            }
            else
            {
                TempData["ErrorAlert"] = "Error in Deleting NewsItem : Try Again";
            }

            return RedirectToAction(nameof(Index));
        }





    }
}