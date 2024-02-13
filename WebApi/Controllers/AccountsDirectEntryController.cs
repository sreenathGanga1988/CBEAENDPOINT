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
    public class AccountsDirectEntryController : AdminBaseController
    {
        private readonly ILogger<AccountsDirectEntryController> _logger;

        string apiUrl = "api/AccountsDirectEntry" + "";
        public AccountsDirectEntryController(ILogger<AccountsDirectEntryController> logger, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _logger = logger;
            
        }
        public async Task<IActionResult> IndexAsync()
        {


            AccountsDirectEntryDTOList AccountsDirectEntryDTOList = new AccountsDirectEntryDTOList();
            AccountsDirectEntryDTOList.AccountsDirectEntrys = new List<AccountsDirectEntryDTO>();



            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var AccountsDirectEntrylist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AccountsDirectEntryDTO>>(data);
                AccountsDirectEntryDTOList.AccountsDirectEntrys = AccountsDirectEntrylist;
            }

            return View(AccountsDirectEntryDTOList);
        }

        // Get: AccountsDirectEntry/Create
        public IActionResult Create()
        {
            AccountsDirectEntryDTO AccountsDirectEntryDTO = new AccountsDirectEntryDTO();

            return View(AccountsDirectEntryDTO);
        }

        // POST: AccountsDirectEntry/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(" Id, Staff No, Name, DpCode, DdIbaDate, Amt, Enrl, Fine, F9, F10, F11")] AccountsDirectEntryDTO AccountsDirectEntryDTO)
        {
            try
            {

              
                HttpResponseMessage response = await HttpTaskService.PostAsyncData(apiUrl, AccountsDirectEntryDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "AccountsDirectEntry Added successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var AccountsDirectEntrylist = Newtonsoft.Json.JsonConvert.DeserializeObject<AccountsDirectEntryDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Adding AccountsDirectEntry : Try Again";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }







        // GET: AccountsDirectEntry/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AccountsDirectEntryDTO AccountsDirectEntryDTO = new AccountsDirectEntryDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                AccountsDirectEntryDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<AccountsDirectEntryDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading AccountsDirectEntry : Try Again";
            }
            if (AccountsDirectEntryDTO == null)
            {
                return NotFound();
            }
            return View(AccountsDirectEntryDTO);
        }


        // POST: AccountsDirectEntry/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Staff No, Name, DpCode, DdIbaDate, Amt, Enrl, Fine, F9, F10, F11")] AccountsDirectEntryDTO AccountsDirectEntryDTO)
        {
            if (id != AccountsDirectEntryDTO.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

              


                apiUrl = apiUrl + "/" + id;
                HttpResponseMessage response = await HttpTaskService.PutAsyncData(apiUrl, AccountsDirectEntryDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "AccountsDirectEntry Editted successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var AccountsDirectEntrylist = Newtonsoft.Json.JsonConvert.DeserializeObject<AccountsDirectEntryDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Editing AccountsDirectEntry : Try Again";
                }

                return RedirectToAction(nameof(Index));
            }
            return View(AccountsDirectEntryDTO);
        }







        // GET: AccountsDirectEntryDTOes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AccountsDirectEntryDTO AccountsDirectEntryDTO = new AccountsDirectEntryDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                AccountsDirectEntryDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<AccountsDirectEntryDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading AccountsDirectEntry : Try Again";
            }
            if (AccountsDirectEntryDTO == null)
            {
                return NotFound();
            }
            return View(AccountsDirectEntryDTO);

        }






        // GET: AccountsDirectEntryDTOes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AccountsDirectEntryDTO AccountsDirectEntryDTO = new AccountsDirectEntryDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                AccountsDirectEntryDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<AccountsDirectEntryDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading AccountsDirectEntry : Try Again";
            }
            if (AccountsDirectEntryDTO == null)
            {
                return NotFound();
            }
            return View(AccountsDirectEntryDTO);

        }


        // POST: AccountsDirectEntryDTOes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            apiUrl = apiUrl + "/" + id;
            HttpResponseMessage response = await HttpTaskService.DeleteAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                TempData["SucessAlert"] = "AccountsDirectEntry Deleted successfully";
                string result = response.Content.ReadAsStringAsync().Result;
                var returndata = await response.Content.ReadAsStringAsync();
                var categorylist = Newtonsoft.Json.JsonConvert.DeserializeObject<AccountsDirectEntryDTO>(returndata);

            }
            else
            {
                TempData["ErrorAlert"] = "Error in Deleting AccountsDirectEntry : Try Again";
            }

            return RedirectToAction(nameof(Index));
        }











    }
}