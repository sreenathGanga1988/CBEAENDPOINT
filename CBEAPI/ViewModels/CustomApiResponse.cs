using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBEAPI.ViewModels
{
        public class CustomApiResponse
        {
            public int StatusCode { get; set; }
            public String Error { get; set; }
            public String CustomMessage { get; set; }
            public bool IsSucess { get; set; }
            public Object Value { get; set; }
        }

    public class PageParameter
    {
        public String ReportType { get; set; }
        public String SearchText { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
    public class PaginatedDataResponse
    {
        public Int32 TotalCount { get; set; }
        public Object  RowData { get; set; }
       
    }
}
