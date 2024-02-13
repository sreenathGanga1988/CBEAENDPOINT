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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DayQuoteController : BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;

        public DayQuoteController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/DayQuote
        [HttpGet]
        public ActionResult<IEnumerable<DayQuote>> GetDayQuote()
        {
             
               return Ok(_unitOfWork.DayQuote.GetAll());
        }

        // POST: api/DayQuote
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult <DayQuote> PostDayQuote(DayQuote DayQuote)
        {
            _unitOfWork.DayQuote.Add(DayQuote);
            _unitOfWork.SaveAllChanges();

            return CreatedAtAction("GetDayQuote", new { id = DayQuote.Id }, DayQuote);
        }




        // GET: api/DayQuote/5
        [HttpGet("{id}")]
        public  ActionResult<DayQuote> GetDayQuote(int id)
        {
            var DayQuote = _unitOfWork.DayQuote.GetById (id);

            if (DayQuote == null)
            {
                return NotFound();
            }

            return DayQuote;
        }

        // PUT: api/DayQuote/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public  IActionResult PutDayQuote(int id, DayQuote DayQuote)
        {
            if (id != DayQuote.Id)
            {
                return BadRequest();
            }

            _unitOfWork.DayQuote.Update(DayQuote);

            try
            {
                 _unitOfWork.SaveAllChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_unitOfWork.DayQuote.DataExists(u=>u.Id==id))
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


        // DELETE: api/DayQuote/5
        [HttpDelete("{id}")]
        public ActionResult<DayQuote> DeleteDayQuote(int id)
        {
            var DayQuote = _unitOfWork.DayQuote.GetById(id);
            if (DayQuote == null)
            {
                return NotFound();
            }

            _unitOfWork.DayQuote.Remove(DayQuote);
             _unitOfWork.SaveAllChanges();

            return DayQuote;
        }

        //private bool DayQuoteExists(int id)
        //{
        //    return _context.DayQuote.Any(e => e.Id == id);
        //}
    }
}
