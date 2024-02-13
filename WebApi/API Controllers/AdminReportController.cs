using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Domain.Entities.BaseEntities;
using System.Data;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;
using System;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AdminReportController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;
        public AdminReportController(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
        }







        [HttpPost]
        public ActionResult PostAdminReport(ReportParameterlist reportParameterlist)
        {



            int typeid = int.Parse(reportParameterlist.ReportType);

            var rpt = _unitOfWork.MainReport.Find(u => u.Id == typeid).FirstOrDefault();

            var rptname = rpt.Name;

            string query = rpt.SQLString;


            if (query.Contains("@param_year"))
            {
                query= query.Replace("@param_year", reportParameterlist.YearOf.ToString ());
            }

            if (query.Contains("@param_month"))
            {
                query = query.Replace("@param_month", reportParameterlist.MonthCode.ToString());
            }
            if (query.Contains("@param_circleid"))
            {
                query = query.Replace("@param_circleid", reportParameterlist.CircleId.ToString());
            }
            if (query.Contains("@param_dpcode"))
            {
                query = query.Replace("@param_dpcode", reportParameterlist.DpCode.ToString());
            }
            if (query.Contains("@param_staffno"))
            {
                query = query.Replace("@param_staffno", reportParameterlist.StaffNO.ToString());
            }
            DataTable dataTable = _unitOfWork.ReportRepository.GetDataReport(query);

            //    var data = _unitOfWork.AccountsRepository.GetContribution(int.Parse(Year.ToString()), int.Parse(Month.ToString()), int.Parse(Branch.ToString()), int.Parse(Circle.ToString()), int.Parse(Member.ToString()));



            return Ok(new { items = DataTableToJSONWithJSONNet(dataTable),reportname=rptname }); ;
        }




     
        [HttpGet()]
        [Route("GetOverAllContribution")]
        public ActionResult GetOverAllContribution()
        {
            string overallccontribution = "";
            var overallccontributionkey = "overallccontributionkey";
            // If found in cache, return cached data
            if (_memoryCache.TryGetValue(overallccontributionkey, out overallccontribution))
            {
                return Ok(new { items = overallccontribution }); ;
            }

            DataTable dataTable = _unitOfWork.Contributions.GetTotalContribution();
            overallccontribution = DataTableToJSONWithJSONNet(dataTable);

            // Set cache options
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));
            _memoryCache.Set(overallccontributionkey, overallccontribution, cacheOptions);

            return Ok(new { items= overallccontribution }); ;

        }



        [HttpGet()]
        [Route("GetMemberDefault")]
        public ActionResult GetMemberDefault()
        {
            string overallmemberdefaulter = "";
            var overallMemberdefaultkey = "overallMemberdefaultkey";
            // If found in cache, return cached data
            if (_memoryCache.TryGetValue(overallMemberdefaultkey, out overallmemberdefaulter))
            {
                return Ok(new { items = overallmemberdefaulter }); ;
            }

            DataTable dataTable = _unitOfWork.Contributions.GetMemberDefaults();
            overallmemberdefaulter = DataTableToJSONWithJSONNet(dataTable);

            // Set cache options
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));
            _memoryCache.Set(overallMemberdefaultkey, overallmemberdefaulter, cacheOptions);

            return Ok(new { items = overallmemberdefaulter }); ;

        }


        public string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }
    }
}

