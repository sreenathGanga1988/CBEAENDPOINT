
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Web;
//using WorkFlowEngine;
//using WorkFlowEngine.DTO;
//public class ApiResult
//{
//    public object Value { get; set; }
//    public string ValueOne { get; set; }
//    public string ValueTwo { get; set; }
//    public string Error { get; set; }
//    public decimal Total { get; set; }
//    public int Code { get; set; }
//}
//public class WorkflowEngine 
//{
    

//    public WorkflowEngine()
//    {

//        //Uncomment the following line if using designed components 
//        //InitializeComponent(); 
//    }



   
   
//    public GridResultCollection<InboxItemDTO> LoadInbox(int offset, int limit, string sort, string order, string searchOptions, string lnk, int lang, string languageCode, bool isRightToLeft)
//    {
//        var ci = new CultureInfo(languageCode);
//        GridResultCollection<InboxItemDTO> result = new GridResultCollection<InboxItemDTO>();
//        try
//        {
//        //    SearchHelper search = SearchHelper.Create(searchOptions);

//            List<InboxItemDTO> inboxItems = new List<InboxItemDTO>();
//         //   PagingParam pagingParam = new PagingParam(offset, limit, sort, order);
//            // var items = App.Get().Individuals.EmployeesInfo(ref pagingParam, keywords, section, null, employees, nationality);

//            long id = App.Get().Users.GetCurrentUserProfile().ID;

//            result = WorkflowEngineManager.GetInBoxofIndividual(isRightToLeft, id);
//        }
//        catch (Exception ex)
//        {
//           // ErrorSignal.FromContext(Context).Raise(ex);
//        }
//        return result;
//    }






//    public ApiResult Approve(long id, long DocumentID, String Remark)
//    {
//        var result = new ApiResult();
//        try
//        {

//            var doc = WorkflowEngineManager.GetDocument(DocumentID);

//            if(doc != null)
//            {
//                Boolean sucess = WorkflowEngineManager.Approve(id, DocumentID, Remark);


              


//                if (sucess == true)
//                {
//                    result.Code = (int)ResultStatusCodes.Success;
//                }

//                else
//                {
//                    result.Code = (int)ResultStatusCodes.UnknownException;
//                }

//                var item = WorkflowEngineManager.GetDocument(DocumentID);

//                result.Value = new { ID = item.ID, TypeCode = item.Wf_WorkflowDocumentType.Code , CurrentLevel = item.CurrentLevel, IsActive = item.IsActive, IsCompleted = item.IsCompleted, Status = item.Status };
//            }
//            else
//            {
//                result.Code = (int)ResultStatusCodes.UnknownException;
//            }           


//        }
//        catch (Exception ex)
//        {
//            result.Code = (int)ResultStatusCodes.UnknownException;
//          //  ErrorSignal.FromContext(Context).Raise(ex);
//        }
//        return result;
//    }



//    public ApiResult Reject(long id, long DocumentID, String Remark)
//    {
//        var result = new ApiResult();
//        try
//        {

//            var doc = WorkflowEngineManager.GetDocument(DocumentID);

//            if (doc != null)
//            {
//                Boolean sucess = WorkflowEngineManager.Reject(id, DocumentID, Remark);





//                if (sucess == true)
//                {
//                    result.Code = (int)ResultStatusCodes.Success;
//                }

//                else
//                {
//                    result.Code = (int)ResultStatusCodes.UnknownException;
//                }

//                var item = WorkflowEngineManager.GetDocument(DocumentID);

//                result.Value = new { ID = item.ID, TypeCode = item.Wf_WorkflowDocumentType.Code, CurrentLevel = item.CurrentLevel, IsActive = item.IsActive, IsCompleted = item.IsCompleted, Status = item.Status };
//            }
//            else
//            {
//                result.Code = (int)ResultStatusCodes.UnknownException;
//            }


//        }
//        catch (Exception ex)
//        {
//            result.Code = (int)ResultStatusCodes.UnknownException;
//         //   ErrorSignal.FromContext(Context).Raise(ex);
//        }
//        return result;
//    }









    
//    public ApiResult GetWorkflowLogs(int id, bool isRightToLeft, string languageCode)
//    {
//        var ci = new CultureInfo(languageCode);
//        var result = new ApiResult();

//        try
//        {
//            var items = WorkflowEngineManager.GetDocumentLog(id, isRightToLeft);



//            if (items != null)
//            {


//                result.Code = (int)ResultStatusCodes.Success;
//                result.Value = items;
//            }
//            else
//            {
//                result.Code = (int)ResultStatusCodes.NoRecordFound;
//                result.Value = 0;
//            }
//        }
//        catch (Exception ex)
//        {
//            result.Code = (int)ResultStatusCodes.UnknownException;
//            result.Value = ex.InnerException.ToString();
//          //  ErrorSignal.FromContext(Context).Raise(ex);
//        }
//        return result;
//    }



//    /// <summary>
//    /// Gets the current workflowpattern of a Document
//    /// </summary>
//    /// <param name="id"></param>
//    /// <param name="isRightToLeft"></param>
//    /// <param name="languageCode"></param>
//    /// <returns></returns>
   
//    public ApiResult GetCurrentLevelPatternOfDocument(int DocID, bool isRightToLeft, string languageCode)
//    {
//        var ci = new CultureInfo(languageCode);
//        var result = new ApiResult();

//        try
//        {
//            long IndividualID = App.Get().Users.GetCurrentUserProfile().ID;

//            var items = WorkflowEngineManager.GetCurrentWorkflowPatternofDocument (DocID, IndividualID,isRightToLeft);
//            if (items != null)
//            {
//                result.Code = (int)ResultStatusCodes.Success;
//                result.Value = items;
//            }
//            else
//            {
//                result.Code = (int)ResultStatusCodes.NoRecordFound;
//                result.Value = 0;
//            }
//        }
//        catch (Exception ex)
//        {
//            result.Code = (int)ResultStatusCodes.UnknownException;
//            result.Value = ex.InnerException.ToString();
//         //   ErrorSignal.FromContext(Context).Raise(ex);
//        }
//        return result;
//    }


   
//    public ApiResult GetWorkflowOfServiceType(int id)
//    {
     
//        var result = new ApiResult();

//        try
//        {
            

//            var items = WorkflowEngineManager.GetWorkflowOfServiceType(id);
//            if (items != null)
//            {
//                result.Code = (int)ResultStatusCodes.Success;
//                result.Value = new {code =items.Code  };
//            }
//            else
//            {
//                result.Code = (int)ResultStatusCodes.NoRecordFound;
//                result.Value = 0;
//            }
//        }
//        catch (Exception ex)
//        {
//            result.Code = (int)ResultStatusCodes.UnknownException;
//            result.Value = ex.InnerException.ToString();
//           // ErrorSignal.FromContext(Context).Raise(ex);
//        }
//        return result;
//    }



//}
