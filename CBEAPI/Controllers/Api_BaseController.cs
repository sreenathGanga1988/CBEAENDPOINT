using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using CBEAPI.Services;
using CBEAPI.ViewModels;
namespace CBEAPI.Controllers
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
