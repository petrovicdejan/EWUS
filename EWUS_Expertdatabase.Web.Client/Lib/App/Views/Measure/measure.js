var measurejs = (function ($) {
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
                width: 10,
                editable: true,
                search: false              
            },
            {
                label: 'Link',
                name: 'Link',
                formatter: 'link',
                formatoptions: { target: '_blank'},
                editable: true,            
                width: 27,
                search:false
            },
            {
                label: "",
                name: "",
                editable: false,
                search: false,
                width: 7,
                formatter: function (rowId, cellval, colpos, rwdat, _act) {
                    return "<div title='Bearbeiten' class='ui-pg-div ui-inline-edit' id='jEditButton_" + cellval.rowId+"' style='float: left; cursor: pointer;' onmouseover='jQuery(this).addClass(\"active\"); ' onmouseout='jQuery(this).removeClass(\"active\"); ' onclick='jQuery.fn.fmatter.rowactions.call(this,\"edit\"); '><span class='glyphicon glyphicon-edit'></span></div>" +
                        "<div title='Löschen' class='ui-pg-div ui-inline-del' id='jDeleteButton_" + cellval.rowId + "' style='float: left; cursor: pointer;' onmouseover='jQuery(this).addClass(\"active\"); ' onmouseout='jQuery(this).removeClass(\"active\"); ' onclick='setGridOptions.deleteRowById(\"jqGridLink\",this);'><span class='glyphicon glyphicon-trash'></span></div>" +
                        "<div title='Übernehmen' class='ui-pg-div ui-inline-save' id='jSaveButton_" + cellval.rowId + "' style='float: left; display: none; ' onmouseover='jQuery(this).addClass(\"active\"); ' onmouseout='jQuery(this).removeClass(\"active\"); ' onclick='measurejs.saveGridRow()'><span class='glyphicon glyphicon-save'></span></div>" +
                        "<div title='Stornieren' class='ui-pg-div ui-inline-cancel' id='jCancelButton_" + cellval.rowId+"' style='float: left; display: none; ' onmouseover='jQuery(this).addClass(\"active\"); ' onmouseout='jQuery(this).removeClass(\"active\"); ' onclick='setGridOptions.deleteRowById(\"jqGridLink\",this); '><span class='glyphicon glyphicon-trash'></span></div>";
                }
            }            
        ];
        
        var relativeGridWidth = $(window).width() / 3.47;
        setGridOptions.setUpGrid("jqGridLink", "jqGridPager", colModel, relativeGridWidth, 120, 15, fetchGridData, null);

        $("#jqGridLink")
            
            .navButtonAdd("#jqGridPager", {
                caption: 'Neu',
                buttonicon : 'glyphicon glyphicon-plus',
                position: 'last',
                id: 'pager-add-btn',
                onClickButton: function () {

                    $("#jqGridLink").jqGrid('addRow', { position: "last" });
                    $("#pager-add-btn").addClass('ui-disabled');
                }
            }).navButtonAdd("#jqGridPager", {
                caption: 'Übernehmen',
                buttonicon: 'glyphicon glyphicon-download-alt pager-button',
                position: 'last',
                id: 'pager-save-btn',
                onClickButton: function () {
                    var row = $("#jqGridLink").jqGrid('getGridParam', 'selrow');
                    $("#jqGridLink").saveRow(row);
                    $("#jqGridLink").find(".ui-inline-save").each(function(ind, val) {
                        val = $(val);
                        val.hide();
                    });
                    $("#jqGridLink").find(".ui-inline-edit").each(function(ind, val) {
                        val = $(val);
                        val.show();
                    });
                    $("#pager-add-btn").removeClass('ui-disabled');

                }
            })
           
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
                        row.SavingPercent = item.SavingPercent;
                        row.InvestmentCost = item.InvestmentCost;
                        row.Saving = item.Saving;

                        if (!IsNullOrUndefined(item.OperationType)) {
                            row.OperationType = item.OperationType.Name;
                        }

                        data.push(row);
                    });

                }, false, false);

                $("#jqGrid").jqGrid('setGridParam', { data: data }).trigger("reloadGrid");
                $("#rowsNumber").text('Anzahl: ' + $('#jqGrid').getGridParam("reccount"));

            }, false, true);
        });
    });

    function saveRow() {
        var row = $("#jqGridLink").jqGrid('getGridParam', 'selrow');
        $("#jqGridLink").saveRow(row);
        $("#jqGridLink").find(".ui-inline-save").each(function(ind, val) {
            val = $(val);
            val.hide();
        });
        $("#jqGridLink").find(".ui-inline-edit").each(function(ind, val) {
            val = $(val);
            val.show();
        });
        $("#pager-add-btn").removeClass('ui-disabled');
    }
    return {
        saveGridRow: saveRow
    }
})(jQuery);
