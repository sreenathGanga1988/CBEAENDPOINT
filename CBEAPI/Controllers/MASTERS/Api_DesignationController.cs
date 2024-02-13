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
    public class Api_DesignationController : Api_BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public Api_DesignationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Designation
        [HttpGet]
        public CustomApiResponse GetDesignation()
        {
            CustomApiResponse response = new CustomApiResponse();

            try
            {
                if (IsUserAuthorized("Designation", "Get", ref response))
                {
                    var items = _unitOfWork.Designation.GetAll();
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

        // GET: api/Designation/5
        [HttpGet("{id}")]
        public CustomApiResponse GetDesignation(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Designation", "Get", ref response))
                {
                    var Designation = _unitOfWork.Designation.GetById(id);
                    if (Designation == null)
                    {
                        response.Error = "No Designation data found";

                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        _resp.SuccessReponse(Designation, ref response);
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

        // POST: api/Designation
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public CustomApiResponse PostDesignation(Designation Designation)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Designation", "Post", ref response))
                {
                    if (_unitOfWork.Designation.DataCount(u => u.Name  == Designation.Name) == 0)
                    {
                        if (IsOkToInsert(Designation, ref response))
                        {

                            Designation.CreatedByUserId = int.Parse(CurrentUserID.ToString());
                            Designation.CreatedDate = DateTime.UtcNow;
                            _unitOfWork.Designation.Add(Designation);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(Designation, ref response, (int)HttpStatusCode.Created);
                            response.CustomMessage = "Designation Added Sucessfully";
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

        // PUT: api/Designation/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public CustomApiResponse PutDesignation(int id, Designation Designation)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Designation", "PUT", ref response))
                {
                    if (id != Designation.Id)
                    {
                        response.Error = "Wrong Designation ID ";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        if (!_unitOfWork.Designation.DataExists(u => u.Id == id))
                        {
                            response.Error = "Record not Found";
                            _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                        }
                        else
                        {
                            if (IsOkToUpdate(Designation, ref response))
                            {
                                _unitOfWork.Designation.Update(Designation);
                                _unitOfWork.SaveAllChanges();

                                _resp.SuccessReponse(Designation, ref response, (int)HttpStatusCode.Created);

                                response.CustomMessage = "Designation Detail Updated Sucessfully";
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

        // DELETE: api/Designation/5
        [HttpDelete("{id}")]
        public CustomApiResponse DeleteDesignation(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Designation", "Delete", ref response))
                {
                    var Designation = _unitOfWork.Designation.GetById(id);
                    if (Designation == null)
                    {
                        response.Error = "No Designation data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToDelete(Designation, ref response))
                        {
                            _unitOfWork.Designation.Remove(Designation);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(Designation, ref response);
                            response.CustomMessage = "Designation Deleted Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong Designation ID ";
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
        [Route("DesignationStatusChange/{id}/{curentuserid}")]
        public CustomApiResponse DesignationStatusChange(int id, String actiontype = "Deactivate")
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Designation", "Activate", ref response))
                {
                    var Designation = _unitOfWork.Designation.GetById(id);
                    if (Designation == null)
                    {
                        response.Error = "No Designation data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToActivationChange(Designation, ref response, actiontype))
                        {
                            if (actiontype == "Deactivate")
                            {
                              //  Designation.IsActive = true;
                            }
                            else if (actiontype == "Activate")
                            {
                            //    Designation.IsActive = false;
                            }
                            else if (actiontype == "Delete")
                            {
                                //Designation.IsDeleted = true;
                                //Designation.DeletedByByUserId = int.Parse(CurrentUserID);
                                //Designation.DeletedDate = DateTime.Now;

                            }

                            _unitOfWork.Designation.Update(Designation);
                            _unitOfWork.SaveAllChanges();
                            _resp.SuccessReponse(Designation, ref response);
                            response.CustomMessage = "Designation Status Changed to  '" + actiontype + "' Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong Designation ID ";
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




        private bool IsOkToInsert(Designation Designation, ref CustomApiResponse response)
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

        private bool IsOkToUpdate(Designation Designation, ref CustomApiResponse response)
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

        private bool IsOkToDelete(Designation Designation, ref CustomApiResponse response)
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

        private bool IsOkToActivationChange(Designation Designation, ref CustomApiResponse response, String actiontype)
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

        //private bool DesignationExists(int id)
        //{
        //    return _context.Designation.Any(e => e.Id == id);
        //}
    }
}