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
    public class Api_StateController : Api_BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public Api_StateController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/State
        [HttpGet]
        public CustomApiResponse GetState()
        {
            CustomApiResponse response = new CustomApiResponse();

            try
            {
                if (IsUserAuthorized("State", "Get", ref response))
                {
                    var items = _unitOfWork.State.GetAll();
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

        // GET: api/State/5
        [HttpGet("{id}")]
        public CustomApiResponse GetState(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("State", "Get", ref response))
                {
                    var State = _unitOfWork.State.GetById(id);
                    if (State == null)
                    {
                        response.Error = "No State data found";

                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        _resp.SuccessReponse(State, ref response);
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

        // POST: api/State
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public CustomApiResponse PostState(State State)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("State", "Post", ref response))
                {
                    if (_unitOfWork.State.DataCount(u => u.Abbreviation == State.Abbreviation) == 0)
                    {
                        if (IsOkToInsert(State, ref response))
                        {

                            State.CreatedByUserId = int.Parse(CurrentUserID.ToString());
                            State.CreatedDate = DateTime.UtcNow;
                            _unitOfWork.State.Add(State);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(State, ref response, (int)HttpStatusCode.Created);
                            response.CustomMessage = "State Added Sucessfully";
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

        // PUT: api/State/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public CustomApiResponse PutState(int id, State State)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("State", "PUT", ref response))
                {
                    if (id != State.Id)
                    {
                        response.Error = "Wrong State ID ";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        if (!_unitOfWork.State.DataExists(u => u.Id == id))
                        {
                            response.Error = "Record not Found";
                            _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                        }
                        else
                        {
                            if (IsOkToUpdate(State, ref response))
                            {
                                _unitOfWork.State.Update(State);
                                _unitOfWork.SaveAllChanges();

                                _resp.SuccessReponse(State, ref response, (int)HttpStatusCode.Created);

                                response.CustomMessage = "State Detail Updated Sucessfully";
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

        // DELETE: api/State/5
        [HttpDelete("{id}")]
        public CustomApiResponse DeleteState(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("State", "Delete", ref response))
                {
                    var State = _unitOfWork.State.GetById(id);
                    if (State == null)
                    {
                        response.Error = "No State data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToDelete(State, ref response))
                        {
                            _unitOfWork.State.Remove(State);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(State, ref response);
                            response.CustomMessage = "State Deleted Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong State ID ";
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
        [Route("StateStatusChange/{id}/{curentuserid}")]
        public CustomApiResponse StateStatusChange(int id, String actiontype = "Deactivate")
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("State", "Activate", ref response))
                {
                    var State = _unitOfWork.State.GetById(id);
                    if (State == null)
                    {
                        response.Error = "No State data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToActivationChange(State, ref response, actiontype))
                        {
                            if (actiontype == "Deactivate")
                            {
                                State.IsActive = true;
                            }
                            else if (actiontype == "Activate")
                            {
                                State.IsActive = false;
                            }
                            else if (actiontype == "Delete")
                            {
                                //State.IsDeleted = true;
                                //State.DeletedByByUserId = int.Parse(CurrentUserID);
                                //State.DeletedDate = DateTime.Now;

                            }

                            _unitOfWork.State.Update(State);
                            _unitOfWork.SaveAllChanges();
                            _resp.SuccessReponse(State, ref response);
                            response.CustomMessage = "State Status Changed to  '" + actiontype + "' Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong State ID ";
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




        private bool IsOkToInsert(State State, ref CustomApiResponse response)
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

        private bool IsOkToUpdate(State State, ref CustomApiResponse response)
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

        private bool IsOkToDelete(State State, ref CustomApiResponse response)
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

        private bool IsOkToActivationChange(State State, ref CustomApiResponse response, String actiontype)
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

        //private bool StateExists(int id)
        //{
        //    return _context.State.Any(e => e.Id == id);
        //}
    }
}