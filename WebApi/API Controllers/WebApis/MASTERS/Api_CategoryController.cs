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
    public class Api_CategoryController : Api_BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public Api_CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Category
        [HttpGet]
        public CustomApiResponse GetCategory()
        {
            CustomApiResponse response = new CustomApiResponse();

            try
            {
                if (IsUserAuthorized("Category", "Get", ref response))
                {
                    var items = _unitOfWork.Category.GetAll();
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

        // GET: api/Category/5
        [HttpGet("{id}")]
        public CustomApiResponse GetCategory(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Category", "Get", ref response))
                {
                    var Category = _unitOfWork.Category.GetById(id);
                    if (Category == null)
                    {
                        response.Error = "No Category data found";

                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        _resp.SuccessReponse(Category, ref response);
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

        // POST: api/Category
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public CustomApiResponse PostCategory(Category Category)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Category", "Post", ref response))
                {
                    if (_unitOfWork.Category.DataCount(u => u.Abbreviation == Category.Abbreviation) == 0)
                    {
                        if (IsOkToInsert(Category, ref response))
                        {

                            Category.CreatedByUserId = int.Parse(CurrentUserID.ToString());
                            Category.CreatedDate = DateTime.UtcNow;
                            _unitOfWork.Category.Add(Category);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(Category, ref response, (int)HttpStatusCode.Created);
                            response.CustomMessage = "Category Added Sucessfully";
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

        // PUT: api/Category/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public CustomApiResponse PutCategory(int id, Category Category)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Category", "PUT", ref response))
                {
                    if (id != Category.Id)
                    {
                        response.Error = "Wrong Category ID ";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        if (!_unitOfWork.Category.DataExists(u => u.Id == id))
                        {
                            response.Error = "Record not Found";
                            _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                        }
                        else
                        {
                            if (IsOkToUpdate(Category, ref response))
                            {
                                _unitOfWork.Category.Update(Category);
                                _unitOfWork.SaveAllChanges();

                                _resp.SuccessReponse(Category, ref response, (int)HttpStatusCode.Created);

                                response.CustomMessage = "Category Detail Updated Sucessfully";
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

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public CustomApiResponse DeleteCategory(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Category", "Delete", ref response))
                {
                    var Category = _unitOfWork.Category.GetById(id);
                    if (Category == null)
                    {
                        response.Error = "No Category data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToDelete(Category, ref response))
                        {
                            _unitOfWork.Category.Remove(Category);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(Category, ref response);
                            response.CustomMessage = "Category Deleted Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong Category ID ";
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
        [Route("CategoryStatusChange/{id}/{curentuserid}")]
        public CustomApiResponse CategoryStatusChange(int id, String actiontype = "Deactivate")
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Category", "Activate", ref response))
                {
                    var Category = _unitOfWork.Category.GetById(id);
                    if (Category == null)
                    {
                        response.Error = "No Category data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToActivationChange(Category, ref response, actiontype))
                        {
                            if (actiontype == "Deactivate")
                            {
                                Category.IsActive = true;
                            }
                            else if (actiontype == "Activate")
                            {
                                Category.IsActive = false;
                            }
                            else if (actiontype == "Delete")
                            {
                                Category.IsDeleted = true;
                                Category.DeletedByByUserId = int.Parse(CurrentUserID);
                                Category.DeletedDate = DateTime.Now;

                            }

                            _unitOfWork.Category.Update(Category);
                            _unitOfWork.SaveAllChanges();
                            _resp.SuccessReponse(Category, ref response);
                            response.CustomMessage = "Category Status Changed to  '" + actiontype + "' Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong Category ID ";
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




        private bool IsOkToInsert(Category Category, ref CustomApiResponse response)
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

        private bool IsOkToUpdate(Category Category, ref CustomApiResponse response)
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

        private bool IsOkToDelete(Category Category, ref CustomApiResponse response)
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

        private bool IsOkToActivationChange(Category Category, ref CustomApiResponse response, String actiontype)
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

        //private bool CategoryExists(int id)
        //{
        //    return _context.Category.Any(e => e.Id == id);
        //}
    }
}