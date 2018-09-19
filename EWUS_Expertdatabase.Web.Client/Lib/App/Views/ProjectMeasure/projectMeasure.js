(function ($) {
    $(document).ready(function () {
        var dataDocumentItems = null;
        if (IsNullOrEmpty(dcProjectMeasure) == false) {
            var data = JSON.parse(base64.decode(dcProjectMeasure));

            if (IsNullOrUndefined(data) == false)
                dataDocumentItems = data.ProjectMeasurePerformances;
        }

        GetProjectMeasure(dcProjectMeasure);

        function GetProjectMeasure(dcProjectMeasure) {
            if (IsNullOrEmpty(dcProjectMeasure) == false) {
                var data = JSON.parse(base64.decode(dcProjectMeasure));

                publicApp.setFormApp($("#ProjectMeasure"), data);
            }
        }

        $('#ProjectMeasure').submit(function (e) {
            publicApp.onFormSubmitApp($('#ProjectMeasure'), e, function () {

                var url = sRootUrl + 'ProjectMeasure/GetAllProjectMeasures/' + projectId;

                publicApp.getWebApi(url, projectMeasureTransform, false);
            }, false, true);
        });

        $(function () {
            $("ol.dz-list").sortable({

            });

        });


        $("#add-performance").click(function () {

            var preview = $("#rows-container").find(".preview-source[data-preview]").clone();
            preview.removeAttr('data-preview');
            preview.removeProp('data-preview');
            preview.removeClass('preview-source');
            preview.css('display', 'block');
            $("#rows-container").append(preview);
            if (preview.find(".row-delete")) {
                preview.find(".row-delete").on('click',function () {
                    publicApp.callSwalDeleteApp(
                        function () {
                            preview.find(".row-delete").parent().parent().parent().remove();
                        },
                        function () { });
                });
            }
            
            var dropzone = preview.find(".dropzone");
            var exactSelector = dropzone[0];
            var exactPreviewSelector = dropzone.find("#dz-projectMeasureDropZone-preview")[0];
            publicApp.initializeMultipleDropZoneApp(exactSelector, exactPreviewSelector, objectId, "ProjectMeasure");

        });

        for (var i = 0; i < dataDocumentItems.length; i++) {
            var preview = $("#rows-container").find(".preview-source[data-preview]").clone();
            preview.removeAttr('data-preview');
            preview.removeProp('data-preview');
            preview.removeClass('preview-source');
            preview.css('display', 'block');
            $("#rows-container").append(preview);

            if (preview.find(".row-delete")) {
                preview.find(".row-delete").on('click',function () {
                    publicApp.callSwalDeleteApp(
                        function () {
                            preview.find(".row-delete").parent().parent().parent().remove();
                        },
                        function () { });
                });
            }
            preview.find('.dzone-add').on('click', function () {
                $(this).parent().parent().find('.dropzone').click();
            });
            

            preview.attr('data-id', dataDocumentItems[i].Id);
            preview.find("#description").val(dataDocumentItems[i].Description);
            preview.find("#projectMeasureDropZone-check").prop('checked', dataDocumentItems[i].Hide);

            var dropzone = preview.find(".dropzone");
            var exactSelector = dropzone[0];
            var exactPreviewSelector = dropzone.find("#dz-projectMeasureDropZone-preview")[0];

            publicApp.initializeMultipleDropZoneApp(exactSelector, exactPreviewSelector, objectId, "ProjectMeasure");
            publicApp.fillExtendedDropZoneApp(dataDocumentItems[i].DocumentItem, dropzone, objectId, "ProjectMeasure");
        }
    });

})(jQuery);
