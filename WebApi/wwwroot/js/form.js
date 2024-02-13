$(".myCheckBox").on('change', function () {
    if ($(this).is(':checked')) {
        $(this).attr('value', true);
        alert("true");
    } else {
        $(this).attr('value', false);
    }

    
});

$(document).on('change', '.myCheckBox', function () {
    if ($(this).is(':checked')) {
        $(this).attr('value', 'true');
        alert("true");
    } else {
        $(this).attr('value', 'false');
    }


});


function MakeSelect2(controlid, placeholder, url, multiple, listtype, minimumResultsForSearch, dependentvalue) {
    $(controlid).select2({
        placeholder: placeholder,
        allowClear: true,
        //width: 'inherit',
        escapeMarkup: function (text) { return text; },
        multiple: multiple,
        minimumInputLength: 0,
        minimumResultsForSearch: minimumResultsForSearch,
        ajax: {
            url: url,
            dataType: 'json',
            quietMillis: 0,
            cache: false,
            type: "POST",
            data: function (term, page) {
                return {
                    q: term,
                    page: page,
                    listType: listtype,
                    dependentvalue: eval(dependentvalue)
                };
            },
            processResults: function (data, page) {
                var more = (page * 30) < data.total_count; // whether or not there are more results available
                return {
                    results: $.map(data.items, function (item) {
                        return {
                            text: item.text,
                            id: item.id
                        }
                    }),
                    more: more
                }
            },
            cache: true
        }
    }).on("change", function () {
        $(controlid).trigger("chosen:updated");
    });
}


function GetAjaxData(myurl, parameter, ActionType) {
    $.ajax({
        type: 'GET',
        url: myurl,
        data: parameter,
        dataType: 'json',
        cache: false,
        success: function (data1) {
            LoadData(data1, ActionType);
        },
        error: function (error) {

        }
    });
}


function GetUserFullNames() {
    let VW_CreatedByUserId = $("#VW_CreatedByUserId").val()
    //moment(String(this.dataModel.mydate)).format('Do MMMM YYYY, h:mm:ss a')
    if ($("#VW_CreatedByUserId").val()) {
        SetNameValue($("#VW_CreatedByUserId").val(), "#VW_CreatedbyUserName")
    }
    if ($("#VW_ModifiedByUserId").val()) {
        SetNameValue($("#VW_ModifiedByUserId").val(), "#VW_ModifiedbyUserName")
    }
    if ($("#VW_DeletedByByUserId").val()) {
        SetNameValue($("#VW_DeletedByByUserId").val(), "#DeletedbyUserName")
    }
}

function SetNameValue(userid, controlid) {
    $.ajax({
        type: 'GET',
        url: "/api/User/" + userid,
        dataType: 'json',
        cache: false,
        success: function (data1) {
            $(controlid).html(data1.userName);
        },
        error: function (error) {

        }
    });
}