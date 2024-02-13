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
    public class Api_MainReportController : Api_BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public Api_MainReportController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/MainReport
        [HttpGet]
        public CustomApiResponse GetMainReport()
        {
            CustomApiResponse response = new CustomApiResponse();

            try
            {
                if (IsUserAuthorized("MainReport", "Get", ref response))
                {
                    var items = _unitOfWork.MainReport.GetAll();
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

        // GET: api/MainReport/5
        [HttpGet("{id}")]
        public CustomApiResponse GetMainReport(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("MainReport", "Get", ref response))
                {
                    var MainReport = _unitOfWork.MainReport.GetById(id);
                    if (MainReport == null)
                    {
                        response.Error = "No MainReport data found";

                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        _resp.SuccessReponse(MainReport, ref response);
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

        // POST: api/MainReport
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public CustomApiResponse PostMainReport(MainReport MainReport)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("MainReport", "Post", ref response))
                {
                    if (_unitOfWork.MainReport.DataCount(u => u.Name == MainReport.Name) == 0)
                    {
                        if (IsOkToInsert(MainReport, ref response))
                        {
                            MainReport.CreatedByUserId = int.Parse(CurrentUserID.ToString());
                            MainReport.CreatedDate = DateTime.UtcNow;
                            _unitOfWork.MainReport.Add(MainReport);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(MainReport, ref response, (int)HttpStatusCode.Created);
                            response.CustomMessage = "MainReport Added Sucessfully";
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

        // PUT: api/MainReport/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public CustomApiResponse PutMainReport(int id, MainReport MainReport)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("MainReport", "PUT", ref response))
                {
                    if (id != MainReport.Id)
                    {
                        response.Error = "Wrong MainReport ID ";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        if (!_unitOfWork.MainReport.DataExists(u => u.Id == id))
                        {
                            response.Error = "Record not Found";
                            _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                        }
                        else
                        {
                            if (IsOkToUpdate(MainReport, ref response))
                            {
                                _unitOfWork.MainReport.Update(MainReport);
                                _unitOfWork.SaveAllChanges();

                                _resp.SuccessReponse(MainReport, ref response, (int)HttpStatusCode.Created);

                                response.CustomMessage = "MainReport Detail Updated Sucessfully";
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

        // DELETE: api/MainReport/5
        [HttpDelete("{id}")]
        public CustomApiResponse DeleteMainReport(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("MainReport", "Delete", ref response))
                {
                    var MainReport = _unitOfWork.MainReport.GetById(id);
                    if (MainReport == null)
                    {
                        response.Error = "No MainReport data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToDelete(MainReport, ref response))
                        {
                            _unitOfWork.MainReport.Remove(MainReport);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(MainReport, ref response);
                            response.CustomMessage = "MainReport Deleted Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong MainReport ID ";
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
        [Route("MainReportStatusChange/{id}/{curentuserid}")]
        public CustomApiResponse MainReportStatusChange(int id, String actiontype = "Deactivate")
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("MainReport", "Activate", ref response))
                {
                    var MainReport = _unitOfWork.MainReport.GetById(id);
                    if (MainReport == null)
                    {
                        response.Error = "No MainReport data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToActivationChange(MainReport, ref response, actiontype))
                        {
                            if (actiontype == "Deactivate")
                            {
                               // MainReport.IsActive = true;
                            }
                            else if (actiontype == "Activate")
                            {
                             //   MainReport.IsActive = false;
                            }
                            else if (actiontype == "Delete")
                            {
                                //   MainReport.IsDeleted = true;
                                //   MainReport.IsDeleted = true;
                                //   MainReport.DeletedByByUserId = int .Parse (CurrentUserID);
                                // MainReport.DeletedDate = DateTime.Now;

                            }

                            _unitOfWork.MainReport.Update(MainReport);
                            _unitOfWork.SaveAllChanges();
                           _resp.SuccessReponse(MainReport, ref response);
                            response.CustomMessage = "MainReport Status Changed to  '"+ actiontype + "' Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong MainReport ID ";
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




        private bool IsOkToInsert(MainReport MainReport, ref CustomApiResponse response)
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

        private bool IsOkToUpdate(MainReport MainReport, ref CustomApiResponse response)
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

        private bool IsOkToDelete(MainReport MainReport, ref CustomApiResponse response)
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

        private bool IsOkToActivationChange(MainReport MainReport, ref CustomApiResponse response, String actiontype)
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

        //private bool MainReportExists(int id)
        //{
        //    return _context.MainReport.Any(e => e.Id == id);
        //}
    }
}