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
    public class Api_SupportTicketController : Api_BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public Api_SupportTicketController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/SupportTicket
        [HttpGet]
        public CustomApiResponse GetSupportTicket()
        {
            CustomApiResponse response = new CustomApiResponse();

            try
            {
                if (IsUserAuthorized("SupportTicket", "Get", ref response))
                {
                    var items = _unitOfWork.SupportTicket.GetAll();
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

        // GET: api/SupportTicket/5
        [HttpGet("{id}")]
        public CustomApiResponse GetSupportTicket(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("SupportTicket", "Get", ref response))
                {
                    var SupportTicket = _unitOfWork.SupportTicket.GetById(id);
                    if (SupportTicket == null)
                    {
                        response.Error = "No SupportTicket data found";

                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        _resp.SuccessReponse(SupportTicket, ref response);
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

        // POST: api/SupportTicket
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public CustomApiResponse PostSupportTicket(SupportTicket SupportTicket)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("SupportTicket", "Post", ref response))
                {
                    if (_unitOfWork.SupportTicket.DataCount(u => u.SupportTicketNum == SupportTicket.SupportTicketNum) == 0)
                    {
                        if (IsOkToInsert(SupportTicket, ref response))
                        {
                            SupportTicket.CreatedByUserId = int.Parse(CurrentUserID.ToString());
                            SupportTicket.CreatedDate = DateTime.UtcNow;
                            _unitOfWork.SupportTicket.Add(SupportTicket);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(SupportTicket, ref response, (int)HttpStatusCode.Created);
                            response.CustomMessage = "SupportTicket Added Sucessfully";
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

        // PUT: api/SupportTicket/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public CustomApiResponse PutSupportTicket(int id, SupportTicket SupportTicket)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("SupportTicket", "PUT", ref response))
                {
                    if (id != SupportTicket.Id)
                    {
                        response.Error = "Wrong SupportTicket ID ";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        if (!_unitOfWork.SupportTicket.DataExists(u => u.Id == id))
                        {
                            response.Error = "Record not Found";
                            _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                        }
                        else
                        {
                            if (IsOkToUpdate(SupportTicket, ref response))
                            {
                                _unitOfWork.SupportTicket.Update(SupportTicket);
                                _unitOfWork.SaveAllChanges();

                                _resp.SuccessReponse(SupportTicket, ref response, (int)HttpStatusCode.Created);

                                response.CustomMessage = "SupportTicket Detail Updated Sucessfully";
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

        // DELETE: api/SupportTicket/5
        [HttpDelete("{id}")]
        public CustomApiResponse DeleteSupportTicket(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("SupportTicket", "Delete", ref response))
                {
                    var SupportTicket = _unitOfWork.SupportTicket.GetById(id);
                    if (SupportTicket == null)
                    {
                        response.Error = "No SupportTicket data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToDelete(SupportTicket, ref response))
                        {
                            _unitOfWork.SupportTicket.Remove(SupportTicket);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(SupportTicket, ref response);
                            response.CustomMessage = "SupportTicket Deleted Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong SupportTicket ID ";
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
        [Route("SupportTicketStatusChange/{id}/{curentuserid}")]
        public CustomApiResponse SupportTicketStatusChange(int id, String actiontype = "Deactivate")
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("SupportTicket", "Activate", ref response))
                {
                    var SupportTicket = _unitOfWork.SupportTicket.GetById(id);
                    if (SupportTicket == null)
                    {
                        response.Error = "No SupportTicket data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToActivationChange(SupportTicket, ref response, actiontype))
                        {
                            if (actiontype == "Deactivate")
                            {
                               SupportTicket.IsActive = true;
                            }
                            else if (actiontype == "Activate")
                            {
                               SupportTicket.IsActive = false;
                            }
                            else if (actiontype == "Delete")
                            {
                                 SupportTicket.IsDeleted = true;
                                  SupportTicket.IsDeleted = true;
                                  SupportTicket.DeletedByByUserId = int .Parse (CurrentUserID);
                                 SupportTicket.DeletedDate = DateTime.Now;

                            }

                            _unitOfWork.SupportTicket.Update(SupportTicket);
                            _unitOfWork.SaveAllChanges();
                           _resp.SuccessReponse(SupportTicket, ref response);
                            response.CustomMessage = "SupportTicket Status Changed to  '"+ actiontype + "' Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong SupportTicket ID ";
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




        private bool IsOkToInsert(SupportTicket SupportTicket, ref CustomApiResponse response)
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

        private bool IsOkToUpdate(SupportTicket SupportTicket, ref CustomApiResponse response)
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

        private bool IsOkToDelete(SupportTicket SupportTicket, ref CustomApiResponse response)
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

        private bool IsOkToActivationChange(SupportTicket SupportTicket, ref CustomApiResponse response, String actiontype)
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

        //private bool SupportTicketExists(int id)
        //{
        //    return _context.SupportTicket.Any(e => e.Id == id);
        //}
    }
}