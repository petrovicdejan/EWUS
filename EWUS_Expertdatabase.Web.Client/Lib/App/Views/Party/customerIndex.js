(function ($) {
    $(document).ready(function () {

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
                width: 30,
                key: true,
                searchoptions: {
                    // show search options
                    sopt: ['cn'], // ge = greater or equal to, le = less or equal to, eq = equal to
                }
            },
        ];

        setGridOptions.setUpGrid("jqGrid_customer", "jqGridPager_customer", colModel, 1500, 0, 15, fetchGridData, "btnCustomAdd","/Party/EditCustomer?key=");

        function fetchGridData() {

            setGridOptions.deleteRows('jqGrid_customer');

            var url = sRootUrl + 'Party/GetAllCustomers';

            publicApp.getWebApi(url, function onFetchData(rData) {
                $('#jqGrid_customer').jqGrid('setGridParam', { data: rData }).trigger('reloadGrid');
                $("#rowsNumber").text('Number of rows: ' + $('#jqGrid_customer').getGridParam("reccount"));
            }, false, false);
        }

        function getDataForGrid() {
            var colModel = jQuery("#jqGrid_customer").jqGrid('getGridParam', 'data');
            return colModel;
        }
        
    })
})(jQuery);
