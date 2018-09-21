﻿(function () {
    var colModel = [
        {
            label: 'Id',
            name: 'Id',
            width: 7,
            key: true,
            editable: true,
            hidden: true
        },
        {
            label: 'Benennung',
            name: 'Name',
            width: 28,
            editable: true,
            searchoptions: {
                sopt: ['cn'],
            }
        },
        {
            label: 'Laufende-Nr',
            name: 'SerialNumber',
            align: 'left',
            sorttype: "number",
            width: 8,
            classes: "grid-col",
            editable: true,
            searchoptions: {
                sopt: ['cn'], 
            }
        },
        {
            label: 'Einsparung [%]',
            name: 'SavingPercent',
            align: 'right',
            formatter: 'number',
            sorttype: "number",
            width: 8,
            classes: "grid-col",
            editable: true,
            searchoptions: {
                sopt: ['bw', "ge", "le", "eq"],
            }
        },
        {
            label: 'Energie/Medium',
            name: 'OperationType',
            width: 8,
            editable: true,
            searchoptions: {
                sopt: ['cn'], 
            }
        },
        {
            label: 'Investitionskosten [€]',
            name: 'InvestmentCost',
            classes: "grid-col",
            formatter: 'number',
            sorttype: "number",
            width: 8,
            align: 'right',
            editable: true,
            searchoptions: {
                sopt: ['bw', "ge", "le", "eq"], 
            }
        },
        {
            label: '',
            name: '',
            width: 2,
            formatter: function (cellvalue, options, rowObject) {
                return '<a href="#" class="btn btn-xs" onclick="publicApp.deleteObjectApp(this,' + fetchGridData + ')" data-type="Measure" data-url="Measure/DeleteMeasure/' + rowObject.Id + '" data-Id=' + rowObject.Id + '><i class="fa fa-trash-o"></i></a>';
            },
            editable: false,
            search: false
        },
    ];

    setGridOptions.setUpGrid("jqGrid", "jqGridPager", colModel, 1400, 0, 100, fetchGridData, false,"/Measure/MeasureEdit?key=");

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
                row.SavingPercent = item.SavingPercent;
                row.InvestmentCost = item.InvestmentCost;
                row.Saving = item.Saving;

                if (!IsNullOrUndefined(item.OperationType)) {
                    row.OperationType = item.OperationType.Name;
                }

                data.push(row);
            });
            
        }, false, false);    
        
        $('#jqGrid').jqGrid('setGridParam', { data: data }).trigger('reloadGrid');

        $('#rowsNumber').text('Anzahl: ' + $('#jqGrid').getGridParam('reccount'));

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