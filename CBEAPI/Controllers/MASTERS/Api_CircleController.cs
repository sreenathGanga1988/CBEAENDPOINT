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
    public class Api_CircleController : Api_BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public Api_CircleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Circle
        [HttpGet]
        public CustomApiResponse GetCircle()
        {
            CustomApiResponse response = new CustomApiResponse();

            try
            {
                if (IsUserAuthorized("Circle", "Get", ref response))
                {
                    var items = _unitOfWork.Circle.GetAll();
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

        // GET: api/Circle/5
        [HttpGet("{id}")]
        public CustomApiResponse GetCircle(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Circle", "Get", ref response))
                {
                    var Circle = _unitOfWork.Circle.GetById(id);
                    if (Circle == null)
                    {
                        response.Error = "No Circle data found";

                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        _resp.SuccessReponse(Circle, ref response);
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

        // POST: api/Circle
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public CustomApiResponse PostCircle(Circle Circle)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Circle", "Post", ref response))
                {
                    if (_unitOfWork.Circle.DataCount(u => u.Abbreviation == Circle.Abbreviation) == 0)
                    {
                        if (IsOkToInsert(Circle, ref response))
                        {

                            
                            Circle.CreatedDate = DateTime.UtcNow;
                            _unitOfWork.Circle.Add(Circle);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(Circle, ref response, (int)HttpStatusCode.Created);
                            response.CustomMessage = "Circle Added Sucessfully";
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

        // PUT: api/Circle/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public CustomApiResponse PutCircle(int id, Circle Circle)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Circle", "PUT", ref response))
                {
                    if (id != Circle.Id)
                    {
                        response.Error = "Wrong Circle ID ";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        if (!_unitOfWork.Circle.DataExists(u => u.Id == id))
                        {
                            response.Error = "Record not Found";
                            _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                        }
                        else
                        {
                            if (IsOkToUpdate(Circle, ref response))
                            {
                                _unitOfWork.Circle.Update(Circle);
                                _unitOfWork.SaveAllChanges();

                                _resp.SuccessReponse(Circle, ref response, (int)HttpStatusCode.Created);

                                response.CustomMessage = "Circle Detail Updated Sucessfully";
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

        // DELETE: api/Circle/5
        [HttpDelete("{id}")]
        public CustomApiResponse DeleteCircle(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Circle", "Delete", ref response))
                {
                    var Circle = _unitOfWork.Circle.GetById(id);
                    if (Circle == null)
                    {
                        response.Error = "No Circle data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToDelete(Circle, ref response))
                        {
                            _unitOfWork.Circle.Remove(Circle);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(Circle, ref response);
                            response.CustomMessage = "Circle Deleted Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong Circle ID ";
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
        [Route("CircleStatusChange/{id}/{curentuserid}")]
        public CustomApiResponse CircleStatusChange(int id, String actiontype = "Deactivate")
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Circle", "Activate", ref response))
                {
                    var Circle = _unitOfWork.Circle.GetById(id);
                    if (Circle == null)
                    {
                        response.Error = "No Circle data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToActivationChange(Circle, ref response, actiontype))
                        {
                            if (actiontype == "Deactivate")
                            {
                                Circle.IsActive = true;
                            }
                            else if (actiontype == "Activate")
                            {
                                Circle.IsActive = false;
                            }
                            else if (actiontype == "Delete")
                            {
                                //Circle.IsDeleted = true;
                                //Circle.DeletedByByUserId = int.Parse(CurrentUserID);
                                //Circle.DeletedDate = DateTime.Now;

                            }

                            _unitOfWork.Circle.Update(Circle);
                            _unitOfWork.SaveAllChanges();
                            _resp.SuccessReponse(Circle, ref response);
                            response.CustomMessage = "Circle Status Changed to  '" + actiontype + "' Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong Circle ID ";
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




        private bool IsOkToInsert(Circle Circle, ref CustomApiResponse response)
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

        private bool IsOkToUpdate(Circle Circle, ref CustomApiResponse response)
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

        private bool IsOkToDelete(Circle Circle, ref CustomApiResponse response)
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

        private bool IsOkToActivationChange(Circle Circle, ref CustomApiResponse response, String actiontype)
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

        //private bool CircleExists(int id)
        //{
        //    return _context.Circle.Any(e => e.Id == id);
        //}
    }
}