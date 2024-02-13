var formajaxurl = "/ControlPanel/APIs/CRM/Staff/HrForms.asmx";
var ajaxVehiclesUrl = "/ControlPanel/APIs/CRM/Staff/Vehicles.asmx";

//alert
//AfterApprovalAction(null);

function AfterApprovalAction(objDcoument) {
    if (objDcoument.TypeCode == "INTR" && objDcoument.IsCompleted == true && objDcoument.Status == "C") {

        StartInterviewAppraisal(objDcoument.ID);

    }
    if (objDcoument.TypeCode == "CHCH" && objDcoument.IsCompleted == true && objDcoument.Status == "C") {
        UpdateVehicleChassisNo(objDcoument.ID);

    }
}

function AfterRejectAction(objDcoument) {
    if (objDcoument.TypeCode == "INTR" && objDcoument.IsCompleted == true && objDcoument.Status == "R") {

        //  StartInterviewAppraisal(objDcoument.ID);

    }
    if (objDcoument.TypeCode == "CHCH" && objDcoument.IsCompleted == true && objDcoument.Status == "R") {
        // UpdateVehicleChassisNo(objDcoument.ID);

    }
}


function UpdateVehicleChassisNo(DocumentID) {
    var formData = new FormData();
    formData.append("WorkFlowDocumentID", DocumentID);

    $.ajax({
        url: ajaxVehiclesUrl + "/CompleteVehicleUpdateRequest",
        type: 'post',
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.Code === 200) {
                Utils.Alert(res.SuccessMessage, '', 'success');


            }
            else {
                Utils.Alert(res.Ooops, res.OoopsSomethingWrong, 'error');
            }
        },
        error: function (response) {
            Utils.Alert(res.Ooops, res.OoopsSomethingWrong, 'error');
        },
        complete: function () {
            $(".preloader").hide();
        }
    });
}
function StartInterviewAppraisal(DocumentID) {

    var formData = new FormData();
    formData.append("DocumentID", DocumentID);

    $.ajax({
        url: formajaxurl + "/SubmitInterviewAppraisalRequest",
        type: 'post',
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.Code === 200) {
                Utils.Alert(res.SuccessMessage, '', 'success');


            }
            else {
                Utils.Alert(res.Ooops, res.OoopsSomethingWrong, 'error');
            }
        },
        error: function (response) {
            Utils.Alert(res.Ooops, res.OoopsSomethingWrong, 'error');
        },
        complete: function () {
            $(".preloader").hide();
        }
    });




}


