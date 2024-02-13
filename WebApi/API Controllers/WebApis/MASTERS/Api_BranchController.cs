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
    public class Api_BranchController : Api_BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public Api_BranchController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Branch
        [HttpGet]
        public CustomApiResponse GetBranch()
        {
            CustomApiResponse response = new CustomApiResponse();

            try
            {
                if (IsUserAuthorized("Branch", "Get", ref response))
                {
                    var items = _unitOfWork.Branch.GetBrachDetails();
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

        // GET: api/Branch/5
        [HttpGet("{id}")]
        public CustomApiResponse GetBranch(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Branch", "Get", ref response))
                {
                    var branch = _unitOfWork.Branch.GetById(id);
                    if (branch == null)
                    {
                        response.Error = "No Branch data found";

                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        _resp.SuccessReponse(branch, ref response);
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

        // POST: api/Branch
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public CustomApiResponse PostBranch(Branch branch)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Branch", "Post", ref response))
                {
                    if (_unitOfWork.Branch.DataCount(u => u.DpCode == branch.DpCode) == 0)
                    {
                        if (IsOkToInsert(branch, ref response))
                        {

                            branch.CreatedByUserId = int.Parse(CurrentUserID.ToString());
                            branch.CreatedDate = DateTime.UtcNow;

                            _unitOfWork.Branch.Add(branch);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(branch, ref response, (int)HttpStatusCode.Created);
                            response.CustomMessage = "Branch Added Sucessfully";
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

        // PUT: api/Branch/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public CustomApiResponse PutBranch(int id, Branch branch)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Branch", "PUT", ref response))
                {
                    if (id != branch.Id)
                    {
                        response.Error = "Wrong Branch ID ";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        if (!_unitOfWork.Branch.DataExists(u => u.Id == id))
                        {
                            response.Error = "Record not Found";
                            _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                        }
                        else
                        {
                            if (IsOkToUpdate(branch, ref response))
                            {
                                _unitOfWork.Branch.Update(branch);
                                _unitOfWork.SaveAllChanges();

                                _resp.SuccessReponse(branch, ref response, (int)HttpStatusCode.Created);

                                response.CustomMessage = "Branch Detail Updated Sucessfully";
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

        // DELETE: api/Branch/5
        [HttpDelete("{id}")]
        public CustomApiResponse DeleteBranch(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Branch", "Delete", ref response))
                {
                    var branch = _unitOfWork.Branch.GetById(id);
                    if (branch == null)
                    {
                        response.Error = "No Branch data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToDelete(branch, ref response))
                        {
                            _unitOfWork.Branch.Remove(branch);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(branch, ref response);
                            response.CustomMessage = "Branch Deleted Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong Branch ID ";
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
        [Route("BranchStatusChange/{id}/{curentuserid}")]
        public CustomApiResponse BranchStatusChange(int id, String actiontype = "Deactivate")
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Branch", "Activate", ref response))
                {
                    var branch = _unitOfWork.Branch.GetById(id);
                    if (branch == null)
                    {
                        response.Error = "No Branch data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToActivationChange(branch, ref response, actiontype))
                        {
                            if (actiontype == "Deactivate")
                            {
                                branch.IsActive = true;
                            }
                            else if (actiontype == "Activate")
                            {
                                branch.IsActive = false;
                            }
                            else if (actiontype == "Delete")
                            {
                                branch.IsDeleted = true;
                                branch.DeletedByByUserId = int .Parse (CurrentUserID);
                                branch.DeletedDate = DateTime.Now;

                            }

                            _unitOfWork.Branch.Update(branch);
                            _unitOfWork.SaveAllChanges();
                           _resp.SuccessReponse(branch, ref response);
                            response.CustomMessage = "Branch Status Changed to  '"+ actiontype + "' Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong Branch ID ";
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




        private bool IsOkToInsert(Branch branch, ref CustomApiResponse response)
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

        private bool IsOkToUpdate(Branch branch, ref CustomApiResponse response)
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

        private bool IsOkToDelete(Branch branch, ref CustomApiResponse response)
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

        private bool IsOkToActivationChange(Branch branch, ref CustomApiResponse response, String actiontype)
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

        //private bool BranchExists(int id)
        //{
        //    return _context.Branch.Any(e => e.Id == id);
        //}
    }
}