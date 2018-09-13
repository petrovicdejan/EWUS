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
            width: 25,
            key: true,
            editable: true,
            searchoptions: {
                sopt: ['cn', "ge", "le", "eq", 'bw'],
            }
        },
        {
            label: 'Liegenschafts-Nr',
            name: 'PropertyNumber',
            width: 10,
            editable: true,
            searchoptions: {
                sopt: ['cn', "ge", "le", "eq", 'bw'],
            }
        },
        {
            label: 'Liegenschaftstyp',
            name: 'PropertyType',
            width: 15,
            editable: true,
            searchoptions: {
                sopt: ['cn', "ge", "le", "eq", 'bw'],
            }
        },
        {
            label: 'Kunde',
            name: 'Customer',
            width: 13,
            editable: true,
            searchoptions: {
                sopt: ['cn', "ge", "le", "eq", 'bw'],
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
            width: 12,
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
            width: 12,            
            editable: true,
            searchoptions: {
                sopt: ['cn'],
            }
        },
        {
            label: 'Investition [€]',
            name: 'InvestmentTotal',
            width: 10,
            formatter: 'number',
            sorttype: "number",
            align:'right',
            editable: true,
            searchoptions: {
                sopt: ['bw'],
            }
        },
        {
            label: 'Einsparung [€/a]',
            name: 'SavingTotal',
            align: 'right',
            formatter: 'number',
            sorttype: "number",
            width: 10,
            editable: true,
            searchoptions: {
                sopt: ['bw'],
            }
        },
    ];

    setGridOptions.setUpGrid("gridProject", "jqGridPager", colModel, 1500, 0, 15, fetchProjectData, false,"/Project/ProjectEdit?key=");

    function fetchProjectData() {

        setGridOptions.deleteRows('gridProject');
       
        var url = sRootUrl + 'Project/GetProjects';

        publicApp.getWebApi(url, applyProjectToGrid , false, false);
    }

    function applyProjectToGrid(rData) {
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
       
        $("#rowsNumber").text('Number of rows: ' + $('#gridProject').getGridParam("reccount"));
    }

    return {
        applyProjectToGrid: function (rData) {
            applyProjectToGrid(rData)
        },
    }
})();
