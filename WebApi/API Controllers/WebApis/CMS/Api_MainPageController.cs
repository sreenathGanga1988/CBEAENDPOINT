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
    public class Api_MainPageController : Api_BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public Api_MainPageController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/MainPage
        [HttpGet]
        public CustomApiResponse GetMainPage()
        {
            CustomApiResponse response = new CustomApiResponse();

            try
            {
                if (IsUserAuthorized("MainPage", "Get", ref response))
                {
                    var items = _unitOfWork.MainPage.GetAll();
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

        // GET: api/MainPage/5
        [HttpGet("{id}")]
        public CustomApiResponse GetMainPage(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("MainPage", "Get", ref response))
                {
                    var MainPage = _unitOfWork.MainPage.GetById(id);
                    if (MainPage == null)
                    {
                        response.Error = "No MainPage data found";

                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        _resp.SuccessReponse(MainPage, ref response);
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

        // POST: api/MainPage
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public CustomApiResponse PostMainPage(MainPage MainPage)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("MainPage", "Post", ref response))
                {
                   
                    if (IsOkToInsert(MainPage, ref response))
                        {
                        MainPage.CreatedByUserId = int.Parse(CurrentUserID.ToString());
                        MainPage.CreatedDate = DateTime.UtcNow;
                        _unitOfWork.MainPage.Add(MainPage);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(MainPage, ref response, (int)HttpStatusCode.Created);
                            response.CustomMessage = "MainPage Added Sucessfully";
                        }
                        else
                        {
                            _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotAcceptable);
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

        // PUT: api/MainPage/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public CustomApiResponse PutMainPage(int id, MainPage MainPage)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("MainPage", "PUT", ref response))
                {
                    if (id != MainPage.Id)
                    {
                        response.Error = "Wrong MainPage ID ";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        if (!_unitOfWork.MainPage.DataExists(u => u.Id == id))
                        {
                            response.Error = "Record not Found";
                            _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                        }
                        else
                        {
                            if (IsOkToUpdate(MainPage, ref response))
                            {
                                _unitOfWork.MainPage.Update(MainPage);
                                _unitOfWork.SaveAllChanges();

                                _resp.SuccessReponse(MainPage, ref response, (int)HttpStatusCode.Created);

                                response.CustomMessage = "MainPage Detail Updated Sucessfully";
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

        // DELETE: api/MainPage/5
        [HttpDelete("{id}")]
        public CustomApiResponse DeleteMainPage(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("MainPage", "Delete", ref response))
                {
                    var MainPage = _unitOfWork.MainPage.GetById(id);
                    if (MainPage == null)
                    {
                        response.Error = "No MainPage data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToDelete(MainPage, ref response))
                        {
                            _unitOfWork.MainPage.Remove(MainPage);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(MainPage, ref response);
                            response.CustomMessage = "MainPage Deleted Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong MainPage ID ";
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
        [Route("MainPageStatusChange/{id}/{curentuserid}")]
        public CustomApiResponse MainPageStatusChange(int id, String actiontype = "Deactivate")
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("MainPage", "Activate", ref response))
                {
                    var MainPage = _unitOfWork.MainPage.GetById(id);
                    if (MainPage == null)
                    {
                        response.Error = "No MainPage data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToActivationChange(MainPage, ref response, actiontype))
                        {
                            if (actiontype == "Deactivate")
                            {
                                MainPage.IsActive = true;
                            }
                            else if (actiontype == "Activate")
                            {
                              MainPage.IsActive = false;
                            }
                            else if (actiontype == "Delete")
                            {
                              MainPage.IsDeleted = true;
                               MainPage.DeletedByByUserId = int .Parse (CurrentUserID);
                             MainPage.DeletedDate = DateTime.Now;

                            }

                            _unitOfWork.MainPage.Update(MainPage);
                            _unitOfWork.SaveAllChanges();
                           _resp.SuccessReponse(MainPage, ref response);
                            response.CustomMessage = "MainPage Status Changed to  '"+ actiontype + "' Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong MainPage ID ";
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




        private bool IsOkToInsert(MainPage MainPage, ref CustomApiResponse response)
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

        private bool IsOkToUpdate(MainPage MainPage, ref CustomApiResponse response)
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

        private bool IsOkToDelete(MainPage MainPage, ref CustomApiResponse response)
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

        private bool IsOkToActivationChange(MainPage MainPage, ref CustomApiResponse response, String actiontype)
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

        //private bool MainPageExists(int id)
        //{
        //    return _context.MainPage.Any(e => e.Id == id);
        //}
    }
}