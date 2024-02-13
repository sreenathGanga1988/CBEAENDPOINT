using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using WebApi.API_Controllers;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController :  BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;

        public BranchController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Branch
        [HttpGet]
        public ActionResult<IEnumerable<Branch>> GetBranch()
        {
             
               return Ok(_unitOfWork.Branch.GetBrachDetails());
        }

        // POST: api/Branch
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult <Branch> PostBranch(Branch Branch)
        {
            if(_unitOfWork.Branch.DataExists(u=>u.DpCode==Branch.DpCode))
            {
                return BadRequest("Dpcode Already Exist");
            }

            _unitOfWork.Branch.Add(Branch);
            _unitOfWork.SaveAllChanges();

            return CreatedAtAction("GetBranch", new { id = Branch.Id }, Branch);
        }




        // GET: api/Branch/5
        [HttpGet("{id}")]
        public  ActionResult<Branch> GetBranch(int id)
        {
            var Branch = _unitOfWork.Branch.GetBrachDetail (id);

            if (Branch == null)
            {
                return NotFound();
            }

            return Branch;
        }

        // PUT: api/Branch/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public  IActionResult PutBranch(int id, Branch Branch)
        {
            if (id != Branch.DpCode)
            {
                return BadRequest();
            }

            _unitOfWork.Branch.Update(Branch);

            try
            {
                 _unitOfWork.SaveAllChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_unitOfWork.Branch.DataExists(u=>u.Id==id))
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


        // DELETE: api/Branch/5
        [HttpDelete("{id}")]
        public ActionResult<Branch> DeleteBranch(int id)
        {
            var Branch = _unitOfWork.Branch.GetBrachDetail(id);
            if (Branch == null)
            {
                return NotFound();
            }

            _unitOfWork.Branch.Remove(Branch);
             _unitOfWork.SaveAllChanges();

            return Branch;
        }

        //private bool BranchExists(int id)
        //{
        //    return _context.Branch.Any(e => e.Id == id);
        //}
    }
}
