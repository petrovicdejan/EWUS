﻿(function () {
    var colModel = [
        {
            label: 'Id',
            name: 'Id',
            width: 7,
            editable: true,
            hidden: true
        },
        {
            label: 'Benennung',
            name: 'Name',
            width: 23,
            editable: true,
            searchoptions: {
                sopt: ['cn', "ge", "le", "eq", 'bw'],
            }
        },
        {
            label: 'Laufende-Nr',
            name: 'SerialNumber',
            width: 7,
            key: true,
            editable: true,
            searchoptions: {
                sopt: ['cn', "ge", "le", "eq", 'bw'], 
            }
        },
        {
            label: 'Massnahmenart',
            name: 'OperationType',
            width: 12,
            editable: true,
            searchoptions: {
                sopt: ['cn', "ge", "le", "eq", 'bw'], 
            }
        },
        {
            label: 'Investitionskosten [€]',
            name: 'InvestmentCost',
            width: 10,
            editable: true,
            searchoptions: {
                sopt: ['cn', "ge", "le", "eq", 'bw'], 
            }
        },
        {
            label: '',
            name: '',
            width: 15,
            formatter: function (cellvalue, options, rowObject) {
                return '<a href="#" class="btn btn-info btn-xs" onclick="publicApp.openModalForm(this)" data-url="/Measure/MeasureEdit?key=' + rowObject.Id + '"><i class="fa fa-pencil"></i> Bearbeiten </a>';
            },
            editable: false,
            search: false
        },
    ];

    setGridOptions.setUpGrid("jqGrid", "jqGridPager", colModel, 1500, 350, 15, fetchGridData);

    function fetchGridData() {

        setGridOptions.deleteRows('jqGrid');

        var data = [];
        var url = sRootUrl + 'Measure/GetMeasures';

        publicApp.getWebApi(url, function returnText(rData) {

            $.each(rData, function (inx, item) {
                var row = new Object();
                row.Id = item.Id;
                row.Name = item.Name;
                row.SerialNumber = item.SerialNumber;
                row.InvestmentCost = item.InvestmentCost;
                row.Saving = item.Saving;

                if (!IsNullOrUndefined(item.OperationType)) {
                    row.OperationType = item.OperationType.Name;
                }

                data.push(row);
            });
            
        }, false, false);    
        
        $('#jqGrid').jqGrid('setGridParam', { data: data }).trigger('reloadGrid');

        $("#rowsNumber").text('Number of rows: ' + $('#jqGrid').getGridParam("reccount"));

    }

    function getDataForGrid() {
        var colModel = jQuery("#jqGrid").jqGrid('getGridParam', 'data');
        return colModel;
    }

    function fetchDataForJqGrid(url,customCallback) {

        publicApp.getWebApi(url, function returnText(rData) {

            if (IsNullOrUndefined(customCallback)) {
                $("#jqGrid_customer").jqGrid('setGridParam', { data: rData }).trigger("reloadGrid");
            }
            else {
                var data = customCallback(rData);
                $('#jqGrid').jqGrid('setGridParam', { data: data }).trigger('reloadGrid');
            }
        }, false, false);
    }
})();

function getDataForGrid() {
    var colModel = jQuery("#jqGrid").jqGrid('getGridParam', 'data');
    return colModel;
}