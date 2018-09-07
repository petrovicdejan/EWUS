(function ($) {
    $(document).ready(function () {

        var data = null;

        if (!IsNullOrUndefined(documentInstance) && !IsNullOrWhiteSpace(documentInstance))
            data = JSON.parse(base64.decode(documentInstance));

        var idDocumentInstance = 0;

        if (!IsNullOrUndefined(data) && data.length > 0)
            idDocumentInstance = data[0].Id;

        publicApp.initializeDropZoneApp("measureDropZone", "measurePreview", idDocumentInstance, objectId, "Measure");
        publicApp.fillDropZoneApp(data, "measureDropZone", objectId, "Measure");  
        
        var colModel = [
            {
                label: 'Name',
                name: 'Name',
                width: 25,
                editable: true,
                search: false              
            },
            {
                label: 'Link',
                name: 'Link',
                editable: true,
                width: 15,
                search:false
            },
            {
                label: "Actions",
                name: "actions",
                width: 7,
                formatter: "actions",
                formatoptions: {
                    keys: true,
                    editOptions: {},
                    addOptions: {},
                    delOptions: {}
                },
                search: false
            },
        ];

        setGridOptions.setUpGrid("jqGridLink", "jqGridPager", colModel, 1500, 300, 15, fetchGridData);

        function fetchGridData() {
        }

        function getDataForGrid() {
            var colModel = jQuery("#jqGridLink").jqGrid('getGridParam', 'data');
            return colModel;
        }

        GetMeasure(dcMeasure);

        function GetMeasure(dcMeasure) {
            if (IsNullOrEmpty(dcMeasure) == false) {
                var data = JSON.parse(base64.decode(dcMeasure));
             
                $('#jqGridLink').jqGrid('setGridParam', { data: data.MeasureLinks }).trigger('reloadGrid');
                publicApp.setFormApp($("#Measure"), data);
            }
        }

        $('#Measure').submit(function (e) {
            publicApp.onFormSubmitApp($('#Measure'), e, function (data) {

                var data = [];

                var url = sRootUrl + "/Measure/GetMeasures";

                publicApp.getWebApi(url, function returnText(rData) {

                    $.each(rData, function (inx, item) {
                        var row = new Object();
                        row.Id = item.Id;
                        row.Name = item.Name;
                        row.SerialNumber = item.SerialNumber;
                        row.InvestmentCost = item.InvestmentCost;
                        row.Saving = item.Saving;

                        if (!IsNullOrUndefined(item.OperationType)) {
                            row.OperationType = item.OperationType.Name;
                        }

                        data.push(row);
                    });

                }, false, false);

                $("#jqGrid").jqGrid('setGridParam', { data: data }).trigger("reloadGrid");
            });
        });
    });   

})(jQuery);
