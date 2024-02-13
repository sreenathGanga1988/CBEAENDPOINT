using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebApi.API_Controllers;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoryController : BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Category
        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetCategory()
        {
             
               return Ok(_unitOfWork.Category.GetAll());
        }

        // POST: api/Category
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult <Category> PostCategory(Category Category)
        {
            _unitOfWork.Category.Add(Category);
            _unitOfWork.SaveAllChanges();

            return CreatedAtAction("GetCategory", new { id = Category.Id }, Category);
        }




        // GET: api/Category/5
        [HttpGet("{id}")]
        public  ActionResult<Category> GetCategory(int id)
        {
            var Category = _unitOfWork.Category.GetById (id);

            if (Category == null)
            {
                return NotFound();
            }

            return Category;
        }

        // PUT: api/Category/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public  IActionResult PutCategory(int id, Category Category)
        {
            if (id != Category.Id)
            {
                return BadRequest();
            }

            _unitOfWork.Category.Update(Category);

            try
            {
                 _unitOfWork.SaveAllChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_unitOfWork.Category.DataExists(u=>u.Id==id))
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


        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public ActionResult<Category> DeleteCategory(int id)
        {
            var Category = _unitOfWork.Category.GetById(id);
            if (Category == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(Category);
             _unitOfWork.SaveAllChanges();

            return Category;
        }

        //private bool CategoryExists(int id)
        //{
        //    return _context.Category.Any(e => e.Id == id);
        //}
    }
}
