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
    public class StateController : BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;

        public StateController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/State
        [HttpGet]
        public ActionResult<IEnumerable<State>> GetState()
        {
             
               return Ok(_unitOfWork.State.GetAll());
        }

        // POST: api/State
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult <State> PostState(State State)
        {
            _unitOfWork.State.Add(State);
            _unitOfWork.SaveAllChanges();

            return CreatedAtAction("GetState", new { id = State.Id }, State);
        }




        // GET: api/State/5
        [HttpGet("{id}")]
        public  ActionResult<State> GetState(int id)
        {
            var State = _unitOfWork.State.GetById (id);

            if (State == null)
            {
                return NotFound();
            }

            return State;
        }

        // PUT: api/State/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public  IActionResult PutState(int id, State State)
        {
            if (id != State.Id)
            {
                return BadRequest();
            }

            _unitOfWork.State.Update(State);

            try
            {
                 _unitOfWork.SaveAllChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_unitOfWork.State.DataExists(u=>u.Id==id))
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


        // DELETE: api/State/5
        [HttpDelete("{id}")]
        public ActionResult<State> DeleteState(int id)
        {
            var State = _unitOfWork.State.GetById(id);
            if (State == null)
            {
                return NotFound();
            }

            _unitOfWork.State.Remove(State);
             _unitOfWork.SaveAllChanges();

            return State;
        }

        //private bool StateExists(int id)
        //{
        //    return _context.State.Any(e => e.Id == id);
        //}
    }
}
