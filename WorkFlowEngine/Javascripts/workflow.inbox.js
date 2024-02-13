define(['jquery', "i18n!nls/resources", 'utils', 'sweetalert', 'datepicker', 'clockpicker', 'dropify'], function ($, res) {
    var con = $('.listing');
    var ajaxUrl = "/ControlPanel/APIs/CRM/Staff/WorkflowEngine.asmx";


    var WorkFlowActionModal = $('#TransactionCaseAction');
    var WorkflowPopupModal = $('#WorkflowPopupModal');


    WorkflowPopupModal.find('.btn-close').click(function () {
        WorkflowPopupModal.modal('hide');
    });

    WorkFlowActionModal.find('.btn-close').click(function () {
        WorkFlowActionModal.modal('hide');
    });
    WorkFlowActionModal.on('shown.bs.modal', function (e) {
        let DocumentID = WorkFlowActionModal.attr("data-id");
        GetCurrentLevelPatternOfDocument(DocumentID)
    })
    WorkFlowActionModal.find('.btn-Approve').click(function () {
        if (Utils.validator(WorkFlowActionModal)) {
            let Reason = $("#txt-notes").val();
            let DocumentID = WorkFlowActionModal.attr("data-id");
            Approve(DocumentID, Reason);
        }
        else {
        }
    });
    WorkFlowActionModal.find('.btn-Reject').click(function () {
        if (Utils.validator(WorkFlowActionModal)) {
            let Reason = $("#txt-notes").val();
            let DocumentID = WorkFlowActionModal.attr("data-id");
            Reject(DocumentID, Reason);
        }
        else {
        }
    });
    WorkflowPopupModal.on('shown.bs.modal', function (e) {
        let DocumentID = WorkflowPopupModal.attr("data-id");
        GetLogs(DocumentID)
    });

    window.operateEvents = {
        'click .btnapprove': function (event, value, row, index) {
            Utils.ConfirmAlert(function () {
                $("#hd_Reject").hide();
                $("#hd_ApproveItem").show();
                $("#btn-Reject").hide();
                $("#btn-Approve").show();
                WorkFlowActionModal.attr('data-id', row.ID);
                WorkFlowActionModal.attr('data-actionType', 'Approve');
                WorkFlowActionModal.modal({ show: true });
            });
        },

        'click .btnreject': function (event, value, row, index) {
            Utils.ConfirmAlert(function () {
                $("#hd_Reject").show();
                $("#btn-Reject").show();
                $("#hd_ApproveItem").hide();

                $("#btn-Approve").hide();
                WorkFlowActionModal.attr('data-id', row.ID);
                WorkFlowActionModal.attr('data-actionType', 'Reject');
                WorkFlowActionModal.modal({ show: true });
            });
        },
        'click .btnView': function (event, value, row, index) {
            alert("view");
        },

        'click .workflowbtn': function (event, value, row, index) {
            $("#hd_workflowname").text(row.Serial);
            WorkflowPopupModal.attr('data-id', row.ID);
            WorkflowPopupModal.modal({ show: true });
        }
    };

    function Approve(DocumentID, Remark) {
        let IndividualID = $("#IndividualID").val();
        var ajaxCallParams = {};
        var ajaxDataParams = {};
        ajaxCallParams.Type = "POST";
        ajaxCallParams.Url = ajaxUrl + "/Approve";
        ajaxDataParams.id = IndividualID;
        ajaxDataParams.DocumentID = DocumentID;
        ajaxDataParams.Remark = Remark;
        Utils.ajaxCall(ajaxCallParams, ajaxDataParams, function (result) {
            if (result.d.Code == 200) {
                con.find('#list').bootstrapTable('refresh');
                AfterApprovalAction(result.d.Value);
                Utils.AlertWithRedirect(res.SuccessMessage, '', 'success', window.location.href);
            }
            else {
                Utils.Alert('error', '<%=Resources.CP.Message_SomethingWrong%>', '<%=Resources.CP.lbl_Oops%>');
            }
        });
    }


    function Reject(DocumentID, Remark) {
        let IndividualID = $("#IndividualID").val();
        var ajaxCallParams = {};
        var ajaxDataParams = {};
        ajaxCallParams.Type = "POST";
        ajaxCallParams.Url = ajaxUrl + "/Reject";
        ajaxDataParams.id = IndividualID;
        ajaxDataParams.DocumentID = DocumentID;
        ajaxDataParams.Remark = Remark;
        Utils.ajaxCall(ajaxCallParams, ajaxDataParams, function (result) {
            if (result.d.Code == 200) {
                con.find('#list').bootstrapTable('refresh');
                AfterRejectAction(result.d.Value);
                Utils.AlertWithRedirect(res.SuccessMessage, '', 'success', window.location.href);
            }
            else {
                Utils.Alert('error', '<%=Resources.CP.Message_SomethingWrong%>', '<%=Resources.CP.lbl_Oops%>');
            }
        });
    }






    function GetCurrentLevelPatternOfDocument(DocumentID) {

        var ajaxCallParams = {};
        var ajaxDataParams = {};
        ajaxCallParams.Type = "POST";
        ajaxCallParams.Url = ajaxUrl + "/GetCurrentLevelPatternOfDocument";
        ajaxDataParams.DocID = DocumentID;
        ajaxDataParams.isRightToLeft = AT.isRightToLeft;
        ajaxDataParams.languageCode = AT.languageCode;
        Utils.ajaxCall(ajaxCallParams, ajaxDataParams, function (result) {
            if (result.d.Code === 200) {

                $("#spnApprovalHTML").html(result.d.Value.ApprovalHTML);



            }
            else {
                Utils.Alert(res.Ooops, res.OoopsSomethingWrong, 'error');
            }
        });


    }
    function GetLogs(DocumentID) {
        var ajaxCallParams = {};
        var ajaxDataParams = {};
        ajaxCallParams.Type = "POST";
        ajaxCallParams.Url = ajaxUrl + "/GetWorkflowLogs";
        ajaxDataParams.id = DocumentID;
        ajaxDataParams.isRightToLeft = AT.isRightToLeft;
        ajaxDataParams.languageCode = AT.languageCode;
        Utils.ajaxCall(ajaxCallParams, ajaxDataParams, function (result) {
            if (result.d.Code === 200) {
                if (result.d.Value.length > 0) {
                    var html = "";

                    $.each(result.d.Value, function (index, row) {
                        html += "<tr><td class='text-center  align-middle p-2'>" + (parseInt(index) + 1) + "</td><td class='text-center align-middle p-2'>" + row.Name + "</td></td><td class='text-center align-middle p-2'>" + row.ActionTime + "</td><td class='text-center align-middle p-2'>" + row.ActionType + "</td><td class='text-center align-middle p-2'>" + row.Remark + "</td></tr>";
                    });

                    WorkflowPopupModal.find('tbody').html(html);
                }


            }
            else {
                Utils.Alert(res.Ooops, res.OoopsSomethingWrong, 'error');
            }
        });
    }

    function GetEmployeeDocuments() {
    }






});