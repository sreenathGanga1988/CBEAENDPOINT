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
    public class DeathClaimController : BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeathClaimController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/DeathClaim
        [HttpGet]
        public ActionResult<IEnumerable<DeathClaim>> GetDeathClaim()
        {
             
               return Ok(_unitOfWork.DeathClaimRepository.GetAllClaims());
        }

        // POST: api/DeathClaim
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult <DeathClaim> PostDeathClaim(DeathClaim DeathClaim)
        {
            _unitOfWork.DeathClaimRepository.Add(DeathClaim);
            _unitOfWork.SaveAllChanges();

            return CreatedAtAction("GetDeathClaim", new { id = DeathClaim.Id }, DeathClaim);
        }




        // GET: api/DeathClaim/5
        [HttpGet("{id}")]
        public  ActionResult<DeathClaim> GetDeathClaim(int id)
        {
            var DeathClaim = _unitOfWork.DeathClaimRepository.GetById (id);

            if (DeathClaim == null)
            {
                return NotFound();
            }

            return DeathClaim;
        }

        // PUT: api/DeathClaim/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public  IActionResult PutDeathClaim(int id, DeathClaim DeathClaim)
        {
            if (id != DeathClaim.Id)
            {
                return BadRequest();
            }

            _unitOfWork.DeathClaimRepository.Update(DeathClaim);

            try
            {
                 _unitOfWork.SaveAllChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_unitOfWork.DeathClaimRepository.DataExists(u=>u.Id==id))
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


        // DELETE: api/DeathClaim/5
        [HttpDelete("{id}")]
        public ActionResult<DeathClaim> DeleteDeathClaim(int id)
        {
            var DeathClaim = _unitOfWork.DeathClaimRepository.GetById(id);
            if (DeathClaim == null)
            {
                return NotFound();
            }

            _unitOfWork.DeathClaimRepository.Remove(DeathClaim);
             _unitOfWork.SaveAllChanges();

            return DeathClaim;
        }

        //private bool DeathClaimExists(int id)
        //{
        //    return _context.DeathClaim.Any(e => e.Id == id);
        //}
    }
}
