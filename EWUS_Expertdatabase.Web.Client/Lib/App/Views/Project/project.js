var openFormAddMeasure = function addMeasure(projectId) {
    publicApp.modalDialogClose();
    var sUrl = sRootUrl + 'projektmitmassnahmen/' + projectId;
    window.location.href = sUrl;
};

(function ($) {
    $(document).ready(function () {

        publicApp.initializeDropZoneApp("projectDropZone", "projectPreview", objectId, "Project");
       // publicApp.fillDropZoneApp(dataDocumentItems, "projectDropZone", objectId, "Project"); 

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
    });

})(jQuery);
