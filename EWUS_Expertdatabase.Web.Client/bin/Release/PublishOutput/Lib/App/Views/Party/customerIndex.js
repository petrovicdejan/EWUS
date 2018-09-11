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
                width: 40,
                key: true,
                searchoptions: {
                    // show search options
                    sopt: ['cn'], // ge = greater or equal to, le = less or equal to, eq = equal to
                }
            },
            {
                label: '',
                name: '',
                width: 7,
                formatter: function (cellvalue, options, rowObject) {
                    return '<a href="#" class="btn btn-info btn-xs" onclick="publicApp.openModalForm(this)" data-url="/Party/EditCustomer?key=' + rowObject.Id + '"><i class="fa fa-pencil"></i> Bearbeiten </a>';
                },
                editable: false,
                search: false
            },
        ];

        setGridOptions.setUpGrid("jqGrid_customer", "jqGridPager_customer", colModel, 1500, 500, 15, fetchGridData, "btnCustomAdd");

        function fetchGridData() {

            setGridOptions.deleteRows('jqGrid_customer');

            var url = sRootUrl + 'Party/GetAllCustomers';

            publicApp.getWebApi(url, function onFetchData(rData) {
                $('#jqGrid_customer').jqGrid('setGridParam', { data: rData }).trigger('reloadGrid');
            }, false, false);
        }

        function getDataForGrid() {
            var colModel = jQuery("#jqGrid_customer").jqGrid('getGridParam', 'data');
            return colModel;
        }
        
    })
})(jQuery);
