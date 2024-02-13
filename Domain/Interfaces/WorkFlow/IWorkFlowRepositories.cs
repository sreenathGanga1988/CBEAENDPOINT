using Domain.Entities;
using Domain.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Domain.Interfaces
{


    public interface IWorkflowDocumentTypeRepository : IGenericRepository<Wf_WorkflowDocumentType> { }
    public interface IWorkFlowDocumentPatternRepository : IGenericRepository<Wf_WorkFlowDocumentPattern> { }
    public interface IWokflowDocumentsRepository : IGenericRepository<Wf_Documents> { }
    public interface IWokflowDocumentLogRepository : IGenericRepository<Wf_DocumentLog> { }
    public interface IWokflowDocumentUsersRepository : IGenericRepository<Wf_DocumentUsers> { }



}