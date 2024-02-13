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
    public class DesignationController : BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;

        public DesignationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Designation
        [HttpGet]
        public ActionResult<IEnumerable<Designation>> GetDesignation()
        {
             
               return Ok(_unitOfWork.Designation.GetAll());
        }

        // POST: api/Designation
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult <Designation> PostDesignation(Designation Designation)
        {
            _unitOfWork.Designation.Add(Designation);
            _unitOfWork.SaveAllChanges();

            return CreatedAtAction("GetDesignation", new { id = Designation.Id }, Designation);
        }




        // GET: api/Designation/5
        [HttpGet("{id}")]
        public  ActionResult<Designation> GetDesignation(int id)
        {
            var Designation = _unitOfWork.Designation.GetById (id);

            if (Designation == null)
            {
                return NotFound();
            }

            return Designation;
        }

        // PUT: api/Designation/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public  IActionResult PutDesignation(int id, Designation Designation)
        {
            if (id != Designation.Id)
            {
                return BadRequest();
            }

            _unitOfWork.Designation.Update(Designation);

            try
            {
                 _unitOfWork.SaveAllChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_unitOfWork.Designation.DataExists(u=>u.Id==id))
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


        // DELETE: api/Designation/5
        [HttpDelete("{id}")]
        public ActionResult<Designation> DeleteDesignation(int id)
        {
            var Designation = _unitOfWork.Designation.GetById(id);
            if (Designation == null)
            {
                return NotFound();
            }

            _unitOfWork.Designation.Remove(Designation);
             _unitOfWork.SaveAllChanges();

            return Designation;
        }

        //private bool DesignationExists(int id)
        //{
        //    return _context.Designation.Any(e => e.Id == id);
        //}
    }
}
