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
    public class MemberController : AdminBaseController
    {
        private readonly ILogger<MemberController> _logger;

        string apiUrl = "api/Member" +            "";
        public MemberController(ILogger<MemberController> logger, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _logger = logger;
       
        }
        public async Task<IActionResult> IndexAsync()
        {


           
            return View();
        }

        public async Task<IActionResult> IndexActiveAsync()
        {


            
            return View();
        }

        // Get: Member/Create
        public IActionResult Create()
        {
            MemberDTO MemberDTO = new MemberDTO();

            return View(MemberDTO);
        }
        
        // POST: Member/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID, Name, StaffNo, GenderId, Dob, DesignationId, CategoryId, Doj, DpCode, DojtoScheme, Nominee, NomineeRelation, NomineeIDentity, UnionMember, StatusId, IsRegCompleted ")] MemberDTO MemberDTO)
        {
            try
            {
                MemberDTO.CreatedByUserId = CurrentUserId;
                MemberDTO.ModifiedByUserId = CurrentUserId;
                MemberDTO.CreatedDate = DateTime.UtcNow;
                MemberDTO.ModifiedDate = DateTime.UtcNow;
                if (MemberDTO.CategoryId == null)
                {
                    MemberDTO.CategoryId = 20;
                }
                HttpResponseMessage response = await HttpTaskService.PostAsyncData(apiUrl, MemberDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "Member Added successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var Memberlist = Newtonsoft.Json.JsonConvert.DeserializeObject<MemberDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Adding Member : Try Again";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }







        // GET: Member/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MemberDTO MemberDTO = new MemberDTO();
           
            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                MemberDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<MemberDTO>(data);                
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading Member : Try Again";
            }
            if (MemberDTO == null)
            {
                return NotFound();
            }
            return View(MemberDTO);
        }


        // POST: Member/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int StaffNo, [Bind("Id, Name, StaffNo, GenderId, Dob, DesignationId, CategoryId, Doj, DpCode, DojtoScheme, Nominee, NomineeRelation, NomineeIDentity, UnionMember, StatusId, IsRegCompleted , CreatedByUserId , CreatedDate, AddedUser ")] MemberDTO MemberDTO)
        {
            if (StaffNo != MemberDTO.StaffNo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                MemberDTO.ModifiedByUserId = CurrentUserId;
                MemberDTO.ModifiedDate = DateTime.UtcNow;
             

                apiUrl = apiUrl + "/" + MemberDTO.Id;
                HttpResponseMessage response = await HttpTaskService.PutAsyncData(apiUrl, MemberDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "Member Editted successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var Memberlist = Newtonsoft.Json.JsonConvert.DeserializeObject<MemberDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Editing Member : Try Again";
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(MemberDTO);
        }







        // GET: MemberDTOes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MemberDTO MemberDTO = new MemberDTO();

            HttpResponseMessage response = await HttpTaskService .GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                MemberDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<MemberDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading Member : Try Again";
            }
            if (MemberDTO == null)
            {
                return NotFound();
            }
            return View(MemberDTO);
           
        }


        // GET: MemberDTOes/MemberReport/5
        public async Task<IActionResult> MemberReport(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //MemberDTO MemberDTO = new MemberDTO();

            //HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            //if (response.IsSuccessStatusCode)
            //{
            //    var data = await response.Content.ReadAsStringAsync();
            //    MemberDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<MemberDTO>(data);
            //}
            //else
            //{
            //    TempData["ErrorAlert"] = "Error in Loading Member : Try Again";
            //}
            //if (MemberDTO == null)
            //{
            //    return NotFound();
            //}


            ProfileDTO profileDTO = new ProfileDTO();
            List<ContributionlistDTO> contributions = new List<ContributionlistDTO>();
            HttpResponseMessage response = await HttpTaskService.GetAsyncData("api/UserProfile" + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                profileDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<ProfileDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading Profile : Try Again";
            }
            if (profileDTO == null)
            {
                return NotFound();
            }


           

          

            List<AccountsDTO> accountdtos = new List<AccountsDTO>();


            HttpResponseMessage response1 = await HttpTaskService.GetAsyncData("api/UserProfile/GetUserContribution" + "/" + id);
            if (response1.IsSuccessStatusCode)
            {
                var data = await response1.Content.ReadAsStringAsync();
                accountdtos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AccountsDTO>>(data);




                if (accountdtos.Count > 0)
                {

                    var Yearsdist = accountdtos.Select(adr => adr.Year).Distinct().OrderBy(Year => Year);

                    foreach (var item in Yearsdist)
                    {


                        ContributionlistDTO contributionlistDTO = new ContributionlistDTO();

                        contributionlistDTO.Year = item;
                        contributionlistDTO.Jan = accountdtos.Where(u => u.Year == item && u.Month == "1").Sum(u => u.Amount);
                        contributionlistDTO.Feb = accountdtos.Where(u => u.Year == item && u.Month == "2").Sum(u => u.Amount);
                        contributionlistDTO.Mar = accountdtos.Where(u => u.Year == item && u.Month == "3").Sum(u => u.Amount);
                        contributionlistDTO.Apr = accountdtos.Where(u => u.Year == item && u.Month == "4").Sum(u => u.Amount);
                        contributionlistDTO.May = accountdtos.Where(u => u.Year == item && u.Month == "5").Sum(u => u.Amount);
                        contributionlistDTO.Jun = accountdtos.Where(u => u.Year == item && u.Month == "6").Sum(u => u.Amount);
                        contributionlistDTO.July = accountdtos.Where(u => u.Year == item && u.Month == "7").Sum(u => u.Amount);
                        contributionlistDTO.Aug = accountdtos.Where(u => u.Year == item && u.Month == "8").Sum(u => u.Amount);
                        contributionlistDTO.Sep = accountdtos.Where(u => u.Year == item && u.Month == "9").Sum(u => u.Amount);
                        contributionlistDTO.Oct = accountdtos.Where(u => u.Year == item && u.Month == "10").Sum(u => u.Amount);
                        contributionlistDTO.Nov = accountdtos.Where(u => u.Year == item && u.Month == "11").Sum(u => u.Amount);
                        contributionlistDTO.Dec = accountdtos.Where(u => u.Year == item && u.Month == "12").Sum(u => u.Amount);
                        contributionlistDTO.Total = accountdtos.Where(u => u.Year == item).Sum(u => u.Amount);
                        contributions.Add(contributionlistDTO);
                    }





                }

            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading Contribution : Try Again";
            }
            if (accountdtos == null)
            {
                return NotFound();
            }
          
            TempData["Total"] = contributions.Sum(u => u.Total).ToString();
            TempData["UserRole"] = CurrentUserRole;


            profileDTO.contributionlistDTO = contributions.OrderByDescending(u => u.Year).ToList ();
            return View(profileDTO);

        }





        // GET: MemberDTOes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MemberDTO MemberDTO = new MemberDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                MemberDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<MemberDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading Member : Try Again";
            }
            if (MemberDTO == null)
            {
                return NotFound();
            }
            return View(MemberDTO);
        
    }


        // POST: MemberDTOes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            apiUrl = apiUrl + "/" + id;
            HttpResponseMessage response = await HttpTaskService.DeleteAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                TempData["SucessAlert"] = "Member Deleted successfully";
                string result = response.Content.ReadAsStringAsync().Result;
                var returndata = await response.Content.ReadAsStringAsync();
                var categorylist = Newtonsoft.Json.JsonConvert.DeserializeObject<MemberDTO>(returndata);

            }
            else
            {
                TempData["ErrorAlert"] = "Error in Deleting Member : Try Again";
            }

            return RedirectToAction(nameof(Index));
        }











    }
}