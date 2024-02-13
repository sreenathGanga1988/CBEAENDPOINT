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
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/User
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUser()
        {
             
               return Ok(_unitOfWork.User.GetAll());
        }

        // POST: api/User
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult<User> PostUser(User User)
        {

            if (_unitOfWork.User.DataCount(u => u.EmployeeId == User.EmployeeId) != 0)
            {
                return BadRequest("User added Already !!!");
            }

            if (_unitOfWork.Member.DataCount(u => u.StaffNo == User.EmployeeId) == 0)
            {
                return BadRequest("Invalid staff number !!!");

            }
            if (_unitOfWork.User.DataExists(u => u.UserName == User.UserName))
            {
                return BadRequest("User Name Already Exist");
            }
            _unitOfWork.User.Add(User);
            _unitOfWork.SaveAllChanges();
            return CreatedAtAction("GetUser", new { id = User.Id }, User);
        }




        // GET: api/User/5
        [HttpGet("{id}")]
        public  ActionResult<User> GetUser(int id)
        {
            var User = _unitOfWork.User.GetById (id);

            if (User == null)
            {
                return NotFound();
            }

            return User;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public  IActionResult PutUser(int id, User User)
        {
            if (id != User.Id)
            {
                return BadRequest();
            }

            _unitOfWork.User.Update(User);

            try
            {
                 _unitOfWork.SaveAllChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_unitOfWork.User.DataExists(u=>u.Id==id))
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


        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public ActionResult<User> DeleteUser(int id)
        {
            var User = _unitOfWork.User.GetById(id);
            if (User == null)
            {
                return NotFound();
            }

            _unitOfWork.User.Remove(User);
             _unitOfWork.SaveAllChanges();

            return User;
        }

        //private bool UserExists(int id)
        //{
        //    return _context.User.Any(e => e.Id == id);
        //}
    }
}
