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
    public class FileUploadDetailController : BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;

        public FileUploadDetailController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/FileUploadDetail
        [HttpGet]
        public ActionResult<IEnumerable<FileUploadDetail>> GetFileUploadDetail()
        {
             
               return Ok(_unitOfWork.FileUploadDetail.GetAll());
        }

        // POST: api/FileUploadDetail
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult <FileUploadDetail> PostFileUploadDetail(FileUploadDetail FileUploadDetail)
        {
            _unitOfWork.FileUploadDetail.Add(FileUploadDetail);
            _unitOfWork.SaveAllChanges();

            return CreatedAtAction("GetFileUploadDetail", new { id = FileUploadDetail.Id }, FileUploadDetail);
        }




        // GET: api/FileUploadDetail/5
        [HttpGet("{id}")]
        public  ActionResult<FileUploadDetail> GetFileUploadDetail(int id)
        {
            var FileUploadDetail = _unitOfWork.FileUploadDetail.GetById (id);

            if (FileUploadDetail == null)
            {
                return NotFound();
            }

            return FileUploadDetail;
        }

        // PUT: api/FileUploadDetail/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public  IActionResult PutFileUploadDetail(int id, FileUploadDetail FileUploadDetail)
        {
            if (id != FileUploadDetail.Id)
            {
                return BadRequest();
            }

            _unitOfWork.FileUploadDetail.Update(FileUploadDetail);

            try
            {
                 _unitOfWork.SaveAllChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_unitOfWork.FileUploadDetail.DataExists(u=>u.Id==id))
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


        // DELETE: api/FileUploadDetail/5
        [HttpDelete("{id}")]
        public ActionResult<FileUploadDetail> DeleteFileUploadDetail(int id)
        {
            var FileUploadDetail = _unitOfWork.FileUploadDetail.GetById(id);
            if (FileUploadDetail == null)
            {
                return NotFound();
            }

            _unitOfWork.FileUploadDetail.Remove(FileUploadDetail);
             _unitOfWork.SaveAllChanges();

            return FileUploadDetail;
        }

        //private bool FileUploadDetailExists(int id)
        //{
        //    return _context.FileUploadDetail.Any(e => e.Id == id);
        //}
    }
}
