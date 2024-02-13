using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using WebApp.Services;

namespace WebApi.API_Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {

        private readonly ILogger<UploadController> _logger;
        private IWebHostEnvironment Environment;


        public UploadController(ILogger<UploadController> logger, IWebHostEnvironment _environment)
        {
            _logger = logger;
            Environment = _environment;

        }

        [HttpPost]
        public IActionResult PostUpload(List<IFormFile> postedFiles)
        {
            List<FileDetail> filedet = new List<FileDetail>();

            try
            {

                string wwwPath = this.Environment.WebRootPath;
                string contentPath = this.Environment.ContentRootPath;


                string relativepath = Path.Combine(HttpTaskService.baseuri, "Contribution_Uploads");
                string path = Path.Combine(this.Environment.WebRootPath, "Contribution_Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }


                List<string> uploadedFiles = new List<string>();
                foreach (IFormFile file in postedFiles)
                {
                    FileDetail fileDetail = new FileDetail();
                    fileDetail.FileActualname = file.FileName;

                    fileDetail.FileExtension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                    fileDetail.FileName = FilenameGenerator(fileDetail.FileExtension);
                    fileDetail.FileLocation = Path.Combine(path, fileDetail.FileName);

                    fileDetail.FileRelativeLocation = Path.Combine(path, fileDetail.FileName);

                    //string fileName = Path.GetFileName(postedFile.FileName);
                    using (FileStream stream = new FileStream(fileDetail.FileLocation, FileMode.Create))
                    {
                        file.CopyTo(stream);

                        filedet.Add(fileDetail);

                    }

                }
                return Ok(filedet);
            }

            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message, ex.InnerException);

                return BadRequest(new { message = "Invalid file extension" });
            }




        }

        public string FilenameGenerator(string extension)
        {
            string filename = "";
            filename = DateTime.Now.Ticks + extension; //Create a new Name for the file due to security reasons.
            return filename;

        }

        private async Task<bool> WriteFile(IFormFile file)
        {
            bool isSaveSuccess = false;
            string fileName;
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                fileName = DateTime.Now.Ticks + extension; //Create a new Name for the file due to security reasons.

                var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files");

                if (!Directory.Exists(pathBuilt))
                {
                    Directory.CreateDirectory(pathBuilt);
                }

                var path = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files",
                   fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                isSaveSuccess = true;
            }
            catch (Exception e)
            {
                //log error
            }

            return isSaveSuccess;
        }
    }

    public class FileDetail
    {
        public String FileActualname { get; set; }
        public String FileExtension { get; set; }

        public String FileName { get; set; }

        public String FileLocation { get; set; }

        public String FileRelativeLocation { get; set; }
    }
}
