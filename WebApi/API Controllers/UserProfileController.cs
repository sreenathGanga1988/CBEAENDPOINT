using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserProfileController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/UserProfile
        [HttpGet]
        public ActionResult<IEnumerable<dynamic>> GetUserProfile()
        {

            return _unitOfWork.Member.GetAllMemberasync(u=>u.StaffNo !=0).ToList<dynamic >();
        }

        // GET: api/UserProfile/5
        [HttpGet("{id}")]
        public ActionResult<dynamic> GetUserProfile(int id)
        {
            return _unitOfWork.Member.GetAllMemberasync(u=>u.StaffNo==id).FirstOrDefault();
            
        }



        [HttpGet()]
        [Route("GetUserContribution/{id}")]
        public ActionResult<IEnumerable<dynamic>> GetUserContribution(int id)
        {
            return _unitOfWork.AccountsRepository.GetContribution(id);

        }





        //// GET: api/UserProfile/5
        //[HttpGet("{id}")]
        //public ActionResult<UserProfile> GetUserProfile(int id)
        //{
        //    var UserProfile = _unitOfWork.UserProfile.GetById(id);

        //    if (UserProfile == null)
        //    {
        //        return NotFound();
        //    }

        //    return UserProfile;
        //}




    }
}
