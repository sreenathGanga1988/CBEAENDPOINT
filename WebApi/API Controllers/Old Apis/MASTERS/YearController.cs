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
    public class YearMasterController : BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;

        public YearMasterController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/YearMaster
        [HttpGet]
        public ActionResult<IEnumerable<YearMaster>> GetYearMaster()
        {
             
               return Ok(_unitOfWork.YearMaster.GetAll());
        }

        // POST: api/YearMaster
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult <YearMaster> PostYearMaster(YearMaster YearMaster)
        {
            _unitOfWork.YearMaster.Add(YearMaster);
            _unitOfWork.SaveAllChanges();

            return CreatedAtAction("GetYearMaster", new { id = YearMaster.YearOf }, YearMaster);
        }




        // GET: api/YearMaster/5
        [HttpGet("{id}")]
        public  ActionResult<YearMaster> GetYearMaster(int id)
        {
            var YearMaster = _unitOfWork.YearMaster.GetById (id);

            if (YearMaster == null)
            {
                return NotFound();
            }

            return YearMaster;
        }

        // PUT: api/YearMaster/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public  IActionResult PutYearMaster(int id, YearMaster YearMaster)
        {
            if (id != YearMaster.YearOf)
            {
                return BadRequest();
            }

            _unitOfWork.YearMaster.Update(YearMaster);

            try
            {
                 _unitOfWork.SaveAllChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_unitOfWork.YearMaster.DataExists(u=>u.YearOf==id))
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


        // DELETE: api/YearMaster/5
        [HttpDelete("{id}")]
        public ActionResult<YearMaster> DeleteYearMaster(int id)
        {
            var YearMaster = _unitOfWork.YearMaster.GetById(id);
            if (YearMaster == null)
            {
                return NotFound();
            }

            _unitOfWork.YearMaster.Remove(YearMaster);
             _unitOfWork.SaveAllChanges();

            return YearMaster;
        }

        //private bool YearMasterExists(int id)
        //{
        //    return _context.YearMaster.Any(e => e.Id == id);
        //}
    }
}
