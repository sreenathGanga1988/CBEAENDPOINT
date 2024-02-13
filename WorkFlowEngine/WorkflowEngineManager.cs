using DataAccess.EFCore.UnitOfWorks;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using WorkFlowEngine.DTO;

namespace WorkFlowEngine
{
    //public enum WorkflowStatuses
    //{
    //    Initiated = 'I',
    //    Approved = 'A',
    //    Rejected = 'R',
    //    Completed = 'C',
    //    Open = 'O'

    //}

    //public enum ResultStatusCodes
    //{
    //    UnknownException = 500, NoRecordFound = 632,
    //    Success = 200,
    //    NotAuthorized = 300,
    //    NotApplicable = 301,
    //}
    //public static class WorkflowEngineManager
    //{

    //    private static string yearcode = "22";
    //    public static long IntiateDocumentWorkFlow(int ServiceTypeDocumentID, String currentusername, long currentUserindividualid)
    //    {

    //        var k = GetWorkflowOfServiceType(ServiceTypeDocumentID);
    //        //App.Get().WorkflowDocumentTypeService.Get(u => u.IsActive == true && u.ServiceTypeDocumentID == ServiceTypeDocumentID);


    //        Wf_Documents wf_Documents = new Wf_Documents();
    //        wf_Documents.WorkFlowDocumentTypeID = k.ID;
    //        wf_Documents.Serial = k.Code.ToUpper() + "-" + yearcode + "-" + GetSerial(wf_Documents.WorkFlowDocumentTypeID);
    //       // wf_Documents.is = true;
    //        wf_Documents.CreatedOn = DateTime.Now;
    //        wf_Documents.CreatedBy = currentusername;
    //        wf_Documents.ModifiedOn = DateTime.Now;
    //        wf_Documents.ModifiedBy = currentusername;
    //        wf_Documents.CurrentLevel = 0;
    //        wf_Documents.IsCompleted = false;
    //        wf_Documents.HtmlTemplate = k.HtmlTemplate;
    //        App.Get().WorkflowDocumentService.Add(wf_Documents);
    //        App.Get().WorkflowDocumentService.Commit();
    //        UpdateLog(currentUserindividualid, wf_Documents, WorkflowStatuses.Initiated);

    //        AddDocumentUser(currentUserindividualid, wf_Documents.ID, 0, "O");

    //        var km = App.Get().WorkflowDocumentPatternService.GetMany(u => u.WorkFlowDocumenTypetID == wf_Documents.WorkFlowDocumentTypeID);
    //        if (km.Count() > 0)
    //        {
    //            var m = km.OrderBy(u => u.ActionLevel).FirstOrDefault();
    //            wf_Documents.CurrentLevel = m.ActionLevel;
    //            wf_Documents.Status = Convert.ToChar(WorkflowStatuses.Open).ToString(); ;


    //            foreach (var item in km)
    //            {
    //                long newindividualid = item.IndividualID;
    //                if (item.RoleID != null && item.RoleID != 0)
    //                {
    //                    //    Write the code of finding the individual user
    //                }

    //                AddDocumentUser(newindividualid, wf_Documents.ID, item.ActionLevel, "A", item.ApprovalHTMLEn, item.ApprovalHTMLAr);
    //            }
    //        }
    //        else
    //        {
    //            wf_Documents.Status = Convert.ToChar(WorkflowStatuses.Completed).ToString(); ;
    //            wf_Documents.IsCompleted = true;
    //            UpdateLog(currentUserindividualid, wf_Documents, WorkflowStatuses.Completed);
    //        }
           
    //        App.Get().WorkflowDocumentService.Commit();

    //        return wf_Documents.ID;
    //    }


    //    /// <summary>
    //    /// Returns the Workflow Document type against a service type document
    //    /// </summary>
    //    /// <param name="ServiceTypeDocumentID"></param>
    //    /// <returns></returns>
    //    public static Wf_WorkflowDocumentType GetWorkflowOfServiceType(int ServiceTypeDocumentID)
    //    {
    //        return App.Get().WorkflowDocumentTypeService.Get(u => u.IsActive == true && u.ServiceTypeDocumentID == ServiceTypeDocumentID);
    //    }


    //    /// <summary>
    //    /// Normal Approval call for a Document
    //    /// it check whether the provided user is part of work flow and if yes if the actionlevel of document is same as the actionlevel of user
    //    /// also marks completed if all the action levels are completed
    //    /// </summary>
    //    /// <param name="Individualid"></param>
    //    /// <param name="DocumentID"></param>
    //    /// <param name="Remark"></param>
    //    /// <returns></returns>
    //    public static Boolean Approve(long Individualid, long DocumentID, String Remark = "")
    //    {

    //        Boolean isSuccess = false;
    //        var doc = App.Get().WorkflowDocumentService.GetById(int.Parse(DocumentID.ToString()));


    //        if (doc != null)
    //        {

    //            if (doc.IsCompleted == false)
    //            {

    //                var Userlevel = doc.Wf_DocumentUsers.Where(u => u.IndividualID == Individualid).FirstOrDefault();

    //                if (Userlevel != null)
    //                {
    //                    if (Userlevel.ActionLevel == doc.CurrentLevel)
    //                    {
    //                        if (doc.Wf_WorkflowDocumentType.ApprovalLevel > Userlevel.ActionLevel)
    //                        {
    //                            doc.CurrentLevel++;

    //                        }
    //                        else if (doc.Wf_WorkflowDocumentType.ApprovalLevel == Userlevel.ActionLevel)
    //                        {

    //                            doc.IsCompleted = true;
    //                            doc.Status = Convert.ToChar(WorkflowStatuses.Completed).ToString(); ;

    //                        }


    //                        App.Get().WorkflowDocumentService.Update(doc);
    //                        App.Get().WorkflowDocumentService.Commit();


    //                        var log = UpdateLog(Individualid, doc, WorkflowStatuses.Approved, Remark);
    //                        Userlevel.LastActionLog = log.ID;
    //                        Userlevel.Remark = log.Remark;
    //                        Userlevel.IsMarkCompleted = true;
    //                        App.Get().WorkflowDocumentUserService.Update(Userlevel);
    //                        if (doc.IsCompleted)
    //                        {
    //                            UpdateLog(Individualid, doc, WorkflowStatuses.Completed, "");
    //                        }
    //                        App.Get().WorkflowDocumentService.Commit();
    //                        App.Get().WorkflowDocumentUserService.Commit();
    //                        isSuccess = true;
    //                    }
    //                }


    //            }
    //        }

    //        return isSuccess;
    //    }

    //    /// <summary>
    //    /// Normal Approval call for a Document
    //    /// it check whether the provided user is part of work flow and if yes if the actionlevel of document is same as the actionlevel of user
    //    /// also marks completed if all the action levels are completed
    //    /// </summary>
    //    /// <param name="Individualid"></param>
    //    /// <param name="DocumentID"></param>
    //    /// <param name="Remark"></param>
    //    /// <returns></returns>
    //    public static Boolean Reject(long Individualid, long DocumentID, String Remark = "")
    //    {

    //        Boolean isSuccess = false;
    //        var doc = App.Get().WorkflowDocumentService.GetById(int.Parse(DocumentID.ToString()));


    //        if (doc != null)
    //        {

    //            if (doc.IsCompleted == false)
    //            {

    //                var Userlevel = doc.Wf_DocumentUsers.Where(u => u.IndividualID == Individualid).FirstOrDefault();

    //                if (Userlevel != null)
    //                {
    //                    if (Userlevel.ActionLevel == doc.CurrentLevel)
    //                    {
    //                        if (doc.Wf_WorkflowDocumentType.ApprovalLevel > Userlevel.ActionLevel)
    //                        {
    //                            doc.IsCompleted = true;
    //                            doc.Status = Convert.ToChar(WorkflowStatuses.Rejected).ToString(); ;

    //                        }
    //                        else if (doc.Wf_WorkflowDocumentType.ApprovalLevel == Userlevel.ActionLevel)
    //                        {

    //                            doc.IsCompleted = true;
    //                            doc.Status = Convert.ToChar(WorkflowStatuses.Rejected).ToString(); ;

    //                        }


    //                        App.Get().WorkflowDocumentService.Update(doc);
    //                        App.Get().WorkflowDocumentService.Commit();


    //                        var log = UpdateLog(Individualid, doc, WorkflowStatuses.Rejected, Remark);
    //                        Userlevel.LastActionLog = log.ID;
    //                        Userlevel.Remark = log.Remark;
    //                        Userlevel.IsMarkCompleted = true;
    //                        App.Get().WorkflowDocumentUserService.Update(Userlevel);
    //                        if (doc.IsCompleted)
    //                        {
    //                            UpdateLog(Individualid, doc, WorkflowStatuses.Completed, "");
    //                        }
    //                        App.Get().WorkflowDocumentService.Commit();
    //                        App.Get().WorkflowDocumentUserService.Commit();
    //                        isSuccess = true;
    //                    }
    //                }


    //            }
    //        }

    //        return isSuccess;
    //    }
    //    /// <summary>
    //    /// This method can be used by anyone to randomly approve the Document discarding the hierarchy
    //    /// You must check for Workflow Completion manually
    //    /// </summary>
    //    /// <param name="Individualid"></param>
    //    /// <param name="DocumentID"></param>
    //    /// <param name="Remark"></param>
    //    /// <returns></returns>
    //    public static Boolean RandomApprove(long Individualid, long DocumentID, String Remark = "")
    //    {
    //        Boolean isSuccess = false;
    //        var doc = App.Get().WorkflowDocumentService.GetById(int.Parse(DocumentID.ToString()));
    //        if (doc != null)
    //        {
    //            if (doc.IsCompleted == false)
    //            {
    //                var wf_DocumentUser = doc.Wf_DocumentUsers.Where(u => u.IndividualID == Individualid && u.Type == "A").FirstOrDefault();
    //                if (wf_DocumentUser != null)
    //                {
    //                    var log = UpdateLog(Individualid, doc, WorkflowStatuses.Approved, Remark);
    //                    wf_DocumentUser.LastActionLog = log.ID;
    //                    wf_DocumentUser.Remark = log.Remark;
    //                    wf_DocumentUser.IsMarkCompleted = true;
    //                    App.Get().WorkflowDocumentUserService.Update(wf_DocumentUser);
    //                    App.Get().WorkflowDocumentUserService.Commit();


    //                    if (IsRandomWorkflowCompleted(DocumentID))
    //                    {
    //                        doc.IsCompleted = true;
    //                        doc.Status = Convert.ToChar(WorkflowStatuses.Completed).ToString(); ;
    //                        UpdateLog(Individualid, doc, WorkflowStatuses.Completed, "");
    //                    }
    //                    App.Get().WorkflowDocumentService.Commit();
    //                }
    //            }
    //        }
    //        return isSuccess;
    //    }



    //    public static Boolean IsRandomWorkflowCompleted(long DocumentID)
    //    {
    //        Boolean isCompleted = true;

    //        var wf_DocumentUsers = App.Get().WorkflowDocumentUserService.GetMany(u => u.WorkFlowDocumentID == DocumentID && u.Type == "A");


    //        foreach (var item in wf_DocumentUsers)
    //        {
    //            if (item.IsMarkCompleted != true)
    //            {
    //                isCompleted = false;
    //            }
    //        }


    //        return isCompleted;
    //    }




    //    public static List<WorkflowLogDTO> GetDocumentLog(long DocumentID, bool isRightToLeft)
    //    {
    //        List<WorkflowLogDTO> logs = new List<WorkflowLogDTO>();
    //        var items = App.Get().WorkflowDocumentLogService.GetMany(u => u.WorkFlowDocumentID == DocumentID);
    //        foreach (var item in items)
    //        {


    //            WorkflowLogDTO log = new WorkflowLogDTO();

    //            log.ID = item.ID;
    //            log.Name = isRightToLeft ? item.Crm_Individual.FullArabicName : item.Crm_Individual.FullEnglishName;
    //            log.ActionType = item.ActionType;
    //            log.ActionTime = item.ActionTime.ToString();
    //            log.Remark = item.Remark;
    //            logs.Add(log);
    //        }

    //        return logs;
    //    }


    //    public static Wf_Documents GetDocument(long DocumentID)
    //    {


    //        return App.Get().WorkflowDocumentService.GetById((int)DocumentID);
    //    }




    //    /// <summary>
    //    /// Gets the Pattern details of that document for that user
    //    /// </summary>
    //    /// <param name="DocumentID"></param>
    //    /// <param name="IndividualId"></param>
    //    /// <param name="isRightToLeft"></param>
    //    /// <returns></returns>
    //    public static DocumentPatternDTO GetCurrentWorkflowPatternofDocument(long DocumentID, long IndividualId, bool isRightToLeft)
    //    {

    //        DocumentPatternDTO log = new DocumentPatternDTO();
    //        var doc = App.Get().WorkflowDocumentService.Get(u => u.ID == DocumentID);
    //        var items = App.Get().WorkflowDocumentUserService.GetMany(u => u.WorkFlowDocumentID == DocumentID && u.IndividualID == IndividualId && u.ActionLevel == doc.CurrentLevel).FirstOrDefault();

    //             log.ID = items.ID;
    //        log.IndividualID = items.IndividualID;
    //        log.ApprovalHTML = GetApprovalHtml(doc, isRightToLeft);

    //        return log;
    //    }

    //    public static String GetApprovalHtml(Wf_Documents doc, bool isRightToLeft)
    //    {
    //        var items = App.Get().WorkflowDocumentUserService.GetMany(u => u.WorkFlowDocumentID == doc.ID && u.ActionLevel == doc.CurrentLevel).FirstOrDefault();


    //        return (isRightToLeft ? items.ApprovalHTMLAr : items.ApprovalHTMLEn);

    //    }

    //    public static Wf_DocumentUsers AddDocumentUser(long Individualid, long DocumentID, int ActionLevel, string Type, String ApprovalHTMLEn = "", String ApprovalHTMLAr = "")
    //    {
    //        Wf_DocumentUsers wf_DocumentUser = new Wf_DocumentUsers();
    //        wf_DocumentUser.IndividualID = Individualid;
    //        wf_DocumentUser.WorkFlowDocumentID = DocumentID;
    //        wf_DocumentUser.ActionLevel = ActionLevel;
    //        wf_DocumentUser.IsMarkCompleted = false;
    //        wf_DocumentUser.Type = Type;
    //        wf_DocumentUser.ApprovalHTMLEn = ApprovalHTMLEn;
    //        wf_DocumentUser.ApprovalHTMLAr = ApprovalHTMLAr;
    //        App.Get().WorkflowDocumentUserService.Add(wf_DocumentUser);
    //        return wf_DocumentUser;
    //    }



    //    public static void ChangeDocumentUser(long Individualid, long DocumentID, int ActionLevel)
    //    {


    //        Wf_DocumentUsers wf_DocumentUser = App.Get().WorkflowDocumentUserService.Get(u => u.WorkFlowDocumentID == DocumentID && u.ActionLevel == ActionLevel);
    //        wf_DocumentUser.IndividualID = Individualid;

    //        App.Get().WorkflowDocumentUserService.Update(wf_DocumentUser);
    //    }


    //    public static void ChangeApprovalHTML(long DocumentID, int ActionLevel, String OldText, String NewText, bool isRightToLeft)
    //    {


    //        Wf_DocumentUsers wf_DocumentUser = App.Get().WorkflowDocumentUserService.Get(u => u.WorkFlowDocumentID == DocumentID && u.ActionLevel == ActionLevel);


    //        if (isRightToLeft)
    //        {
    //            wf_DocumentUser.ApprovalHTMLAr = wf_DocumentUser.ApprovalHTMLAr.Replace(OldText, NewText);
    //        }
    //        else
    //        {
    //            wf_DocumentUser.ApprovalHTMLEn = wf_DocumentUser.ApprovalHTMLEn.Replace(OldText, NewText);
    //        }

    //        App.Get().WorkflowDocumentUserService.Update(wf_DocumentUser);
    //    }


    //    public static Wf_DocumentLog UpdateLog(long currentindividualid, Wf_Documents wf_Documents, WorkflowStatuses workflowStatuses, String Remark = "")
    //    {
    //        Wf_DocumentLog wf_DocumentLog = new Wf_DocumentLog();
    //        wf_DocumentLog.ActionTime = DateTime.Now;
    //        wf_DocumentLog.ActionType = Convert.ToChar(workflowStatuses).ToString(); ;
    //        wf_DocumentLog.IndividualID = currentindividualid;
    //        wf_DocumentLog.Remark = Remark;
    //        wf_DocumentLog.WorkFlowDocumentID = wf_Documents.ID;
    //        App.Get().WorkflowDocumentLogService.Add(wf_DocumentLog);
    //        App.Get().WorkflowDocumentLogService.Commit();
    //        return wf_DocumentLog;
    //    }





    //    public static GridResultCollection<InboxItemDTO> GetInBoxofIndividual(bool isRightToLeft, long IndividualID)
    //    {
    //        GridResultCollection<InboxItemDTO> result = new GridResultCollection<InboxItemDTO>();
    //        Crm_Individual crm_Individual = App.Get().Individuals.GetByID(IndividualID);
    //        if (crm_Individual != null)
    //        {
    //            var itemlist = App.Get().WorkflowDocumentUserService.GetAsync(u => u.IndividualID == IndividualID && u.Wf_Documents.Wf_WorkflowDocumentType.IsInbox == true && u.Wf_Documents.IsActive == true);

               
    //            var newitems = itemlist
    //                             .Select(x => new InboxItemDTO
    //                             {
    //                                 ID = x.WorkFlowDocumentID,
    //                                 Serial = x.Wf_Documents.Serial,
    //                                 DocumentTypeID = x.Wf_Documents.WorkFlowDocumentTypeID.ToString(),
    //                                 DocumentTypeName = isRightToLeft ? x.Wf_Documents.Wf_WorkflowDocumentType.Crm_ServiceTypeDocument.NameAR : x.Wf_Documents.Wf_WorkflowDocumentType.Crm_ServiceTypeDocument.NameEN,
    //                                 ActionDate = x.Wf_Documents.ModifiedOn.ToString(),
    //                                 CreatedOn = x.Wf_Documents.CreatedOn.ToString(),
    //                                 CurrentLevel = x.Wf_Documents.CurrentLevel.ToString(),
    //                                 Userlevel = x.ActionLevel.ToString(),
    //                                 IsActive = x.Wf_Documents.IsActive,
    //                                 Status = x.Wf_Documents.Status,
    //                                 IsCompleted = x.Wf_Documents.IsCompleted,
    //                                 IsShowApproveBtn = x.Wf_Documents.Wf_WorkflowDocumentType.IsShowApproveBtn,
    //                                 IsShowRejectBtn = x.Wf_Documents.Wf_WorkflowDocumentType.IsShowRejectBtn,
    //                                 IsShowViewBtn = x.Wf_Documents.Wf_WorkflowDocumentType.IsShowViewBtn,
    //                             }).ToList();

    //            foreach (var item in newitems)
    //            {
    //                item.Actions = GetActionButton(item.CurrentLevel.ToInt32(), item.Userlevel.ToInt32(), item.ID, item.IsCompleted, "", item.IsShowApproveBtn, item.IsShowViewBtn, item.IsShowRejectBtn);
    //                item.Status = GetStatus(item.CurrentLevel.ToInt32(), item.ID, item.IsCompleted, item.Status, isRightToLeft, ci);
    //            }

    //            result.rows = newitems;
    //            result.code = (int)ResultStatusCodes.Success;
    //        }

    //        return result;
    //    }






    //    public static String GetActionButton(int Currentlevel, int Userlevel, long DocumentID, Boolean iscompleted, string lnk, bool IsShowApproveBtn = false, bool IsShowViewBtn = false, bool IsShowRejectBtn = false)
    //    {
    //        String ActionButton = "<div class='btn-group'>";

    //        String ApproveBtn = "<a class='btn waves-effect waves-light btn-success btnapprove' data-toggle='tooltip' data-original-title='Approve' ><i class='fas fa-check'></i></a>";

    //        String ViewButton = "<a class='btn waves-effect waves-light btn-info btnreject' data-toggle='tooltip' data-original-title='View' ><i class='fas fa-eye'></i></a>";

    //        String RejectButton = "<a class='btn waves-effect waves-light btn-danger btnView' data-toggle='tooltip' data-original-title='Reject'><i class='fas fa-ban'></i></a>";

    //        if (iscompleted)
    //        {
    //            ActionButton = ActionButton + ViewButton;
    //        }
    //        else
    //        {
    //            if (Currentlevel == Userlevel)
    //            {
    //                if (IsShowApproveBtn && IsShowViewBtn && IsShowRejectBtn)
    //                {
    //                    ActionButton = ActionButton + ApproveBtn + ViewButton + RejectButton;
    //                }
    //                if (IsShowApproveBtn && IsShowViewBtn && !IsShowRejectBtn)
    //                {
    //                    ActionButton = ActionButton + ApproveBtn + ViewButton;
    //                }
    //                if (IsShowApproveBtn && IsShowRejectBtn && !IsShowViewBtn)
    //                {
    //                    ActionButton = ActionButton + ApproveBtn + RejectButton;
    //                }

    //            }
    //        }

    //        ActionButton = ActionButton + "</div>";
    //        return ActionButton;
    //    }


    //    public static String GetStatus(int Currentlevel, long DocumentID, Boolean iscompleted, string status, bool isRightToLeft)
    //    {
    //        String ActionButton = "<div class='btn-group'>";

    //        if (iscompleted)
    //        {
    //            return string.Format("<span class=' workflowbtn text-white label label-success'>{0}</span>", "Completed");
    //            return "Completed";
    //        }
    //        else
    //        {
    //            if (status.ToUpper().Trim() == "C")
    //            {
    //                return string.Format("<span class='workflowbtn text-white label label-success'>{0}</span>", "Completed");

    //            }
    //            if (status.ToUpper().Trim() == "O")
    //            {



    //                var user = App.Get().WorkflowDocumentUserService.Get(u => u.WorkFlowDocumentID == DocumentID && u.ActionLevel == Currentlevel);
    //                if (user != null)
    //                {
    //                    var username = isRightToLeft ? user.Crm_Individual.FullArabicName : user.Crm_Individual.FullEnglishName;

    //                    var pending = "Pending With"+ " " + username;

    //                    return string.Format("<span class='workflowbtn text-white label label-warning'>{0}</span>", pending);
    //                }


    //            }
    //        }

    //        ActionButton = ActionButton + "</div>";
    //        return ActionButton;
    //    }


    //    public static String GetRandomWorkflowStatus(long DocumentID, Boolean iscompleted, string status, bool isRightToLeft)
    //    {
    //        String ActionButton = "<div class='btn-group'>";

    //        if (iscompleted)
    //        {
    //            return string.Format("<span class=' workflowbtn text-white label label-success'>{0}</span>", "Completed");
    //            return "Completed";
    //        }
    //        else
    //        {
    //            if (status.ToUpper().Trim() == "C")
    //            {
    //                return string.Format("<span class='workflowbtn text-white label label-success'>{0}</span>", "Completed");

    //            }
    //            if (status.ToUpper().Trim() == "O")
    //            {



    //                var users = App.Get().WorkflowDocumentUserService.GetMany(u => u.WorkFlowDocumentID == DocumentID && u.IsMarkCompleted == false);
    //                if (users != null)
    //                {
    //                    var username = "";


    //                    foreach (var item in users)
    //                    {
    //                        if (username.Trim().Length > 0)
    //                        {
    //                            username = username + " , ";
    //                        }
    //                        username = username + (isRightToLeft ? item.Crm_Individual.FullArabicName : item.Crm_Individual.FullEnglishName);
    //                    }

    //                    var pending = "Pending With" + " " + username;

    //                    return string.Format("<span class='workflowbtn text-white label label-warning'>{0}</span>", pending);
    //                }


    //            }
    //        }

    //        ActionButton = ActionButton + "</div>";
    //        return ActionButton;
    //    }






    //    public static String GetStatusOfDocumentID(long DocumentID, bool isRightToLeft)
    //    {


    //        Wf_Documents wf_Documents = App.Get().WorkflowDocumentService.GetById((int)DocumentID);



    //        return GetStatus(wf_Documents.CurrentLevel, wf_Documents.ID, wf_Documents.IsCompleted, wf_Documents.Status, isRightToLeft);


    //    }
    //    public static String GetStatusOfRandomWorkflowDocumentID(long DocumentID, bool isRightToLeft)
    //    {


    //        Wf_Documents wf_Documents = App.Get().WorkflowDocumentService.GetById((int)DocumentID);



    //        return GetRandomWorkflowStatus(wf_Documents.ID, wf_Documents.IsCompleted, wf_Documents.Status, isRightToLeft);


    //    }
    //    public static String GetSerial(long? id)
    //    {
    //        var k = App.Get().WorkflowDocumentService.DataCount(u => u.WorkFlowDocumentTypeID == id);
    //        return k.ToString();
    //    }

    //}
}
