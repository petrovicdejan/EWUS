 (function ($) {
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
                width: 25,
                key: true,
                searchoptions: {
                    // show search options
                    sopt: ['cn'], // ge = greater or equal to, le = less or equal to, eq = equal to
                }
            },
            {
                label: 'Email',
                name: 'Email',
                width: 12,
                searchoptions: {
                    // show search options
                    sopt: ['cn'],
                }
            },
            {
                label: '',
                name: '',
                width: 1,
                formatter: function (cellvalue, options, rowObject) {
                    return '<a href="#" class="btn btn-xs" onclick="publicApp.deleteObjectApp(this,' + fetchGridData + ')" data-type="MaintenanceCompany" data-url="Party/DeleteMaintenanceCompany/' + rowObject.Id + '" data-Id=' + rowObject.Id + '><i class="fa fa-trash-o"></i></a>';
                },
                editable: false,
                search: false
            },
        ];
        
     setGridOptions.setUpGrid("jqGrid_mci", "jqGridPager", colModel, 1300, 0, 15, fetchGridData, false,"/Party/EditMaintenanceCompany?key=");
     
         function fetchGridData() {      

            setGridOptions.deleteRows('jqGrid_mci');
            var url = sRootUrl + 'Party/GetAllMaintenanceCompanies';

            publicApp.getWebApi(url, function onFetchData(rData) {
                $('#jqGrid_mci').jqGrid('setGridParam', { data: rData }).trigger('reloadGrid');
                $('#rowsNumber').text('Anzahl: ' + $('#jqGrid_mci').getGridParam('reccount'));
            }, false, false);            
         }    

        function getDataForGrid() {
            var colModel = jQuery("#jqGrid_mci").jqGrid('getGridParam', 'data');
            return colModel;
        }

 })(jQuery);
