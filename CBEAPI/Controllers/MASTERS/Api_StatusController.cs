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
    public class Api_StatusController : Api_BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public Api_StatusController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Status
        [HttpGet]
        public CustomApiResponse GetStatus()
        {
            CustomApiResponse response = new CustomApiResponse();

            try
            {
                if (IsUserAuthorized("Status", "Get", ref response))
                {
                    var items = _unitOfWork.Status.GetAll();
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

        // GET: api/Status/5
        [HttpGet("{id}")]
        public CustomApiResponse GetStatus(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Status", "Get", ref response))
                {
                    var Status = _unitOfWork.Status.GetById(id);
                    if (Status == null)
                    {
                        response.Error = "No Status data found";

                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        _resp.SuccessReponse(Status, ref response);
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

        // POST: api/Status
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public CustomApiResponse PostStatus(Status Status)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Status", "Post", ref response))
                {
                    if (_unitOfWork.Status.DataCount(u => u.Abbreviation == Status.Abbreviation) == 0)
                    {
                        if (IsOkToInsert(Status, ref response))
                        {

                            Status.CreatedByUserId = int.Parse(CurrentUserID.ToString());
                            Status.CreatedDate = DateTime.UtcNow;
                            _unitOfWork.Status.Add(Status);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(Status, ref response, (int)HttpStatusCode.Created);
                            response.CustomMessage = "Status Added Sucessfully";
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

        // PUT: api/Status/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public CustomApiResponse PutStatus(int id, Status Status)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Status", "PUT", ref response))
                {
                    if (id != Status.Id)
                    {
                        response.Error = "Wrong Status ID ";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        if (!_unitOfWork.Status.DataExists(u => u.Id == id))
                        {
                            response.Error = "Record not Found";
                            _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                        }
                        else
                        {
                            if (IsOkToUpdate(Status, ref response))
                            {
                                _unitOfWork.Status.Update(Status);
                                _unitOfWork.SaveAllChanges();

                                _resp.SuccessReponse(Status, ref response, (int)HttpStatusCode.Created);

                                response.CustomMessage = "Status Detail Updated Sucessfully";
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

        // DELETE: api/Status/5
        [HttpDelete("{id}")]
        public CustomApiResponse DeleteStatus(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Status", "Delete", ref response))
                {
                    var Status = _unitOfWork.Status.GetById(id);
                    if (Status == null)
                    {
                        response.Error = "No Status data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToDelete(Status, ref response))
                        {
                            _unitOfWork.Status.Remove(Status);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(Status, ref response);
                            response.CustomMessage = "Status Deleted Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong Status ID ";
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
        [Route("StatusStatusChange/{id}/{curentuserid}")]
        public CustomApiResponse StatusStatusChange(int id, String actiontype = "Deactivate")
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Status", "Activate", ref response))
                {
                    var Status = _unitOfWork.Status.GetById(id);
                    if (Status == null)
                    {
                        response.Error = "No Status data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToActivationChange(Status, ref response, actiontype))
                        {
                            if (actiontype == "Deactivate")
                            {
                                //  Status.IsActive = true;
                            }
                            else if (actiontype == "Activate")
                            {
                                //  Status.IsActive = false;
                            }
                            else if (actiontype == "Delete")
                            {
                                //Status.IsDeleted = true;
                                //Status.DeletedByByUserId = int.Parse(CurrentUserID);
                                //Status.DeletedDate = DateTime.Now;

                            }

                            _unitOfWork.Status.Update(Status);
                            _unitOfWork.SaveAllChanges();
                            _resp.SuccessReponse(Status, ref response);
                            response.CustomMessage = "Status Status Changed to  '" + actiontype + "' Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong Status ID ";
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




        private bool IsOkToInsert(Status Status, ref CustomApiResponse response)
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

        private bool IsOkToUpdate(Status Status, ref CustomApiResponse response)
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

        private bool IsOkToDelete(Status Status, ref CustomApiResponse response)
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

        private bool IsOkToActivationChange(Status Status, ref CustomApiResponse response, String actiontype)
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

        //private bool StatusExists(int id)
        //{
        //    return _context.Status.Any(e => e.Id == id);
        //}
    }
}