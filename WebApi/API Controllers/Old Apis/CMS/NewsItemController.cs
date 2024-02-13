using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Domain;
using System;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsItemController : BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;

        public NewsItemController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/NewsItem
        [HttpGet]
        public ActionResult<IEnumerable<NewsItem>> GetNewsItem()
        {
             
               return Ok(_unitOfWork.NewsItem.GetAll());
        }

        // POST: api/NewsItem
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult <NewsItem> PostNewsItem(NewsItem NewsItem)
        {
            _unitOfWork.NewsItem.Add(NewsItem);
            _unitOfWork.SaveAllChanges();

            return CreatedAtAction("GetNewsItem", new { id = NewsItem.Id }, NewsItem);
        }




        // GET: api/NewsItem/5
        [HttpGet("{id}")]
        public  ActionResult<NewsItem> GetNewsItem(int id)
        {
            var NewsItem = _unitOfWork.NewsItem.GetById (id);

            if (NewsItem == null)
            {
                return NotFound();
            }

            return NewsItem;
        }

        // PUT: api/NewsItem/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public  IActionResult PutNewsItem(int id, NewsItem NewsItem)
        {
            if (id != NewsItem.Id)
            {
                return BadRequest();
            }

            _unitOfWork.NewsItem.Update(NewsItem);

            try
            {
                 _unitOfWork.SaveAllChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_unitOfWork.NewsItem.DataExists(u=>u.Id==id))
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


        // DELETE: api/NewsItem/5
        [HttpDelete("{id}")]
        public ActionResult<NewsItem> DeleteNewsItem(int id)
        {
            var NewsItem = _unitOfWork.NewsItem.GetById(id);
            if (NewsItem == null)
            {
                return NotFound();
            }

            _unitOfWork.NewsItem.Remove(NewsItem);
             _unitOfWork.SaveAllChanges();

            return NewsItem;
        }

          
        [HttpPost, Route("RemoveNewsItem")]
        public ActionResult PostDataTable([FromBody] String deldate)
        {
            var deletedate = DateTime.Parse(deldate.ToString());
            var q = _unitOfWork.NewsItem.Find(u => u.DateofAction == deletedate);
            _unitOfWork.NewsItem.RemoveRange(q);
            _unitOfWork.SaveAllChanges();
            return Ok();
        }


        //private bool NewsItemExists(int id)
        //{
        //    return _context.NewsItem.Any(e => e.Id == id);
        //}
    }
}
