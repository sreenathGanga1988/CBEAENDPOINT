using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsDirectEntryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountsDirectEntryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/AccountsDirectEntry
        [HttpGet]
        public ActionResult<IEnumerable<AccountsDirectEntry>> GetAccountsDirectEntry()
        {
             
               return Ok(_unitOfWork.AccountsDirectEntry.GetAll());
        }

        // POST: api/AccountsDirectEntry
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult <AccountsDirectEntry> PostAccountsDirectEntry(AccountsDirectEntry AccountsDirectEntry)
        {
            AccountsDirectEntry.isApproved = false;
            AccountsDirectEntry.status = "P";
            _unitOfWork.AccountsDirectEntry.Add(AccountsDirectEntry);
            _unitOfWork.SaveAllChanges();

            return CreatedAtAction("GetAccountsDirectEntry", new { id = AccountsDirectEntry.ID}, AccountsDirectEntry);
        }




        // GET: api/AccountsDirectEntry/5
        [HttpGet("{id}")]
        public  ActionResult<AccountsDirectEntry> GetAccountsDirectEntry(int id)
        {
            var AccountsDirectEntry = _unitOfWork.AccountsDirectEntry.GetById (id);

            if (AccountsDirectEntry == null)
            {
                return NotFound();
            }

            return AccountsDirectEntry;
        }

        [HttpPut("{id}")]
        public IActionResult PutAccountsDirectEntry(int id, AccountsDirectEntry AccountsDirectEntry)
        {
            if (id != AccountsDirectEntry.ID)
            {
                return BadRequest();
            }

            _unitOfWork.AccountsDirectEntry.Update(AccountsDirectEntry);

            if (AccountsDirectEntry.isApproved == true)
            {
                Accounts accounts = GetAccountsEntry(AccountsDirectEntry);
                _unitOfWork.AccountsRepository.Add(accounts);
            }

            try
            {
                _unitOfWork.SaveAllChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_unitOfWork.AccountsDirectEntry.DataExists(u => u.ID == id))
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
        public Accounts GetAccountsEntry(AccountsDirectEntry AccountsDirectEntry)
        {
            Accounts accounts = new Accounts();
            if (AccountsDirectEntry != null)
            {
                var user = _unitOfWork.Member.GetById(int.Parse(AccountsDirectEntry.StaffNo.ToString()));

                accounts.CircleId = AccountsDirectEntry.ID;
                accounts.DpCode = int.Parse(AccountsDirectEntry.DpCode.ToString());
                accounts.StaffNo = int.Parse(AccountsDirectEntry.StaffNo.ToString());
                accounts.Amount = decimal.Parse(AccountsDirectEntry.Amt.ToString());

            }

            return accounts;
        }

        // DELETE: api/AccountsDirectEntry/5
        [HttpDelete("{id}")]
        public ActionResult<AccountsDirectEntry> DeleteAccountsDirectEntry(int id)
        {
            var AccountsDirectEntry = _unitOfWork.AccountsDirectEntry.GetById(id);
            if (AccountsDirectEntry == null)
            {
                return NotFound();
            }

            _unitOfWork.AccountsDirectEntry.Remove(AccountsDirectEntry);
             _unitOfWork.SaveAllChanges();

            return AccountsDirectEntry;
        }

        //private bool AccountsDirectEntryExists(int id)
        //{
        //    return _context.AccountsDirectEntry.Any(e => e.Id == id);
        //}
    }
}
