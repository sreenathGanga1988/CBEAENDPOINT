using DataAccess.EFCore.UnitOfWorks;
using Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApi.Services;
using WebApi.ViewModels;
namespace WebApi.Controllers
{
    [EnableCors("AllowOrigin")]
    //[Route("api/[controller]")]
    //[ApiController]   
    public class Api_BaseController : ControllerBase
    {
        public CustomResponseBuilder _resp;

        public String CurrentUserID = "0";
        public String EntityName = "";


     

        public Api_BaseController()
        {
           
            _resp = new CustomResponseBuilder();
            
        }
        
        public Boolean IsUserAuthorized(String ModuleName, String ActionName, ref CustomApiResponse response)
        {
            Boolean isPermitted = false;

            try
            {
                isPermitted = true;

            }
            catch (Exception ex)
            {
                response.Error = "You are not authorized for this action";
                isPermitted = false;
            }


            return isPermitted;

        }
        public void AddLogER(string ActionType, String PrimaryKry, Object obj)
        {
            // unitOfWork.AppAuditRepository.AddLogs(EntityName, PrimaryKry, ActionType, CurrentUserID, obj);
            // unitOfWork.SaveAllChanges();
        }



    }
}
