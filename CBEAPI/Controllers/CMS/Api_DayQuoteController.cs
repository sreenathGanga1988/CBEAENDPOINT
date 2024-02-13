using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using CBEAPI.ViewModels;

namespace CBEAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class Api_DayQuoteController : Api_BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public Api_DayQuoteController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/DayQuote
        [HttpGet]
        public CustomApiResponse GetDayQuote()
        {
            CustomApiResponse response = new CustomApiResponse();

            try
            {
                if (IsUserAuthorized("DayQuote", "Get", ref response))
                {
                    var items = _unitOfWork.DayQuote.GetAll();
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

        // GET: api/DayQuote/5
        [HttpGet("{id}")]
        public CustomApiResponse GetDayQuote(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("DayQuote", "Get", ref response))
                {
                    var DayQuote = _unitOfWork.DayQuote.GetById(id);
                    if (DayQuote == null)
                    {
                        response.Error = "No DayQuote data found";

                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        _resp.SuccessReponse(DayQuote, ref response);
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

        // POST: api/DayQuote
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public CustomApiResponse PostDayQuote(DayQuote DayQuote)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("DayQuote", "Post", ref response))
                {
                    if (_unitOfWork.DayQuote.DataCount(u => u.UnformatedContent == DayQuote.UnformatedContent) == 0)
                    {
                        if (IsOkToInsert(DayQuote, ref response))
                        {
                            DayQuote.CreatedByUserId = int.Parse ( CurrentUserID.ToString ());
                            DayQuote.CreatedDate = DateTime.UtcNow ;

                            _unitOfWork.DayQuote.Add(DayQuote);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(DayQuote, ref response, (int)HttpStatusCode.Created);
                            response.CustomMessage = "DayQuote Added Sucessfully";
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

        // PUT: api/DayQuote/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public CustomApiResponse PutDayQuote(int id, DayQuote DayQuote)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("DayQuote", "PUT", ref response))
                {
                    if (id != DayQuote.Id)
                    {
                        response.Error = "Wrong DayQuote ID ";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        if (!_unitOfWork.DayQuote.DataExists(u => u.Id == id))
                        {
                            response.Error = "Record not Found";
                            _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                        }
                        else
                        {
                            if (IsOkToUpdate(DayQuote, ref response))
                            {
                                _unitOfWork.DayQuote.Update(DayQuote);
                                _unitOfWork.SaveAllChanges();

                                _resp.SuccessReponse(DayQuote, ref response, (int)HttpStatusCode.Created);

                                response.CustomMessage = "DayQuote Detail Updated Sucessfully";
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

        // DELETE: api/DayQuote/5
        [HttpDelete("{id}")]
        public CustomApiResponse DeleteDayQuote(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("DayQuote", "Delete", ref response))
                {
                    var DayQuote = _unitOfWork.DayQuote.GetById(id);
                    if (DayQuote == null)
                    {
                        response.Error = "No DayQuote data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToDelete(DayQuote, ref response))
                        {
                            _unitOfWork.DayQuote.Remove(DayQuote);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(DayQuote, ref response);
                            response.CustomMessage = "DayQuote Deleted Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong DayQuote ID ";
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
        [Route("DayQuoteStatusChange/{id}/{curentuserid}")]
        public CustomApiResponse DayQuoteStatusChange(int id, String actiontype = "Deactivate")
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("DayQuote", "Activate", ref response))
                {
                    var DayQuote = _unitOfWork.DayQuote.GetById(id);
                    if (DayQuote == null)
                    {
                        response.Error = "No DayQuote data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToActivationChange(DayQuote, ref response, actiontype))
                        {
                            if (actiontype == "Deactivate")
                            {
                               // DayQuote.IsActive = true;
                            }
                            else if (actiontype == "Activate")
                            {
                             //   DayQuote.IsActive = false;
                            }
                            else if (actiontype == "Delete")
                            {
                                //   DayQuote.IsDeleted = true;
                                //   DayQuote.IsDeleted = true;
                                //   DayQuote.DeletedByByUserId = int .Parse (CurrentUserID);
                                // DayQuote.DeletedDate = DateTime.Now;

                            }

                            _unitOfWork.DayQuote.Update(DayQuote);
                            _unitOfWork.SaveAllChanges();
                           _resp.SuccessReponse(DayQuote, ref response);
                            response.CustomMessage = "DayQuote Status Changed to  '"+ actiontype + "' Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong DayQuote ID ";
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




        private bool IsOkToInsert(DayQuote DayQuote, ref CustomApiResponse response)
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

        private bool IsOkToUpdate(DayQuote DayQuote, ref CustomApiResponse response)
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

        private bool IsOkToDelete(DayQuote DayQuote, ref CustomApiResponse response)
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

        private bool IsOkToActivationChange(DayQuote DayQuote, ref CustomApiResponse response, String actiontype)
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

        //private bool DayQuoteExists(int id)
        //{
        //    return _context.DayQuote.Any(e => e.Id == id);
        //}
    }
}