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
    public class Api_ManagingComiteeController : Api_BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public Api_ManagingComiteeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/ManagingComitee
        [HttpGet]
        public CustomApiResponse GetManagingComitee()
        {
            CustomApiResponse response = new CustomApiResponse();

            try
            {
                if (IsUserAuthorized("ManagingComitee", "Get", ref response))
                {
                    var items = _unitOfWork.ManagingComitee.GetAll();
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

        // GET: api/ManagingComitee/5
        [HttpGet("{id}")]
        public CustomApiResponse GetManagingComitee(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("ManagingComitee", "Get", ref response))
                {
                    var ManagingComitee = _unitOfWork.ManagingComitee.GetById(id);
                    if (ManagingComitee == null)
                    {
                        response.Error = "No ManagingComitee data found";

                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        _resp.SuccessReponse(ManagingComitee, ref response);
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

        // POST: api/ManagingComitee
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public CustomApiResponse PostManagingComitee(ManagingComitee ManagingComitee)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("ManagingComitee", "Post", ref response))
                {
                    
                    if (_unitOfWork.ManagingComitee.DataCount(u => u.Name == ManagingComitee.Name) == 0)
                    {
                        if (IsOkToInsert(ManagingComitee, ref response))
                        {
                            ManagingComitee.CreatedByUserId = int.Parse(CurrentUserID.ToString());
                            ManagingComitee.CreatedDate = DateTime.UtcNow;
                            _unitOfWork.ManagingComitee.Add(ManagingComitee);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(ManagingComitee, ref response, (int)HttpStatusCode.Created);
                            response.CustomMessage = "ManagingComitee Added Sucessfully";
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

        // PUT: api/ManagingComitee/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public CustomApiResponse PutManagingComitee(int id, ManagingComitee ManagingComitee)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("ManagingComitee", "PUT", ref response))
                {
                    if (id != ManagingComitee.Id)
                    {
                        response.Error = "Wrong ManagingComitee ID ";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        if (!_unitOfWork.ManagingComitee.DataExists(u => u.Id == id))
                        {
                            response.Error = "Record not Found";
                            _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                        }
                        else
                        {
                            if (IsOkToUpdate(ManagingComitee, ref response))
                            {
                                _unitOfWork.ManagingComitee.Update(ManagingComitee);
                                _unitOfWork.SaveAllChanges();

                                _resp.SuccessReponse(ManagingComitee, ref response, (int)HttpStatusCode.Created);

                                response.CustomMessage = "ManagingComitee Detail Updated Sucessfully";
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

        // DELETE: api/ManagingComitee/5
        [HttpDelete("{id}")]
        public CustomApiResponse DeleteManagingComitee(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("ManagingComitee", "Delete", ref response))
                {
                    var ManagingComitee = _unitOfWork.ManagingComitee.GetById(id);
                    if (ManagingComitee == null)
                    {
                        response.Error = "No ManagingComitee data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToDelete(ManagingComitee, ref response))
                        {
                            _unitOfWork.ManagingComitee.Remove(ManagingComitee);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(ManagingComitee, ref response);
                            response.CustomMessage = "ManagingComitee Deleted Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong ManagingComitee ID ";
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
        [Route("ManagingComiteeStatusChange/{id}/{curentuserid}")]
        public CustomApiResponse ManagingComiteeStatusChange(int id, String actiontype = "Deactivate")
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("ManagingComitee", "Activate", ref response))
                {
                    var ManagingComitee = _unitOfWork.ManagingComitee.GetById(id);
                    if (ManagingComitee == null)
                    {
                        response.Error = "No ManagingComitee data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToActivationChange(ManagingComitee, ref response, actiontype))
                        {
                            if (actiontype == "Deactivate")
                            {
                               // ManagingComitee.IsActive = true;
                            }
                            else if (actiontype == "Activate")
                            {
                             //   ManagingComitee.IsActive = false;
                            }
                            else if (actiontype == "Delete")
                            {
                             //   ManagingComitee.IsDeleted = true;
                             //   ManagingComitee.DeletedByByUserId = int .Parse (CurrentUserID);
                             // ManagingComitee.DeletedDate = DateTime.Now;

                            }

                            _unitOfWork.ManagingComitee.Update(ManagingComitee);
                            _unitOfWork.SaveAllChanges();
                           _resp.SuccessReponse(ManagingComitee, ref response);
                            response.CustomMessage = "ManagingComitee Status Changed to  '"+ actiontype + "' Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong ManagingComitee ID ";
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




        private bool IsOkToInsert(ManagingComitee ManagingComitee, ref CustomApiResponse response)
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

        private bool IsOkToUpdate(ManagingComitee ManagingComitee, ref CustomApiResponse response)
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

        private bool IsOkToDelete(ManagingComitee ManagingComitee, ref CustomApiResponse response)
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

        private bool IsOkToActivationChange(ManagingComitee ManagingComitee, ref CustomApiResponse response, String actiontype)
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

        //private bool ManagingComiteeExists(int id)
        //{
        //    return _context.ManagingComitee.Any(e => e.Id == id);
        //}
    }
}