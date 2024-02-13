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
    public class SupportTicketController : BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;

        public SupportTicketController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/SupportTicket
        [HttpGet]
        public ActionResult<IEnumerable<SupportTicket>> GetSupportTicket()
        {
             
               return Ok(_unitOfWork.SupportTicket.GetAll());
        }

        // POST: api/SupportTicket
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult <SupportTicket> PostSupportTicket(SupportTicket SupportTicket)
        {
            try
            {

                _unitOfWork.SupportTicket.Add(SupportTicket);
                _unitOfWork.SaveAllChanges();
            }
            catch (System.Exception  ex)
            {

                var k = ex;

                throw;
            }

            return CreatedAtAction("GetSupportTicket", new { id = SupportTicket.Id }, SupportTicket);
        }




        // GET: api/SupportTicket/5
        [HttpGet("{id}")]
        public  ActionResult<SupportTicket> GetSupportTicket(int id)
        {
            var SupportTicket = _unitOfWork.SupportTicket.GetById (id);

            if (SupportTicket == null)
            {
                return NotFound();
            }

            return SupportTicket;
        }

        // PUT: api/SupportTicket/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public  IActionResult PutSupportTicket(int id, SupportTicket SupportTicket)
        {
            if (id != SupportTicket.Id)
            {
                return BadRequest();
            }

            _unitOfWork.SupportTicket.Update(SupportTicket);

            try
            {
                 _unitOfWork.SaveAllChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_unitOfWork.SupportTicket.DataExists(u=>u.Id==id))
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


        // DELETE: api/SupportTicket/5
        [HttpDelete("{id}")]
        public ActionResult<SupportTicket> DeleteSupportTicket(int id)
        {
            var SupportTicket = _unitOfWork.SupportTicket.GetById(id);
            if (SupportTicket == null)
            {
                return NotFound();
            }

            _unitOfWork.SupportTicket.Remove(SupportTicket);
             _unitOfWork.SaveAllChanges();

            return SupportTicket;
        }

        //private bool SupportTicketExists(int id)
        //{
        //    return _context.SupportTicket.Any(e => e.Id == id);
        //}
    }
}
