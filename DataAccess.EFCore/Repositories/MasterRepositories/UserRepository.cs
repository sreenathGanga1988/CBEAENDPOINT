using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DataAccess.EFCore.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext context) : base(context)
        {
        }



        public User GetvalidUser(string UserName, string Password, out string message)
        {
            message = "";

            Boolean isvaliduser = false;
            var user = _context.User.Where(u => u.UserName == UserName && u.IsLocked == false && u.IsActive == true).FirstOrDefault();
            if (user != null)
            {

                if (user.Password == Password)
                {
                    user.WrongTryCount = 0;
                    user.LastLoggedTime = DateTime.UtcNow;

                    //UserLog userLog = new UserLog();
                    //userLog.ActionTimeUTC = DateTime.UtcNow;
                    //userLog.ActionType = "Login";
                    //userLog.userid = user.Id;

                    //_context.UserLogs.Add(userLog);

                    isvaliduser = true;
                }
                else
                {

                    if (user.WrongTryCount == null)
                    {
                        user.WrongTryCount = 0;
                    }
                    user.WrongTryCount++;

                    if (user.WrongTryCount > 10)
                    {
                        user.IsLocked = true;

                        message = "User Locked due to multiple wrong attempts";
                    }

                    isvaliduser = false;
                }
            }
            else
            {
                isvaliduser = false;
            }


            _context.SaveChanges();


            if (isvaliduser == true)
            {
                message = "Login successfull";


              



                var k = (from u in _context.User
                         join m in _context.Member on u.EmployeeId equals m.StaffNo
                         where u.UserName == UserName && u.Password == Password && u.IsActive == true
                         select new User { EmployeeId = u.EmployeeId, Id = u.Id, UserTypeId = u.UserTypeId, UserName = u.UserName, EmployeeName = m.Name }).FirstOrDefault();
                return k;
            }
            else
            {
                message = "Invalid userName or Passowrd " + message;
                return null;
            }



        }

        public IQueryable<User> GetAllMemberasync(Expression<Func<User, bool>> expression)
        {
            var result = (from usr in _context.User
                          join typ in _context.UserType on usr.UserTypeId equals typ.Id
                          join mmbr in _context.Member on usr.EmployeeId equals mmbr.StaffNo
                          select new User
                          {


                              Id = usr.Id,
                              UserName = usr.UserName,
                              EmployeeId = usr.EmployeeId,
                              UserTypeId = usr.UserTypeId,
                              EmployeeName = mmbr.Name,
                              UserTypeName = typ.Abbreviation,
                              EmailId = usr.EmailId,
                              PhoneNum = usr.PhoneNum,
                              UserStatus = usr.UserStatus

                          }).AsNoTracking().AsQueryable();
            result = result.Where(expression);


            return result;
        }



        public Boolean Approve(int ID, int UseriD, Boolean approve)
        {
            Boolean issucess = false;

            var usr = _context.User.Find(ID);

            if (usr != null)
            {
                if (approve == true)
                {
                    usr.IsActive = true;
                    usr.UserStatus = "A";
                    issucess = true;
                }
                else
                {
                    usr.IsActive = false;
                    usr.UserStatus = "R";
                    issucess = true;
                }
            }
            return issucess;
        }

    }

}
