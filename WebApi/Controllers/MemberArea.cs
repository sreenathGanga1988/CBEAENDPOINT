using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Services;

namespace WebApi.Controllers
{
    public class MemberArea : BaseController
    {

        public MemberArea(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {

            var authenticatedUser = contextAccessor.HttpContext.User.Identity.Name;
            //if (HttpContext.Session.Get<string>("staffNo") != default)
            //{

            //    CurrentStaffNo = int.Parse(HttpContext.Session.Get<string>("staffNo"));

            //}

            

        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Profile()
        {
            int id = CurrentStaffno ;


            ProfileDTO profileDTO = new ProfileDTO();

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
            ViewBag.CurrentUserRole = CurrentUserRole;
            return View(profileDTO);



        }

        public async Task<IActionResult> Contribution()
        {
            List<ContributionlistDTO> contributions = new List<ContributionlistDTO>();

            int id = CurrentStaffno;

            List<AccountsDTO> accountdtos = new List<AccountsDTO>();


            HttpResponseMessage response = await HttpTaskService.GetAsyncData("api/UserProfile/GetUserContribution" + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
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
                        contributionlistDTO.July = accountdtos.Where(u => u.Year == item && u.Month == "9").Sum(u => u.Amount);
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
                TempData["ErrorAlert"] = "Error in Loading Branch : Try Again";
            }
            if (accountdtos == null)
            {
                return NotFound();
            }
            ViewBag.CurrentUserRole = CurrentUserRole;
            TempData["Total"] = contributions.Sum(u => u.Total).ToString();
            TempData["UserRole"] = CurrentUserRole;
            return View(contributions.OrderByDescending(u => u.Year));



        }



        // GET: User/ProfileEdit/5
        public async Task<IActionResult> ProfileEdit()
        {
            int id = CurrentUserId;
            if (id == null)
            {
                return NotFound();
            }
            string apiUrl = "api/User";
            UserDTO UserDTO = new UserDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                UserDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDTO>(data);
                UserDTO.OldPassWord = UserDTO.Password;
                UserDTO.Password = "";
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


        // POST: User/ProfileEdit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfileEdit(int id, [Bind("Id, UserName, Password, EmailId, PhoneNum, UserTypeId, EmployeeId,  CreatedByUserID, CreatedDate, OldPassWord,ConfirmPassword")] UserDTO UserDTO)
        {
            if (id != UserDTO.Id)
            {
                return NotFound();
            }
            string apiUrl = "api/User";
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
            return RedirectToAction(nameof(Profile));
            
        }





        // GET: Member/Edit/5
        public async Task<IActionResult> MembershipEdit(int? id)
        {
            if (id == null)
            {
                id = CurrentStaffno;
            }

            MemberDTO MemberDTO = new MemberDTO();
            string apiUrl = "api/member";
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
        public async Task<IActionResult> MembershipEdit(int StaffNo, [Bind("Id, Name, StaffNo, GenderId, Dob, DesignationId, CategoryId, Doj, DpCode, DojtoScheme, Nominee, NomineeRelation, NomineeIDentity, UnionMember, StatusId, CreatedByUserId , CreatedDate ")] MemberDTO MemberDTO)
        {
            if (StaffNo != MemberDTO.StaffNo)
            {
                return NotFound();
            }
            MemberDTO.IsRegCompleted = false;
            if (ModelState.IsValid)
            {

                MemberDTO.ModifiedByUserId = CurrentUserId;
                MemberDTO.ModifiedDate = DateTime.UtcNow;

                string apiUrl = "api/member";
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

                return RedirectToAction(nameof(Profile));
            }
            return View(MemberDTO);
        }



        // Get: AccountsDirectEntry/Create
        public IActionResult AccountsDirectEntryCreate()
        {
            AccountsDirectEntryDTO AccountsDirectEntryDTO = new AccountsDirectEntryDTO();
            AccountsDirectEntryDTO.StaffNo = CurrentStaffno;
            AccountsDirectEntryDTO.Name = CurrentEmployeeName;
            return View(AccountsDirectEntryDTO);
        }

        // POST: AccountsDirectEntry/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AccountsDirectEntryCreate([Bind(" ID, StaffNo , Name, DpCode, MonthCode, YearOf, DdIba, DdIbaDate, Amt, Enrl, Fine, F9, F10, F11")] AccountsDirectEntryDTO AccountsDirectEntryDTO)
        {
            try
            {
                string apiUrl = "api/AccountsDirectEntry" + "";
              

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
                return RedirectToAction(nameof(Profile));
            }
            catch
            {
                return View();
            }
        }







    }
}
