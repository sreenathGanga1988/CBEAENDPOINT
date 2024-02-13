using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Domain;
using System.Data;
using Dapper;

namespace DataAccess.EFCore.Repositories
{

   
   

    public class WorkflowDocumentTypeRepository : GenericRepository<Wf_WorkflowDocumentType>, IWorkflowDocumentTypeRepository
    {
        public WorkflowDocumentTypeRepository(ApplicationContext context) : base(context)
        {
        }
    }
    public class WorkFlowDocumentPatternRepository : GenericRepository<Wf_WorkFlowDocumentPattern>, IWorkFlowDocumentPatternRepository
    {
        public WorkFlowDocumentPatternRepository(ApplicationContext context) : base(context)
        {
        }
    }
    public class WokflowDocumentsRepository : GenericRepository<Wf_Documents>, IWokflowDocumentsRepository
    {
        public WokflowDocumentsRepository(ApplicationContext context) : base(context)
        {
        }

    }
    public class WokflowDocumentLogRepository : GenericRepository<Wf_DocumentLog>, IWokflowDocumentLogRepository
    {
        public WokflowDocumentLogRepository(ApplicationContext context) : base(context)
        {
        }
    }
    public class WokflowDocumentUsersRepository : GenericRepository<Wf_DocumentUsers>, IWokflowDocumentUsersRepository
    {
        public WokflowDocumentUsersRepository(ApplicationContext context) : base(context)
        {
        }
    }

}
