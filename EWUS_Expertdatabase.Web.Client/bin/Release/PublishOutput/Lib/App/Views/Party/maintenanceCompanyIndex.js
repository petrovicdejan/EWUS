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
                width: 15,
                searchoptions: {
                    // show search options
                    sopt: ['cn'],
                }
            },
            {
                label: '',
                name: '',
                width: 7,
                formatter: function (cellvalue, options, rowObject) {
                    return '<a href="#" class="btn btn-info btn-xs" onclick="publicApp.openModalForm(this)" data-url="/Party/EditMaintenanceCompany?key=' + rowObject.Id + '"><i class="fa fa-pencil"></i> Bearbeiten </a>';
                },
                editable: false,
                search: false
            }
        ];
        
        setGridOptions.setUpGrid("jqGrid_mci", "jqGridPager", colModel, 1500, 500, 15, fetchGridData);
     
         function fetchGridData() {      

            setGridOptions.deleteRows('jqGrid_mci');
            var url = sRootUrl + 'Party/GetAllMaintenanceCompanies';

            publicApp.getWebApi(url, function onFetchData(rData) {
                $('#jqGrid_mci').jqGrid('setGridParam', { data: rData }).trigger('reloadGrid');
            }, false, false);            
         }    

        function getDataForGrid() {
            var colModel = jQuery("#jqGrid_mci").jqGrid('getGridParam', 'data');
            return colModel;
        }

 })(jQuery);
