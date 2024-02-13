using Domain;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class Api_NewsItemController : Api_BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public Api_NewsItemController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/NewsItem
        [HttpGet]
        public CustomApiResponse GetNewsItem()
        {
            CustomApiResponse response = new CustomApiResponse();

            try
            {
                if (IsUserAuthorized("NewsItem", "Get", ref response))
                {
                    var items = _unitOfWork.NewsItem.GetAll();
                    _resp.SuccessReponse(items, ref response);
                }
                else
                {
                    _resp.ActionNotAuthorizedResponse(ref response);
                }
            }
            catch (Exception ex)
            {
                _resp.ExceptionResponse(ex, ref response);
            }
            return response;
        }

        // GET: api/NewsItem/5
        [HttpGet("{id}")]
        public CustomApiResponse GetNewsItem(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("NewsItem", "Get", ref response))
                {
                    var NewsItem = _unitOfWork.NewsItem.GetById(id);
                    if (NewsItem == null)
                    {
                        response.Error = "No NewsItem data found";

                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        _resp.SuccessReponse(NewsItem, ref response);
                    }
                }
                else
                {
                    _resp.ActionNotAuthorizedResponse(ref response);
                }
            }
            catch (Exception ex)
            {
                _resp.ExceptionResponse(ex, ref response);
            }
            return response;
        }

        // POST: api/NewsItem
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public CustomApiResponse PostNewsItem(NewsItem NewsItem)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("NewsItem", "Post", ref response))
                {
                    if (_unitOfWork.NewsItem.DataCount(u => u.NewsLink == NewsItem.NewsLink) == 0)
                    {

                        if (IsOkToInsert(NewsItem, ref response))
                        {
                            NewsItem.CreatedByUserId = int.Parse(CurrentUserID.ToString());
                            NewsItem.CreatedDate = DateTime.UtcNow;
                            _unitOfWork.NewsItem.Add(NewsItem);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(NewsItem, ref response, (int)HttpStatusCode.Created);
                            response.CustomMessage = "NewsItem Added Sucessfully";
                        }
                        else
                        {
                            _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotAcceptable);
                        }
                    }
                    else
                    {
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.Ambiguous);
                    }
                }
                else
                {
                    _resp.ActionNotAuthorizedResponse(ref response);
                }
            }
            catch (Exception ex)
            {
                _resp.ExceptionResponse(ex, ref response);
            }
            return response;
        }

        // PUT: api/NewsItem/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public CustomApiResponse PutNewsItem(int id, NewsItem NewsItem)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("NewsItem", "PUT", ref response))
                {
                    if (id != NewsItem.Id)
                    {
                        response.Error = "Wrong NewsItem ID ";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        if (!_unitOfWork.NewsItem.DataExists(u => u.Id == id))
                        {
                            response.Error = "Record not Found";
                            _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                        }
                        else
                        {
                            if (IsOkToUpdate(NewsItem, ref response))
                            {
                                _unitOfWork.NewsItem.Update(NewsItem);
                                _unitOfWork.SaveAllChanges();

                                _resp.SuccessReponse(NewsItem, ref response, (int)HttpStatusCode.Created);

                                response.CustomMessage = "NewsItem Detail Updated Sucessfully";
                            }
                            else
                            {
                                response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                                response.IsSucess = false;
                                response.Value = null;
                            }
                        }
                    }
                }
                else
                {
                    _resp.ActionNotAuthorizedResponse(ref response);
                }
            }
            catch (Exception ex)
            {
                _resp.ExceptionResponse(ex, ref response);
            }
            return response;
        }

        // DELETE: api/NewsItem/5
        [HttpDelete("{id}")]
        public CustomApiResponse DeleteNewsItem(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("NewsItem", "Delete", ref response))
                {
                    var NewsItem = _unitOfWork.NewsItem.GetById(id);
                    if (NewsItem == null)
                    {
                        response.Error = "No NewsItem data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToDelete(NewsItem, ref response))
                        {
                            _unitOfWork.NewsItem.Remove(NewsItem);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(NewsItem, ref response);
                            response.CustomMessage = "NewsItem Deleted Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong NewsItem ID ";
                            _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotAcceptable);
                        }
                    }
                }
                else
                {
                    _resp.ActionNotAuthorizedResponse(ref response);
                }
            }
            catch (Exception ex)
            {
                _resp.ExceptionResponse(ex, ref response);
            }
            return response;
        }






        [HttpGet()]
        [Route("NewsItemStatusChange/{id}/{curentuserid}")]
        public CustomApiResponse NewsItemStatusChange(int id, String actiontype = "Deactivate")
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("NewsItem", "Activate", ref response))
                {
                    var NewsItem = _unitOfWork.NewsItem.GetById(id);
                    if (NewsItem == null)
                    {
                        response.Error = "No NewsItem data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToActivationChange(NewsItem, ref response, actiontype))
                        {
                            if (actiontype == "Deactivate")
                            {
                               // NewsItem.IsActive = true;
                            }
                            else if (actiontype == "Activate")
                            {
                             //   NewsItem.IsActive = false;
                            }
                            else if (actiontype == "Delete")
                            {
                             //   NewsItem.IsDeleted = true;
                             //   NewsItem.DeletedByByUserId = int .Parse (CurrentUserID);
                             // NewsItem.DeletedDate = DateTime.Now;

                            }

                            _unitOfWork.NewsItem.Update(NewsItem);
                            _unitOfWork.SaveAllChanges();
                           _resp.SuccessReponse(NewsItem, ref response);
                            response.CustomMessage = "NewsItem Status Changed to  '"+ actiontype + "' Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong NewsItem ID ";
                            _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotAcceptable);
                        }
                    }
                }
                else
                {
                    _resp.ActionNotAuthorizedResponse(ref response);
                }
            }
            catch (Exception ex)
            {
                _resp.ExceptionResponse(ex, ref response);
            }
            return response;
        }




        private bool IsOkToInsert(NewsItem NewsItem, ref CustomApiResponse response)
        {
            bool _isoktoinsert = false;
            try
            {
                _isoktoinsert = true;
            }
            catch (Exception ex)
            {
                _isoktoinsert = false;
            }
            return _isoktoinsert;
        }

        private bool IsOkToUpdate(NewsItem NewsItem, ref CustomApiResponse response)
        {
            bool _isOktoInsert = false;
            try
            {
                _isOktoInsert = true;
            }
            catch (Exception ex)
            {
                _isOktoInsert = false;
            }
            return _isOktoInsert;
        }

        private bool IsOkToDelete(NewsItem NewsItem, ref CustomApiResponse response)
        {
            bool _isoktoDelete = false;
            try
            {
                _isoktoDelete = true;
            }
            catch (Exception ex)
            {
                response.Error = "Custom error";
                _isoktoDelete = false;
            }
            return _isoktoDelete;
        }

        private bool IsOkToActivationChange(NewsItem NewsItem, ref CustomApiResponse response, String actiontype)
        {
            bool _isoktoDeactivate = false;
            try
            {
                _isoktoDeactivate = true;
            }
            catch (Exception ex)
            {
                response.Error = "Custom error";
                _isoktoDeactivate = false;
            }
            return _isoktoDeactivate;
        }

        //private bool NewsItemExists(int id)
        //{
        //    return _context.NewsItem.Any(e => e.Id == id);
        //}
    }
}