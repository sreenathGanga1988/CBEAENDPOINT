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
    public class Api_AuditLogController : Api_BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public Api_AuditLogController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/App_EntityLog
        [HttpGet]
        public CustomApiResponse GetApp_EntityLog()
        {
            CustomApiResponse response = new CustomApiResponse();

            try
            {
                if (IsUserAuthorized("App_EntityLog", "Get", ref response))
                {
                    var items = _unitOfWork.AppAuditRepository.GetAll();
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

        // GET: api/App_EntityLog/5
        [HttpGet("{id}")]
        public CustomApiResponse GetApp_EntityLog(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("App_EntityLog", "Get", ref response))
                {
                    var App_EntityLog = _unitOfWork.AppAuditRepository.GetById(id);
                    if (App_EntityLog == null)
                    {
                        response.Error = "No App_EntityLog data found";

                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        _resp.SuccessReponse(App_EntityLog, ref response);
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

        // POST: api/App_EntityLog
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public CustomApiResponse PostApp_EntityLog(App_EntityLog App_EntityLog)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {

                _unitOfWork.AppAuditRepository.Add(App_EntityLog);
                _unitOfWork.SaveAllChanges();
            }
            catch (Exception ex)
            {
                _resp.ExceptionResponse(ex, ref response);
            }
            return response;
        }

        // PUT: api/App_EntityLog/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public CustomApiResponse PutApp_EntityLog(int id, App_EntityLog App_EntityLog)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("App_EntityLog", "PUT", ref response))
                {
                    if (id != App_EntityLog.Id)
                    {
                        response.Error = "Wrong App_EntityLog ID ";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        if (!_unitOfWork.AppAuditRepository.DataExists(u => u.Id == id))
                        {
                            response.Error = "Record not Found";
                            _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                        }
                        else
                        {
                            if (IsOkToUpdate(App_EntityLog, ref response))
                            {
                                _unitOfWork.AppAuditRepository.Update(App_EntityLog);
                                _unitOfWork.SaveAllChanges();

                                _resp.SuccessReponse(App_EntityLog, ref response, (int)HttpStatusCode.Created);

                                response.CustomMessage = "App_EntityLog Detail Updated Sucessfully";
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

        // DELETE: api/App_EntityLog/5
        [HttpDelete("{id}")]
        public CustomApiResponse DeleteApp_EntityLog(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("App_EntityLog", "Delete", ref response))
                {
                    var App_EntityLog = _unitOfWork.AppAuditRepository.GetById(id);
                    if (App_EntityLog == null)
                    {
                        response.Error = "No App_EntityLog data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToDelete(App_EntityLog, ref response))
                        {
                            _unitOfWork.AppAuditRepository.Remove(App_EntityLog);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(App_EntityLog, ref response);
                            response.CustomMessage = "App_EntityLog Deleted Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong App_EntityLog ID ";
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
        [Route("App_EntityLogStatusChange/{id}/{curentuserid}")]
        public CustomApiResponse App_EntityLogStatusChange(int id, String actiontype = "Deactivate")
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("App_EntityLog", "Activate", ref response))
                {
                    var App_EntityLog = _unitOfWork.AppAuditRepository.GetById(id);
                    if (App_EntityLog == null)
                    {
                        response.Error = "No App_EntityLog data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToActivationChange(App_EntityLog, ref response, actiontype))
                        {
                            //if (actiontype == "Deactivate")
                            //{
                            //    App_EntityLog.IsActive = true;
                            //}
                            //else if (actiontype == "Activate")
                            //{
                            //    App_EntityLog.IsActive = false;
                            //}
                            //else if (actiontype == "Delete")
                            //{
                            //    App_EntityLog.IsDeleted = true;
                            //    App_EntityLog.DeletedByByUserId = int.Parse(CurrentUserID);
                            //    App_EntityLog.DeletedDate = DateTime.Now;

                            //}

                            _unitOfWork.AppAuditRepository.Update(App_EntityLog);
                            _unitOfWork.SaveAllChanges();
                            _resp.SuccessReponse(App_EntityLog, ref response);
                            response.CustomMessage = "App_EntityLog Status Changed to  '" + actiontype + "' Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong App_EntityLog ID ";
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




        private bool IsOkToInsert(App_EntityLog App_EntityLog, ref CustomApiResponse response)
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

        private bool IsOkToUpdate(App_EntityLog App_EntityLog, ref CustomApiResponse response)
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

        private bool IsOkToDelete(App_EntityLog App_EntityLog, ref CustomApiResponse response)
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

        private bool IsOkToActivationChange(App_EntityLog App_EntityLog, ref CustomApiResponse response, String actiontype)
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

        //private bool App_EntityLogExists(int id)
        //{
        //    return _context.App_EntityLog.Any(e => e.Id == id);
        //}
    }
}