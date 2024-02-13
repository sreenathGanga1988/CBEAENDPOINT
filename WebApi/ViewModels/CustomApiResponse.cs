using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels
{
        public class CustomApiResponse
        {
            public int StatusCode { get; set; }
            public String Error { get; set; }
            public String CustomMessage { get; set; }
            public bool IsSucess { get; set; }
            public Object Value { get; set; }
        }
}
