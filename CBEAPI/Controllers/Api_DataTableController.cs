using Domain.Interfaces;
using System.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using DatatableAjaxModel;
using System.Linq.Expressions;
using Domain.Entities;
using CBEAPI.ViewModels;
using System.Reflection;
using static Dapper.SqlMapper;
using ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CBEAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Api_DataTableController : Api_BaseController
    {
        private readonly IUnitOfWork _unitOfWork;


        public Api_DataTableController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [AllowAnonymous()]
        [HttpGet()]
        [Route("GetDummyString")]
        public String GetDummyString()
        {


            return "Working";
        }
        public string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }


        //[HttpGet()]
        //[Route("GetPageinatedDataAsync")]
        //public CustomApiResponse GetPageinatedDataAsync(PageParameter pageParameter)
        //{
        //    CustomApiResponse response = new CustomApiResponse();
        //    PaginatedDataResponse paginatedDataResponse = new PaginatedDataResponse();
        //    paginatedDataResponse.TotalCount = 0;
        //    try
        //    {

        //        var items = _unitOfWork.Category.GetFilteredPaginatedData(pageParameter.ReportType, pageParameter.SearchText, pageParameter.PageNumber, pageParameter.PageSize);
        //        if (items != null)
        //        {
        //            if (items.Tables[0] != null)
        //            {
        //                paginatedDataResponse.TotalCount = Convert.ToInt16(items.Tables[0].Rows[0]["TotalCount"]);
        //            }

                    
        //            // Other Codes Here
        //            paginatedDataResponse.RowData =items.Tables[1].ToDynamic();

        //        }
               


        //        _resp.SuccessReponse(paginatedDataResponse, ref response);

        //    }
        //    catch (Exception ex)
        //    {
        //        _resp.ExceptionResponse(ex, ref response);
        //    }
        //    return response;
        //}




        [HttpGet()]
        [Route("GetPageinatedDataAsync")]
        public CustomApiResponse GetPageinatedDataAsync(String ReportType,String SearchText="",int PageNumber = 0,int PageSize=0)
       {
            CustomApiResponse response = new CustomApiResponse();
            PaginatedDataResponse paginatedDataResponse = new PaginatedDataResponse();
            paginatedDataResponse.TotalCount = 0;
            try
            {

                var items = _unitOfWork.Category.GetFilteredPaginatedData(ReportType, SearchText, PageSize, PageNumber);
                if (items != null)
                {
                    if (items.Tables[0] != null)
                    {
                        paginatedDataResponse.TotalCount = Convert.ToInt32(items.Tables[0].Rows[0]["TotalCount"]);
                    }


                    // Other Codes Here
                    paginatedDataResponse.RowData = items.Tables[1].ToDynamic();

                }



                _resp.SuccessReponse(paginatedDataResponse, ref response);

            }
            catch (Exception ex)
            {
                _resp.ExceptionResponse(ex, ref response);
            }
            return response;
        }









        [HttpGet()]
        [Route("GetLookUpDataAsync")]
        public CustomApiResponse GetLookUpDataAsync(String listType, String SearchTerm = "", String pageNumber = "", String pageSize = "", String selecteditem ="")
        {

            CustomApiResponse response = new CustomApiResponse();
            try
            {


                int page = 0;
                int NOP = 30;
                if (pageNumber.Trim().Length > 0)
                {
                    page = int.Parse(pageNumber);
                }
                if (pageNumber.Trim().Length > 0)
                {
                    NOP = int.Parse(pageSize);
                }

                int totalcount = 0;
               
                
                if (page > 0)
                {
                    page -= 1;
                }
                if (IsUserAuthorized(listType, "GetData", ref response))
                {

                    if (listType.Trim().ToUpper() == "YEAR")
                    {
                        totalcount = _unitOfWork.YearMaster.DataCount(u => u.YearOf.ToString().Contains(SearchTerm));
                        var data = _unitOfWork.YearMaster.GetSelectList(u => u.YearOf.ToString().Contains(SearchTerm), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "MONTH")
                    {
                        totalcount = _unitOfWork.Month.DataCount(u => u.MonthName.Contains(SearchTerm));
                        var data = _unitOfWork.Month.GetSelectList(u => u.MonthName.Contains(SearchTerm), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "BRANCH")
                    {
                        totalcount = _unitOfWork.Branch.DataCount(u => u.Name.Contains(SearchTerm) || u.DpCode.ToString().Contains(SearchTerm));
                        var data = _unitOfWork.Branch.GetSelectList(u => u.Name.Contains(SearchTerm) || u.DpCode.ToString().Contains(SearchTerm), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "CIRCLE")
                    {
                        totalcount = _unitOfWork.Circle.DataCount(u => u.Name.Contains(SearchTerm) || u.CircleCode.ToString().Contains(SearchTerm) || u.Id.ToString().Contains(SearchTerm));
                        var data = _unitOfWork.Circle.GetSelectList(u => u.Name.Contains(SearchTerm) || u.CircleCode.ToString().Contains(SearchTerm) || u.Id.ToString().Contains(SearchTerm), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "MEMBER")
                    {
                        totalcount = _unitOfWork.Member.DataCount(u => u.StatusId == 3 && (u.Name.Contains(SearchTerm) || u.StaffNo.ToString().Contains(SearchTerm)));
                        var data = _unitOfWork.Member.GetSelectList(u => (u.Name.Contains(SearchTerm) || u.StaffNo.ToString().Contains(SearchTerm)) && u.StatusId == 3, NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "USERTYPE")
                    {
                        totalcount = _unitOfWork.UserType.DataCount(u => u.Abbreviation.Contains(SearchTerm));
                        var data = _unitOfWork.UserType.GetSelectList(u => u.Abbreviation.Contains(SearchTerm), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "CATEGORY")
                    {
                        totalcount = _unitOfWork.Category.DataCount(u => u.Name.Contains(SearchTerm) || u.Id.ToString().Contains(SearchTerm));
                        var data = _unitOfWork.Category.GetSelectList(u => u.Name.Contains(SearchTerm) || u.Id.ToString().Contains(SearchTerm), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "STATUS")
                    {
                        totalcount = _unitOfWork.Status.DataCount(u => u.Name.Contains(SearchTerm) || u.Id.ToString().Contains(SearchTerm));
                        var data = _unitOfWork.Status.GetSelectList(u => u.Name.Contains(SearchTerm) || u.Id.ToString().Contains(SearchTerm), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "STATE")
                    {
                        totalcount = _unitOfWork.State.DataCount(u => u.Name.Contains(SearchTerm) || u.Id.ToString().Contains(SearchTerm));
                        var data = _unitOfWork.State.GetSelectList(u => u.Name.Contains(SearchTerm) || u.Id.ToString().Contains(SearchTerm), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "DESIGNATION")
                    {
                        totalcount = _unitOfWork.Designation.DataCount(u => u.Name.Contains(SearchTerm) || u.Id.ToString().Contains(SearchTerm));
                        var data = _unitOfWork.Designation.GetSelectList(u => u.Name.Contains(SearchTerm) || u.Id.ToString().Contains(SearchTerm), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "FILEUPLOAD")
                    {
                        totalcount = _unitOfWork.FileUploadDetail.DataCount(u => u.Filename.Contains(SearchTerm));
                        var data = _unitOfWork.FileUploadDetail.GetSelectList(u => u.Filename.Contains(SearchTerm), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "REPORT")
                    {
                        totalcount = _unitOfWork.MainReport.DataCount(u => u.Name.Contains(SearchTerm));
                        var data = _unitOfWork.MainReport.GetSelectList(u => u.Name.Contains(SearchTerm), NOP * page, NOP);
                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "ALLMEMBER")
                    {
                        totalcount = _unitOfWork.Member.DataCount(u => u.Name.Contains(SearchTerm) || u.StaffNo.ToString().Contains(SearchTerm));
                        var data = _unitOfWork.Member.GetSelectList(u => (u.Name.Contains(SearchTerm) || u.StaffNo.ToString().Contains(SearchTerm)), NOP * page, NOP);

                        var items = new { total_count = totalcount, items = data };
                        _resp.SuccessReponse(items, ref response);
                    }
                    else if (listType.Trim().ToUpper() == "STAFFNO")
                    {
                        totalcount = _unitOfWork.Member.DataCount(u => u.Name.Contains(SearchTerm) || u.StaffNo.ToString().Contains(SearchTerm));
                        var data = _unitOfWork.Member.GetSelectList(u => (u.Name.Contains(SearchTerm) || u.StaffNo.ToString().Contains(SearchTerm)), NOP * page, NOP);

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

namespace DatatableAjaxModel
{
    ///This view model class has been referred from example created by Marien Monnier at Soft.it. All credits to Marien for this class

    /// <summary>
    /// A full result, as understood by jQuery DataTables.
    /// </summary>
    /// <typeparam name="T">The data type of each row.</typeparam>
    public class DtResult<T>
    {
        /// <summary>
        /// The draw counter that this object is a response to - from the draw parameter sent as part of the data request.
        /// Note that it is strongly recommended for security reasons that you cast this parameter to an integer, rather than simply echoing back to the client what it sent in the draw parameter, in order to prevent Cross Site Scripting (XSS) attacks.
        /// </summary>
        [JsonProperty("draw")]
        public int Draw { get; set; }

        /// <summary>
        /// Total records, before filtering (i.e. the total number of records in the database)
        /// </summary>
        [JsonProperty("recordsTotal")]
        public int RecordsTotal { get; set; }

        /// <summary>
        /// Total records, after filtering (i.e. the total number of records after filtering has been applied - not just the number of records being returned for this page of data).
        /// </summary>
        [JsonProperty("recordsFiltered")]
        public int RecordsFiltered { get; set; }

        /// <summary>
        /// The data to be displayed in the table.
        /// This is an array of data source objects, one for each row, which will be used by DataTables.
        /// Note that this parameter's name can be changed using the ajax option's dataSrc property.
        /// </summary>
        [JsonProperty("data")]
        public IEnumerable<T> Data { get; set; }

        /// <summary>
        /// Optional: If an error occurs during the running of the server-side processing script, you can inform the user of this error by passing back the error message to be displayed using this parameter.
        /// Do not include if there is no error.
        /// </summary>
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }

        public string PartialView { get; set; }
    }

    /// <summary>
    /// The additional columns that you can send to jQuery DataTables for automatic processing.
    /// </summary>
    public abstract class DtRow
    {
        /// <summary>
        /// Set the ID property of the dt-tag tr node to this value
        /// </summary>
        [JsonProperty("DT_RowId")]
        public virtual string DtRowId => null;

        /// <summary>
        /// Add this class to the dt-tag tr node
        /// </summary>
        [JsonProperty("DT_RowClass")]
        public virtual string DtRowClass => null;

        /// <summary>
        /// Add the data contained in the object to the row using the jQuery data() method to set the data, which can also then be used for later retrieval (for example on a click event).
        /// </summary>
        [JsonProperty("DT_RowData")]
        public virtual object DtRowData => null;

        /// <summary>
        /// Add the data contained in the object to the row dt-tag tr node as attributes.
        /// The object keys are used as the attribute keys and the values as the corresponding attribute values.
        /// This is performed using using the jQuery param() method.
        /// Please note that this option requires DataTables 1.10.5 or newer.
        /// </summary>
        [JsonProperty("DT_RowAttr")]
        public virtual object DtRowAttr => null;
    }

    /// <summary>
    /// The parameters sent by jQuery DataTables in AJAX queries.
    /// </summary>
    public class DtParameters
    {
        /// <summary>
        /// Draw counter.
        /// This is used by DataTables to ensure that the Ajax returns from server-side processing requests are drawn in sequence by DataTables (Ajax requests are asynchronous and thus can return out of sequence).
        /// This is used as part of the draw return parameter (see below).
        /// </summary>
        public int Draw { get; set; }

        /// <summary>
        /// An array defining all columns in the table.
        /// </summary>
        public DtColumn[] Columns { get; set; }

        /// <summary>
        /// An array defining how many columns are being ordering upon - i.e. if the array length is 1, then a single column sort is being performed, otherwise a multi-column sort is being performed.
        /// </summary>
        public DtOrder[] Order { get; set; }

        /// <summary>
        /// Paging first record indicator.
        /// This is the start point in the current data set (0 index based - i.e. 0 is the first record).
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Number of records that the table can display in the current draw.
        /// It is expected that the number of records returned will be equal to this number, unless the server has fewer records to return.
        /// Note that this can be -1 to indicate that all records should be returned (although that negates any benefits of server-side processing!)
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Global search value. To be applied to all columns which have searchable as true.
        /// </summary>
        public DtSearch Search { get; set; }

        /// <summary>
        /// Custom column that is used to further sort on the first Order column.
        /// </summary>
        public string SortOrder => Columns != null && Order != null && Order.Length > 0
            ? (Columns[Order[0].Column].Data +
               (Order[0].Dir == DtOrderDir.Desc ? " " + Order[0].Dir : string.Empty))
            : null;

        /// <summary>
        /// For Posting Additional Parameters to Server
        /// </summary>
        public IEnumerable<string> AdditionalValues { get; set; }

        public String Listtype { get; set; }

    }

    /// <summary>
    /// A jQuery DataTables column.
    /// </summary>
    public class DtColumn
    {
        /// <summary>
        /// Column's data source, as defined by columns.data.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Column's name, as defined by columns.name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Flag to indicate if this column is searchable (true) or not (false). This is controlled by columns.searchable.
        /// </summary>
        public bool Searchable { get; set; }

        /// <summary>
        /// Flag to indicate if this column is orderable (true) or not (false). This is controlled by columns.orderable.
        /// </summary>
        public bool Orderable { get; set; }

        /// <summary>
        /// Search value to apply to this specific column.
        /// </summary>
        public DtSearch Search { get; set; }
    }

    /// <summary>
    /// An order, as sent by jQuery DataTables when doing AJAX queries.
    /// </summary>
    public class DtOrder
    {
        /// <summary>
        /// Column to which ordering should be applied.
        /// This is an index reference to the columns array of information that is also submitted to the server.
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Ordering direction for this column.
        /// It will be dt-string asc or dt-string desc to indicate ascending ordering or descending ordering, respectively.
        /// </summary>
        public DtOrderDir Dir { get; set; }
    }

    /// <summary>
    /// Sort orders of jQuery DataTables.
    /// </summary>
    public enum DtOrderDir
    {
        Asc,
        Desc
    }

    /// <summary>
    /// A search, as sent by jQuery DataTables when doing AJAX queries.
    /// </summary>
    public class DtSearch
    {
        /// <summary>
        /// Global search value. To be applied to all columns which have searchable as true.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// true if the global filter should be treated as a regular expression for advanced searching, false otherwise.
        /// Note that normally server-side processing scripts will not perform regular expression searching for performance reasons on large data sets, but it is technically possible and at the discretion of your script.
        /// </summary>
        public bool Regex { get; set; }
    }

    public static class LinqExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(
            this IQueryable<T> query,
            string orderByMember,
            DtOrderDir ascendingDirection)
        {
            var param = Expression.Parameter(typeof(T), "c");

            var body = orderByMember.Split('.').Aggregate<string, Expression>(param, Expression.PropertyOrField);

            var queryable = ascendingDirection == DtOrderDir.Asc ?
                (IOrderedQueryable<T>)Queryable.OrderBy(query.AsQueryable(), (dynamic)Expression.Lambda(body, param)) :
                (IOrderedQueryable<T>)Queryable.OrderByDescending(query.AsQueryable(), (dynamic)Expression.Lambda(body, param));

            return queryable;
        }

        public static IQueryable<T> WhereDynamic<T>(
            this IQueryable<T> sourceList, string query)
        {

            if (string.IsNullOrEmpty(query))
            {
                return sourceList;
            }

            try
            {

                var properties = typeof(T).GetProperties()
                    .Where(x => x.CanRead && x.CanWrite && !x.GetGetMethod().IsVirtual);

                //Expression
                sourceList = sourceList.Where(c =>
                    properties.Any(p => p.GetValue(c) != null && p.GetValue(c).ToString()
                        .Contains(query, StringComparison.InvariantCultureIgnoreCase)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return sourceList;
        }
    }
}
