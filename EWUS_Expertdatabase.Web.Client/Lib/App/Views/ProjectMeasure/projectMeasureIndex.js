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
            width: 12,
            editable: true,
            searchoptions: {
                sopt: ['cn'],
            }
        },
        {
            label: 'Status',
            name: 'PerformanceStatus',
            width: 12,
            editable: true,
            searchoptions: {
                sopt: ['cn'],
            }
        },
        {
            label: 'Wartungsfirma',
            name: 'MaintenanceCompany',
            width: 11,
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
        }
        {
            label: 'Monetärer Aufwand [€]',
            name: 'InvestmentCost',
            width: 12,
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
            width: 4,
            formatter: function (cellvalue, options, rowObject) {
                return '<a href="#" class="btn btn-xs" onclick="publicApp.deleteObjectApp(this,' + fetchProjectMeasureData + ')" data-type="Project" data-url="Project/DeleteProject/' + rowObject.Id + '" data-Id=' + rowObject.Id + '><i class="fa fa-trash-o"></i></a>';
            },
            editable: false,
            search: false
        },
    ];

    setGridOptions.setUpGrid("gridProjectMeasure", "jqGridPager", colModel, 1500, 0, 15, fetchProjectMeasureData, false,"/Project/ProjectEdit?key=");

    function fetchProjectMeasureData() {

        setGridOptions.deleteRows('gridProjectMeasure');
       
        var url = sRootUrl + 'ProjectMeasure/GetProjectMeasures';

        publicApp.getWebApi(url, function (rData) {
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

            $('#gridProjectMeasure').jqGrid('setGridParam', { data: data }).trigger('reloadGrid');

            $('#rowsNumber').text('Anzahl: ' + $('#gridProjectMeasure').getGridParam('reccount'));

        }, false, false);
    }
})();
