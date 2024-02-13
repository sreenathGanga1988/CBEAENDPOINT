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
    public class Api_MemberController : Api_BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public Api_MemberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Member
        [HttpGet]
        public CustomApiResponse GetMember()
        {
            CustomApiResponse response = new CustomApiResponse();

            try
            {
                if (IsUserAuthorized("Member", "Get", ref response))
                {
                    var items = _unitOfWork.Member.GetAll();
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

        // GET: api/Member/5
        [HttpGet("{id}")]
        public CustomApiResponse GetMember(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Member", "Get", ref response))
                {
                    var Member = _unitOfWork.Member.GetById(id);
                    if (Member == null)
                    {
                        response.Error = "No Member data found";

                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        _resp.SuccessReponse(Member, ref response);
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

        // POST: api/Member
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public CustomApiResponse PostMember(Member Member)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Member", "Post", ref response))
                {
                    if (_unitOfWork.Member.DataCount(u => u.Name == Member.Name) == 0)
                    {
                        if (IsOkToInsert(Member, ref response))
                        {
                            Member.CreatedByUserId = int.Parse(CurrentUserID.ToString());
                            Member.CreatedDate = DateTime.UtcNow;
                            _unitOfWork.Member.Add(Member);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(Member, ref response, (int)HttpStatusCode.Created);
                            response.CustomMessage = "Member Added Sucessfully";
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

        // PUT: api/Member/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public CustomApiResponse PutMember(int id, Member Member)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Member", "PUT", ref response))
                {
                    if (id != Member.Id)
                    {
                        response.Error = "Wrong Member ID ";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        if (!_unitOfWork.Member.DataExists(u => u.Id == id))
                        {
                            response.Error = "Record not Found";
                            _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                        }
                        else
                        {
                            if (IsOkToUpdate(Member, ref response))
                            {
                                _unitOfWork.Member.Update(Member);
                                _unitOfWork.SaveAllChanges();

                                _resp.SuccessReponse(Member, ref response, (int)HttpStatusCode.Created);

                                response.CustomMessage = "Member Detail Updated Sucessfully";
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

        // DELETE: api/Member/5
        [HttpDelete("{id}")]
        public CustomApiResponse DeleteMember(int id)
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Member", "Delete", ref response))
                {
                    var Member = _unitOfWork.Member.GetById(id);
                    if (Member == null)
                    {
                        response.Error = "No Member data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToDelete(Member, ref response))
                        {
                            _unitOfWork.Member.Remove(Member);
                            _unitOfWork.SaveAllChanges();

                            _resp.SuccessReponse(Member, ref response);
                            response.CustomMessage = "Member Deleted Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong Member ID ";
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
        [Route("MemberStatusChange/{id}/{curentuserid}")]
        public CustomApiResponse MemberStatusChange(int id, String actiontype = "Deactivate")
        {
            CustomApiResponse response = new CustomApiResponse();
            try
            {
                if (IsUserAuthorized("Member", "Activate", ref response))
                {
                    var Member = _unitOfWork.Member.GetById(id);
                    if (Member == null)
                    {
                        response.Error = "No Member data found";
                        _resp.CustomErrorReponse(ref response, (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        if (IsOkToActivationChange(Member, ref response, actiontype))
                        {
                            if (actiontype == "Deactivate")
                            {
                               // Member.IsActive = true;
                            }
                            else if (actiontype == "Activate")
                            {
                             //   Member.IsActive = false;
                            }
                            else if (actiontype == "Delete")
                            {
                                //   Member.IsDeleted = true;
                                //   Member.IsDeleted = true;
                                //   Member.DeletedByByUserId = int .Parse (CurrentUserID);
                                // Member.DeletedDate = DateTime.Now;

                            }

                            _unitOfWork.Member.Update(Member);
                            _unitOfWork.SaveAllChanges();
                           _resp.SuccessReponse(Member, ref response);
                            response.CustomMessage = "Member Status Changed to  '"+ actiontype + "' Sucessfully";
                        }
                        else
                        {
                            response.Error = "Wrong Member ID ";
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




        private bool IsOkToInsert(Member Member, ref CustomApiResponse response)
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

        private bool IsOkToUpdate(Member Member, ref CustomApiResponse response)
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

        private bool IsOkToDelete(Member Member, ref CustomApiResponse response)
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

        private bool IsOkToActivationChange(Member Member, ref CustomApiResponse response, String actiontype)
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

        //private bool MemberExists(int id)
        //{
        //    return _context.Member.Any(e => e.Id == id);
        //}
    }
}