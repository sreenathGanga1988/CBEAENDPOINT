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
    public class MemberController : BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Member
        [HttpGet]
        public ActionResult<IEnumerable<Domain.Entities.Member>> GetMember()
        {
             
               return Ok(_unitOfWork.Member.GetAllemployeeProfile());
        }

        // POST: api/Member
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult <Domain.Entities.Member> PostMember(Domain.Entities.Member Member)
        {
            if (_unitOfWork.Member.DataExists(u => u.StaffNo == Member.StaffNo))
            {
                return BadRequest("StaffNo Code Already Exist");
            }
            _unitOfWork.Member.Add(Member);
            _unitOfWork.SaveAllChanges();

            return CreatedAtAction("GetMember", new { id = Member.Id }, Member);
        }




        // GET: api/Member/5
        [HttpGet("{id}")]
        public  ActionResult<Domain.Entities.Member> GetMember(int id)
        {
            var Member = _unitOfWork.Member.GetemployeeProfile (id);

            if (Member == null)
            {
                return NotFound();
            }

            return Member;
        }

        // PUT: api/Member/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public  IActionResult PutMember(int id, Domain.Entities.Member Member)
        {
            if (id != Member.Id)
            {
                return BadRequest();
            }

            _unitOfWork.Member.Update(Member);

            try
            {
                 _unitOfWork.SaveAllChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_unitOfWork.Member.DataExists(u=>u.Id==id))
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


        // DELETE: api/Member/5
        [HttpDelete("{id}")]
        public ActionResult<Domain.Entities.Member> DeleteMember(int id)
        {
            var Member = _unitOfWork.Member.GetById(id);
            if (Member == null)
            {
                return NotFound();
            }

            _unitOfWork.Member.Remove(Member);
             _unitOfWork.SaveAllChanges();

            return Member;
        }

        //private bool MemberExists(int id)
        //{
        //    return _context.Member.Any(e => e.Id == id);
        //}
    }
}
