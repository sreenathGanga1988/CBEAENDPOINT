
function LoadData() {

    console.log(dataModels);

    var createlink = document.getElementById("addbtn");
    createlink.setAttribute("href", CreateUrl(AddAction));

    let headerrow = document.getElementById('Kidu_tdheader');
    let ColumnHeaderCount = ColumnHeader.length;
    let Allth = "";
    for (let i = 0; i < ColumnHeaderCount; i++) {
        let th = '<th>' + ColumnHeader[i].toString(); + '</th>';
        Allth = Allth + th;
    }
    Allth = Allth + '<th>Action </th>';
    headerrow.innerHTML = Allth;
    let Kidu_tbody = document.getElementById('Kidu_tbody');
    let rowhtml = ""
    let rowCount = dataModels.length;
    for (let i = 0; i < rowCount; i++) {
        rowhtml = rowhtml + '<tr>';
        let colCount = Columns.length;
        let Alltd = "";
        for (let j = 0; j < colCount; j++) {
            let colname =Columns[j].toString();
          

            let colvalue = dataModels[i][colname];
            if (isDate(colvalue))
            {
                colvalue = moment(new Date(colvalue)).format("DD/MMM/YYYY");
            } 

            if (colname.toUpperCase() == 'ISACTIVE') {

                if (colvalue === true) {
                    colvalue = ' <span class="badge badge-success">Active</span>';
                } else {
                    colvalue = ' <span class="badge badge-danger">In Active</span>';
                }
               
            }


            let td = '<td>' + colvalue + '</td>';
            Alltd = Alltd + td;
        }
        let edittd = ""
        if (EditAction.trim().length > 0) {         
        
            let editurl = CreateUrl(EditAction) + dataModels[i][IdColumn.toString()];
            edittd = '<a  a href="' + editurl +  '" style="padding:10px"> <i class="material-icons" style="color :blue">edit</i> </a>';
        }
        let viewtd = "";
        if (ViewAction.trim().length > 0) {
           
            let viewurl = CreateUrl(ViewAction) + dataModels[i][IdColumn.toString()];
            viewtd = '<a a href="' + viewurl + '" style="padding:10px"><i class="material-icons" style="color :blue">preview</i> </a>';
        }

        let deltd = "";
        if (DeleteAction.trim().length > 0) {
           
            let delurl = CreateUrl(DeleteAction) + dataModels[i][IdColumn.toString()];
            deltd = '<a a href="' + delurl + '" style="padding:10px"><i class="material-icons" style="color :red">delete</i> </a>';
        }


        if (edittd.trim().length > 0 || viewtd.trim().length > 0 || deltd.trim().length > 0) {
            let actiontd = '<td>' + viewtd + edittd + deltd + '</td>';
            Alltd = Alltd + actiontd;
        }
        else {
            let actiontd = '<td></td>';
            Alltd = Alltd + actiontd;
        }
        rowhtml = rowhtml + Alltd     ;
        rowhtml = rowhtml + "</tr>";

    }

    Kidu_tbody.innerHTML = rowhtml;

};


function CreateUrl(baseurl) {
    var lastChar = baseurl.substr(-1); // Selects the last character
    if (lastChar != '/') {         // If the last character is not a slash
        baseurl = baseurl + '/';            // Append a slash to it.
    }
        return baseurl;
}

//function isDate(dateStr) {
//    return !isNaN(Date.parse(dateStr));
//}

function isDate(_date){
    const _regExp = new RegExp('^(-?(?:[1-9][0-9]*)?[0-9]{4})-(1[0-2]|0[1-9])-(3[01]|0[1-9]|[12][0-9])T(2[0-3]|[01][0-9]):([0-5][0-9]):([0-5][0-9])(.[0-9]+)?(Z)?$');
    return _regExp.test(_date);
}

function MarkasDatatable(elementid) {

    $('.KiduDatatables').dataTable({

        dom: 'Bfrtip',
        buttons: [
            'pageLength', 'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });

};