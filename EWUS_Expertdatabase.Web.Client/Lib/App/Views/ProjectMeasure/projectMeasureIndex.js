var funcAddMeasureToProject = function addMeasureToProject(projectId, measureId) {
    var sUrl = sRootUrl + 'ProjectMeasure/AddProjectToMeasure';
    var data = new Object();
    data.ProjectId = projectId;
    data.MeasureId = measureId;

    publicApp.postWebApi(sUrl, data, function (data) {
        
        publicApp.getWebApi(url, projectMeasureTransform, false, false);

    }, false);

};

var projectMeasureTransform = function transformData(rData) {
    setGridOptions.deleteRows('gridProjectMeasure');
    var data = [];
    $.each(rData, function (inx, item) {
        var row = new Object();
        row.Id = item.Id;
        row.Name = item.Name;
        row.PerformanseSheetNumber = item.PerformanseSheetNumber;
        row.MeasureName = item.MeasureName;

        if (!IsNullOrUndefined(item.PerformanseSheetStatus)) {
            row.PerformanceSheetStatus = item.PerformanseSheetStatus.Value;
        }

        if (!IsNullOrUndefined(item.MaintenanceCompany)) {
            row.MaintenanceCompany = item.MaintenanceCompany.Name;
        }

        if (!IsNullOrUndefined(item.OperationType)) {
            row.OperationType = item.OperationType;
        }

        row.InvestmentCost = item.InvestmentCost;


        data.push(row);
    });

    $('#gridProjectMeasure').jqGrid('setGridParam', { data: data }).trigger('reloadGrid');

    $('#rowsNumber').text('Anzahl: ' + $('#gridProjectMeasure').getGridParam('reccount'));

    publicApp.setUpSelectApp("#projectMeasureList ");

}

var projectModule = (function () {
    var colModel = [
        {
            label: 'Id',
            name: 'Id',
            width: 7,
            editable: true,
            hidden: true
        },
        {
            label: 'LB-Nr',
            name: 'PerformanseSheetNumber',
            width: 8,
            key: true,
            editable: true,
            searchoptions: {
                sopt: ['cn'],
            }
        },
        {
            label: 'Maßnahmenbenennung',
            name: 'MeasureName',
            width: 25,
            editable: true,
            searchoptions: {
                sopt: ['cn'],
            }
        },
        {
            label: 'Status',
            name: 'PerformanceSheetStatus',
            width: 10,
            editable: true,
            searchoptions: {
                sopt: ['cn'],
            }
        },
        {
            label: 'Wartungsfirma',
            name: 'MaintenanceCompany',
            width: 15,
            editable: true,
            searchoptions: {
                sopt: ['cn'],
            }
        },
        {
            label: 'Maßnahmenart',
            name: 'OperationType',
            width: 10,
            editable: true,
            searchoptions: {
                sopt: ['cn'],
            }
        },
        {
            label: 'Monetärer Aufwand [€]',
            name: 'InvestmentCost',
            width: 10,
            classes: "grid-col",
            formatter: 'number',
            sorttype: "number",
            align:'right',
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
                return '<a href="#" class="btn btn-xs" onclick="publicApp.deleteObjectApp(this,' + fetchProjectMeasureData + ')" data-type="Project" data-url="ProjectMeasure/DeleteProjectMeasure/' + rowObject.Id + '" data-Id=' + rowObject.Id + '><i class="fa fa-trash-o"></i></a>';
            },
            editable: false,
            search: false
        },
    ];

    setGridOptions.setUpGrid("gridProjectMeasure", "jqGridPager", colModel, 1500, 0, 15, fetchProjectMeasureData, false, "/leistungsblatt/");
    
    $("#ProjectName").val(projectName);

    $("#AddMeasureToProject").on('click', function () {
        var value = publicApp.getSelectedFieldApp('#Measure');
        if (!IsNullOrUndefined(value)) {
            var sUrl = sRootUrl + 'ProjectMeasure/AddProjectToMeasure';
            var data = new Object();
            data.ProjectId = objectId;
            data.MeasureId = value.Value;

            publicApp.postWebApi(sUrl, data, fetchProjectMeasureData, false);
        }

    });

    function fetchProjectMeasureData() {

        setGridOptions.deleteRows('gridProjectMeasure');
       
        var url = sRootUrl + 'ProjectMeasure/GetAllProjectMeasures/' + objectId;

        publicApp.getWebApi(url, projectMeasureTransform);
    }
})();
