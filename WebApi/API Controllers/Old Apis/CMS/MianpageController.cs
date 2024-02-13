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
    public class MainPageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public MainPageController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/MainPage
        [HttpGet]
        public ActionResult<IEnumerable<MainPage>> GetMainPage()
        {
             
               return Ok(_unitOfWork.MainPage .GetAll());
        }

        // POST: api/MainPage
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult <MainPage> PostMainPage(MainPage MainPage)
        {
            _unitOfWork.MainPage.Add(MainPage);
            _unitOfWork.SaveAllChanges();

            return CreatedAtAction("GetMainPage", new { id = MainPage.Id }, MainPage);
        }




        // GET: api/MainPage/5
        [HttpGet("{id}")]
        public  ActionResult<MainPage> GetMainPage(int id)
        {
            var MainPage = _unitOfWork.MainPage.GetById (id);

            if (MainPage == null)
            {
                return NotFound();
            }

            return MainPage;
        }

        // PUT: api/MainPage/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public  IActionResult PutMainPage(int id, MainPage MainPage)
        {
            if (id != MainPage.Id)
            {
                return BadRequest();
            }

            _unitOfWork.MainPage.Update(MainPage);

            try
            {
                 _unitOfWork.SaveAllChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_unitOfWork.MainPage.DataExists(u=>u.Id==id))
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


        // DELETE: api/MainPage/5
        [HttpDelete("{id}")]
        public ActionResult<MainPage> DeleteMainPage(int id)
        {
            var MainPage = _unitOfWork.MainPage.GetById(id);
            if (MainPage == null)
            {
                return NotFound();
            }

            _unitOfWork.MainPage.Remove(MainPage);
             _unitOfWork.SaveAllChanges();

            return MainPage;
        }

        //private bool MainPageExists(int id)
        //{
        //    return _context.MainPage.Any(e => e.Id == id);
        //}
    }
}
