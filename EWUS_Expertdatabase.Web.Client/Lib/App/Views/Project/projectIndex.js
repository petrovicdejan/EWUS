var projectTransform = function transformData(rData) {
    var data = [];
    $.each(rData, function (inx, item) {
        var row = new Object();
        row.Id = item.Id;
        row.Name = item.Name;
        row.PropertyNumber = item.PropertyNumber;

        if (!IsNullOrUndefined(item.Property)) {
            row.PropertyType = item.Property.Name;
        }

        if (!IsNullOrUndefined(item.Customer)) {
            row.Customer = item.Customer.Name;
        }

        if (!IsNullOrUndefined(item.Region)) {
            row.Region = item.Region.Name;
        }

        row.Location = item.Location;
        row.ZipCode = item.ZipCode;
        row.City = item.City;
        row.InvestmentTotal = item.InvestmentTotal;
        row.SavingTotal = item.SavingTotal;


        data.push(row);
    });

    $('#gridProject').jqGrid('setGridParam', { data: data }).trigger('reloadGrid');

    $('#rowsNumber').text('Anzahl: ' + $('#gridProject').getGridParam('reccount'));

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
            label: 'Projekt',
            name: 'Name',
            width: 26,
            key: true,
            editable: true,
            searchoptions: {
                sopt: ['cn'],
            }
        },
        {
            label: 'Liegenschafts-Nr',
            name: 'PropertyNumber',
            width: 12,
            editable: true,
            searchoptions: {
                sopt: ['cn'],
            }
        },
        {
            label: 'Liegenschaftstyp',
            name: 'PropertyType',
            width: 12,
            editable: true,
            searchoptions: {
                sopt: ['cn'],
            }
        },
        {
            label: 'Kunde',
            name: 'Customer',
            width: 11,
            editable: true,
            searchoptions: {
                sopt: ['cn'],
            }
        },
        {
            label: 'Region',
            name: 'Region',
            width: 10,
            editable: true,
            searchoptions: {
                sopt: ['cn'],
            }
        },
        {
            label: 'Standort',
            name: 'Location',
            width: 10,
            editable: true,
            searchoptions: {
                sopt: ['cn'],
            }
        },
        {
            label: 'Plz',
            name: 'ZipCode',
            width: 10,
            editable: true,
            searchoptions: {
                sopt: ['cn'],
            }
        },
        {
            label: 'Ort',
            name: 'City',
            width: 10,
            editable: true,
            searchoptions: {
                sopt: ['cn'],
            }
        },
        {
            label: 'Investition [€]',
            name: 'InvestmentTotal',
            width: 12,
            classes: "grid-col",
            formatter: 'number',
            sorttype: "number",
            align: 'right',
            editable: true,
            searchoptions: {
                sopt: ['bw', "ge", "le", "eq"],
            }
        },
        {
            label: 'Einsparung [€/a]',
            name: 'SavingTotal',
            align: 'right',
            classes: "grid-col",
            formatter: 'number',
            sorttype: "number",
            width: 13,
            editable: true,
            searchoptions: {
                sopt: ['bw', "ge", "le", "eq"],
            }
        },
        {
            label: '',
            name: '',
            width: 4,
            formatter: function (cellvalue, options, rowObject) {
                return '<a href="#" class="btn btn-xs" onclick="publicApp.deleteObjectApp(this,' + fetchProjectData + ')" data-type="Project" data-url="Project/DeleteProject/' + rowObject.Id + '" data-Id=' + rowObject.Id + '><i class="fa fa-trash-o"></i></a>';
            },
            editable: false,
            search: false
        },
    ];

    setGridOptions.setUpGrid("gridProject", "jqGridPager", colModel, 1500, 0, 15, fetchProjectData, false, "/Project/ProjectEdit?key=");



    //function applyProjectToGrid(rData) {
    //    var data = [];

    //    $.each(rData, function (inx, item) {
    //        var row = new Object();
    //        row.Id = item.Id;
    //        row.Name = item.Name;
    //        row.PropertyNumber = item.PropertyNumber;

    //        if (!IsNullOrUndefined(item.Property)) {
    //            row.PropertyType = item.Property.Name;
    //        }

    //        if (!IsNullOrUndefined(item.Customer)) {
    //            row.Customer = item.Customer.Name;
    //        }

    //        if (!IsNullOrUndefined(item.Region)) {
    //            row.Region = item.Region.Name;
    //        }

    //        row.Location = item.Location;
    //        row.ZipCode = item.ZipCode;
    //        row.City = item.City;
    //        row.InvestmentTotal = item.InvestmentTotal;
    //        row.SavingTotal = item.SavingTotal;


    //        data.push(row);
    //    });

    //    $('#gridProject').jqGrid('setGridParam', { data: data }).trigger('reloadGrid');

    //    $('#rowsNumber').text('Number of rows: ' + $('#gridProject').getGridParam('reccount'));
    //}

    //return {
    //    applyProjectToGrid: function (rData) {
    //        applyProjectToGrid(rData)
    //    },
    //}
    
    function fetchProjectData() {

        setGridOptions.deleteRows('gridProject');

        var url = sRootUrl + 'Project/GetProjects';

        publicApp.getWebApi(url, projectTransform, false, false);
    }
    return {
        fetchData: fetchProjectData
    }
})();
