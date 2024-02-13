using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using DTO;
using ExtensionMethods;
using WebApi.Controllers;

namespace WebApi.API_Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ContributionController : BaseAPIController
    {

        private readonly IUnitOfWork _unitOfWork;

        public ContributionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<dynamic>> GetContribution()
        {

            return Ok(_unitOfWork.Contributions.GetAllContribution());
        }
        [HttpPost]
        public ActionResult<Circle> PostContribution(ContributionMasterDTO fileUploadDetail)
        {
            //_unitOfWork.Circle.Add(Circle);
            //_unitOfWork.SaveAllChanges();

            //return CreatedAtAction("GetCircle", new { id = Circle.Id }, Circle);
            try
            {

                var fileref = "Bankfile-" + fileUploadDetail.Year.ToString() + "/" + fileUploadDetail.Month.ToString();

                var q = _unitOfWork.Contributions.Find(u => u.Month == fileUploadDetail.Month.ToString() && u.Year == fileUploadDetail.Year.ToString()).ToList ();



                if (q != null)
                {
                    foreach (var item in q)
                {
                    var q1 = _unitOfWork.ContributionDetails.Find(u => u.ContributionMasterId == item.Id);
                    _unitOfWork.ContributionDetails.RemoveRange(q1);
                        _unitOfWork.Contributions.Remove(item);
                    }

               

                    var errorlog= _unitOfWork.CustomErrorLogRepository .Find(u => u.Reference  ==  fileref);
                    _unitOfWork.CustomErrorLogRepository.RemoveRange(errorlog);
                }

               

                fileUploadDetail.ReadContributionFromFile();

                ContributionMaster contributionMaster = new ContributionMaster();

                contributionMaster.FileName = fileUploadDetail.FileName;
                contributionMaster.FileLocation = fileUploadDetail.FileLocation;
                contributionMaster.FileType = fileUploadDetail.FileType;
                contributionMaster.FileExtension = fileUploadDetail.FileExtension;
                contributionMaster.Month = fileUploadDetail.Month.ToString ();
                contributionMaster.Year = fileUploadDetail.Year.ToString ();
                contributionMaster.Circle = fileUploadDetail.Circle;
                contributionMaster.totalamount = fileUploadDetail.totalamount;
                contributionMaster.totalentry = fileUploadDetail.totalentry;
                contributionMaster.NewMemberCount = fileUploadDetail.NewMemberCount;
                contributionMaster.isApproved = false;
                contributionMaster.ContributionStatus = fileUploadDetail.ContributionStatus;
               


                foreach (var item in fileUploadDetail.ContributionDetails)
                {

                   
                        ContributionDetail contributionDetail = new ContributionDetail();

                        contributionDetail.Circle = int.Parse(item.Circle);
                        contributionDetail.FullString = item.FullString;
                        contributionDetail.Month = item.Month.ToString ();
                        contributionDetail.Year = item.Year.ToString();
                    contributionDetail.DpCode = item.DpCode;
                        contributionDetail.StaffNo = item.StaffNo;
                        contributionDetail.Name = item.Name;
                        contributionDetail.Designation = item.Designation;
                        contributionDetail.Amount = item.Contribution;

                        contributionMaster.ContributionDetails.Add(contributionDetail);

                    
                   
                }

                if (fileUploadDetail.customErrorLogDTOs.Count > 0)
                {
                    foreach (var item in fileUploadDetail.customErrorLogDTOs)
                    {


                        CustomErrorLog customErrorLog = new CustomErrorLog();
                        customErrorLog.Detail = item.Detail;
                        customErrorLog.log = item.log;
                        customErrorLog.Remark = item.Remark;
                        customErrorLog.Reference = fileref;
                        _unitOfWork.CustomErrorLogRepository.Add(customErrorLog);
                    }
                }
               
                _unitOfWork.Contributions.Add(contributionMaster);
                
                _unitOfWork.SaveAllChanges();



                contributionMaster.NewMemberCount = _unitOfWork.ContributionDetails.GetWrongMembers(int.Parse ( contributionMaster.Id.ToString ())).ToString ();
            }
            catch (System.Exception ex)
            {

                try
                {
                    System.IO.File.Delete(fileUploadDetail.FileLocation);
                }
                catch (System.Exception)                {

                   
                }
                return  BadRequest(ex);
            }

            return Ok();
        }

        [HttpGet("{id}")]
        public ActionResult <ContributionMaster> GetContribution(int id)
        {
            return _unitOfWork.Contributions.Find(U=>U.Id==id).FirstOrDefault();

            //return _unitOfWork.Contributions.GetById(id);
        }
        [HttpPut("{id}")]
        public IActionResult PutContribution(int id, ContributionMaster contributionMaster)
        {
            if (id != contributionMaster.Id)
            {
                return BadRequest();
            }

            _unitOfWork.Contributions.Update(contributionMaster);

            try
            {
                _unitOfWork.SaveAllChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_unitOfWork.Contributions.DataExists(u => u.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

          // DELETE: api/Contribution/5
        [HttpDelete("{id}")]
        public ActionResult<ContributionMaster> DeleteContribution(int id)
        {
            var item = _unitOfWork.Contributions.Find(U => U.Id == id).FirstOrDefault();
            if (item == null)
            {
                return NotFound();
            }
            else
            {
                if(  item.ContributionStatus.TrimUpper() == "A")
                {
                    return BadRequest("Contribution Already Approved" );
                   
                }
                else
                {
                    var q1 = _unitOfWork.ContributionDetails.Find(u => u.ContributionMasterId == item.Id);
                    _unitOfWork.ContributionDetails.RemoveRange(q1);
                    _unitOfWork.Contributions.Remove(item);
                  _unitOfWork.SaveAllChanges();
                 
                }            

            }

            return Ok();
        }

    }
}
