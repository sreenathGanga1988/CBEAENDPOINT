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
    public class SupportTicketController : AdminBaseController
    {
        private readonly ILogger<SupportTicketController> _logger;
   
        string apiUrl = "api/supportticket" +            "";
        public SupportTicketController(ILogger<SupportTicketController> logger, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _logger = logger;
           
        }
        public async Task<IActionResult> IndexAsync()
        {


            SupportTicketDTOList SupportTicketDTOList = new SupportTicketDTOList();
            SupportTicketDTOList.SupportTickets = new List<SupportTicketDTO>();



            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var SupportTicketlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SupportTicketDTO>>(data);
                SupportTicketDTOList.SupportTickets = SupportTicketlist;
            }
            
            return View(SupportTicketDTOList);
        }

        // Get: SupportTicket/Create
        public IActionResult Create()
        {
            SupportTicketDTO SupportTicketDTO = new SupportTicketDTO();

            return View(SupportTicketDTO);
        }
        
        // POST: SupportTicket/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID, SupportTicketNum, Description, Priority, CreatedByUserId, CreatedDate, Duration, DeveloperRemark, isApproved")] SupportTicketDTO SupportTicketDTO)
        {
            try
            {
                SupportTicketDTO.CreatedByUserId = CurrentUserId;
                SupportTicketDTO.CreatedDate = DateTime.UtcNow;
                SupportTicketDTO.isApproved = false;
                //HttpTaskService.token = HttpContext.Session.GetString("token");
                HttpResponseMessage response = await HttpTaskService.PostAsyncData(apiUrl, SupportTicketDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "SupportTicket Added successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var SupportTicketlist = Newtonsoft.Json.JsonConvert.DeserializeObject<SupportTicketDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Adding SupportTicket : Try Again";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }







        // GET: SupportTicket/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SupportTicketDTO SupportTicketDTO = new SupportTicketDTO();
           
            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                SupportTicketDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<SupportTicketDTO>(data);                
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading SupportTicket : Try Again";
            }
            if (SupportTicketDTO == null)
            {
                return NotFound();
            }
            return View(SupportTicketDTO);
        }


        // POST: SupportTicket/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, SupportTicketNum, Description, Priority, CreatedByUserId, CreatedDate, Duration, DeveloperRemark, isApproved")] SupportTicketDTO SupportTicketDTO)
        {
            if (id != SupportTicketDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                SupportTicketDTO.isApproved = false;


                apiUrl = apiUrl + "/" + id;
                HttpResponseMessage response = await HttpTaskService.PutAsyncData(apiUrl, SupportTicketDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "SupportTicket Editted successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var SupportTicketlist = Newtonsoft.Json.JsonConvert.DeserializeObject<SupportTicketDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Editing SupportTicket : Try Again";
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(SupportTicketDTO);
        }







        // GET: SupportTicketDTOes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SupportTicketDTO SupportTicketDTO = new SupportTicketDTO();

            HttpResponseMessage response = await HttpTaskService .GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                SupportTicketDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<SupportTicketDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading SupportTicket : Try Again";
            }
            if (SupportTicketDTO == null)
            {
                return NotFound();
            }
            return View(SupportTicketDTO);
           
        }






        // GET: SupportTicketDTOes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SupportTicketDTO SupportTicketDTO = new SupportTicketDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                SupportTicketDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<SupportTicketDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading SupportTicket : Try Again";
            }
            if (SupportTicketDTO == null)
            {
                return NotFound();
            }
            return View(SupportTicketDTO);
        
    }


        // POST: SupportTicketDTOes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            apiUrl = apiUrl + "/" + id;
            HttpResponseMessage response = await HttpTaskService.DeleteAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                TempData["SucessAlert"] = "SupportTicket Deleted successfully";
                string result = response.Content.ReadAsStringAsync().Result;
                var returndata = await response.Content.ReadAsStringAsync();
                var categorylist = Newtonsoft.Json.JsonConvert.DeserializeObject<SupportTicketDTO>(returndata);

            }
            else
            {
                TempData["ErrorAlert"] = "Error in Deleting SupportTicket : Try Again";
            }

            return RedirectToAction(nameof(Index));
        }











    }
}