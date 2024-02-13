using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApi.Controllers;
using WebApp.Models;
using WebApp.Services;
namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicAreaController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PublicAreaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        [HttpGet()]
        [Route("GetMainPageLast")]
        public ActionResult<MainPageModel> GetMainPageLast()
        {
            MainPageModel mainPageModel = new MainPageModel();

            var k = _unitOfWork.MainPage.GetAll().FirstOrDefault();
            mainPageModel.CorouselImage1 = k.CorouselImage1;
            mainPageModel.CorouselImage2 = k.CorouselImage2;
            mainPageModel.CorouselImage3 = k.CorouselImage3;
            mainPageModel.MainText = k.MainText;
            mainPageModel.Slogan = k.Slogan;

            var newsitem = _unitOfWork.NewsItem.GetAll().ToList();
            foreach (var item in newsitem. OrderByDescending(u=>u.DateofAction).ThenBy(u=>u.Id).Take(20))
            {
                NewsModel newsModel = new NewsModel();
                newsModel.NewsID = item.Id;
                newsModel.NewsText = item.NewsText;
                newsModel.DateofAction = item.DateofAction;
                newsModel.NewsLink = item.NewsLink;
                mainPageModel.newsModels.Add(newsModel);
            }


            String quote = DateTime.UtcNow.ToString("dddd, dd MMMM yyyy") + " : ";

            int day = DateTime.UtcNow.Day;
            int Month = DateTime.UtcNow.Month;


            var p = _unitOfWork.DayQuote.Find(u => u.Day == day && u.MonthCode == Month).ToList();

            if (p.Count > 0)
            {
                foreach (var item in p)
                {
                    quote = quote + ";" + item.ToDayQuote;
                }
            }
            else
            {
                //   quote = quote + q.DayQuote;
            }

            mainPageModel.DayQuote = quote;
            return mainPageModel;

        }


        [AllowAnonymous]
        [HttpGet()]
        [Route("GetRules")]
        public ActionResult<MainPageModel> GetRules()
        {
            MainPageModel mainPageModel = new MainPageModel();
            var k = _unitOfWork.MainPage.GetAll().FirstOrDefault();
            mainPageModel.RulesRegulation = k.RulesRegulation;
            return mainPageModel;
        }


        [AllowAnonymous]
        [HttpGet()]
        [Route("GetContact")]
        public ActionResult<MainPageModel> GetContact()
        {
            MainPageModel mainPageModel = new MainPageModel();
            var k = _unitOfWork.MainPage.GetAll().FirstOrDefault();
            mainPageModel.ContactDesc1 = k.ContactDesc1;
            mainPageModel.ContactDesc2 = k.ContactDesc2;
            mainPageModel.ContactLine1 = k.ContactLine1;
            mainPageModel.ContactLine2 = k.ContactLine2;
            mainPageModel.ContactLine3 = k.ContactLine3;
            mainPageModel.Phonenum = k.Phonenum;
            mainPageModel.Faxnum = k.Faxnum;
            mainPageModel.Website = k.Website;
            mainPageModel.Email = k.Email;
            return mainPageModel;
        }


        [AllowAnonymous]
        [HttpGet()]
        [Route("GetDownloads")]
        public ActionResult<FileUploadDetailDTOList> GetDownloads()
        {
            FileUploadDetailDTOList fileUploadDetailDTOList = new FileUploadDetailDTOList();
            var q = _unitOfWork.FileUploadDetail.Find(u => u.isPublic == true).ToList();

            foreach (var item in q)
            {
                FileUploadDetailDTO fileUploadDetailDTO = new FileUploadDetailDTO();
                fileUploadDetailDTO.Filename = item.Filename;
                fileUploadDetailDTO.FileDesription = item.FileDesription;
                fileUploadDetailDTO.FileLocation = item.FileLocation;
                fileUploadDetailDTO.FileRelativeLocation = item.FileRelativeLocation;
                fileUploadDetailDTOList.FileUploadDetails.Add(fileUploadDetailDTO);
            }

            return fileUploadDetailDTOList;
        }



        [AllowAnonymous]
        [HttpGet()]
        [Route("GetManagingComitee")]
        public ActionResult<ManagingComiteeDTOList> GetManagingComitee()
        {
            ManagingComiteeDTOList managingComiteeDTOList = new ManagingComiteeDTOList();
            var q = _unitOfWork.ManagingComitee.Find(u => u.IsActive == true).OrderBy(u => u.order);
            foreach (var item in q)
            {
                ManagingComiteeDTO managingComiteeDTO = new ManagingComiteeDTO();
                managingComiteeDTO.Name = item.Name;
                managingComiteeDTO.Position = item.Position;
                managingComiteeDTO.Description1 = item.Description1;
                managingComiteeDTO.Description2 = item.Description2;
                managingComiteeDTO.imageLocation = item.imageLocation;
                managingComiteeDTOList.ManagingComitees.Add(managingComiteeDTO);
            }

            return managingComiteeDTOList;
        }

    }

    

}
