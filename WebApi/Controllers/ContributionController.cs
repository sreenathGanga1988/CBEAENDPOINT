using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Domain.Interfaces;
using DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApi.API_Controllers;
using WebApi.Controllers;
using WebApp.Services;

namespace WebApp.Controllers
{
  
    public class ContributionController : AdminBaseController
    {
        private readonly ILogger<ContributionController> _logger;
        private IWebHostEnvironment Environment;
        private readonly IMemoryCache _memoryCache;

        private readonly IReportRepository reportRepository;

        public ContributionController(ILogger<ContributionController> logger, IWebHostEnvironment _environment, IHttpContextAccessor contextAccessor, IReportRepository _reportRepository,IMemoryCache memoryCache) : base(contextAccessor)
        {
            _logger = logger;
            Environment = _environment;
            reportRepository = _reportRepository;
            _memoryCache = memoryCache;

        }


        public async Task<IActionResult> Index()
        {
            string apiUrl = "api/contribution" +  "";

            var ContributionMasterlist = new List<dynamic>();
            //HttpTaskService.token = HttpContext.Session.GetString("token");

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                ContributionMasterlist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<dynamic>>(data);
                
            }

            return View(ContributionMasterlist);
        }

        [HttpGet]
        public IActionResult FileUpload(ContributionMasterDTO contributionMaster=null)
        {
            if (contributionMaster == null) {
                contributionMaster = new ContributionMasterDTO();
            }
            
            return View(contributionMaster);
        }

        [HttpGet]
        public IActionResult ContriReports(int id=0, String type="")
        {
            String sql = "";


            ReportParameter reportParameter = new ReportParameter();
            reportParameter.ids = id.ToString ();
            reportParameter.ReportType = type;

        //    DataTable dt=    reportRepository.GetDataReport(reportParameter.ReportType, reportParameter.ids);
            string Heading = "";
            if (reportParameter.ReportType.Trim().ToUpper() == "NewMembers".Trim().ToUpper())
            {
                Heading = "Contributions with New / Wrong Employee ";
            }
            else if (reportParameter.ReportType.Trim().ToUpper() == "WrongBranch".Trim().ToUpper())
            {
                Heading = "Contributions with New / Wrong Branch";
            }
            else if (reportParameter.ReportType.Trim().ToUpper() == "WrongCircle".Trim().ToUpper())
            {
                Heading = "Contributions with  New / Wrong Circle";
            }
             else if (reportParameter.ReportType.Trim().ToUpper() == "ParkedItem".Trim().ToUpper())
            {
                Heading = "Parked Items of the Contribution";
            }
            else if (reportParameter.ReportType.Trim().ToUpper() == "All".Trim().ToUpper())
            {
                Heading = "All Contributions";
            }
            ViewData["msghead"] = Heading;


            return View(reportParameter);
        }

        [HttpGet]
        public IActionResult ContriReportsData(int id = 0, String type = "")
        {
            String sql = "";


            ReportParameter reportParameter = new ReportParameter();
            reportParameter.ids = id.ToString();
            reportParameter.ReportType = type;

            if (reportParameter.ReportType.Trim().ToUpper() == "Defaulter".Trim().ToUpper())
            {
                ViewData["msghead"] = "Defaulters List";

                return Ok(new { items = GetDefaukterlist() }); 

            }
            else
            {
                DataTable dt = reportRepository.GetDataReport(reportParameter.ReportType, reportParameter.ids);
                string Heading = "";
                if (reportParameter.ReportType.Trim().ToUpper() == "NewMembers".Trim().ToUpper())
                {
                    Heading = "Contributions with New / Wrong Employee ";
                }
                else if (reportParameter.ReportType.Trim().ToUpper() == "WrongBranch".Trim().ToUpper())
                {
                    Heading = "Contributions with New / Wrong Branch";
                }
                else if (reportParameter.ReportType.Trim().ToUpper() == "WrongCircle".Trim().ToUpper())
                {
                    Heading = "Contributions with  New / Wrong Circle";
                }
                else if (reportParameter.ReportType.Trim().ToUpper() == "ParkedItem".Trim().ToUpper())
                {
                    Heading = "Parked Items of the Contribution";
                }
                else if (reportParameter.ReportType.Trim().ToUpper() == "All".Trim().ToUpper())
                {
                    Heading = "All Contributions";
                }
                ViewData["msghead"] = Heading;


                return Ok(new { items = DataTableToJSONWithJSONNet(dt) }); ;
            }


            
        }
        public string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }

        [HttpPost]
        public async Task<IActionResult> FileUpload(List<IFormFile> postedFiles, String Month, String Year)
        {
            var apiUrl = @"api/upload";

            Boolean UploadComplete = false;

            foreach (IFormFile postedFile in postedFiles)
            {
                byte[] data;
                using (var br = new BinaryReader(postedFile.OpenReadStream()))
                {
                    data = br.ReadBytes((int)postedFile.OpenReadStream().Length);
                }

                ByteArrayContent bytes = new ByteArrayContent(data);
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(bytes, "postedFiles", postedFile.FileName);

                HttpResponseMessage response = await HttpTaskService.PostMediaAsyncData(apiUrl, multiContent);
                if (response.IsSuccessStatusCode)
                {

                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var filedet = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FileDetail>>(returndata);
                    ContributionMasterDTO contributionMaster = new ContributionMasterDTO();
                    contributionMaster.Year = int.Parse(Year);
                    contributionMaster.Month = int.Parse(Month);
                    contributionMaster.FileName = filedet[0].FileName;
                    contributionMaster.FileLocation = filedet[0].FileLocation;
                    contributionMaster.ContributionStatus = "O";
                    UploadComplete = true;
                    apiUrl = @"api/Contribution";
                    HttpResponseMessage response1 = await HttpTaskService.PostAsyncData(apiUrl, contributionMaster);

                    if (response1.IsSuccessStatusCode)
                    {
                        TempData["SucessAlert"] = "Contribution Added successfully";
                    }
                    else {
                        TempData["ErrorAlert"] = "Error in Adding Contribution : Try Again";
                    }
                    }
                else
                {
                    TempData["ErrorAlert"] = "Error in Adding ContributionMaster : Try Again";
                }



                if (UploadComplete == true)
                {


                }
            }
            return RedirectToAction("Index", "Contribution");
        }

        [HttpGet]
        public async Task<IActionResult> ForwardContribution(int id = 0)
        {
            string apiUrl = "api/contribution" + "";

            if (id == null)
            {
                return NotFound();
            }

            ContributionMasterDTO contributionMasterDTO = new ContributionMasterDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            //HttpTaskService.token = HttpContext.Session.GetString("token");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                contributionMasterDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<ContributionMasterDTO>(data);
                apiUrl = apiUrl + "/" + id;
                if (contributionMasterDTO.ContributionStatus == "O") {
                    contributionMasterDTO.ContributionStatus = "P";
                HttpResponseMessage response1 = await HttpTaskService.PutAsyncData(apiUrl, contributionMasterDTO);
                if (response1.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "Contribution Forwarded for approval successfully";
                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Submitting Contribution : Try Again";
                }
                }
                else
                {
                    TempData["ErrorAlert"] = "Only open contribution can be forwarded : Try Again";
                }
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Submitting Contribution : Try Again";
            }
            if (contributionMasterDTO == null)
            {
                return NotFound();
            }
           

            return  RedirectToAction("Index");
        }



        // GET: DayQuoteDTOes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string apiUrl = "api/contribution" + "";
            apiUrl = apiUrl + "/" + id;
            HttpResponseMessage response = await HttpTaskService.DeleteAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                TempData["SucessAlert"] = "Contribution Deleted successfully";
              

            }
            else
            {
               String ErrorMessage= "Error in Deleted DayQuote : Try Again";
                ErrorMessage = ErrorMessage + " :" + response.Content.ReadAsStringAsync().Result;
                TempData["ErrorAlert"] = ErrorMessage;
            }

            return RedirectToAction(nameof(Index));
           

        }

         public async Task<IActionResult> ParkItem(int? id, string type="")
        {
            if (id == null)
            {
                return NotFound();
            }
            ContributionDetailDTO  contributionDetailDTO = new ContributionDetailDTO();
            string apiUrl = "api/ContributionDetail";
   
            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                contributionDetailDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<ContributionDetailDTO>(data);
                apiUrl = apiUrl + "/" + id;
                if (contributionDetailDTO.isParked == false)
                {
                    contributionDetailDTO.isParked = true;
                    contributionDetailDTO.ParkReason = type;
                    contributionDetailDTO.Parkedon = DateTime.Now;
                    HttpResponseMessage response1 = await HttpTaskService.PutAsyncData(apiUrl, contributionDetailDTO);
                    if (response1.IsSuccessStatusCode)
                    {
                        TempData["SucessAlert"] = "Contribution Parked  successfully";
                    }
                    else
                    {
                        TempData["ErrorAlert"] = "Error in Parking Contribution : Try Again";
                    }
                }
                else
                {
                    TempData["ErrorAlert"] = "Contribution Already Parked : Refresh and Try Again";
                }
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Parking Contribution : Try Again";
            }

            return RedirectToAction(nameof(Index));


        }


        public async Task<IActionResult> UnParkItem(int? id, string type = "")
        {
            if (id == null)
            {
                return NotFound();
            }
            ContributionDetailDTO contributionDetailDTO = new ContributionDetailDTO();
            string apiUrl = "api/ContributionDetail";

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                contributionDetailDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<ContributionDetailDTO>(data);
                apiUrl = apiUrl + "/" + id;
                if (contributionDetailDTO.isParked == true)
                {
                    contributionDetailDTO.isParked = true;
                    contributionDetailDTO.ParkReason = type;
                     apiUrl = "api/ContributionDetail/UnParkContribution";
                    
                    HttpResponseMessage response1 =   await HttpTaskService.GetAsyncData(apiUrl + "/" + id+ "/" + CurrentUserId);
                    if (response1.IsSuccessStatusCode)
                    {
                        TempData["SucessAlert"] = "Contribution Parked  successfully";
                    }
                    else
                    {
                        TempData["ErrorAlert"] = "Error in Parking Contribution : Try Again";
                    }
                }
                else
                {
                    TempData["ErrorAlert"] = "Contribution Already Un Parked : Refresh and Try Again";
                }
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Parking Contribution : Try Again";
            }

            return RedirectToAction(nameof(Index));


        }



        [HttpGet]
        public IActionResult DefaulterList()
        {







            return View();
        }

       
        public String GetDefaukterlist()
        {
            string overallmemberdefaulter = "";
            var overallMemberdefaultkey = "overallMemberdefaultkey";
            // If found in cache, return cached data
            if (_memoryCache.TryGetValue(overallMemberdefaultkey, out overallmemberdefaulter))
            {
                return  overallmemberdefaulter ;
            }

            DataTable dataTable = reportRepository.GetDataReport("Defaulter","");
            overallmemberdefaulter = DataTableToJSONWithJSONNet(dataTable);

            // Set cache options
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));
            _memoryCache.Set(overallMemberdefaultkey, overallmemberdefaulter, cacheOptions);

            return overallmemberdefaulter;
        }
    }
}