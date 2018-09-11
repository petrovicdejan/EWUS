(function ($) {
    $(document).ready(function () {
        GetMaintenanceCompany(dcMaintenanceCompany);
    });

    $('#MaintenanceCompany').submit(function (e) {
        publicApp.onFormSubmitApp($('#MaintenanceCompany'), e, function (data) {
            var url = sRootUrl + "Party/GetAllMaintenanceCompanies";

            publicApp.getWebApi(url, function onFetchData(rData) {
                $("#jqGrid_mci").jqGrid('setGridParam', { data: rData }).trigger('reloadGrid');
            }, false, false);
        }, false);
    });

    function GetMaintenanceCompany(dcMaintCompany) {
        if (IsNullOrEmpty(dcMaintCompany) == false) {
            var data = JSON.parse(base64.decode(dcMaintCompany));
            publicApp.setFormApp($("#MaintenanceCompany"), data);
        }
    }

})(jQuery);
