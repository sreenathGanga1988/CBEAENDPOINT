using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DTO;
using ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApi.Controllers;
using WebApp.Controllers;
using WebApp.Services;

namespace WebApi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ApprovalController : AdminBaseController
    {
        private readonly ILogger<ApprovalController> _logger;

        string apiUrl = "api/AdminApproval" +
            "";

        enum ApprovalTypes
        {
            PendingDirectContribution = 1,
            PendingContribution = 2,
            PendingUser = 3,

        }

        public ApprovalController(ILogger<ApprovalController> logger, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _logger = logger;

        }



        public IActionResult Index()
        {

            return View();
        }

        public async Task<IActionResult> UserApproval()
        {
            var id = ApprovalTypes.PendingUser.ToString().ToUpper();
            List<UserDTO> pendinglist = new List<UserDTO>();
            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "?id=" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                pendinglist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserDTO>>(data);

            }
            return View(pendinglist);
        }

        public async Task<IActionResult> DirectEntryApproval()
        {
            var id = ApprovalTypes.PendingDirectContribution.ToString().ToUpper();
            List<AccountsDirectEntryDTO> pendinglist = new List<AccountsDirectEntryDTO>();
            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "?id=" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                pendinglist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AccountsDirectEntryDTO>>(data);

            }
            return View(pendinglist);
        }
        public async Task<IActionResult> ContributionApproval()
        {
            var id = ApprovalTypes.PendingContribution.ToString().ToUpper();
            List<ContributionMasterDTO> pendinglist = new List<ContributionMasterDTO>();
            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "?id=" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                pendinglist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ContributionMasterDTO>>(data);

            }
            return View(pendinglist);
        }
        public async Task<IActionResult> ClaimEntryApproval()
        {
            var id = "";
            List<UserDTO> pendinglist = new List<UserDTO>();
            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                pendinglist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserDTO>>(data);

            }
            return View(pendinglist);
        }




        [HttpGet]
        public async Task<IActionResult> ApproveAsync(int id, string type = "", string tag = "")
        {
            Boolean issucess = false;

            ApproveDTO approveDTO = new ApproveDTO();
            approveDTO.ID = id;
            approveDTO.type = tag;
            approveDTO.UseriD = CurrentUserId;
            if (type.TrimUpper() == "A")
            {
                approveDTO.approve = true;
            }
            else
            {
                approveDTO.approve = false;

            }

            string apiUrl = "api/AdminApproval" ;
            HttpResponseMessage response = await HttpTaskService.PostAsyncData(apiUrl, approveDTO);
            if (response.IsSuccessStatusCode)
            {
                TempData["SucessAlert"] = "Approval successfully";
               

            }
            else
            {
                TempData["ErrorAlert"] = "Error in Approving : Try Again";
            }

            return RedirectToAction("Index");
        }


        public async Task<bool> ApproveUserAsync(int id, string type)
        {
            Boolean issucess = false;


            string apiUrl = "api/user" + "";

            UserDTO userDTO = new UserDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            HttpTaskService.token = HttpContext.Session.GetString("token");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                userDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDTO>(data);

                if (userDTO.UserStatus.TrimUpper() == "P")
                {

                    if (type.TrimUpper() == "P")
                    {
                        userDTO.IsActive = true;
                        userDTO.IsDeleted = false;
                        userDTO.UserStatus = "A";
                    }
                    else
                    {
                        userDTO.IsActive = false;
                        userDTO.IsDeleted = false;
                        userDTO.UserStatus = "R";
                    }


                    HttpResponseMessage response1 = await HttpTaskService.PutAsyncData(apiUrl, userDTO);
                    if (response1.IsSuccessStatusCode)
                    {
                        issucess = true;
                        TempData["SucessAlert"] = "User Approved  successfully";
                    }
                    else
                    {
                        TempData["ErrorAlert"] = "Error in Approving  User : Try Again";
                    }
                }
                else
                {
                    TempData["ErrorAlert"] = "User Already Approved";
                }
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Fetching User Details : Try Again";
            }
            return issucess;

        }

        public async Task<bool> ApproveDirectContributionAsync(int id, string type)
        {
            Boolean issucess = false;


            string apiUrl = "api/AccountsDirectEntry" + "";

            AccountsDirectEntryDTO directEntry = new AccountsDirectEntryDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            HttpTaskService.token = HttpContext.Session.GetString("token");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                directEntry = Newtonsoft.Json.JsonConvert.DeserializeObject<AccountsDirectEntryDTO>(data);

                if (directEntry.status.TrimUpper() == "P")
                {

                    if (type.TrimUpper() == "P")
                    {
                        directEntry.isApproved = true;
                        directEntry.status = "A";
                        directEntry.ApprovedDate = DateTime.UtcNow.ToString();
                        directEntry.ApprovedBy = CurrentUserId.ToString();
                    }
                    else
                    {
                        directEntry.isApproved = false;
                        directEntry.status = "R";
                        directEntry.ApprovedDate = DateTime.UtcNow.ToString();
                        directEntry.ApprovedBy = CurrentUserId.ToString();
                    }


                    HttpResponseMessage response1 = await HttpTaskService.PutAsyncData(apiUrl, directEntry);
                    if (response1.IsSuccessStatusCode)
                    {
                        issucess = true;
                        TempData["SucessAlert"] = "User Approved  successfully";
                    }
                    else
                    {
                        TempData["ErrorAlert"] = "Error in Approving  User : Try Again";
                    }
                }
                else
                {
                    TempData["ErrorAlert"] = "User Already Approved";
                }
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Fetching User Details : Try Again";
            }
            return issucess;

        }

    }
}
