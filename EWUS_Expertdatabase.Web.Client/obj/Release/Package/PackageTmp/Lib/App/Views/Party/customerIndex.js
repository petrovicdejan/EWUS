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
                width: 35,
                key: true,
                searchoptions: {
                    // show search options
                    sopt: ['cn'], // ge = greater or equal to, le = less or equal to, eq = equal to
                }
            },
            {
                label: '',
                name: '',
                width: 1,
                formatter: function (cellvalue, options, rowObject) {
                    return '<a href="#" class="btn btn-xs" onclick="publicApp.deleteObjectApp(this,' + fetchGridData + ')" data-type="Customer" data-url="Party/DeleteCustomer/' + rowObject.Id + '" data-Id=' + rowObject.Id + '><i class="fa fa-trash-o"></i></a>';
                },
                editable: false,
                search: false
            },
        ];

        setGridOptions.setUpGrid("jqGrid_customer", "jqGridPager_customer", colModel, 1500, 0, 15, fetchGridData, "btnCustomAdd", "/Party/EditCustomer?key=");

        function fetchGridData() {

            setGridOptions.deleteRows('jqGrid_customer');

            var url = sRootUrl + 'Party/GetAllCustomers';

            publicApp.getWebApi(url, function onFetchData(rData) {
                $('#jqGrid_customer').jqGrid('setGridParam', { data: rData }).trigger('reloadGrid');
                $('#rowsNumber').text('Anzahl: ' + $('#jqGrid_customer').getGridParam('reccount'));
            }, false, false);
        }

        function getDataForGrid() {
            var colModel = jQuery('#jqGrid_customer').jqGrid('getGridParam', 'data');
            return colModel;
        }
        
    })
})(jQuery);
