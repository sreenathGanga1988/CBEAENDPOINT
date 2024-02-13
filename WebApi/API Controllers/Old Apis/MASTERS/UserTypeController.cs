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
    public class UserTypeController : BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/UserType
        [HttpGet]
        public ActionResult<IEnumerable<UserType>> GetUserType()
        {

            try
            {
                var k = _unitOfWork.UserType.GetAll();
                return Ok(k);
            }
            catch (System.Exception ex )
            {
             
                throw;
            }
             
        }

        // POST: api/UserType
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult <UserType> PostUserType(UserType UserType)
        {
            _unitOfWork.UserType.Add(UserType);
            _unitOfWork.SaveAllChanges();

            return CreatedAtAction("GetUserType", new { id = UserType.Id }, UserType);
        }




        // GET: api/UserType/5
        [HttpGet("{id}")]
        public  ActionResult<UserType> GetUserType(int id)
        {
            var UserType = _unitOfWork.UserType.GetById (id);

            if (UserType == null)
            {
                return NotFound();
            }

            return UserType;
        }

        // PUT: api/UserType/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public  IActionResult PutUserType(int id, UserType UserType)
        {
            if (id != UserType.Id)
            {
                return BadRequest();
            }

            _unitOfWork.UserType.Update(UserType);

            try
            {
                 _unitOfWork.SaveAllChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_unitOfWork.UserType.DataExists(u=>u.Id==id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // DELETE: api/UserType/5
        [HttpDelete("{id}")]
        public ActionResult<UserType> DeleteUserType(int id)
        {
            var UserType = _unitOfWork.UserType.GetById(id);
            if (UserType == null)
            {
                return NotFound();
            }

            _unitOfWork.UserType.Remove(UserType);
             _unitOfWork.SaveAllChanges();

            return UserType;
        }

        //private bool UserTypeExists(int id)
        //{
        //    return _context.UserType.Any(e => e.Id == id);
        //}
    }
}
