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
    public class MainReportController : BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;

        public MainReportController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/MainReport
        [HttpGet]
        public ActionResult<IEnumerable<MainReport>> GetMainReport()
        {
             
               return Ok(_unitOfWork.MainReport.GetAll());
        }

        // POST: api/MainReport
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult <MainReport> PostMainReport(MainReport MainReport)
        {
            _unitOfWork.MainReport.Add(MainReport);
            _unitOfWork.SaveAllChanges();

            return CreatedAtAction("GetMainReport", new { id = MainReport.Id }, MainReport);
        }




        // GET: api/MainReport/5
        [HttpGet("{id}")]
        public  ActionResult<MainReport> GetMainReport(int id)
        {
            var MainReport = _unitOfWork.MainReport.GetById (id);

            if (MainReport == null)
            {
                return NotFound();
            }

            return MainReport;
        }

        // PUT: api/Category/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public  IActionResult PutMainReport(int id, MainReport MainReport)
        {
            if (id != MainReport.Id)
            {
                return BadRequest();
            }

            _unitOfWork.MainReport.Update(MainReport);

            try
            {
                 _unitOfWork.SaveAllChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_unitOfWork.MainReport.DataExists(u=>u.Id==id))
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


        // DELETE: api/MainReport/5
        [HttpDelete("{id}")]
        public ActionResult<MainReport> DeleteMainReport(int id)
        {
            var MainReport = _unitOfWork.MainReport.GetById(id);
            if (MainReport == null)
            {
                return NotFound();
            }

            _unitOfWork.MainReport.Remove(MainReport);
             _unitOfWork.SaveAllChanges();

            return MainReport;
        }

        //private bool CategoryExists(int id)
        //{
        //    return _context.Category.Any(e => e.Id == id);
        //}
    }
}
