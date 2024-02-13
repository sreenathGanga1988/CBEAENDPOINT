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
    public class Api_UserController : Api_BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public Api_UserController(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
            this.EntityName = "User";
        }

        // GET: api/User
        [HttpGet]
        public CustomApiResponse GetUser()
        {
            CustomApiResponse response = new CustomApiResponse();

            try
            {
                if (IsUserAuthorized("User", "Get", ref response))
                {
                    var items = _unitOfWork.User.GetAll();
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

        // GET: api/User/5
        [HttpGet("{id}")]
        public CustomApiResponse GetUser(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("User", "Get", ref response))
                {
                    var User = _unitOfWork.User.GetById(id);
                    if (User == null)
                    {
                        response.Error = "No User data found";

                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        _resp.SuccessReponse(User, ref response);
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

        // POST: api/User
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public CustomApiResponse PostUser(User User)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("User", "Post", ref response))
                {
                    if (_unitOfWork.User.DataCount(u => u.UserName == User.UserName) == 0)
                    {
                        if (_unitOfWork.User.DataCount(u => u.EmployeeId == User.EmployeeId) == 0)
                        {
                            if (_unitOfWork.Member.DataCount(u => u.StaffNo == User.EmployeeId) == 0)
                            {
                                if (IsOkToInsert(User, ref response))
                                {

                                    User.CreatedByUserId = int.Parse(CurrentUserID.ToString());
                                    User.CreatedDate = DateTime.UtcNow;
                                    _unitOfWork.User.Add(User);
                                    _unitOfWork.SaveAllChanges();
                                 //   AddLogER(EntityName, User.Id, User, _unitOfWork);
                                     _resp.SuccessReponse(User, ref response, (int)HttpStatusCode.Created);
                                    response.CustomMessage = "User Added Sucessfully";
                                }
                                else
                                {
                                    _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotAcceptable);
                                }
                            }
                            else
                            {
                                _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.Ambiguous);
                                response.Error = "Invalid Staff Number";
                            }
                        }
                        else
                        {
                            _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.Ambiguous);
                            response.Error = "User for this Employee  Already Exist";
                        }
                    }
                    else
                    {
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.Ambiguous);
                        response.Error = "User Name Already Exist";
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

        // PUT: api/User/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public CustomApiResponse PutUser(int id, User User)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("User", "PUT", ref response))
                {
                    if (id != User.Id)
                    {
                        response.Error = "Wrong User ID ";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        if (!_unitOfWork.User.DataExists(u => u.Id == id))
                        {
                            response.Error = "Record not Found";
                            _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                        }
                        else
                        {
                            if (IsOkToUpdate(User, ref response))
                            {
                                _unitOfWork.User.Update(User);
                                _unitOfWork.SaveAllChanges();

                                _resp.SuccessReponse(User, ref response, (int)HttpStatusCode.Created);

                                response.CustomMessage = "User Detail Updated Sucessfully";
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


        
        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public CustomApiResponse DeleteUser(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("User", "Delete", ref response))
                {
                    var User = _unitOfWork.User.GetById(id);
                    if (User == null)
                    {
                        response.Error = "No User data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToDelete(User, ref response))
                        {
                            _unitOfWork.User.Remove(User);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(User, ref response);
                            response.CustomMessage = "User Deleted Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong User ID ";
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
        [Route("UserStatusChange/{id}/{curentuserid}")]
        public CustomApiResponse UserStatusChange(int id, String actiontype = "Deactivate")
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("User", "Activate", ref response))
                {
                    var User = _unitOfWork.User.GetById(id);
                    if (User == null)
                    {
                        response.Error = "No User data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToActivationChange(User, ref response, actiontype))
                        {
                            if (actiontype == "Deactivate")
                            {
                                User.IsActive = true;
                            }
                            else if (actiontype == "Activate")
                            {
                                User.IsActive = false;
                            }
                            else if (actiontype == "Delete")
                            {
                             //   User.IsDeleted = true;
                             //   User.DeletedByByUserId = int.Parse(CurrentUserID);
                              //  User.DeletedDate = DateTime.Now;

                            }

                            _unitOfWork.User.Update(User);
                            _unitOfWork.SaveAllChanges();
                            _resp.SuccessReponse(User, ref response);
                            response.CustomMessage = "User Status Changed to  '" + actiontype + "' Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong User ID ";
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




        private bool IsOkToInsert(User User, ref CustomApiResponse response)
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

        private bool IsOkToUpdate(User User, ref CustomApiResponse response)
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

        private bool IsOkToDelete(User User, ref CustomApiResponse response)
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

        private bool IsOkToActivationChange(User User, ref CustomApiResponse response, String actiontype)
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

        //private bool UserExists(int id)
        //{
        //    return _context.User.Any(e => e.Id == id);
        //}
    }
}