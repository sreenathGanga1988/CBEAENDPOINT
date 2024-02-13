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
    public class Api_YearMasterController : Api_BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public Api_YearMasterController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/YearMaster
        [HttpGet]
        public CustomApiResponse GetYearMaster()
        {
            CustomApiResponse response = new CustomApiResponse();

            try
            {
                if (IsUserAuthorized("YearMaster", "Get", ref response))
                {
                    var items = _unitOfWork.YearMaster.GetAll();
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

        // GET: api/YearMaster/5
        [HttpGet("{id}")]
        public CustomApiResponse GetYearMaster(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("YearMaster", "Get", ref response))
                {
                    var YearMaster = _unitOfWork.YearMaster.GetById(id);
                    if (YearMaster == null)
                    {
                        response.Error = "No YearMaster data found";

                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        _resp.SuccessReponse(YearMaster, ref response);
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

        // POST: api/YearMaster
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public CustomApiResponse PostYearMaster(YearMaster YearMaster)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("YearMaster", "Post", ref response))
                {
                    if (_unitOfWork.YearMaster.DataCount(u => u.YearOf == YearMaster.YearOf) == 0)
                    {
                        if (IsOkToInsert(YearMaster, ref response))
                        {


                          
                            _unitOfWork.YearMaster.Add(YearMaster);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(YearMaster, ref response, (int)HttpStatusCode.Created);
                            response.CustomMessage = "YearMaster Added Sucessfully";
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

        // PUT: api/YearMaster/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public CustomApiResponse PutYearMaster(int id, YearMaster YearMaster)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("YearMaster", "PUT", ref response))
                {
                    if (id != YearMaster.YearOf)
                    {
                        response.Error = "Wrong YearMaster ID ";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        if (!_unitOfWork.YearMaster.DataExists(u => u.YearOf == id))
                        {
                            response.Error = "Record not Found";
                            _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                        }
                        else
                        {
                            if (IsOkToUpdate(YearMaster, ref response))
                            {
                                _unitOfWork.YearMaster.Update(YearMaster);
                                _unitOfWork.SaveAllChanges();

                                _resp.SuccessReponse(YearMaster, ref response, (int)HttpStatusCode.Created);

                                response.CustomMessage = "YearMaster Detail Updated Sucessfully";
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

        // DELETE: api/YearMaster/5
        [HttpDelete("{id}")]
        public CustomApiResponse DeleteYearMaster(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("YearMaster", "Delete", ref response))
                {
                    var YearMaster = _unitOfWork.YearMaster.GetById(id);
                    if (YearMaster == null)
                    {
                        response.Error = "No YearMaster data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToDelete(YearMaster, ref response))
                        {
                            _unitOfWork.YearMaster.Remove(YearMaster);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(YearMaster, ref response);
                            response.CustomMessage = "YearMaster Deleted Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong YearMaster ID ";
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
        [Route("YearMasterStatusChange/{id}/{curentuserid}")]
        public CustomApiResponse YearMasterStatusChange(int id, String actiontype = "Deactivate")
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("YearMaster", "Activate", ref response))
                {
                    var YearMaster = _unitOfWork.YearMaster.GetById(id);
                    if (YearMaster == null)
                    {
                        response.Error = "No YearMaster data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToActivationChange(YearMaster, ref response, actiontype))
                        {
                            if (actiontype == "Deactivate")
                            {
                                //  YearMaster.IsActive = true;
                            }
                            else if (actiontype == "Activate")
                            {
                                //  YearMaster.IsActive = false;
                            }
                            else if (actiontype == "Delete")
                            {
                                //YearMaster.IsDeleted = true;
                                //YearMaster.DeletedByByUserId = int.Parse(CurrentUserID);
                                //YearMaster.DeletedDate = DateTime.Now;

                            }

                            _unitOfWork.YearMaster.Update(YearMaster);
                            _unitOfWork.SaveAllChanges();
                            _resp.SuccessReponse(YearMaster, ref response);
                            response.CustomMessage = "YearMaster Status Changed to  '" + actiontype + "' Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong YearMaster ID ";
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




        private bool IsOkToInsert(YearMaster YearMaster, ref CustomApiResponse response)
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

        private bool IsOkToUpdate(YearMaster YearMaster, ref CustomApiResponse response)
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

        private bool IsOkToDelete(YearMaster YearMaster, ref CustomApiResponse response)
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

        private bool IsOkToActivationChange(YearMaster YearMaster, ref CustomApiResponse response, String actiontype)
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

        //private bool YearMasterExists(int id)
        //{
        //    return _context.YearMaster.Any(e => e.Id == id);
        //}
    }
}