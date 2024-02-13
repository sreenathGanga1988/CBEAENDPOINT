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
    public class ManagingComiteeController : BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManagingComiteeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/ManagingComitee
        [HttpGet]
        public ActionResult<IEnumerable<ManagingComitee>> GetManagingComitee()
        {
             
               return Ok(_unitOfWork.ManagingComitee.GetAll());
        }

        // POST: api/ManagingComitee
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult <ManagingComitee> PostManagingComitee(ManagingComitee ManagingComitee)
        {
            _unitOfWork.ManagingComitee.Add(ManagingComitee);
            _unitOfWork.SaveAllChanges();

            return CreatedAtAction("GetManagingComitee", new { id = ManagingComitee.Id }, ManagingComitee);
        }




        // GET: api/ManagingComitee/5
        [HttpGet("{id}")]
        public  ActionResult<ManagingComitee> GetManagingComitee(int id)
        {
            var ManagingComitee = _unitOfWork.ManagingComitee.GetById (id);

            if (ManagingComitee == null)
            {
                return NotFound();
            }

            return ManagingComitee;
        }

        // PUT: api/ManagingComitee/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public  IActionResult PutManagingComitee(int id, ManagingComitee ManagingComitee)
        {
            if (id != ManagingComitee.Id)
            {
                return BadRequest();
            }

            _unitOfWork.ManagingComitee.Update(ManagingComitee);

            try
            {
                 _unitOfWork.SaveAllChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_unitOfWork.ManagingComitee.DataExists(u=>u.Id==id))
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


        // DELETE: api/ManagingComitee/5
        [HttpDelete("{id}")]
        public ActionResult<ManagingComitee> DeleteManagingComitee(int id)
        {
            var ManagingComitee = _unitOfWork.ManagingComitee.GetById(id);
            if (ManagingComitee == null)
            {
                return NotFound();
            }

            _unitOfWork.ManagingComitee.Remove(ManagingComitee);
             _unitOfWork.SaveAllChanges();

            return ManagingComitee;
        }

        //private bool ManagingComiteeExists(int id)
        //{
        //    return _context.ManagingComitee.Any(e => e.Id == id);
        //}
    }
}
