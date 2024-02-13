using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApi.ViewModels;

namespace WebApi.Services
{
    public class CustomResponseBuilder
    {

        public void SuccessReponse(object data, ref CustomApiResponse response)
        {
            response.IsSucess = true;
            response.Value = data;
            response.StatusCode = (int)HttpStatusCode.OK;


        }
        public void SuccessReponse(object data, ref CustomApiResponse response, int statuscode)
        {
            response.IsSucess = true;
            response.Value = data;

            response.StatusCode = (int)statuscode;


        }

        public void CustomErrorReponse(ref CustomApiResponse response, int? statuscode = 0)
        {
            response.IsSucess = false;
            if (statuscode == null || statuscode == 0)
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                response.StatusCode = (int)statuscode;
            }

        }


        public void ExceptionResponse(Exception ex, ref CustomApiResponse response)
        {
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Error = ex.Message;
            response.IsSucess = false;
            response.Value = null;
        }
        public void ActionNotAuthorizedResponse(ref CustomApiResponse response)
        {
            response.StatusCode = (int)HttpStatusCode.Unauthorized;
            response.CustomMessage = "Not Authorized for this action";
            response.IsSucess = false;
            response.Value = null;
        }
    }
}
