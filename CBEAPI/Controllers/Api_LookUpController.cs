using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
using Domain.Entities.BaseEntities;
using System.Data;
using Newtonsoft.Json;
using CBEAPI.ViewModels;
using CBEAPI.Services;
using CBEAPI.Controllers;

namespace CBEAPI.API_Controllers.CBEAPIs
{

    [Route("api/[controller]")]
    [ApiController]
    public class Api_LookUpController : Api_BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public CustomResponseBuilder _resp;
        public Api_LookUpController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _resp = new CustomResponseBuilder();
        }


        // GET: api/Data
        [HttpPost]
        public CustomApiResponse GetLookUpData(String listType, String q,String pagestr,string SearchTerm,string selecteditem)
        {

            CustomApiResponse response = new CustomApiResponse();
            try
            {
              

                int page = 0;

                if (pagestr.Trim().Length > 0)
                {
                    page = int.Parse(pagestr);
                }

                int totalcount = 0;
                int NOP = 30;
                if (page > 0)
                {
                    page -= 1;
                }
                if (IsUserAuthorized(listType, "GetData", ref response))
                {

                    if (listType.Trim().ToUpper() == "YEAR")
                    {
                        totalcount = _unitOfWork.YearMaster.DataCount(u => u.YearOf.ToString().Contains(q));
                        var data = _unitOfWork.YearMaster.GetSelectList(u => u.YearOf.ToString().Contains(q), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "MONTH")
                    {
                        totalcount = _unitOfWork.Month.DataCount(u => u.MonthName.Contains(q));
                        var data = _unitOfWork.Month.GetSelectList(u => u.MonthName.Contains(q), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "BRANCH")
                    {
                        totalcount = _unitOfWork.Branch.DataCount(u => u.Name.Contains(q) || u.DpCode.ToString().Contains(q));
                        var data = _unitOfWork.Branch.GetSelectList(u => u.Name.Contains(q) || u.DpCode.ToString().Contains(q), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "CIRCLE")
                    {
                        totalcount = _unitOfWork.Circle.DataCount(u => u.Name.Contains(q) || u.CircleCode.ToString().Contains(q) || u.Id.ToString().Contains(q));
                        var data = _unitOfWork.Circle.GetSelectList(u => u.Name.Contains(q) || u.CircleCode.ToString().Contains(q) || u.Id.ToString().Contains(q), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "MEMBER")
                    {
                        totalcount = _unitOfWork.Member.DataCount(u => u.StatusId == 3 && (u.Name.Contains(q) || u.StaffNo.ToString().Contains(q)));
                        var data = _unitOfWork.Member.GetSelectList(u => (u.Name.Contains(q) || u.StaffNo.ToString().Contains(q)) && u.StatusId == 3, NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "USERTYPE")
                    {
                        totalcount = _unitOfWork.UserType.DataCount(u => u.Abbreviation.Contains(q));
                        var data = _unitOfWork.UserType.GetSelectList(u => u.Abbreviation.Contains(q), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "CATEGORY")
                    {
                        totalcount = _unitOfWork.Category.DataCount(u => u.Name.Contains(q) || u.Id.ToString().Contains(q));
                        var data = _unitOfWork.Category.GetSelectList(u => u.Name.Contains(q) || u.Id.ToString().Contains(q), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "STATUS")
                    {
                        totalcount = _unitOfWork.Status.DataCount(u => u.Name.Contains(q) || u.Id.ToString().Contains(q));
                        var data = _unitOfWork.Status.GetSelectList(u => u.Name.Contains(q) || u.Id.ToString().Contains(q), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "STATE")
                    {
                        totalcount = _unitOfWork.State.DataCount(u => u.Name.Contains(q) || u.Id.ToString().Contains(q));
                        var data = _unitOfWork.State.GetSelectList(u => u.Name.Contains(q) || u.Id.ToString().Contains(q), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "DESIGNATION")
                    {
                        totalcount = _unitOfWork.Designation.DataCount(u => u.Name.Contains(q) || u.Id.ToString().Contains(q));
                        var data = _unitOfWork.Designation.GetSelectList(u => u.Name.Contains(q) || u.Id.ToString().Contains(q), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "FILEUPLOAD")
                    {
                        totalcount = _unitOfWork.FileUploadDetail.DataCount(u => u.Filename.Contains(q));
                        var data = _unitOfWork.FileUploadDetail.GetSelectList(u => u.Filename.Contains(q), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "REPORT")
                    {
                        totalcount = _unitOfWork.MainReport.DataCount(u => u.Name.Contains(q));
                        var data = _unitOfWork.MainReport.GetSelectList(u => u.Name.Contains(q), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "ALLMEMBER")
                    {
                        totalcount = _unitOfWork.Member.DataCount(u => u.Name.Contains(q) || u.StaffNo.ToString().Contains(q));
                        var data = _unitOfWork.Member.GetSelectList(u => (u.Name.Contains(q) || u.StaffNo.ToString().Contains(q)), NOP * page, NOP);

                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "STAFFNO")
                    {
                        totalcount = _unitOfWork.Member.DataCount(u => u.Name.Contains(q) || u.StaffNo.ToString().Contains(q));
                        var data = _unitOfWork.Member.GetSelectList(u => (u.Name.Contains(q) || u.StaffNo.ToString().Contains(q)), NOP * page, NOP);

                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                }
                else
                {
                    _resp.ActionNotAuthorizedResponse(ref response);
                }
            }
            catch (Exception ex)
            {
                _resp.ExceptionResponse(ex, ref response);
            }
            return response;









        }
















        // GET: api/Data
        [HttpPost]
        public CustomApiResponse GetData()
        {

            CustomApiResponse response = new CustomApiResponse();
            try
            {
                string q = Request.Form["q"].ToString();
                string pagestr = Request.Form["page"].ToString();
                string listType = Request.Form["listType"].ToString();
                string SearchTerm = Request.Form["dependentvalue"].ToString();
                string selecteditem = Request.Form["selecteditem"].ToString();

                int page = 0;

                if (pagestr.Trim().Length > 0)
                {
                    page = int.Parse(pagestr);
                }

                int totalcount = 0;
                int NOP = 30;
                if (page > 0)
                {
                    page -= 1;
                }
                if (IsUserAuthorized(listType, "GetData", ref response))
                {

                    if (listType.Trim().ToUpper() == "YEAR")
                    {
                        totalcount = _unitOfWork.YearMaster.DataCount(u => u.YearOf.ToString().Contains(q));
                        var data = _unitOfWork.YearMaster.GetSelectList(u => u.YearOf.ToString().Contains(q), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "MONTH")
                    {
                        totalcount = _unitOfWork.Month.DataCount(u => u.MonthName.Contains(q));
                        var data = _unitOfWork.Month.GetSelectList(u => u.MonthName.Contains(q), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "BRANCH")
                    {
                        totalcount = _unitOfWork.Branch.DataCount(u => u.Name.Contains(q) || u.DpCode.ToString().Contains(q));
                        var data = _unitOfWork.Branch.GetSelectList(u => u.Name.Contains(q) || u.DpCode.ToString().Contains(q), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "CIRCLE")
                    {
                        totalcount = _unitOfWork.Circle.DataCount(u => u.Name.Contains(q) || u.CircleCode.ToString().Contains(q) || u.Id.ToString().Contains(q));
                        var data = _unitOfWork.Circle.GetSelectList(u => u.Name.Contains(q) || u.CircleCode.ToString().Contains(q) || u.Id.ToString().Contains(q), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "MEMBER")
                    {
                        totalcount = _unitOfWork.Member.DataCount(u => u.StatusId == 3 && (u.Name.Contains(q) || u.StaffNo.ToString().Contains(q)));
                        var data = _unitOfWork.Member.GetSelectList(u => (u.Name.Contains(q) || u.StaffNo.ToString().Contains(q)) && u.StatusId == 3, NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "USERTYPE")
                    {
                        totalcount = _unitOfWork.UserType.DataCount(u => u.Abbreviation.Contains(q));
                        var data = _unitOfWork.UserType.GetSelectList(u => u.Abbreviation.Contains(q), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "CATEGORY")
                    {
                        totalcount = _unitOfWork.Category.DataCount(u => u.Name.Contains(q) || u.Id.ToString().Contains(q));
                        var data = _unitOfWork.Category.GetSelectList(u => u.Name.Contains(q) || u.Id.ToString().Contains(q), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "STATUS")
                    {
                        totalcount = _unitOfWork.Status.DataCount(u => u.Name.Contains(q) || u.Id.ToString().Contains(q));
                        var data = _unitOfWork.Status.GetSelectList(u => u.Name.Contains(q) || u.Id.ToString().Contains(q), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "STATE")
                    {
                        totalcount = _unitOfWork.State.DataCount(u => u.Name.Contains(q) || u.Id.ToString().Contains(q));
                        var data = _unitOfWork.State.GetSelectList(u => u.Name.Contains(q) || u.Id.ToString().Contains(q), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "DESIGNATION")
                    {
                        totalcount = _unitOfWork.Designation.DataCount(u => u.Name.Contains(q) || u.Id.ToString().Contains(q));
                        var data = _unitOfWork.Designation.GetSelectList(u => u.Name.Contains(q) || u.Id.ToString().Contains(q), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "FILEUPLOAD")
                    {
                        totalcount = _unitOfWork.FileUploadDetail.DataCount(u => u.Filename.Contains(q));
                        var data = _unitOfWork.FileUploadDetail.GetSelectList(u => u.Filename.Contains(q), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "REPORT")
                    {
                        totalcount = _unitOfWork.MainReport.DataCount(u => u.Name.Contains(q));
                        var data = _unitOfWork.MainReport.GetSelectList(u => u.Name.Contains(q), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "ALLMEMBER")
                    {
                        totalcount = _unitOfWork.Member.DataCount(u => u.Name.Contains(q) || u.StaffNo.ToString().Contains(q));
                        var data = _unitOfWork.Member.GetSelectList(u => (u.Name.Contains(q) || u.StaffNo.ToString().Contains(q)), NOP * page, NOP);

                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "STAFFNO")
                    {
                        totalcount = _unitOfWork.Member.DataCount(u => u.Name.Contains(q) || u.StaffNo.ToString().Contains(q));
                        var data = _unitOfWork.Member.GetSelectList(u => (u.Name.Contains(q) || u.StaffNo.ToString().Contains(q)), NOP * page, NOP);

                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                }
                else
                {
                    _resp.ActionNotAuthorizedResponse(ref response);
                }
            }
            catch (Exception ex)
            {
                _resp.ExceptionResponse(ex, ref response);
            }
            return response;
           
           
          
            
      




        }


        [HttpGet]
        public ActionResult<IEnumerable<dynamic>> GetData(ReportParameterlist reportParameterlist)
        {



            int typeid = int.Parse(reportParameterlist.ReportType);

            var rpt = _unitOfWork.MainReport.Find(u => u.Id == typeid).FirstOrDefault();

            string query = rpt.SQLString;

            DataTable dataTable = _unitOfWork.ReportRepository.GetDataReport(query);

            //    var data = _unitOfWork.AccountsRepository.GetContribution(int.Parse(Year.ToString()), int.Parse(Month.ToString()), int.Parse(Branch.ToString()), int.Parse(Circle.ToString()), int.Parse(Member.ToString()));



            return Ok(new { items = DataTableToJSONWithJSONNet(dataTable) }); ;
        }
        public string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }


    }
}
