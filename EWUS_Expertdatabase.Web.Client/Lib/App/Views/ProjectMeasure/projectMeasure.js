(function ($) {
    $(document).ready(function () {
        var dataDocumentItems = null;
        if (IsNullOrEmpty(dcProjectMeasure) == false) {
            var data = JSON.parse(base64.decode(dcProjectMeasure));

            if (IsNullOrUndefined(data) == false)
                dataDocumentItems = data.DocumentItems;
        }

        publicApp.initializeDropZoneApp("projectMeasureDropZone", "projectMeasurePreview", objectId, "ProjectMeasure");
        publicApp.fillDropZoneApp(dataDocumentItems, "projectMeasureDropZone", objectId, "ProjectMeasure");

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
            $("ol.example").sortable({

            });

        });

    });
    
})(jQuery);
