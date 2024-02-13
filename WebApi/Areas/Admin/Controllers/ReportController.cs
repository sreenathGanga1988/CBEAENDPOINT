using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Controllers;
using WebApp.Services;

namespace WebApi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReportController : BaseController
    {
        private readonly ILogger<ReportController> _logger;

        string apiUrl = "api/AdminReport" +
            "";

        public ReportController(ILogger<ReportController> logger, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {

            _logger = logger;

        }
        
        [HttpGet]
        public IActionResult Index(ReportDTO reportDTO = null)
        {
            if (reportDTO == null)
            {
                reportDTO = new ReportDTO();
            }

            return View(reportDTO);
        }

        [HttpPost]
        public async Task<IActionResult> IndexLoadAsync(ReportDTO reportDTO)
        {
          //  HttpTaskService.token = HttpContext.Session.GetString("token");

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl, reportDTO);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
           
            }


            return RedirectToAction("Index", reportDTO);
        }


      
        [HttpGet]
        public async Task<IActionResult> GetReportData(String type = "", string Year = "", string Month = "", string Member = "", string Branch = "", string Circle = "")
        {
            string result = "";

            if (Year == null)
            {
                Year = "0";
            }
            if (Month == null)
            {
                Month = "0";
            }
            if (Member == null)
            {
                Member = "0";
            }
            if (Branch == null)
            {
                Branch = "0";
            }
            if (Circle == null)
            {
                Circle = "0";
            }
            ReportParameter reportDTO = new ReportParameter();
            reportDTO.YearOf = int.Parse(Year.ToString());
            reportDTO.MonthCode = int.Parse(Month.ToString());
            reportDTO.StaffNO = Member.ToString();
            reportDTO.DpCode = int.Parse(Branch.ToString());
            reportDTO.CircleId = int.Parse(Circle.ToString());
            reportDTO.ReportType = type;
        //    HttpTaskService.token = HttpContext.Session.GetString("token");
            HttpResponseMessage response = await HttpTaskService.PostAsyncData(apiUrl, reportDTO);
            if (response.IsSuccessStatusCode)
            {
                var returndata = await response.Content.ReadAsStringAsync();
                result = response.Content.ReadAsStringAsync().Result;

            }







            return Ok(new { items = result }); ;
        }



        [HttpGet]
        public async Task<IActionResult> GetOverAllContribution()
        {
            string result = "";

            //    HttpTaskService.token = HttpContext.Session.GetString("token");
            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl+ "/GetOverAllContribution");
            if (response.IsSuccessStatusCode)
            {
                var returndata = await response.Content.ReadAsStringAsync();
                result = response.Content.ReadAsStringAsync().Result;

            }

            return Ok(new { items = result }); ;
        }
    }
}
