(function ($) {
    $(document).ready(function () {
        var dataDocumentItems = null;
        if (IsNullOrEmpty(dcCustomer) == false) {
            var data = JSON.parse(base64.decode(dcCustomer));

            if (IsNullOrUndefined(data) == false)
                dataDocumentItems = data.DocumentItems;
        }

        publicApp.initializeDropZoneApp("customerDropZone", "customerPreview", objectId, "Customer");
        publicApp.fillDropZoneApp(dataDocumentItems, "customerDropZone", objectId, "Customer");  

        GetCustomer(dcCustomer);

        setFocus($("#Name"));
    });

    $('#Customer').submit(function (e) {
        publicApp.onFormSubmitApp($('#Customer'), e, function (data) {
            var url = sRootUrl + "Party/GetAllCustomers";

            publicApp.getWebApi(url, function onFetchData(rData) {
                $("#jqGrid_customer").jqGrid('setGridParam', { data: rData }).trigger("reloadGrid");
            }, false, false);
        },false);
    });

    function GetCustomer(dcCustomer) {
        if (IsNullOrEmpty(dcCustomer) == false) {
            var data = JSON.parse(base64.decode(dcCustomer));
            publicApp.setFormApp($("#Customer"), data);
        }
    }

   
})(jQuery);
