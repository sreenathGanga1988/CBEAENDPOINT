using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Text.Json;

namespace DataAccess.EFCore.Repositories
{
    public class AppAuditRepository : GenericRepository<App_EntityLog>, IAppAuditRepository
    {
        public AppAuditRepository(ApplicationContext context) : base(context)
        {
        }

        public void AddLogs(String EntityName, Object PrimaryKey, String ActionType, String CreatedByUserId,Object objdata)
        {
            App_EntityLog appAudit = new App_EntityLog();
            appAudit.ActionType = ActionType.ToString();
            appAudit.EntityName = EntityName.ToUpper();
            appAudit.EntityPrimaryKey = PrimaryKey.ToString();
            appAudit.DisplayAction = ActionType.ToString();
            appAudit.ActionTime = DateTime.UtcNow;
            appAudit.NewContent = SerializeRecord(objdata);
            appAudit.CreatedByUserId = CreatedByUserId.ToString();
            _context.Add(appAudit);
            _context.SaveChanges();
        }

        public static String SerializeRecord (Object objData)
        {
            String jsonData = "";

            try
            {
                jsonData = JsonSerializer.Serialize(objData);
            }
            catch (Exception)
            {


            }

            return jsonData;
        }
    }

}
