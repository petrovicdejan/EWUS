(function ($) {
    $(document).ready(function () {

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
                
            },false);
            
        });
    });

})(jQuery);
