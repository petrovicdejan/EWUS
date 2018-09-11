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
            label: 'Name',
            name: 'Name',
            width: 12,
            key: true,
            editable: true,
            searchoptions: {
                sopt: ['cn', "ge", "le", "eq", 'bw'],
            }
        },
        {
            label: 'LiegenschaftsNr',
            name: 'PropertyNumber',
            width: 18,
            editable: true,
            searchoptions: {
                sopt: ['cn', "ge", "le", "eq", 'bw'],
            }
        },
        {
            label: 'Liegenschaftstyp',
            name: 'PropertyType',
            width: 19,
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
            width: 8,
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
            width: 8,
            editable: true,
            searchoptions: {
                sopt: ['cn'],
            }
        },
        {
            label: 'Investition Gesamt',
            name: 'InvestmentTotal',
            width: 20,
            editable: true,
            searchoptions: {
                sopt: ['cn'],
            }
        },
        {
            label: 'Einsparung Gesamt',
            name: 'SavingTotal',
            width: 21,
            editable: true,
            searchoptions: {
                sopt: ['cn'],
            }
        },
        {
            label: '',
            name: '',
            width: 15,
            formatter: function (cellvalue, options, rowObject) {
                return '<a href="#" class="btn btn-info btn-xs" onclick="publicApp.openModalForm(this)" data-url="/Project/ProjectEdit?key=' + rowObject.Id + '"><i class="fa fa-pencil"></i> Bearbeiten </a>';
            },
            editable: false,
            search: false
        }, ,
    ];

    setGridOptions.setUpGrid("gridProject", "jqGridPager", colModel, 1500, 350, 15, fetchProjectData);

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
    }

    return {
        applyProjectToGrid: function (rData) {
            applyProjectToGrid(rData)
        },
    }
})();
