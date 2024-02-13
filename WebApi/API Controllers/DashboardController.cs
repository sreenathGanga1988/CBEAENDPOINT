using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using System;
using Domain.Entities.BaseEntities;
using System.Data;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashBoardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/DashBoard
        [HttpGet("{id}")]
        public ActionResult<dynamic> GetDashBoard(String id)
        {
            string qry = "";

            if (id == "YEARCONTR")
            {
                qry = "SELECT  YearOF, SUM(Amount) AS TotalAmount FROM Accounts  GROUP BY YearOF ORDER BY YearOF";
                DataTable dataTable = _unitOfWork.ReportRepository.GetDataReport(qry);


                string[] years = dataTable.Rows.Cast<DataRow>()
                    .Select(row => row["YearOF"].ToString())
                    .ToArray();

                string[] Amount = dataTable.Rows.Cast<DataRow>()
                    .Select(row => row["TotalAmount"].ToString())
                    .ToArray();

               

                return Ok(new { label = years, data = Amount }); ;
            }
            else if (id == "YEARJOIN")
            {
                string[] years = { "2003", "2004", "2005", };

                string[] Amount = { "10000", "20000", "30000", };

                return Ok(new { label = years, data = Amount }); ;
            }
                else if (id == "CurrentYear")
            {
                qry = @"SELECT        SUM(Accounts.Amount) AS TotalAmount, Month.MonthName
FROM            Accounts INNER JOIN
                         Month ON Accounts.MonthCode = Month.MonthCode
WHERE(Accounts.YearOF = 2022)
GROUP BY Accounts.MonthCode, Month.MonthName
ORDER BY Accounts.MonthCode";
                DataTable dataTable = _unitOfWork.ReportRepository.GetDataReport(qry);


                string[] years = dataTable.Rows.Cast<DataRow>()
                    .Select(row => row["MonthName"].ToString())
                    .ToArray();

                string[] Amount = dataTable.Rows.Cast<DataRow>()
                    .Select(row => row["TotalAmount"].ToString())
                    .ToArray();
                return Ok(new { label = years, data = Amount }); ;
            }
            else
            {
                return Ok(new { label = "", data = "" }); ;
            }


        }




    }
}
