var openFormAddMeasure = function addMeasure(projectId,e) {

    if (IsNullOrUndefined(projectId) || projectId == 0) {
        publicApp.onFormSubmitApp($('#Project'), e, null);
    }

    publicApp.modalDialogClose();
    var sUrl = sRootUrl + 'projektmitmassnahmen/' + projectId;
    window.location.href = sUrl;
};

(function ($) {
    $(document).ready(function () {
        var dataDocumentItems = [];
        if (IsNullOrEmpty(dcProject) == false) {
            var data = JSON.parse(base64.decode(dcProject));

            if (IsNullOrUndefined(data) == false)
                dataDocumentItems.push(data.DocumentItem);
        }

        publicApp.initializeDropZoneApp("projectDropZone", "projectPreview", objectId, "Project");
        publicApp.fillDropZoneApp(dataDocumentItems, "projectDropZone", objectId, "Project"); 

        GetProject(dcProject);

        function GetProject(dcProject) {
            if (IsNullOrEmpty(dcProject) == false) {
                var data = JSON.parse(base64.decode(dcProject));
                publicApp.setFormApp($("#Project"), data);
            }
        }

        $('#Project').submit(function (e) {
            publicApp.onFormSubmitApp($('#Project'), e, function (data) {

                var url = sRootUrl + 'Project/GetProjects';
                
                publicApp.getWebApi(url, projectTransform, false, false);
                
            },false,null,null,false);
            
        });

        $('#buttonAddMeasure').on('click', function (e) {
            var projectId = 0
            if (IsNullOrUndefined(objectId) || objectId == 0) {
                var result = publicApp.onFormSubmitApp($('#Project'), e, null, false, false, null, null, "Project", false);
                if (IsNullOrUndefined(result))
                    return false;
                else
                    projectId = result.Id;
            }
            else
                projectId = objectId;

            publicApp.modalDialogClose();
            var sUrl = sRootUrl + 'projektmitmassnahmen/' + projectId;
            window.location.href = sUrl;
        });
    });

})(jQuery);
