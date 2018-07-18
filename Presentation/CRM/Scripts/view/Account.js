
var NomeTabela

function pageSetUpTable(i)
{
    alert(i);
    NomeTabela = i;
}



/* // DOM Position key index //

l - Length changing (dropdown)
f - Filtering input (search)
t - The Table! (datatable)
i - Information (records)
p - Pagination (paging)
r - pRocessing
< and > - div elements
<"#id" and > - div with an id
<"class" and > - div with a class
<"#id.class" and > - div with an id and class

Also see: http://legacy.datatables.net/usage/features
*/

var responsiveHelper_datatable_col_reorder = undefined;

var breakpointDefinition = {
    tablet: 1024,
    phone: 480
};


var url = "/Account/ReadAccountJs";

/* COLUMN SHOW - HIDE */
$('#datatable_col_reorder').dataTable({

    // Tabletools options:
    //   https://datatables.net/extensions/tabletools/button_options
    "sDom": "<'dt-toolbar'<'col-xs-12 col-sm-6'f><'col-sm-6 col-xs-6 hidden-xs'TC>r>" +
            "t" +
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-sm-6 col-xs-12'p>>",
    "oTableTools": {
        "aButtons": [
        "copy",
        "csv",
           {
               "sExtends": "print",
               "sMessage": "Generated by CRM <i>(press Esc to close)</i>"
           }
        ],
        "sSwfPath": "/Scripts/plugin/datatables/swf/copy_csv_xls_pdf.swf"
    },
    "autoWidth": true,
    "preDrawCallback": function () {
        // Initialize the responsive datatables helper once.
        if (!responsiveHelper_datatable_col_reorder) {
            responsiveHelper_datatable_col_reorder = new ResponsiveDatatablesHelper($('#datatable_col_reorder'), breakpointDefinition);
        }
    },
    "rowCallback": function (nRow) {
        responsiveHelper_datatable_col_reorder.createExpandIcon(nRow);
    },
    "drawCallback": function (oSettings) {
        responsiveHelper_datatable_col_reorder.respond();
    },
    "bServerSide": true,
    "sAjaxSource": url,
    //"bProcessing": true,
    "pageLength": 10,
    "aoColumns": [
                    //{
                    //    "sName": "View",
                    //    "sWidth": 30
                    //               ,
                    //    "mRender": function (data, type, full) {
                    //        return '<a class=\"btn btn-warning btn-xs\" href=\"/Cadastros/Cadastro/Edit/' + full[1] + '\">' + "Editar" + '</a>';
                    //    }
                    //},
                   { "sName": "cod_pessoa" },
                   { "sName": "des_pessoa", "mRender": function (data, type, full) { return '<a href=\"/Account/ViewProfile/' + full[0] + '\">' + full[1] + '</a>'; } },
                   { "sName": "des_represetante"},
                   { "sName": "tel_contato" },
                   //{ "sName": "des_email_cli", "sWidth": 110, },
                   { "sName": "dta_ult_compra" },
                   { "sName": "_daysNotSale" },
                   { "sName": "vlr_faturamento  " }

    ]
});
