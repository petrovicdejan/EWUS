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
                
                publicApp.getWebApi(url, function returnText(rData) {
                    var data = [];

                    $.each(rData, function (inx, item) {
                        var row = new Object();
                        row.Id = item.Id;
                        row.Name = item.Name;
                        row.PropertyNumber = item.PropertyNumber;

                        if (!IsNullOrUndefined(item.Property)) {
                            row.PropertyType = item.Property.Name;
                        }

                        if (!IsNullOrUndefined(item.Customer)) {
                            row.Customer = item.Customer.Name;
                        }

                        if (!IsNullOrUndefined(item.Region)) {
                            row.Region = item.Region.Name;
                        }

                        row.Location = item.Location;
                        row.ZipCode = item.ZipCode;
                        row.City = item.City;
                        row.InvestmentTotal = item.InvestmentTotal;
                        row.SavingTotal = item.SavingTotal;


                        data.push(row);
                    });

                    $('#gridProject').jqGrid('setGridParam', { data: data }).trigger('reloadGrid');

                    $('#rowsNumber').text('Anzahl: ' + $('#gridProject').getGridParam('reccount'));
                }, false, false);
                
            },false);
            
        });
    });

})(jQuery);
