using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Domain.Entities.BaseEntities;
using System.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatatableAjaxModel;
using System.Linq.Expressions;
using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using WebApi.ViewModels;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Api_DataTableController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public Api_DataTableController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        // GET: api/Data
        [HttpGet]
        public ActionResult<IEnumerable<dynamic>> GetDataTable()
        {

            return Ok("");
        }
        [HttpPost, Route("AddCustomer")]
        public CustomApiResponse PostDataTable([FromBody] DtParameters dtParameters)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                var searchBy = dtParameters.Search?.Value;
                var orderCriteria = "Id";
                var orderAscendingDirection = true;

                if (dtParameters.Order != null)
                {
                    // in this example we just default sort on the 1st column
                    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
                }

                var totalResultsCount = 0;
                var filteredResultsCount = 0;

                totalResultsCount = _unitOfWork.Member.DataCount(null);

                if (dtParameters.Listtype == "MEMBER")
                {

                    if (!string.IsNullOrEmpty(searchBy))
                    {
                        var result = _unitOfWork.Member.GetAllMemberasync(r => r.Name.ToUpper().Contains(searchBy.ToUpper()) ||
                                                   r.StaffNo.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                                                    r.DpCode.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                                                    r.NomineeRelation.ToUpper().Contains(searchBy.ToUpper()) ||
                                                    r.NomineeIDentity.ToUpper().Contains(searchBy.ToUpper())
                                                   );
                        result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);
                        filteredResultsCount = result.Count();
                        var result1 = result
                            .Skip(dtParameters.Start)
                            .Take(dtParameters.Length)
                            .ToList();

                        response.Value = new DtResult<Member> { Draw = dtParameters.Draw, RecordsTotal = totalResultsCount, RecordsFiltered = filteredResultsCount, Data = result1 };
                        
                    }
                    else
                    {
                        var result = _unitOfWork.Member.GetAllMemberasync(r => r.Name != null);
                        result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);
                        filteredResultsCount = result.Count();
                        var result1 = result
                            .Skip(dtParameters.Start)
                            .Take(dtParameters.Length)
                            .ToList();
                        response.Value = new DtResult<Member> { Draw = dtParameters.Draw, RecordsTotal = totalResultsCount, RecordsFiltered = filteredResultsCount, Data = result1 };

                    }

                }
                else if (dtParameters.Listtype == "USER")
                {

                    if (!string.IsNullOrEmpty(searchBy))
                    {
                        var result = _unitOfWork.User.GetAllMemberasync(r => r.UserName.ToUpper().Contains(searchBy.ToUpper()) ||
                                                   r.EmployeeId.ToString().ToUpper().Contains(searchBy.ToUpper())
                                                                                             );
                        result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);
                        filteredResultsCount = result.Count();
                        var result1 = result
                            .Skip(dtParameters.Start)
                            .Take(dtParameters.Length)
                            .ToList();
                        response.Value = new DtResult<User> { Draw = dtParameters.Draw, RecordsTotal = totalResultsCount, RecordsFiltered = filteredResultsCount, Data = result1 };
                    }
                    else
                    {
                        var result = _unitOfWork.User.GetAllMemberasync(r => r.UserName != null);
                        result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);
                        filteredResultsCount = result.Count();
                        var result1 = result
                            .Skip(dtParameters.Start)
                            .Take(dtParameters.Length)
                            .ToList();
                        response.Value = new DtResult<User> { Draw = dtParameters.Draw, RecordsTotal = totalResultsCount, RecordsFiltered = filteredResultsCount, Data = result1 };

                    }

                }
                else if (dtParameters.Listtype == "BRANCH")
                {
                    if (!string.IsNullOrEmpty(searchBy))
                    {
                        try
                        {
                            var result = _unitOfWork.Branch.GetAllBranchasync(r => r.Name.ToUpper().Contains(searchBy.ToUpper()) ||
                                                  r.DpCode.ToString().ToUpper().Contains(searchBy.ToUpper()));
                            result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);
                            filteredResultsCount = result.Count();
                            var result1 = result
                                .Skip(dtParameters.Start)
                                .Take(dtParameters.Length)
                                .ToList();
                            response.Value = new DtResult<Branch> { Draw = dtParameters.Draw, RecordsTotal = totalResultsCount, RecordsFiltered = filteredResultsCount, Data = result1 };
                        }
                        catch (Exception ex)
                        {
                            response.Value = new DtResult<Member> { Draw = dtParameters.Draw, RecordsTotal = totalResultsCount, RecordsFiltered = filteredResultsCount };

                            //return Ok(new DtResult<Branch> { Draw = dtParameters.Draw, RecordsTotal = totalResultsCount, RecordsFiltered = filteredResultsCount }); ;

                        }
                    }
                    else
                    {
                        var result = _unitOfWork.Branch.GetAllBranchasync(r => r.Name != null);
                        result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);
                        filteredResultsCount = result.Count();
                        var result1 = result
                            .Skip(dtParameters.Start)
                            .Take(dtParameters.Length)
                            .ToList();
                        //return Ok(new DtResult<Branch> { Draw = dtParameters.Draw, RecordsTotal = totalResultsCount, RecordsFiltered = filteredResultsCount, Data = result1 }); ;
                        response.Value = new DtResult<Branch> { Draw = dtParameters.Draw, RecordsTotal = totalResultsCount, RecordsFiltered = filteredResultsCount, Data = result1 };

                    }

                }
                else
                {
                    List<User> usr = new List<User>();
                    //return Ok(new DtResult<User> { Draw = dtParameters.Draw, RecordsTotal = totalResultsCount, RecordsFiltered = filteredResultsCount, Data = usr });
                    response.Value = new DtResult<User> { Draw = dtParameters.Draw, RecordsTotal = totalResultsCount, RecordsFiltered = filteredResultsCount, Data = usr };

                }
            }
            catch (Exception ex)
            {

                throw;
            }


            return response;

        }


    }
}


