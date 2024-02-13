using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContributionDetailController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContributionDetailController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/ContributionDetail
        [HttpGet]
        public ActionResult<IEnumerable<ContributionDetail>> GetContributionDetail()
        {
             
               return Ok(_unitOfWork.ContributionDetails.GetAll());
        }

        // POST: api/ContributionDetail
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult <ContributionDetail> PostContributionDetail(ContributionDetail ContributionDetail)
        {
            _unitOfWork.ContributionDetails.Add(ContributionDetail);
            _unitOfWork.SaveAllChanges();

            return CreatedAtAction("GetContributionDetail", new { id = ContributionDetail.Id }, ContributionDetail);
        }




        // GET: api/ContributionDetail/5
        [HttpGet("{id}")]
        public  ActionResult<ContributionDetail> GetContributionDetail(int id)
        {
            var ContributionDetail = _unitOfWork.ContributionDetails.Find(u => u.Id == id).FirstOrDefault();

            if (ContributionDetail == null)
            {
                return NotFound();
            }

            return ContributionDetail;
        }

        // PUT: api/ContributionDetail/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public  IActionResult PutContributionDetail(int id, ContributionDetail ContributionDetail)
        {
            if (id != ContributionDetail.Id)
            {
                return BadRequest();
            }

            _unitOfWork.ContributionDetails.Update(ContributionDetail);

            try
            {
                 _unitOfWork.SaveAllChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_unitOfWork.ContributionDetails.DataExists(u=>u.Id==id))
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


        // DELETE: api/ContributionDetail/5
        [HttpDelete("{id}")]
        public ActionResult<ContributionDetail> DeleteContributionDetail(int id)
        {
            var ContributionDetail = _unitOfWork.ContributionDetails.GetById(id);
            if (ContributionDetail == null)
            {
                return NotFound();
            }

            _unitOfWork.ContributionDetails.Remove(ContributionDetail);
             _unitOfWork.SaveAllChanges();

            return ContributionDetail;
        }

        //private bool ContributionDetailExists(int id)
        //{
        //    return _context.ContributionDetail.Any(e => e.Id == id);
        //}

        [HttpGet()]
        [Route("UnParkContribution/{id}/{curentuserid}")]
        public ActionResult<IEnumerable<dynamic>> UnParkContribution(int id, int curentuserid=0)
        {
            try
            {
                string Message = "";
                var k = _unitOfWork.ContributionDetails.UnParkContribution(id, curentuserid, out Message);
                return Ok();
            }
            catch (System.Exception ex)
            {

                throw;
            }

        }
    }
}
