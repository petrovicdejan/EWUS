(function ($) {
    $(document).ready(function () {
        var dataDocumentItems = null;
        if (IsNullOrEmpty(dcMeasure) == false) {
            var data = JSON.parse(base64.decode(dcMeasure));

            if (IsNullOrUndefined(data) == false)
                dataDocumentItems = data.DocumentItems;
        }
        
        publicApp.initializeDropZoneApp("measureDropZone", "measurePreview", objectId, "Measure");     
        publicApp.fillDropZoneApp(dataDocumentItems, "measureDropZone", objectId, "Measure");  
       
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
                label: "",
                name: "",
                width: 10,
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

        setGridOptions.setUpGrid("jqGridLink", "jqGridPager", colModel, 1500, 150, 15, fetchGridData);

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

        if (objectId == 0)
            $('#SerialNumber').val(maxSerialNumber);

        $('#Measure').submit(function (e) {
            publicApp.onFormSubmitApp($('#Measure'), e, function (data) {

                var data = [];

                var url = sRootUrl + "Measure/GetMeasures";

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
                $("#rowsNumber").text('Number of rows: ' + $('#jqGrid').getGridParam("reccount"));

            }, false, true);
        });
    });   

})(jQuery);
