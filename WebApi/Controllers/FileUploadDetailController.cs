using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApi.Controllers;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class FileUploadDetailController : AdminBaseController
    {
        private readonly ILogger<FileUploadDetailController> _logger;
        private IWebHostEnvironment Environment;

        string apiUrl = "api/FileUploadDetail" +            "";
       

        public FileUploadDetailController(ILogger<FileUploadDetailController> logger, IWebHostEnvironment _environment, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _logger = logger;
            Environment = _environment;

        }

        public async Task<IActionResult> IndexAsync()
        {


            FileUploadDetailDTOList FileUploadDetailDTOList = new FileUploadDetailDTOList();
            FileUploadDetailDTOList.FileUploadDetails = new List<FileUploadDetailDTO>();



            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var FileUploadDetaillist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FileUploadDetailDTO>>(data);
                FileUploadDetailDTOList.FileUploadDetails = FileUploadDetaillist;
            }
            
            return View(FileUploadDetailDTOList);
        }

        // Get: FileUploadDetail/Create
        public IActionResult Create()
        {
            FileUploadDetailDTO FileUploadDetailDTO = new FileUploadDetailDTO();

            return View(FileUploadDetailDTO);
        }
        
        // POST: FileUploadDetail/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID, Filename, FileDesription, FileType, FileLocation,FileRelativeLocation, isPublic")] FileUploadDetailDTO FileUploadDetailDTO, List<IFormFile> files)
        {


            try
            {
                string filelocation = "";
                string wwwPath = this.Environment.WebRootPath;
                string contentPath = this.Environment.ContentRootPath;

                String FolderPath = "File_Uploads";
                string relativepath = Path.Combine(HttpTaskService.baseuri, FolderPath);
                string path = Path.Combine(this.Environment.WebRootPath, FolderPath);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }


                List<string> uploadedFiles = new List<string>();
                foreach (IFormFile postedFile in files)
                {


                    string flenameactual = postedFile.FileName;

                    string FileExtension = "." + postedFile.FileName.Split('.')[postedFile.FileName.Split('.').Length - 1];
                    string FileName = FilenameGenerator(FileExtension);
                    filelocation = Path.Combine(path, FileName);

                    if (postedFile.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            postedFile.CopyTo(ms);
                            var fileBytes = ms.ToArray();

                            FileUploadDetailDTO.FileContent = fileBytes;
                            string s = Convert.ToBase64String(fileBytes);
                            // act on the Base64 data
                        }
                    }

                    //string fileName = Path.GetFileName(postedFile.FileName);
                    using (FileStream stream = new FileStream(filelocation, FileMode.Create))
                    {
                        postedFile.CopyTo(stream);


                    }

                    FileUploadDetailDTO.FileLocation = filelocation;

                    FileUploadDetailDTO.FileRelativeLocation = Path.Combine(relativepath, FileName); ;


                }

                HttpResponseMessage response = await HttpTaskService.PostAsyncData(apiUrl, FileUploadDetailDTO);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SucessAlert"] = "FileUploadDetail Added successfully";
                    string result = response.Content.ReadAsStringAsync().Result;
                    var returndata = await response.Content.ReadAsStringAsync();
                    var FileUploadDetaillist = Newtonsoft.Json.JsonConvert.DeserializeObject<FileUploadDetailDTO>(returndata);

                }
                else
                {
                    TempData["ErrorAlert"] = "Error in Adding FileUploadDetail : Try Again";
                }
                return RedirectToAction(nameof(Index));
                // TempData["contributionMaster"] = JsonConvert.SerializeObject(contributionMaster);
            }

            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message, ex.InnerException);

                TempData["ErrorAlert"] = "Error in Editing FileUploadDetail : Try Again";
                return View(FileUploadDetailDTO);
            }



        }

        


        private static  byte[] ConvertFiletoByte(String fileLocation)
        {
            byte[] filecontent=null;

            MemoryStream ms = new MemoryStream();
            using (FileStream file = new FileStream(fileLocation, FileMode.Open, FileAccess.Read))
            {
                file.CopyTo(ms);
            }
            filecontent = ms.ToArray();
          return filecontent ;
        }


        public string FilenameGenerator(string extension)
        {
            string filename = "";
            filename = DateTime.Now.Ticks + extension; //Create a new Name for the file due to security reasons.
            return filename;

        }



        // GET: FileUploadDetail/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FileUploadDetailDTO FileUploadDetailDTO = new FileUploadDetailDTO();
           
            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                FileUploadDetailDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<FileUploadDetailDTO>(data);                
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading FileUploadDetail : Try Again";
            }
            if (FileUploadDetailDTO == null)
            {
                return NotFound();
            }
            return View(FileUploadDetailDTO);
        }


        // POST: FileUploadDetail/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Filename, FileDesription, FileType, FileLocation,FileRelativeLocation, isPublic")] FileUploadDetailDTO FileUploadDetailDTO ,List<IFormFile> files)
        {
            if (id != FileUploadDetailDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {


                try
                {
                    string filelocation = "";
                    string wwwPath = this.Environment.WebRootPath;
                    string contentPath = this.Environment.ContentRootPath;

                    string path = Path.Combine(this.Environment.WebRootPath, "File_Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                   

                    List<string> uploadedFiles = new List<string>();
                    foreach (IFormFile postedFile in files)
                    {


                        string flename = postedFile.FileName;

                         

                        filelocation = Path.Combine(path, flename);


                        //string fileName = Path.GetFileName(postedFile.FileName);
                        using (FileStream stream = new FileStream(filelocation, FileMode.Create))
                        {
                            postedFile.CopyTo(stream);
                        }
                        if (postedFile.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                postedFile.CopyTo(ms);
                                var fileBytes = ms.ToArray();

                                FileUploadDetailDTO.FileContent = fileBytes;
                                string s = Convert.ToBase64String(fileBytes);
                                // act on the Base64 data
                            }
                        }
                        FileUploadDetailDTO.FileLocation = filelocation;

                       
                    }
                  
                    FileUploadDetailDTO.FileContent = ConvertFiletoByte (FileUploadDetailDTO.FileLocation);
                  
                    apiUrl = apiUrl + "/" + id;
                    HttpResponseMessage response = await HttpTaskService.PutAsyncData(apiUrl, FileUploadDetailDTO);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SucessAlert"] = "FileUploadDetail Editted successfully";
                        string result = response.Content.ReadAsStringAsync().Result;
                        var returndata = await response.Content.ReadAsStringAsync();
                        var FileUploadDetaillist = Newtonsoft.Json.JsonConvert.DeserializeObject<FileUploadDetailDTO>(returndata);

                    }
                    else
                    {
                        TempData["ErrorAlert"] = "Error in Editing FileUploadDetail : Try Again";
                    }

                    return RedirectToAction(nameof(Index));
                    // TempData["contributionMaster"] = JsonConvert.SerializeObject(contributionMaster);
                }

                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message, ex.InnerException);

                    TempData["ErrorAlert"] = "Error in Editing FileUploadDetail : Try Again";
                    return View(FileUploadDetailDTO);
                }

               
            }
            return View(FileUploadDetailDTO);
        }







        // GET: FileUploadDetailDTOes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FileUploadDetailDTO FileUploadDetailDTO = new FileUploadDetailDTO();

            HttpResponseMessage response = await HttpTaskService .GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                FileUploadDetailDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<FileUploadDetailDTO>(data);
                DownloadFile(FileUploadDetailDTO);

            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading FileUploadDetail : Try Again";
            }
            if (FileUploadDetailDTO == null)
            {
                return NotFound();
            }
            return View(FileUploadDetailDTO);
           
        }

        private static String DownloadFile(FileUploadDetailDTO FileUploadDetailDTO)
        {
            var path = FileUploadDetailDTO.FileLocation;

            if (!System.IO.File.Exists(FileUploadDetailDTO.FileLocation))
            {
                var imageBytes = FileUploadDetailDTO.FileContent;
                var fileExtension = System.IO.Path.GetExtension(path);
                System.IO.File.WriteAllBytes(FileUploadDetailDTO.FileLocation, imageBytes);
            }
            return FileUploadDetailDTO.FileRelativeLocation;
        }






        // GET: FileUploadDetailDTOes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FileUploadDetailDTO FileUploadDetailDTO = new FileUploadDetailDTO();

            HttpResponseMessage response = await HttpTaskService.GetAsyncData(apiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                FileUploadDetailDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<FileUploadDetailDTO>(data);
            }
            else
            {
                TempData["ErrorAlert"] = "Error in Loading FileUploadDetail : Try Again";
            }
            if (FileUploadDetailDTO == null)
            {
                return NotFound();
            }
            return View(FileUploadDetailDTO);
        
    }


        // POST: FileUploadDetailDTOes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            apiUrl = apiUrl + "/" + id;
            HttpResponseMessage response = await HttpTaskService.DeleteAsyncData(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                TempData["SucessAlert"] = "FileUploadDetail Deleted successfully";
                string result = response.Content.ReadAsStringAsync().Result;
                var returndata = await response.Content.ReadAsStringAsync();
                var categorylist = Newtonsoft.Json.JsonConvert.DeserializeObject<FileUploadDetailDTO>(returndata);

            }
            else
            {
                TempData["ErrorAlert"] = "Error in Deleting FileUploadDetail : Try Again";
            }

            return RedirectToAction(nameof(Index));
        }











    }
}