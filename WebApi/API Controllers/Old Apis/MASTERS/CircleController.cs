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
    public class CircleController : BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;

        public CircleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Circle
        [HttpGet]
        public ActionResult<IEnumerable<Circle>> GetCircle()
        {
             
               return Ok(_unitOfWork.Circle.GetAll());
        }

        // POST: api/Circle
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult <Circle> PostCircle(Circle Circle)
        {
            if (_unitOfWork.Circle.DataExists(u => u.CircleCode == Circle.CircleCode))
            {
                return BadRequest("Circle Code Already Exist");
            }
            _unitOfWork.Circle.Add(Circle);
            _unitOfWork.SaveAllChanges();

            return CreatedAtAction("GetCircle", new { id = Circle.Id }, Circle);
        }




        // GET: api/Circle/5
        [HttpGet("{id}")]
        public  ActionResult<Circle> GetCircle(int id)
        {
            var Circle = _unitOfWork.Circle.GetById (id);

            if (Circle == null)
            {
                return NotFound();
            }

            return Circle;
        }

        // PUT: api/Circle/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public  IActionResult PutCircle(int id, Circle Circle)
        {
            if (id != Circle.Id)
            {
                return BadRequest();
            }

            _unitOfWork.Circle.Update(Circle);

            try
            {
                 _unitOfWork.SaveAllChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_unitOfWork.Circle.DataExists(u=>u.Id==id))
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


        // DELETE: api/Circle/5
        [HttpDelete("{id}")]
        public ActionResult<Circle> DeleteCircle(int id)
        {
            var Circle = _unitOfWork.Circle.GetById(id);
            if (Circle == null)
            {
                return NotFound();
            }

            _unitOfWork.Circle.Remove(Circle);
             _unitOfWork.SaveAllChanges();

            return Circle;
        }

        //private bool CircleExists(int id)
        //{
        //    return _context.Circle.Any(e => e.Id == id);
        //}
    }
}
