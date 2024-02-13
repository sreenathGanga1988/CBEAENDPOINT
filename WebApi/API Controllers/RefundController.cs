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
    public class RefundController : BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;

        public RefundController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/RefundContribution
        [HttpGet]
        public ActionResult<IEnumerable<RefundContribution>> GetRefundContribution()
        {
             
               return Ok(_unitOfWork.RefundRepository.GetAllRefunds());
        }

        // POST: api/RefundContribution
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult <RefundContribution> PostRefundContribution(RefundContribution RefundContribution)
        {
            _unitOfWork.RefundRepository.Add(RefundContribution);
            _unitOfWork.SaveAllChanges();

            return CreatedAtAction("GetRefundContribution", new { id = RefundContribution.Id }, RefundContribution);
        }




        // GET: api/RefundContribution/5
        [HttpGet("{id}")]
        public  ActionResult<RefundContribution> GetRefundContribution(int id)
        {
            var RefundContribution = _unitOfWork.RefundRepository.GetById (id);

            if (RefundContribution == null)
            {
                return NotFound();
            }

            return RefundContribution;
        }

        // PUT: api/RefundContribution/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public  IActionResult PutRefundContribution(int id, RefundContribution RefundContribution)
        {
            if (id != RefundContribution.Id)
            {
                return BadRequest();
            }

            _unitOfWork.RefundRepository.Update(RefundContribution);

            try
            {
                 _unitOfWork.SaveAllChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_unitOfWork.RefundRepository.DataExists(u=>u.Id==id))
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


        // DELETE: api/RefundContribution/5
        [HttpDelete("{id}")]
        public ActionResult<RefundContribution> DeleteRefundContribution(int id)
        {
            var RefundContribution = _unitOfWork.RefundRepository.GetById(id);
            if (RefundContribution == null)
            {
                return NotFound();
            }

            _unitOfWork.RefundRepository.Remove(RefundContribution);
             _unitOfWork.SaveAllChanges();

            return RefundContribution;
        }

        //private bool RefundContributionExists(int id)
        //{
        //    return _context.RefundContribution.Any(e => e.Id == id);
        //}
    }
}
