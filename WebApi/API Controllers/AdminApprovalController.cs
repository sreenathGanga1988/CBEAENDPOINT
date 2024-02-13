using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Domain.Entities.BaseEntities;
using System.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using DTO;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminApprovalController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminApprovalController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }




        [HttpGet]
        public ActionResult<IEnumerable<dynamic>> GetAdminApproval(string id = "")
        {
            List<dynamic> pendinglist = new List<dynamic>();

            if (id.ToString().ToUpper().Trim() == ("PendingUser").ToString().ToUpper().Trim())
            {
                pendinglist = _unitOfWork.User.Find(u => u.UserStatus == "P" && u.IsActive == false).ToList<dynamic>();
            }
            else if (id.ToString().ToUpper().Trim() == ("PendingContribution").ToString().ToUpper().Trim())
            {
                pendinglist = _unitOfWork.Contributions.Find(u => u.ContributionStatus == "P" && u.isApproved == false).ToList<dynamic>();
            }
            else if (id.ToString().ToUpper().Trim() == ("PendingDirectContribution").ToString().ToUpper().Trim())
            {
                pendinglist = _unitOfWork.AccountsDirectEntry.Find(u => u.status == "P" && u.isApproved == false).ToList<dynamic>();
            }

            return Ok(pendinglist);
        }

        //[HttpPost]
        //public ActionResult<IEnumerable<dynamic>> PostAdminApproval(int ID, int UseriD, Boolean approve,string id = "")
        //{
        //    List<dynamic> pendinglist = new List<dynamic>();

        //    if (id.ToString().ToUpper().Trim() == ("PendingUser").ToString().ToUpper().Trim())
        //    {
        //        pendinglist = _unitOfWork.User.Find(u => u.UserStatus == "P" && u.IsActive == false).ToList<dynamic>();
        //    }
        //    else if (id.ToString().ToUpper().Trim() == ("PendingContribution").ToString().ToUpper().Trim())
        //    {
        //        pendinglist = _unitOfWork.Contributions.Find(u => u.ContributionStatus == "P" && u.isApproved == false).ToList<dynamic>();
        //    }
        //    else if (id.ToString().ToUpper().Trim() == ("PendingDirectContribution").ToString().ToUpper().Trim())
        //    {
        //        pendinglist = _unitOfWork.AccountsDirectEntry.Find(u => u.status == "P" && u.isApproved == false).ToList<dynamic>();
        //    }

        //    return Ok(pendinglist);
        //}

        [HttpPost]
        public ActionResult<IEnumerable<dynamic>> PostAdminApproval(ApproveDTO approveDTO)
        {
            List<dynamic> pendinglist = new List<dynamic>();

            Boolean issucess = false;

            try
            {
                if (approveDTO.type.ToString().ToUpper().Trim() == ("PendingUser").ToString().ToUpper().Trim())
                {
                    issucess = _unitOfWork.User.Approve(approveDTO.ID, approveDTO.UseriD, approveDTO.approve);
                }
                else if (approveDTO.type.ToString().ToUpper().Trim() == ("PendingContribution").ToString().ToUpper().Trim())
                {
                    issucess = _unitOfWork.Contributions.Approve(approveDTO.ID, approveDTO.UseriD, approveDTO.approve);
                }
                else if (approveDTO.type.ToString().ToUpper().Trim() == ("PendingDirectContribution").ToString().ToUpper().Trim())
                {
                    issucess = _unitOfWork.AccountsDirectEntry.Approve(approveDTO.ID, approveDTO.UseriD, approveDTO.approve);
                }
                _unitOfWork.SaveAllChanges();
                issucess = true;
            }
            catch (Exception ex)
            {
                
                issucess=false;
            }
            
            if (issucess==true)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
            
        }
    }
}