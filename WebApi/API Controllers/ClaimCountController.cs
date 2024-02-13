using System.Collections.Generic;
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
    public class ClaimCountController : BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClaimCountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/ClaimCount
        [HttpGet]
        public ActionResult<IEnumerable<ClaimCount>> GetClaimCount()
        {
             
               return Ok(_unitOfWork.ClaimCount.GetAll());
        }

        // POST: api/ClaimCount
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult <ClaimCount> PostClaimCount(ClaimCount ClaimCount)
        {
            _unitOfWork.ClaimCount.Add(ClaimCount);
            _unitOfWork.SaveAllChanges();

            return CreatedAtAction("GetClaimCount", new { id = ClaimCount.Id }, ClaimCount);
        }




        // GET: api/ClaimCount/5
        [HttpGet("{id}")]
        public  ActionResult<ClaimCount> GetClaimCount(int id)
        {
            var ClaimCount = _unitOfWork.ClaimCount.GetById (id);

            if (ClaimCount == null)
            {
                return NotFound();
            }

            return ClaimCount;
        }

        // PUT: api/ClaimCount/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public  IActionResult PutClaimCount(int id, ClaimCount ClaimCount)
        {
            if (id != ClaimCount.Id)
            {
                return BadRequest();
            }

            _unitOfWork.ClaimCount.Update(ClaimCount);

            try
            {
                 _unitOfWork.SaveAllChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_unitOfWork.ClaimCount.DataExists(u=>u.Id==id))
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


        // DELETE: api/ClaimCount/5
        [HttpDelete("{id}")]
        public ActionResult<ClaimCount> DeleteClaimCount(int id)
        {
            var ClaimCount = _unitOfWork.ClaimCount.GetById(id);
            if (ClaimCount == null)
            {
                return NotFound();
            }

            _unitOfWork.ClaimCount.Remove(ClaimCount);
             _unitOfWork.SaveAllChanges();

            return ClaimCount;
        }

        //private bool ClaimCountExists(int id)
        //{
        //    return _context.ClaimCount.Any(e => e.Id == id);
        //}
    }
}
