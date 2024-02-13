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
    public class Api_UserTypeController : Api_BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public Api_UserTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/UserType
        [HttpGet]
        public CustomApiResponse GetUserType()
        {
            CustomApiResponse response = new CustomApiResponse();

            try
            {
                if (IsUserAuthorized("UserType", "Get", ref response))
                {
                    var items = _unitOfWork.UserType.GetAll();
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

        // GET: api/UserType/5
        [HttpGet("{id}")]
        public CustomApiResponse GetUserType(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("UserType", "Get", ref response))
                {
                    var UserType = _unitOfWork.UserType.GetById(id);
                    if (UserType == null)
                    {
                        response.Error = "No UserType data found";

                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        _resp.SuccessReponse(UserType, ref response);
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

        // POST: api/UserType
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public CustomApiResponse PostUserType(UserType UserType)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("UserType", "Post", ref response))
                {
                    if (_unitOfWork.UserType.DataCount(u => u.Abbreviation == UserType.Abbreviation) == 0)
                    {
                        if (IsOkToInsert(UserType, ref response))
                        {
                            
                            _unitOfWork.UserType.Add(UserType);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(UserType, ref response, (int)HttpStatusCode.Created);
                            response.CustomMessage = "UserType Added Sucessfully";
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

        // PUT: api/UserType/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public CustomApiResponse PutUserType(int id, UserType UserType)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("UserType", "PUT", ref response))
                {
                    if (id != UserType.Id)
                    {
                        response.Error = "Wrong UserType ID ";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        if (!_unitOfWork.UserType.DataExists(u => u.Id == id))
                        {
                            response.Error = "Record not Found";
                            _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                        }
                        else
                        {
                            if (IsOkToUpdate(UserType, ref response))
                            {
                                _unitOfWork.UserType.Update(UserType);
                                _unitOfWork.SaveAllChanges();

                                _resp.SuccessReponse(UserType, ref response, (int)HttpStatusCode.Created);

                                response.CustomMessage = "UserType Detail Updated Sucessfully";
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

        // DELETE: api/UserType/5
        [HttpDelete("{id}")]
        public CustomApiResponse DeleteUserType(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("UserType", "Delete", ref response))
                {
                    var UserType = _unitOfWork.UserType.GetById(id);
                    if (UserType == null)
                    {
                        response.Error = "No UserType data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToDelete(UserType, ref response))
                        {
                            _unitOfWork.UserType.Remove(UserType);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(UserType, ref response);
                            response.CustomMessage = "UserType Deleted Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong UserType ID ";
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
        [Route("UserTypeStatusChange/{id}/{curentuserid}")]
        public CustomApiResponse UserTypeStatusChange(int id, String actiontype = "Deactivate")
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("UserType", "Activate", ref response))
                {
                    var UserType = _unitOfWork.UserType.GetById(id);
                    if (UserType == null)
                    {
                        response.Error = "No UserType data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToActivationChange(UserType, ref response, actiontype))
                        {
                            if (actiontype == "Deactivate")
                            {
                              //  UserType.IsActive = true;
                            }
                            else if (actiontype == "Activate")
                            {
                              //  UserType.IsActive = false;
                            }
                            else if (actiontype == "Delete")
                            {
                                //UserType.IsDeleted = true;
                                //UserType.DeletedByByUserId = int.Parse(CurrentUserID);
                                //UserType.DeletedDate = DateTime.Now;

                            }

                            _unitOfWork.UserType.Update(UserType);
                            _unitOfWork.SaveAllChanges();
                            _resp.SuccessReponse(UserType, ref response);
                            response.CustomMessage = "UserType Status Changed to  '" + actiontype + "' Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong UserType ID ";
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




        private bool IsOkToInsert(UserType UserType, ref CustomApiResponse response)
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

        private bool IsOkToUpdate(UserType UserType, ref CustomApiResponse response)
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

        private bool IsOkToDelete(UserType UserType, ref CustomApiResponse response)
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

        private bool IsOkToActivationChange(UserType UserType, ref CustomApiResponse response, String actiontype)
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

        //private bool UserTypeExists(int id)
        //{
        //    return _context.UserType.Any(e => e.Id == id);
        //}
    }
}