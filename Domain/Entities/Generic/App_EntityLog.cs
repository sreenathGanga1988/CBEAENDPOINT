using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
        public abstract class BaseEntity
    {
        public int Id { get; private set; }
    }
    public interface IMustHaveTenant
    {
        public int TenantId { get; set; }
    }
    public interface IMustHaveSubTenant
    {
        public int SubTenantId { get; set; }
    }



    public class YearMasters
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Year { get; set; }

    }
    public class Country : BaseEntity
    {
        public String Name { get; set; }
    }


    public class AppEntity : BaseEntity
    {

        public string EntityName { get; set; }
        public string Description { get; set; }
        public string ModuleName { get; set; }
        public string TableName { get; set; }
    }
    public class AppEntityOperation : BaseEntity
    {

        public string OperationName { get; set; }
        public string OperationDisplayName { get; set; }
        public int AppEntityId { get; set; }

        public bool IsDefault { get; set; }
        public bool IsDisplay { get; set; }

        public virtual AppEntity Entity { get; set; }
    }
    public class EntityRight : BaseEntity
    {


        public int AppEntityOperationId { get; set; }
        public string RoleId { get; set; }
        public bool HasAccess { get; set; }
        public virtual AppEntityOperation AppEntityOperation { get; set; }

    }
    public class App_EntityLog : BaseEntity
    {

        public int AppEntityId { get; set; }
        public String EntityPrimaryKey { get; set; }
        public String EntityName { get; set; }
        public String ActionType { get; set; }

        public String DisplayAction { get; set; }
        public String NewContent { get; set; }

        public DateTime? ActionTime { get; set; } = DateTime.Now;

        public String CreatedByUserId { get; set; }

    }
    public class App_EntityLogValues : BaseEntity
    {

        public long AppEntityLogId { get; set; }
        public String ActionType { get; set; }
        public String OldValue { get; set; }
        public String NewValue { get; set; }

        public virtual App_EntityLog App_EntityLog { get; set; }
    }


    public class Tenant : BaseEntity
    {

        public string Name { get; set; }
        public string Address { get; set; }
        public string Licence { get; set; }

    }
    public class TenantConfiguration : BaseEntity, IMustHaveTenant
    {

        public string ConfigurationType { get; set; }
        public string ConfigurationKey { get; set; }
        public string ConfigurationValue { get; set; }
        public int TenantId { get; set; }
        public Boolean IsActive { get; set; }
        public virtual Tenant Tenant { get; set; }
    }

    public class LookUpItems : BaseEntity
    {
        public int MyProperty { get; set; }
        public string GroupName { get; set; }
        public string ItemValue { get; set; }
        public string ItemText { get; set; }

        public int? TenantId { get; set; }
        public int? SubTenantId { get; set; }
        public Boolean IsGlobal { get; set; }
        public Boolean IsTenantOnly { get; set; }
        public Boolean IsSubTenantOnly { get; set; }
        public Boolean IsActive { get; set; }

    }

    public class SubTenant : Tenant, IMustHaveTenant
    {

        public int TenantId { get; set; }
        public Boolean IsActive { get; set; }
        public virtual Tenant Tenant { get; set; }

    }
    public class SubTenantConfiguration : BaseEntity, IMustHaveSubTenant
    {

        public string ConfigurationType { get; set; }
        public string ConfigurationKey { get; set; }
        public string ConfigurationValue { get; set; }
        public int SubTenantId { get; set; }
        public Boolean IsActive { get; set; }
        public virtual SubTenant SubTenant { get; set; }
    }

    public class Role : BaseEntity, IMustHaveSubTenant
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int SubTenantId { get; set; }
        public Boolean IsActive { get; set; }
        public virtual SubTenant SubTenant { get; set; }

    }
    public partial class AppUser : BaseEntity , IMustHaveSubTenant 
    {
    
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailId { get; set; }
        public string PhoneNum { get; set; }
        public string UserStatus { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; } = false;
        public int? WrongTryCount { get; set; } = 0;
        public DateTime? LastLoggedTime { get; set; }
        public int SubTenantId { get; set; }
        public virtual SubTenant SubTenant { get; set; }
       
    }
    public partial class AppUserRole: BaseEntity
    {

        public virtual AppUser AppUser { get; set; }
        public virtual Role Role { get; set; }
    }


}
