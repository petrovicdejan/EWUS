var myDropzone = [];


function SetUpDropzone(dataspec, sRootUrl, sModelId, isReadOnly) {

    var dataModel = base64.decode(dataspec);
    dataModel = JSON.parse(dataModel);

    var firstTime = myDropzone == null;

    if (!firstTime) {
        for (i = 0; i < myDropzone.length; i++) {
            //if (myDropzone[i] != null && myDropzone[i] != 'undefined') {
            //    myDropzone[i] = null;
            //}
            if (!IsNullOrUndefined(myDropzone[i])) {
                myDropzone[i].Value.disable();
            }
        }
        myDropzone = new Array();
    }

    for (j = 0; j < dataModel.length; j++) {
        InitializeDropzone((j + 1), sRootUrl, sModelId, isReadOnly);
    };
};

function FillDropzone(oData, sRootUrl, objectPermissions) {
    var j = 1;
    //var params = "usesSmartwaveDMS:" + usesSmartwaveDMS + ";usesDMSPathUpload:" + usesDMSPathUpload;
    //params = encodeURIComponent(base64.encode(params));

    if (objectPermissions) {
        var dataPermission = base64.decode(objectPermissions)
        dataPermission = JSON.parse(dataPermission);
    }

    try {
        Object.getOwnPropertyNames(oData).forEach(function (val, idx, array) {

            var value = oData[val];
            var name = val;
            name = name.replace("m_", "");

            var sSelector = "div[data-name='" + name + "']";
            var element = $(sSelector).first();

            if (!(IsNullOrUndefined(element)) && element.length > 0) {
                if (!(IsNullOrUndefined(value)) && value.length > 0) {
                    for (i = 0; i < value.length; i++) {
                        if (showObjectByPermission(dataPermission, value[i].DocumentBehaviorSpecification)) {
                            var mockfile = { name: value[i].DocumentName, size: setFileSize(value[i].DocumentSize), type: value[i].DocumentMimeType, id: value[i].ObjectId, status: "added", isnew: false };

                            var dropto = $.grep(myDropzone, function (e) {
                                return e.id == element[0].id;
                            })[0];

                            var dropzoneItem = null;

                            dropzoneItem = dropto.Value;
                            dropzoneItem.files.push(mockfile);
                            dropzoneItem.emit("addedfile", mockfile);
                            if (isImage(value[i].DocumentName)) {
                                dropzoneItem.createThumbnailFromUrl(mockfile, sRootUrl + "api/download/contentstream?Number=" + value[i].ObjectId + "&Key='" + value[i].DocumentName);
                            }
                            dropzoneItem.emit("complete", mockfile);
                        }
                    };
                }
                j++;
            }
        });
    }
    catch (e) {

    };
}

function AddDropzone() {

    var index = $("#previewheader>div").length + 1;
    var iddropzone = "dropzone" + index;
    var idpreview = "preview" + index;

    var item = [{ IdDropzone: iddropzone, ItemId: -100, ItemCaption: "Other", IdPreview: idpreview, ItemName: "Other" }];

    $("#dropzone-template").tmpl(item).appendTo($('#previewheader'));

    $("#adddropzone")[0].parentNode.removeChild($("#adddropzone")[0]);

    InitializeDropzone(index);
};


function InitializeDropzone(index, sRootUrl, sModelId, isReadOnly) {

    try {
        //if (myDropzone[index - 1] != null && myDropzone[index] != 'undefined') {
        //    myDropzone[index - 1] = null;
        //}
        if (!IsNullOrUndefined(myDropzone[index - 1])) {
            myDropzone[index - 1].Value.disable();
        }

        //myDropzone = new Array();
    }
    catch (e)
    { }

    var iddropzone = sModelId + "-dropzone" + (index);
    var idpreview = "preview" + (index);
    var tmpClickable = "#" + iddropzone;
    if (IsNullOrUndefined(isReadOnly) == false && isReadOnly == "True")
        tmpClickable = "";

    //var params = "usesSmartwaveDMS:" + usesSmartwaveDMS + ";usesDMSPathUpload:" + usesDMSPathUpload;
    //params = encodeURIComponent(base64.encode(params));

    var previewNode = document.querySelector("#" + idpreview);
    if (previewNode != null && previewNode != 'undefined') {
        previewNode.id = "";
        var previewTemplate = previewNode.parentNode.innerHTML;
        previewNode.parentNode.removeChild(previewNode);

        var myDropzoneItem = new Dropzone(document.querySelector("#" + iddropzone), {
            url: sRootUrl + "api/insert/contentstream?Tag=1",
            addRemoveLinks: true,
            addOpenLinks: true,
            parallelUploads: 20,
            sendFileId: true,
            clickable: tmpClickable
        });

        myDropzoneItem.on("addedfile", function (file) {

            if (file.status == "added") {
                file.previewElement.setAttribute("data-mimetype", file.type);
                file.previewElement.setAttribute("data-objectid", file.id);
                file.previewElement.setAttribute("data-isnew", IsNullOrUndefined(file.isnew));
                file.previewElement.setAttribute("data-isdeleted", "false");
            }
        });

        myDropzone.push({ id: iddropzone, Value: myDropzoneItem });
    }
};

function CloseForm() {
    try {
        for (i = 0; i < myDropzone.length; i++) {
            //if (myDropzone[i] != null && myDropzone[i] != 'undefined') {
            //    myDropzone[i] = null;
            //}
            if (!IsNullOrUndefined(myDropzone[i])) {
                myDropzone[i].Value.disable();
            }
            myDropzone.pop();
        }
        myDropzone.length = 0;
    }
    catch (e) {
    }
};



