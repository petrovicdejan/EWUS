
var publicApp = (function () {
    $(window).resize(windowResize);

    const autoNumericOptionsEuro = {
        digitGroupSeparator: '.',
        decimalCharacter: ',',
        decimalCharacterAlternative: '.',
        decimalPlaces: 0,
        currencySymbol: '\u202f€',
        currencySymbolPlacement: AutoNumeric.options.currencySymbolPlacement.suffix,
        roundingMethod: AutoNumeric.options.roundingMethod.halfUpSymmetric,
    };

    function webApiGet(sUri, fOnData, bShowError, bAsync) {
        if (IsNullOrUndefined(bAsync)) {
            bAsync = true;
        };

        return $.ajax({
            url: sUri,
            cache: false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            async: bAsync,
            headers: GetWebApiHeaders(),
            success: function (data, textStatus, xhr) {
                var mPagingTotal = xhr.getResponseHeader("m-paging-total");
                fOnData(data, mPagingTotal);
            },
            error: function (xhr, textStatus, errorThrown) {
                if (IsNullOrUndefined(bShowError))
                    bShowError = true;
                webApiPostOnError("Error on get data " + sUri, xhr, textStatus, errorThrown, bShowError, sUri);
            }
        });
    }

    function webApiPostOnError(header, xhr, textStatus, errorThrown, bShowAlert, fOnError, sUri, closeModal) {
        if (xhr.status == 200 && xhr.statusText == "")
            return;
        if (IsNullOrUndefined(header))
            header = "ErrorGeneral";
        if (IsNullOrUndefined(bShowAlert))
            bShowAlert = true;

        if (IsNullOrUndefined(closeModal))
            closeModal = true;
        if (closeModal)
            closeModalDialog();
        else
            ClosePageLoader();

        if (bShowAlert && xhr.statusText) {
            var r = xhr.statusText;
            if (IsNullOrUndefined(r)) {
                alert(errorThrown);
            } else {
                alert(r);
            }
        } else if (bShowAlert) {
            alert(errorThrown);
        };
        console.log(header);
    }

    function webApiPost(sUri, oData, fOnData, bShowAlertOnSuccess, bShowAlertOnFailure, fOnError, bCloseModalDialog, sFormId, isAsync) {
        if (IsNullOrUndefined(bShowAlertOnSuccess))
            bShowAlertOnSuccess = true;
        if (IsNullOrUndefined(bShowAlertOnFailure))
            bShowAlertOnFailure = true;
        if (IsNullOrUndefined(bCloseModalDialog))
            bCloseModalDialog = true;
        if (IsNullOrUndefined(sFormId))
            sFormId = "";
        if (IsNullOrUndefined(isAsync))
            isAsync = true;
        var data = "";
        var bHasKey = false;
        if (IsNullOrUndefined(oData) == false) {
            if (typeof oData == "string") {
                data = oData
            }
            else if (oData != null && typeof oData == "object") {
                TryCatchWraper(function () {
                    if (IsNullOrUndefined(oData.ObjectId) == false)
                        bHasKey = (IsNullOrWhiteSpace(oData.ObjectId) == false && (oData.ObjectId != "0" || oData.ObjectId != 0));
                });
                data = JSON.stringify(oData);
            }
        }
        var type = "POST"
        //if (bHasKey && sUri.indexOf("command") == -1) {
        //    type = "PUT";
        //}
        $.ajax({
            url: sUri,
            type: type,
            cache: false,
            dataType: 'json',
            data: data,
            async: isAsync,
            headers: GetWebApiHeaders(),
            contentType: 'application/json; charset=utf-8',
            success: function (data, textStatus, xhr) {
                webApiPostOnSuccess(data, textStatus, xhr, bShowAlertOnSuccess, fOnData, bCloseModalDialog, sUri, sFormId)
            },
            error: function (xhr, textStatus, errorThrown) {
                webApiPostOnError(null, xhr, textStatus, errorThrown, bShowAlertOnFailure, fOnError, sUri);
            }
        });
    }

    function webApiPostOnSuccess(data, textStatus, xhr, bShowAlert, fOnData, bCloseModalDialog, sUri, sFormId) {
        if (IsNullOrUndefined(bShowAlert))
            bShowAlert = true;
        if (IsNullOrUndefined(sUri))
            sUri = "";
        if (IsNullOrWhiteSpace(sFormId))
            sFormId = "Unknow";
        if (IsNullOrUndefined(bCloseModalDialog))
            bCloseModalDialog = true;
        ClosePageLoader();
        try {
            if (bCloseModalDialog)
                closeModalDialog();
            //if (data.Success) {
            var nextLink = "";

            TryCatchWraper(function () {
                if ($.isFunction(fOnData)) {
                    nextLink = fOnData(data);
                } else {
                    var eventArgs = new Object();
                    eventArgs.name = sFormId;
                    eventArgs.data = data;
                    eventArgs.url = sUri
                    $(document).trigger("behaviorTask:afterSubmit", eventArgs);
                }
            });

            if (IsNullOrUndefined(nextLink) || nextLink == "undefined")
                nextLink = "";

            if (bShowAlert) {
                alertshow(1, "Successfully saved!", "Recommended Action",
                    nextLink + "<br/>"
                    + "<span class='pull-right'>Execute Time: "
                    + data.FormatedExecuteTime
                    + " RecordsAffected: "
                    + data.RecordsAffected + "</span>");
            }
            //} else {
            //    webApiPostOnSuccessFalse("ErrorGeneral", data, bShowAlert);
            //}
        } catch (e) {
            console.log('webApiPost: ' + e.message);
        }
    }

    function webApiPostOnSuccessFalse(header, data, bShowAlert) {
        if (IsNullOrUndefined(bShowAlert))
            bShowAlert = true;
        if (bShowAlert) {
            alertshow(2, "ErrorGeneral", getErrorMessageForUser(data));
        }
    }

    function webApiDelete(el, objectType, fOnSuccess, fOnError) {

        if (IsNullOrWhiteSpace(objectType))
            return;

        var $el = $(el);
        var objectId = $el.attr("data-id");

        if (IsNullOrUndefined(objectId) || objectId == 0 || objectId == "0")
            return;

        if (elClickDisabled($el))
            return false;

        var i = new Object();
        i.Id = objectId;
        //i.Name = objectType;

        var sUri = $el.attr("data-url");

        if (IsNullOrWhiteSpace(sUri)) {
            sUri = sRootUrl + 'api/' + objectType + '/' + objectId;
            metod = ""
        }
        else {
            sUri = sRootUrl + sUri;
        }

        var type = 'POST'

        ShowPageLoader();
        $.ajax({
            url: sUri,
            cache: false,
            type: type,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(i),
            headers: GetWebApiHeaders(),
            success: function (data, textStatus, xhr) {
                elResetClickDisabled($el);
                ClosePageLoader();

                if (data.Success) {
                    var nextLink = "";
                    if ($.isFunction(fOnSuccess)) {
                        nextLink = fOnSuccess(data);
                    }

                    //if (IsNullOrUndefined(nextLink) || nextLink == "undefined")
                    //    nextLink = "";

                    //alertshow(1, "Successfully Deleted", "Recommended Action",
                    //    nextLink
                    //    + "<br/><span class='pull-right'>" + "ExecuteTime"
                    //    + data.FormatedExecuteTime
                    //    + " RecordsAffected: "
                    //    + data.RecordsAffected + "</span>");
                } else {

                    alertshow(2, "FailedDeleting", getErrorMessageForUser(data));
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                elResetClickDisabled($el);
                webApiPostOnError("Failed delete!", xhr, textStatus, errorThrown, true, sUri);
                ClosePageLoader();
                if ($.isFunction(fOnError)) {
                    fOnError(xhr, textStatus, errorThrown, sUri);
                }
            }
        });
    }

    function GetWebApiHeaders(data) {

        if (IsNullOrUndefined(data))
            data = new Object();

        return data;
    }

    function elClickDisabled(el) {
        if (IsNullOrUndefined(el))
            return false;

        var sUrl = $(el).attr("data-url");

        if ($(el).prop('disabled')) {

            return true;
        }

        if ($(el).length) {
            $(el).prop('disabled', true);
            $(el).addClass('ui-state-disabled');
            var cCount = $(el).attr("data-click-count");
            if (IsNullOrUndefined(el) || isNaN(cCount))
                cCount = "0";
            cCount = Number(cCount);
            cCount++;
            $(el).attr("data-click-count", "" + cCount);
            if (cCount > 1)
                return true;
            else
                return false;
        } else {
            return false;
        }
    }

    function openModalDialog(el) {
        var $el = $(el);
        if (IsNullOrUndefined($el.attr("data-url")))
            $el = $(el.target);

        var sUrl = $el.attr("data-url");

        if (elClickDisabled($el))
            return null;

        if (IsNullOrWhiteSpace(sUrl))
            return null;

        var elWidth = null;
        if (!IsNullOrUndefined($(el).attr('data-modal-options-width')))
            elWidth = $(el).attr('data-modal-options-width');

        $(document).on("modalDialog:Display", function (e, args) {
            var $el = $("*[data-url='" + args.uri + "']");

            elResetClickDisabled($el);
        });
        if (IsNullOrUndefined(elWidth)) {
            return showModalDialog(sUrl);
        } else {
            var sOptions = { Width: elWidth };
            showModalDialog(sUrl, null, sOptions);
        }
    }

    function elResetClickDisabled(el) {
        $(el).attr("data-click-count", "0");
        $(el).prop('disabled', false);
        $(el).removeClass('ui-state-disabled');
    }

    function showModalDialog(sUri, fFunc, aOptions, data, closeOnOusideClick) {
        if (IsNullOrUndefined(data))
            data = '';

        var mm = $('body').modalmanager('loading');
        mm = mm.data('modalmanager');
        var $modal = null;
        if (mm.hasOpenModal()) {
            var inx = mm.getOpenModals().length + 1;
            var name = "ajax-modal-stack" + inx;
            var stackContener = '<div tabindex="-1" class="modal fade" id="' + name + '" style="display: none;" data-focus-on="input:first"></div>'
            $('body').append(stackContener);
            $modal = $('#' + name);
        } else {
            $modal = $('#ajax-modal');
        }
        setTimeout(function () {
            $modal.load(sUri, data, function () {
                try {
                    setModalDialogWithContent($modal, sUri, null, fFunc, aOptions, closeOnOusideClick);
                    //setUpTabClick();
                } catch (e) {
                    console.log(e);
                }
            });
        }, 5);

        return $modal;
    }

    function setModalDialogWithContent($modal, sUri, sContent, fFunc, aOptions, closeOnOusideClick) {
        
        if (IsNullOrUndefined(sContent) == false)
            $modal.html(sContent);

        if (IsNullOrUndefined(closeOnOusideClick))
            closeOnOusideClick = 'static';  // treba proslediti 'true' ukoliko zelimo da klik van forme gasi formu

        if (IsNullOrUndefined(aOptions))
            aOptions = {};

        var innerHeight = $(".modal-layout-2").height();
        var innerWidth = $("#ajax-modal").width();
        
        aOptions.Width = $(window).width() * 0.9;
        aOptions.Height = $(window).height() *0.95;
        aOptions.MaxWidth = $(window).width() * 0.97;
        //if (IsNullOrUndefined(aOptions))
        //    aOptions = { Width: "640px" };

        //if ($modal.find(".modal-layout-2").length > 0) {
        //    aOptions.Width = $(window).width() * 0.9 + "px";
        //}

        if (innerHeight > aOptions.Height - 200 || innerHeight < 100) {
            aOptions.MaxHeight = $(window).height() * 0.97;
        } else {
            aOptions.Height = innerHeight + 160;
            aOptions.MaxHeight = innerHeight + 200;
        }

        if ($modal.find('.stretch').length > 0) {
        } else {
            aOptions.Width = innerWidth
            aOptions.MaxWidth = innerWidth + 10;
        }

        if (aOptions.Height)
            $modal.css("height", aOptions.Height + "px");

        if (aOptions.width)
            $modal.css("width", aOptions.Width + "px");

        $('button.close').on('click', function () {
            closeModalDialog("eventX");
        });

        $modal.keydown(function (e) {
            currentKeyCodes['x' + e.keyCode] = true;
        });

        $modal.keyup(function (e) {
            if ((e.keyCode == 27) && currentKeyCodes['x27']) {
                closeModalDialog("eventEsc");
            }

            var s = 'x' + e.keyCode;
            if (currentKeyCodes[s])
                currentKeyCodes[s] = false;
        });

        $modal.css("top", Math.max(0, ($(window).height() - aOptions.Height) / 2) + "px");
        $modal.css("max-width", aOptions.MaxWidth + "px");
        $modal.css("width", aOptions.Width + "px");
        $modal.css("left", Math.max(0, ($(window).width() - aOptions.Width) / 2) + "px");
        setTimeout(function () { $modal.css("margin-top", "0px") },50);

        height = $modal.height();
        $modal.css('max-height', aOptions.MaxHeight + "px");
        var body = $modal.find('.modal-body')[0];
        //if ((height + 10) > ($(window).height() * 0.9)) {
        //    //$(body).css('height', $(window).height() *0.57);
        //} else
        //    // $(body).css('max-height', $(window).height() * 0.57);

        $(body).css('width', aOptions.Width + "px");
        $(body).css('overflow-y', 'auto');
        $modal.modal({
            backdrop: closeOnOusideClick
        });
        setUpModelForm($modal);

        if (fFunc)
            TryCatchWraper(function () { fFunc() });

        var eventArgs = new Object();
        eventArgs.uri = sUri;
        $(document).trigger("modalDialog:Display", eventArgs);
        $(document).on("bs.modal.resize", resizeModal);
        $(document).trigger('bs.modal.resize');
    }

    function resizeModal() {
        var $modal = $('#ajax-modal');

        var innerHeight = $(".modal-layout-2").height();
        var innerWidth = $("#ajax-modal").width();

        var aOptions = {};

        aOptions.Height = $(window).height() * 0.95;
        aOptions.MaxWidth = $(window).width() * 0.97;
        aOptions.Width = $(window).width() * 0.9;

        if (innerHeight > aOptions.Height-200) {
            aOptions.MaxHeight = $(window).height() * 0.97;
        } else {
            aOptions.Height = innerHeight + 160;
            aOptions.MaxHeight = innerHeight + 200;
        }

        if ($modal.find('.stretch').length > 0) {

        } else {
            aOptions.Width = innerWidth
            aOptions.MaxHeight = innerWidth + 10;
        }

        if (aOptions.Height)
            $modal.css("height", aOptions.Height + "px");

        
        if (aOptions.width)
            $modal.css("width", aOptions.Width + "px");

        $modal.css("top", Math.max(0, ($(window).height() - aOptions.Height) / 2) + "px");
        $modal.css("max-width", aOptions.MaxWidth + "px");
        $modal.css("width", aOptions.Width + "px");
        $modal.css("left", Math.max(0, ($(window).width() - aOptions.Width) / 2) + "px");
        setTimeout(function () { $modal.css("margin-top", "0px") }, 50);

        $modal.css('max-height', aOptions.MaxHeight + "px");
        var body = $modal.find('.modal-body')[0];
        $(body).css('width', aOptions.Width + "px");
        
    }
    function windowResize() {
        if ($('#ajax-modal').length > 0) {
            $(document).trigger("bs.modal.resize");
        }
    }
    var currentKeyCodes = new Object();

    function closeModalDialog(text) {

        var mm = $('body').modalmanager();
        mm = mm.data('modalmanager');

        ClosePageLoader();

        if (IsNullOrWhiteSpace(text) == false && (text == "eventEsc" || text == "eventX")) {
            var inx = mm.getOpenModals().length;
            var dlg = mm.getOpenModals()[inx - 1];

            if (inx == 1) {
                closeAjaxModalDialog();
            } else {
                if (IsNullOrUndefined(dlg) == false)
                    mm.destroyModal(dlg);

                var name = "ajax-modal-stack" + inx;

                $('#' + name).html('');
                $('#' + name).remove();
            }
        } else if (mm.getOpenModals().length > 1) {
            var inx = mm.getOpenModals().length - 1;
            var dlg = mm.getOpenModals()[inx];
            mm.destroyModal(dlg);
            var name = "ajax-modal-stack" + inx;

            $('#' + name).html('');
            $('#' + name).modal('hide');
        } else {
            if ($('#ajax-modal').is(":visible")) {
                closeAjaxModalDialog();
            }
        }
    }

    function closeAjaxModalDialog(modal) {

        $(document).off("bs.modal.resize");
        if (IsNullOrUndefined(modal))
            modal = '#ajax-modal';

        $(modal).modal('hide');
        $(modal).html('');
        $(modal).remove();
        $('.modal-backdrop').remove();
        $('.modal-scrollable').remove();
        if ($('#ajax-modal').length == 0) {
            $('body').append('<div id="ajax-modal" class="modal fade" tabindex="-1" style="display: none;"></div>');
        }
    }

    function setUpModelForm(el) {
        var id = "";

        if (el) {
            var rootId = $(el).attr("id");

            if (rootId)
                id = "#" + rootId + " ";
        }
        

        setUpClearOnInput(id);
        setUpTypeHead(id);
        setUpFloatLabels(id);
        setUpSelect(id);

        TryCatchWraper(function () {
            $(id + '.dropdown-toggle').dropdown();
        });

        //TryCatchWraper(function () {
        //    $(id + '.money2').autoNumeric('init');
        //});

        //TryCatchWraper(function () {
        //    $(id + '.money4').autoNumeric('init', { mDec: 4 });
        //});

        //TryCatchWraper(function () {
        //    $(id + ".starrr").starrr();
        //});

        TryCatchWraper(function () {
            $(id).find('[data-money]').each(function (index) {
                var th = $(this);
                var an = AutoNumeric.getAutoNumericElement('input[name="' + th.attr('name') + '"][data-money]');
                if (an === undefined || an === null)
                    an = new AutoNumeric('input[name="' + th.attr('name') + '"][data-money]', 0, autoNumericOptionsEuro);
                th.blur();
            });
            
        })
        TryCatchWraper(function () {
            $(id + '.field-date input').each(function (index) {
                var $el = $(this);
                var format = getDateFormat($el);

                $el.datepicker({
                    format: format.toLowerCase(),
                    autoclose: true,
                    container: "body",
                    disableEntry: false,
                    orientation: "auto",
                    keyboardNavigation: true,
                    disableTouchKeyboard: false
                });
                $el.keyup();
            });
        });

        TryCatchWraper(function () {
            $(id + 'input.field-dataperiod').on('apply.daterangepicker', function (ev, picker) {
                var $el = $(ev.target);
                var format = getDateFormat($el);
                $el.attr("data-selected-start-date", picker.startDate.format(format));
                $el.attr("data-selected-end-date", picker.endDate.format(format));
            });
        });

        TryCatchWraper(function () {
            $(id + 'input.field-timeperiod').on('apply.daterangepicker', function (ev, picker) {
                var $el = $(ev.target);
                var format = getDateTimeFormat($el);
                $el.attr("data-selected-start-date", picker.startDate.format(format));
                $el.attr("data-selected-end-date", picker.endDate.format(format));
            });
        });
        
        setupValidate(id);
        // setUpEditable(id);

       // $("#Name").trigger("click");
        
    }

    function startsWithTwo(str, prefix) {
        if (IsNullOrWhiteSpace(str))
            return false;
        if (IsNullOrWhiteSpace(prefix))
            return false;
        if (str.length < prefix.length)
            return false;
        for (var i = prefix.length - 1; (i >= 0) && (str[i] === prefix[i]); --i)
            continue;
        return i < 0;
    }

    function formatWithoutId(repo) {
        if (repo.loading)
            return repo.text;

        var markup = '<div class="clearfix">';
        try {
            markup += repo.text;
            if (repo.Description)
                markup += "<br /> <i>" + repo.Description + '</i>';
        } catch (ex) { }

        markup += '</div>';

        return markup;
    }
    function formatWithoutIdSelection(repo) {
        return repo.text;
    }
    function formatRepo(repo) {
        if (repo.loading)
            return repo.text;

        var markup = '<div class="clearfix">';
        try {

            if (repo.Value)
                markup = markup + repo.Value + " " + repo.text;
            else if (repo.id)
                markup = markup + repo.id + " " + repo.text;
            else
                markup = markup + repo.text;

            if (repo.Description)
                markup = markup + "<br /> <i>" + repo.Description + '</i>';
        } catch (ex) {

        }

        markup = markup + '</div>';

        return markup;
    }

    function setSelected(sValue, sSelector) {
        if ((IsNullOrUndefined(sValue)) || (IsNullOrUndefined(sValue.Id) == false && sValue.Id == 0))
            return;

        sSelector = sSelector.replace("-ddl", "");

        var el = $(sSelector + "-ddl");

        if (el.length == 0) {
            var tmp = $(sSelector);

            if (tmp.length == 0) {
                return;
            } else {
                sSelector = tmp.attr("id");
                sSelector = "#" + sSelector;
                el = $(sSelector + "-ddl");
                if (el.length == 0)
                    return;
            }
        }
        var select2Data = null;
        if (startsWithTwo(sSelector, "#") == false && startsWithTwo(sSelector, ".") == false)
            sSelector = "#" + sSelector;
        try {
            if (IsNullOrUndefined(sValue.Value) == false && isNaN(sValue.Value) == false) {
                sValue = sValue.Value;
                var n = $(sSelector + '-ddl option[value="' + sValue + '"]').text();
                if (IsNullOrWhiteSpace(n)) {
                    return;
                }
                select2Data = { id: sValue, text: n, Data: sValue };
            }
            else if (IsNullOrUndefined(sValue.Id)) {
                select2Data = sValue;
            } else {
                select2Data = { id: sValue.Id, text: (IsNullOrWhiteSpace(sValue.DisplayName) ? sValue.Name : sValue.DisplayName), Data: sValue };
                sValue = sValue.Id;
            }
        }
        catch (err) {
            select2Data = sValue;
        }

        el.attr("data-default-value", sValue);
        el.val(select2Data);
        TryCatchWraper(function () { el.val(select2Data).trigger("change") });
        if (isNaN(sValue) == false) {

            var tmp = $(sSelector + '-ddl option[value="' + sValue + '"]');

            if (IsNullOrUndefined(tmp) || tmp.length == 0) {

                if ($(sSelector + '-ddl option:eq(0)').attr("value") == "")
                    sValue = sValue + 1;
                $(sSelector + '-ddl option:eq(' + sValue + ')').prop("selected", true);
            }
            else {
                tmp.prop("selected", true);
                TryCatchWraper(function () {
                    $(sSelector + '-ddl').trigger("change")
                });

            }
        }
        else if (Array.isArray(sValue)) {
            $.each(sValue, function (key, value) {
                var tmp = $(sSelector + '-ddl option[value="' + value + '"]');
                if (!IsNullOrUndefined(tmp) && tmp.length > 0) {
                    tmp.prop("selected", true);
                }
            });
        }

        if (el.hasClass("chosen-select")) {
            el.trigger("chosen:updated");
        }
    }

    function formatRepoSelection(repo) {
        var data = "";
        try {
            if (IsNullOrUndefined(repo.Value) == false)
                return repo.Value + " " + repo.text;
            else
                return repo.id + " " + repo.text;
        }
        catch (er1) {
            try {
                return repo.id + " " + repo.text;
            }
            catch (er2) {
                return "";
            }
        }
    }

    function onInputChange(elem, isValid) {
        var icon = $($(elem).parent().find("i")[0]);

        var isEdit = true;

        //if (isEdit) {
        //    if (IsNullOrUndefined(isValid)) {
        //        isValid = elem.checkValidity();
        //        if (!isValid)
        //            isValid = $(elem).valid();
        //    }
        //    try {
        //        if ($(elem).val() == '' && isValid) {
        //            isValid = $(elem).valid();
        //        }
        //    } catch (e1) {

        //    }
        //}

        icon.attr('valid', isValid);

        //if (icon.hasClass("fa-terminal")) {
        //    if (isValid && isEdit)
        //        icon.removeClass('fa-terminal').addClass('fa-check');
        //    else
        //        icon.removeClass('fa-check').addClass('fa-terminal');
        //}
        icon.css('color', '');
        //if (isValid) {
        //    icon.css('color', '#5cb85c');
        //}

        //if (isValid && !isEdit) {
        //    icon.css('color', '#999');
        //}

        //if (!isValid && isEdit)
        //    icon.css('color', '#d9534f');

        if (isEdit) {
            $(elem).attr('data-edit', 'true');
            icon.attr('data-edit', 'true');
        }
        else {
            $(elem).attr('data-edit', 'false');
            icon.attr('data-edit', 'false');
        }

    }

    function setupValidate(id) {
        if (!id)
            id = "";
        $(id + 'input[data-val=true]').on('blur', function () {
            fnValidateDynamicContent($(this));
            var isValid = $(this).valid();
            $(".tooltip").css('left', '0.2px');
            onInputChange($(this), isValid);
        });
        $(id + 'input[data-val=true]').on('focus', function () {
            $(".tooltip").css('left', '0.2px');
        });
        $(id + 'textarea[data-val=true]').on('blur', function () {
            fnValidateDynamicContent($(this));
            var isValid = $(this).valid();
            $(".tooltip").css('left', '0.2px');
            onInputChange($(this), isValid);
        });
        $(id + 'textarea[data-val=true]').on('focus', function () {
            $(".tooltip").css('left', '0.2px');
        });
    }

    function TryCatchWraper(func) {
        try { func(); } catch (ex) { logConsole("TryCatchWraper", ex); }
    }

    function setUpSelect(id) {
        if (!id)
            id = "";
        TryCatchWraper(function () {

            $(id + '.selectTwo').each(function (i, el) {

                var dataDefaultValue = $(this).attr("data-default-value");
                var elementId = "#" + $(this).attr("Id");

                var url = $(this).attr("data-url");
                //var inqInputProps = $(this).attr("data-inquiry-input-props");

                var dataFormat = $(this).attr("data-format");
                var dataFormatSelection = $(this).attr("data-format-selection");
                var dataValueIdentifier = $(this).attr("data-value-identifier");
                var dataWidth = $(this).attr("data-width");

                if (IsNullOrEmpty(dataWidth) || IsNullOrUndefined(dataWidth))
                    dataWidth = 'auto';

                if (IsNullOrUndefined(url) == false && startsWithTwo(url, "http") == false)
                    url = sRootUrl + url;

                var dataMinLength = $(this).attr("data-min-length");

                if (IsNullOrUndefined(dataMinLength))
                    dataMinLength = 0;
                else
                    dataMinLength = parseInt(dataMinLength);

                var placeholderText = $(this).attr("placeholder");
                if (IsNullOrWhiteSpace(placeholderText))
                    placeholderText = "Please Select Value";

                var multipleAttribute = $(this).attr("multiple");
                if (IsNullOrWhiteSpace(multipleAttribute))
                    multipleAttribute = false;

                if (IsNullOrUndefined(dataFormat))
                    dataFormat = formatRepo;
                else
                    dataFormat = eval(dataFormat);

                if (IsNullOrUndefined(dataFormatSelection))
                    dataFormatSelection = formatRepoSelection;
                else
                    dataFormatSelection = eval(dataFormatSelection);

                if (IsNullOrUndefined(url)) {

                    $(this).select2({
                        allowClear: true,
                        placeholder: 'Existing Value',
                        multiple: multipleAttribute,
                        tags: true,
                        tokenSeparators: [',', '  ']
                    });

                    $(this).prop('tabindex', $(this).attr("data-tabindex"));

                    if (IsNullOrUndefined(dataDefaultValue) == false) {
                        setSelected(dataDefaultValue, elementId);
                        $(elementId).attr("data-edit", "true");
                    }
                }
                else {

                    if (dataMinLength == 0) {
                        var data = [{ id: 0, text: '' }];
                        var el = $(this);
                        el.prop("disabled", true);
                        var sel = el.parent().find("#field-spinner").first();
                        sel.addClass('loadinggif');

                        $.ajax({
                            url: url,
                            cache: false,
                            dataType: "json",
                            headers: GetWebApiHeaders(),
                            success: function (data, textStatus, xhr) {
                                el.attr("data-loaded", "true");

                                var select2Data = [];
                                var dataLen = data.length;
                                for (i = 0; i < dataLen; ++i) {
                                    var newObj = new Object();
                                    var d = data[i];

                                    if (IsNullOrUndefined(dataValueIdentifier) == false && dataValueIdentifier != "") {
                                        try {
                                            var identifier = d[dataValueIdentifier];
                                            newObj.id = identifier;
                                        } catch (er) {
                                            newObj.id = d.Id;
                                        };
                                    }
                                    else {
                                        newObj.id = d.Id;
                                    }

                                    if (IsNullOrWhiteSpace(d.DisplayName))
                                        newObj.text = d.Name
                                    else
                                        newObj.text = d.DisplayName;

                                    if (IsNullOrWhiteSpace(d.Value))
                                        newObj.Value = d.Id;
                                    else
                                        newObj.Value = d.Value;

                                    if (IsNullOrWhiteSpace(d.Description) == false)
                                        newObj.Description = d.Description;

                                    select2Data.push(newObj);
                                }

                                var disabledSelect = false;
                                if (IsNullOrUndefined(el.attr("data-disable-select")) == false)
                                    disabledSelect = el.attr("data-disable-select");

                                el.prop("disabled", disabledSelect);

                                sel.removeClass('loadinggif');

                                el.select2({
                                    id: function (bond) { return bond._id; },
                                    placeholder: placeholderText,
                                    multiple: multipleAttribute,
                                    minimumResultsForSearch: Infinity,
                                    data: select2Data,
                                    escapeMarkup: function (markup) { return markup; },
                                    minimumInputLength: dataMinLength,
                                    templateResult: dataFormat,
                                    templateSelection: dataFormatSelection
                                });

                                $(document).trigger("select2:load");

                                if (IsNullOrWhiteSpace(el.val()) == false)
                                    el.select2("val", "");

                                el.prop('tabindex', $(this).attr("data-tabindex"));

                                dataDefaultValue = el.attr("data-default-value");
                                elementId = "#" + el.attr("Id");

                                if (IsNullOrWhiteSpace(dataDefaultValue) == false) {
                                    setSelected(dataDefaultValue, elementId);
                                    $(elementId).attr("data-edit", "true");
                                }
                            }
                        });


                    }
                    else {
                        var tInx = $(this).attr("data-tabindex");

                        $(this).select2({
                            id: function (bond) { return bond._id; },
                            multiple: false,
                            ajax: {
                                url: url,
                                dataType: 'json',
                                delay: 250,
                                data: function (params) {

                                    return {
                                        SearchTerm: params.term,
                                        Page: params.page
                                    };
                                },
                                processResults: function (data, page) {

                                    var select2Data = $.map(data, function (obj) {
                                        var newObj = new Object();

                                        newObj.id = obj.Id;

                                        if (IsNullOrWhiteSpace(obj.DisplayName))
                                            newObj.text = obj.Name
                                        else
                                            newObj.text = obj.DisplayName;

                                        if (IsNullOrWhiteSpace(obj.Value))
                                            newObj.Value = obj.Id;
                                        else
                                            newObj.Value = obj.Value;

                                        if (IsNullOrWhiteSpace(obj.Description) == false)
                                            newObj.Description = obj.Description;

                                        return newObj;
                                    });

                                    return {
                                        results: select2Data
                                    };
                                },
                                cache: true
                            },
                            escapeMarkup: function (markup) { return markup; },
                            minimumInputLength: dataMinLength,
                            templateResult: dataFormat,
                            templateSelection: dataFormatSelection
                        });

                        $(this).prop('tabindex', $(this).attr("data-tabindex"));
                        $('span[aria-labelledby="select2-' + $(this).attr("id") + '-container"]').attr('tabindex', tInx);
                        $('span[aria-labelledby="select2-' + $(this).attr("id") + '-container"]').attr('data-tabindex', tInx);
                        $('span[id="select2-' + $(this).attr("id") + '-container"]').attr('tabindex', tInx);
                        $('span[id="select2-' + $(this).attr("id") + '-container"]').attr('data-tabindex', tInx);
                    }
                }
            });
        });
    }

    function setUpFloatLabels(id) {
        if (!id)
            id = "";
        $(id + "input:not([data-float-label-set])[type='email']").on("change paste keyup blur mousedown", function () {
            onInputChange(this);
        });

        $(id + "input:not([data-float-label-set])[type='text']").on("change paste keyup blur mousedown", function () {
            onInputChange(this);
        });

        $(id + "textarea:not([data-float-label-set])").on("change paste keyup blur mousedown", function () {
            onInputChange(this);
        });

        $(id + '.float-label-control').floatLabels();

        $(id + '.float-label-control label').on("click", function () {
            var fr = $(this).attr("for");
            setFocus(fr);
        });
    }

    function setUpTypeHead(id) {
        if (!id)
            id = "";
        $(id + '.typeahead2').each(function (i, el) {

            var dataRemote = eval($(this).attr("data-remote"));

            dataRemote.initialize();

            var sessionKey = $(this).attr("data-recent");

            var dataRecent = eval(sessionKey);

            dataRecent.initialize();
            dataRecent.clearRemoteCache();

            $(this).typeahead({
                autoselect: true,
                minLength: 1
            },
                {
                    name: 'remote',
                    displayKey: function (data) {
                        return data.name;// + " - " + data.number;
                    },
                    source: dataRemote.ttAdapter(),
                    templates: {
                        header: '<h3 class="heading"><small>&nbsp;&nbsp;Suggestion</small></h3>',
                        empty: [
                            '<div class="empty-message">',
                            'Unable to find any data that match the current query',
                            '</div>'
                        ].join('\n')
                    }
                }).on('typeahead:selected', function (obj, data) {

                    TryCatchWraper(function () {

                        //var recentItems = $.sessionStorage.get(sessionKey)
                        var recentItems = sessionStorage.getItem(sessionKey)
                        if (!recentItems)
                            recentItems = new Array();

                        var found = -1;

                        for (var i = 0; i < recentItems.length; i++) {
                            if (data.Id == recentItems[i].Id) {
                                found = i;
                                break;
                            }
                        }

                        if (found == -1) {

                            if (recentItems.length > 5)
                                var i = recentItems.shift();

                            recentItems.push(data);

                            //$.sessionStorage.set(sessionKey, recentItems);
                            sessionStorage.setItem(sessionKey, JSON.stringify(recentItems));
                        }
                    });

                    var oldValue = $(this).attr("data-display-text");
                    var oldId = $(this).attr("data-selected-id");
                    var oldNumber = $(this).attr("data-selected-number");
                    var oldItem = $(this).attr("data-selected-item");
                    var oldType = $(this).attr("data-selected-type");

                    $(this).attr("data-selected-item", data);
                    $(this).attr("data-selected-id", data.lId);
                    $(this).attr("data-selected-value", data.name);
                    $(this).attr("data-selected-number", data.number);

                    var dataTypeName = "";

                    if (IsNullOrWhiteSpace(data.type))
                        dataTypeName = data.typeName;
                    else
                        dataTypeName = data.type;

                    $(this).attr("data-selected-type", dataTypeName);
                    $(this).attr("data-display-text", $(this).val());

                    if (oldId != data.lId || oldNumber != data.number || dataTypeName != oldType)
                        resetInquiryFields($(this).attr("data-inq-props-to-reset"), $(this).closest("form").attr("id"));

                    var eventArgs = new Object();
                    eventArgs.event = obj;
                    eventArgs.isSelected = true;
                    eventArgs.data = data;
                    eventArgs.current = $(this).val();
                    eventArgs.previous = oldValue;
                    eventArgs.previousData = oldItem;
                    eventArgs.previousId = oldId;
                    eventArgs.previousNumber = oldNumber;
                    eventArgs.previoudType = oldType;

                    $(this).trigger("lookup:changevalue", eventArgs);

                    if (!IsNullOrUndefined(data.funcCall)) {
                        $(this).attr("data-selected-function", data.funcCall);
                        $(this).trigger(data.funcCall);
                    }

                }).on('typeahead:autocompleted', function (obj, data) {
                    var id = ":";
                    var oldValue = $(this).attr("data-display-text");
                    var oldId = $(this).attr("data-selected-id");
                    var oldNumber = $(this).attr("data-selected-number");
                    var oldItem = $(this).attr("data-selected-item");
                    var oldType = $(this).attr("data-selected-type");

                    $(this).attr("data-selected-item", data);
                    $(this).attr("data-selected-id", data.lId);
                    $(this).attr("data-selected-value", data.name);
                    $(this).attr("data-selected-number", data.number);

                    if (IsNullOrWhiteSpace(data.type))
                        $(this).attr("data-selected-type", data.typeName);
                    else
                        $(this).attr("data-selected-type", data.type);

                    $(this).attr("data-display-text", $(this).val());

                    var eventArgs = new Object();
                    eventArgs.event = obj;
                    eventArgs.isSelected = true;
                    eventArgs.data = data;
                    eventArgs.current = $(this).val();
                    eventArgs.previous = oldValue;
                    eventArgs.previousData = oldItem;
                    eventArgs.previousId = oldId;
                    eventArgs.previousNumber = oldNumber;
                    eventArgs.previoudType = oldType;

                    $(this).trigger("lookup:changevalue", eventArgs);

                    if (!IsNullOrUndefined(data.funcCall)) {
                        $(this).attr("data-selected-function", data.funcCall);
                        $(this).trigger(data.funcCall);
                    }
                });
        });

        $(id + '.typeahead2').on('change', function (evt, data) {

            var self = $(this);

            var oldValue = self.attr("data-display-text");
            var oldItemId = self.attr("data-selected-id");
            var oldItem = null;
            var value = self.val();

            var isValid = true;

            if (IsNullOrUndefined(oldValue) == false && value != oldValue) {

                if (value)
                    isValid = false;
                else
                    isValid = true;

                oldItem = self.attr("data-selected-item");
                oldItemId = self.attr("data-selected-id");
                oldItemValue = self.attr("data-selected-value");

                self.attr("data-selected-item", null);
                self.attr("data-selected-id", null);
                self.attr("data-selected-value", null);

                resetInquiryFields($(this).attr("data-inq-props-to-reset"), $(this).closest("form").attr("id"));
            };

            if (!isValid) {
                self.css('border-bottom-color', '#d9534f');
            } else {
                self.css('border-bottom-color', 'none');
            }

            var eventArgs = new Object();
            eventArgs.event = evt;
            eventArgs.isSelected = false;
            eventArgs.current = value;
            eventArgs.previous = oldValue;
            eventArgs.previousData = oldItem;
            eventArgs.previousId = oldItemId;

            return self.trigger("lookup:changevalue", eventArgs);
        });


        $(id + '.typeahead2').on('cleared', function () {
            var self = $(this);

            var oldValue = self.attr("data-display-text");
            var oldItemId = self.attr("data-selected-id");
            var oldItem = null;
            var value = self.val();
            var selectedFunc = self.attr("data-selected-function");

            var isValid = true;

            if (value != oldValue) {

                if (value)
                    isValid = false;
                else
                    isValid = true;

                oldItem = self.attr("data-selected-item");
                oldItemId = self.attr("data-selected-id");
                oldItemValue = self.attr("data-selected-value");

                self.attr("data-selected-item", null);
                self.attr("data-selected-id", null);
                self.attr("data-selected-value", null);
            };

            if (!value) {
                self.typeahead('val', '');
                TryCatchWraper(function () {
                    self.typeahead('val', '').blur();
                });
            }

            if (!isValid) {
                self.css('border-bottom-color', '#d9534f');
            } else {
                self.css('border-bottom-color', 'none');
            }

            var eventArgs = new Object();

            eventArgs.isSelected = false;
            eventArgs.current = value;
            eventArgs.previous = oldValue;
            eventArgs.previousData = oldItem;
            eventArgs.previousId = oldItemId;

            if (!IsNullOrUndefined(selectedFunc)) {
                self.trigger(selectedFunc);
            }

            return self.trigger("lookup:changevalue", eventArgs);
        });

        $(id + '.typeahead2').on('focus', function () {

            var sKey = $(this).attr("data-recent");

            //var dataRecent = $.sessionStorage.get(sKey);
            var dataRecent = sessionStorage.getItem(sKey);

            var hasRecent = false

            if (dataRecent && dataRecent.length > 0) {
                hasRecent = true;
            }

            if ($(this).hasClass("typeahead2") && $(this).hasClass("tt-input")) {

                $field = $(this);
                $this = $field.parent().parent();

                $hint = $this.find('.tt-hint').first();
                $label = $this.find('label').first();

                if ($field.val() || $hint.val() || hasRecent) {
                    $field.removeClass('empty');
                    if ($this)
                        $this.removeClass('empty');
                    $label.text($label.data('original-text'));
                } else {

                    if ($field.hasClass("empty") == false)
                        $field.addClass('empty');

                    if ($this && $this.hasClass("empty") == false)
                        $this.addClass('empty');

                    if ($field.hasClass("empty") == false || ($this && $this.hasClass("empty") == false))
                        $label.text($label.data('placeholder-text'));
                }
            }
        });
    }
    function setEditableNumber(el) {
        $(el).editable({
            type: "number",
            step: "any",
            validate: function (value) {
                var regexp = /^\d+(\.\d{1,2})?$/i;

                if ($(this).attr("data-representing-type") == "Number4")
                    regexp = /^\d+(\.\d{1,4})?$/i;

                if (!regexp.test(value))
                    return "This field is not valid";
            }
        });
    };
    function setUpClearOnInput(id) {
        if (!id)
            id = "";

        $(id + "input").bind("mouseup", function (e) {
            var $input = $(this),
                oldValue = $input.val();

            if (oldValue == "") return;

            setTimeout(function () {
                var newValue = $input.val();

                if (newValue == "") {

                    $input.trigger("cleared");
                }
            }, 1);
        });
    }

    var addTypeaheadSelectedEdiable = true;
    //http://stackoverflow.com/questions/19637281/x-editable-bootstrap-3-twitter-typeahead-js-not-working
    function setupEdiableTypeahead(el) {

        var id = $(el).attr("data-local-id");

        var value = $(el).text();

        var remoteUrl = eval($(el).attr("data-remote"));
        if (IsNullOrUndefined(remoteUrl))
            return;

        remoteUrl = remoteUrl.replace("remote_", "remote_uri_");

        var ajax = {
            type: 'post', dataType: 'json', headers: { 'cache-control': 'no-cache' }
        };

        var dataRemote = new Bloodhound({
            datumTokenizer: function (d) {
                return Bloodhound.tokenizers.whitespace(d.name);
            },
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            name: 'remote',
            remote: {
                url: remoteUrl,
                ajax: ajax,
                filter: function (parsedResponse) {
                    var result = (parsedResponse.length >= 1) ? parsedResponse : [];
                    try {
                        return $.map(result, function (i) {

                            return {
                                number: i.Number, name: i.FullName.Value, id: i.Id, lId: i.LocalId, type: i.TypeName, version: i.Version,
                                tokens: [' 0 ', i.FullName.FirstName, i.FullName.LastName, i.Number]
                            };
                        });
                    }
                    catch (er3) {
                        return $.map(result, function (i) {

                            return {
                                number: i.Number, name: i.Name, id: i.Id, lId: i.LocalId, type: i.TypeName, version: i.Version,
                                tokens: [' 0 ', i.Name, i.Number]
                            };
                        });
                    }
                }
            }
        });

        dataRemote.initialize();

        $(el).editable({
            mode: "inline",
            value: value,
            typeahead: [
                {
                    minLength: 1,
                    highlight: true,
                    hint: true
                },
                {
                    name: 'remote',
                    displayKey: function (data) {
                        return data.name;// + " - " + data.number;
                    },
                    source: dataRemote.ttAdapter(),
                    templates: {
                        header: '<h3 class="heading"><small>&nbsp;&nbsp;Suggestion</small></h3>',
                        empty: [
                            '<div class="empty-message">',
                            'Unable to find any data that match the current query',
                            '</div>'
                        ].join('\n')
                    }
                }
            ]
        }).on('shown', function (e, editable) {
            $(editable.input.$input[0]).select();
        }).on('hidden', function (e, editable) {
            var v = editable.value;
        });

        if (addTypeaheadSelectedEdiable) {
            addTypeaheadSelectedEdiable = false;
            $('body').on('typeahead:selected', '.tt-input', function (obj, datum, name) {
                try {
                    var $el = $(obj.target).closest('td').find('span.cell-display-mode[data-type="typeaheadjs"]');
                    if ($el.length) {

                        var dataTypeName = $el.attr("data-type-name");

                        var filedValue = new Object();

                        filedValue.Id = datum.lId;
                        filedValue.Name = datum.name;
                        filedValue.Number = datum.number;
                        filedValue.TypeName = dataTypeName;
                        filedValue.Version = datum.version;

                        $el.attr("data-orginal-value", JSON.stringify(filedValue));
                        $el.text(filedValue.Name);
                        $el.editable('setValue', filedValue.Name);
                        $el.addClass("editable-unsaved");
                        $el.editable('hide');
                    }
                } catch (e) {

                }
            });
        }
    }

    try {
        //https://github.com/vitalets/x-editable/issues/358
        //http://jsfiddle.net/qzjpr0t7/2/
        (function ($) {
            "use strict";

            var Constructor = function (options) {
                this.init('typeaheadjs', options, Constructor.defaults);
            };

            $.fn.editableutils.inherit(Constructor, $.fn.editabletypes.text);

            $.extend(Constructor.prototype, {
                render: function () {
                    this.renderClear();
                    this.setClass();
                    this.setAttr('placeholder');
                    this.$input.typeahead.apply(this.$input, this.options.typeahead);

                    // copy `input-sm | input-lg` classes to placeholder input
                    if ($.fn.editableform.engine === 'bs3') {
                        if (this.$input.hasClass('input-sm')) {
                            this.$input.siblings('input.tt-hint').addClass('input-sm');
                        }
                        if (this.$input.hasClass('input-lg')) {
                            this.$input.siblings('input.tt-hint').addClass('input-lg');
                        }
                    }
                }
            });

            Constructor.defaults = $.extend({}, $.fn.editabletypes.list.defaults, {

                tpl: '<input type="text">',
                typeahead: null,
                clear: false
            });

            $.fn.editabletypes.typeaheadjs = Constructor;

        }(window.jQuery));
    }
    catch (ex) {

    }

    function loading_init() {
        var el = $('.loading-overlay');
        if (!el || !el.html()) {
            var strHtml = '<div class="loading-overlay"><div class="loading-box"><div class="loader2">Loading...</div></div></div>';
            $('body').append(strHtml);
            el = $('.loading-overlay');
        }
        return el;
    }

    function ShowPageLoader() {
        var overlay = loading_init();
        overlay.addClass("show");
    }
    function ClosePageLoader() {
        $(".loading-overlay").remove();
        $(".loader").hide();
        $("#loadingGrid").hide();
    }

    function fnValidateDynamicContent(element) {
        var currForm = element.closest("form");
        var isSet = currForm.attr("setvalidate");
        if (isSet)
            return;
        currForm.removeData("validator");
        currForm.removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse(currForm);
        currForm.validate();

        $(function () {
            $.each($.validator.methods, function (key, value) {
                $.validator.methods[key] = function () {
                    var el = $(arguments[1]);
                    if (el.is('[placeholder]') && arguments[0] == el.attr('placeholder'))
                        arguments[0] = '';
                    return value.apply(this, arguments);
                };
            });
        });
        currForm.attr("setvalidate", "true");
    }

    function getInvalidFormElements(elForm, bAll) {
        var fields = new Array();
        var sInputSelector = "input[data-edit='true']";
        if (bAll)
            sInputSelector = "*[data-val='true']";

        $(elForm).find(sInputSelector).each(function (index, val) {
            var type = $(this).attr('type');
            var id = $(this).attr('id');
            if ((type == "text" || type == "number" || type == "hidden" || type == "radio" || type == "range" || type == "file")) {
                isValid = $(this).attr('valid');
                if (IsNullOrUndefined(isValid)) {
                    var name = $(this).attr('data-name');
                    if (!IsNullOrUndefined(name)) {
                        var dp = $(this).prev().attr("data-path");
                        if (dp == name) {
                            var valid = $(this).prev().attr('valid');
                            if (valid == "false")
                                isValid = false;
                            else isValid = true;
                        }
                    }
                }
                if (!isValid) {
                    var validationMessage = $(this).get(0).validationMessage;
                    if (IsNullOrWhiteSpace(validationMessage)) {
                        var icon = $($(this).parent().find("i")[0]);
                        validationMessage = icon.attr("data-original-title");
                    }
                    if (IsNullOrWhiteSpace(validationMessage)) {
                        var icon = $($(this).parent().find("i")[0]);
                        validationMessage = icon.attr("data-error-msg");
                    }
                    fields.push({ isValid: isValid, id: id, message: validationMessage });
                }
            }
        });

        return fields;
    }

    function validateForm(elForm) {

        fnValidateDynamicContent($(elForm));

        var element = $(elForm);
        var currForm = element.closest("form");
        var val = currForm.validate();

        var isValid = false;
        isValid = $(elForm).valid();

        $(".validation-summary-errors-sw").css("display", "none");
        $(".pendingErrorLoader").remove();
        var fields = getInvalidFormElements(elForm, true);
        var list = $(".validation-summary-errors-sw ul");
        if (list && fields && fields.length) {            
            list.empty();
            int = 0;
            if (val.pendingRequest > 0) {
                $("#errorLoaderOuter").append("<div class='pendingErrorLoader'></div>");
                $("<li />").html("There is some pending validation, wait for it to finish!<a href='#' class='alert-link' ></a>").appendTo(list);
                int++;
            };
          
                for (var i = 0; i < fields.length; i++) {
                    //if (IsNullOrWhiteSpace(this.message) == false && IsNullOrUndefined(this.isValid)) {
                    //    $("<li />").html(this.message + "! <a href='#' class='alert-link' onclick='setFocus(\"#" + this.id + "\")'>Fokus element!</a>").appendTo(list);
                    //    int++;
                    //}
                    setFocus("#" + fields[i].id);
                    $("#" + fields[i].id).blur();
                   
                    
                }

                //$.each(fields, function () {
                //    if (IsNullOrWhiteSpace(this.message) == false && IsNullOrUndefined(this.isValid)) {
                //        $("<li />").html(this.message + "! <a href='#' class='alert-link' onclick='setFocus(\"#" + this.id + "\")'>Fokus element!</a>").appendTo(list);
                //        int++;
                //    }
                //    setFocus("#" + this.id);
                //    $("#" + this.id).blur();

                //});
                      
          
            if (int > 0) {
                $(".validation-summary-errors-sw").css("display", "block");
                // location.href = "#innerBehaviorTaskHtml";

                return false;
            }
        }

        if (isValid) {

            $(".validation-summary-errors-sw").css("display", "none");
            $(".input-validation-error1-sw").removeClass("input-validation-error");
            return true;
        }
        else {

            if ($(".validation-summary-errors-sw li").length > 0) {
                $(".validation-summary-errors-sw").css("display", "block");
                // location.href = "#innerBehaviorTaskHtml";
            }

            setTimeout(function () {
              var firstEl = $(".field-set:first .field-group").find("[data-val='true']"); 
                firstEl.blur();
            }, 200);            

            return false;
        }
    }

    function numberWithCommas(x) {
        return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
    }

    function setForm(elForm, oData, behDefValues) {

        try {
            if (IsNullOrUndefined(oData))
                return;
            Object.getOwnPropertyNames(oData).forEach(function (val, idx, array) {
                try {
                    var value = oData[val];

                    var bIsArry = $.isArray(value);

                    var name = val;
                    name = name.replace("m_", "");
                    var sSelector = "input[data-filed-name=" + name + "]";

                    if (IsNullOrUndefined(value) && IsNullOrUndefined(behDefValues) == false) {
                        var defVal = behDefValues[name];
                        if (IsNullOrUndefined(defVal) == false)
                            value = defVal;
                    }

                    TryCatchWraper(function () {
                        var el = $(elForm).find(sSelector).first();

                        var represent = $(el).attr('data-representing-type');

                        sSelector = $(elForm).selector + " " + sSelector;

                        if (represent) {

                            if (represent == "Amount")
                                setAmount(value, sSelector);
                            else if (represent == "Lookup")
                                setLookup(value, sSelector);
                            else if (represent == "Boolean")
                                setBoolean(value, sSelector);
                            else if (represent == "Select")
                                setSelected(value, sSelector);
                            else if (represent == "Rating")
                                setRating(value, sSelector);
                            else if (represent == "Range")
                                setRange(value, sSelector);
                            else if (represent == "Date")
                                setDate(value, sSelector);
                            else if (represent == "DatePeriod")
                                setDateRange(value, sSelector)
                            else if (represent == "TimePeriod")
                                setTimeRange(value, sSelector)
                            else if (represent == "Quantity")
                                setQuantity(value, sSelector)
                            else
                                setValue(value, sSelector);
                        } else {
                            setValue(value, sSelector);
                        }
                    });

                    sSelector = "div[class*='field-value'][data-path='" + name + "']";

                    //var el = $(sSelector);
                    var el = $(elForm).find(sSelector).first();

                    if (el.length != 0) {
                        var represent = $(el).attr('data-representing-type');

                        if (represent) {

                            if (represent == "Amount") {
                                setAmountDisplay(value, el)
                            } else if (represent == "Lookup") {
                                setLookupDisplay(value, el);
                            } else if (represent == "Boolean") {
                                setBooleanDisplay(value, el);
                            } else if (represent == "Select") {
                                setSelectedDisplay(value, el);
                            } else if (represent == "Rating") {
                                setRating(value, sSelector);
                            } else if (represent == "Range") {
                                setRangeDisplay(value, sSelector);
                            } else if (represent == "Date") {
                                setDateDisplay(value, sSelector);
                            } else if (represent == "DatePeriod") {
                                setDateRangeDisplay(value, sSelector)
                            } else if (represent == "TimePeriod") {
                                setTimeRangeDisplay(value, sSelector)
                            } else if (represent == "Quantity") {
                                setQuantityDisplay(value, sSelector)
                            } else {
                                setValueDisplay(value, el);
                            }
                        }
                        else {
                            setValue(value, sSelector);
                        }

                        if (IsNullOrUndefined(value)) {
                            var expr = $(el).attr('data-hide-if-empty');
                            if (expr == "True" || expr == "true") {
                                $(el).parent().parent().parent().hide();
                            }
                        }
                    }
                }
                catch (e) {
                    console.log(e.message);
                }
            });

            var id = "#" + $(elForm).attr("id");
            $(id + " div.display-field-container div.field-value[data-show-if-empty='false']").filter(function () {
                var text = $(this).text();
                return IsNullOrWhiteSpace(text) || text == "---";
            }).parent().parent().parent().hide();
        }
        catch (error) {
            console.log(error.message);
        }

        var test = "";
    }

    function getDecimalPartFormat(decimalPlaces) {
        if (IsNullOrUndefined(decimalPlaces))
            decimalPlaces = 2;

        var decimalPartFormat = "";
        if (decimalPlaces > 0) {
            for (var i = 0; i < decimalPlaces; i++)
                decimalPartFormat += "0";

            decimalPartFormat = "." + decimalPartFormat;
        }

        return decimalPartFormat;
    }

    function getCurrencyFormat(el, defaultFormat, decimalPlaces) {
        var format = defaultFormat;

        if (IsNullOrWhiteSpace(format) == false)
            return format;

        if (IsNullOrUndefined(el) == false) {
            var dFormat = $(el).attr("data-format-pattern");
            if (IsNullOrWhiteSpace(dFormat) == false)
                format = dFormat;
        }

        if (IsNullOrWhiteSpace(format)) {

            var lCode = 0;
            if (IsNullOrUndefined(LangCode) == false)
                lCode = LangCode;

            var decimalPartFormat = getDecimalPartFormat(decimalPlaces);

            if (lCode == 1033) {
                format = "0,0" + decimalPartFormat;
                numeral.language('en');
            } else {
                format = "0,0" + decimalPartFormat;
                numeral.language('en');//
            }
        }

        return format;
    }
    function getDateFormat(el, defaultFormat) {

        var format = defaultFormat;

        //Get default
        if (IsNullOrWhiteSpace(format) == false)
            return format;

        //Falback to Filed
        if (IsNullOrUndefined(el) == false) {
            var dFormat = $(el).attr("data-format-pattern");
            if (IsNullOrWhiteSpace(dFormat) == false)
                format = dFormat;
        }

        //Fallback to System fomat based on Active Language
        if (IsNullOrWhiteSpace(format)) {

            var lCode = 0;
            if (IsNullOrUndefined(LangCode) == false)
                lCode = LangCode;

            if (lCode == 2074 || lCode == 3098)
                format = "DD.MM.YYYY";
            else if (lCode == 1033)
                format = "MM/DD/YYYY";
            else
                format = "DD.MM.YYYY";//System Default
        }

        return format;
    }

    function getTimeFormat(el, defaultFormat) {

        var format = defaultFormat;

        //Get default
        if (IsNullOrWhiteSpace(format) == false)
            return format;

        //Falback to Filed
        if (IsNullOrUndefined(el) == false) {
            var dFormat = $(el).attr("data-format-pattern");
            if (IsNullOrWhiteSpace(dFormat) == false)
                format = dFormat;
        }

        //Fallback to System fomat based on Language
        if (IsNullOrWhiteSpace(format)) {
            var lCode = 0;
            if (IsNullOrUndefined(LangCode) == false)
                lCode = LangCode;

            if (lCode == 2074)
                format = "hh:mm";
            else if (lCode == 1033)
                format = "h:mm";
            else
                format = "h:mm";//System Default
        }

        return format;
    }
    function getDateTimeFormat(el, defaultFormat) {
        return getDateFormat(el, defaultFormat) + " " + getTimeFormat(el, defaultFormat);
    }

    function GetTimePeriodUnitOfMeasure(el, fOnData) {
        $el = $(el);
        if (IsNullOrUndefined($el))
            return null;
        var cListUrl = $el.attr("data-url");
        if (IsNullOrWhiteSpace(cListUrl) == false) {
            webApiGet(cListUrl, fOnData, false);
        }
        else {
            cListUrl = sRootUrl + "/api/Classification/DefaultUnitOfTime"
            webApiGet(cListUrl, fOnData, false);
        }
    }
    function onTimePeriodUnitOfMeasureClick(sSelector, sValue, sText) {
        if (IsNullOrUndefined(sValue) == false)
            $(sSelector).attr('data-unit-of-time-id', sValue);
        if (IsNullOrUndefined(sText) == false)
            $(sSelector).attr('data-unit-of-time-name', sText);
        $(sSelector + 'Code').text(sText);
    }
    function getTimeRangeDisplay(sSelector) {
        var el = $(sSelector);

        var timePeriod = new Object();
        timePeriod.UnitOfTimeId = null;
        timePeriod.Value = 0;

        var value = el.attr("data-time-range-value");

        TryCatchWraper(function () {
            timePeriod.Value = parseFloat(value);

            if (isNaN(timePeriod.Value))
                timePeriod.Value = 0;
        });

        var unitOfTimeId = el.attr("data-unit-of-time-id");
        if (IsNullOrUndefined(unitOfTimeId) == false)
            timePeriod.UnitOfTimeId = unitOfTimeId;

        return timePeriod;
    }
    function getTimeRange(sSelector) {
        var timePeriod = new Object();
        timePeriod.UnitOfTimeId = null;
        timePeriod.Value = 0;

        var value = $(sSelector).val();
        var startIndex = 0;
        var pattern = "'";
        while (value.indexOf(pattern, startIndex) != -1) {
            value = value.slice(1, -1);
        }

        TryCatchWraper(function () {
            timePeriod.Value = parseFloat(value);

            if (isNaN(timePeriod.Value))
                timePeriod.Value = 0;
        });

        var unitOfTimeId = $(sSelector).attr("data-unit-of-time-id");
        if (IsNullOrUndefined(unitOfTimeId) == false)
            timePeriod.UnitOfTimeId = unitOfTimeId;


        return timePeriod;
    }
    function getTimeRangeField(sSelector) {
        var value = getTimeRange(sSelector)
        return { Name: $(sSelector).attr("data-filed-name"), Value: JSON.stringify(value) }
    }

    function setTimeRange(value, sSelector) {
        var $el = $(sSelector);
        $el.keyup();
        if (!IsNullOrUndefined(value) && !IsNullOrUndefined(value.Value)) {
            $el.val(value.Value);

        } else {
            return;
        }

        if (IsNullOrWhiteSpace(value.UnitOfTime) == false)
            $el.attr("data-unit-of-time-name", value.UnitOfTime);

        if (IsNullOrUndefined(value.UnitOfTimeId) == false)
            $el.attr("data-unit-of-time-id", value.UnitOfTimeId);
    }

    function setTimeRangeDisplay(value, sSelector) {
        if (IsNullOrUndefined(value))
            return "";

        var $el = $(sSelector);

        $el.attr("data-time-range-value", value.Value);
        if (IsNullOrUndefined(value.UnitOfTimeId) == false)
            $el.attr("data-unit-of-time-id", value.UnitOfTimeId);

        if (IsNullOrUndefined(value.UnitOfTime) == false) {
            if (IsNullOrWhiteSpace(sSelector) == false)
                $el.text(value.Value + " " + getUnitOfTimeName(value.UnitOfTime));
            else
                return value.Value + " " + getUnitOfTimeName(value.UnitOfTime);
        } else if (IsNullOrUndefined(value.UnitOfTimeId) == false) {
            var url = sRootUrl + "api/query/GetAllClassificationValues?Filter=(Id%20eq%20" + value.UnitOfTimeId + "L)&NoCache=true&LangCode=" + LangCode;

            webApiGet(url, function returnText(rData) {

                var text = "";
                if (rData != null && rData[0] != null) {
                    text = value.Value + " " + rData[0].Name;
                } else {
                    text = value.Value;
                }

                if (IsNullOrWhiteSpace(sSelector) == false)
                    $el.text(text);
                else
                    return text;
            }, true, false);
        }
    }
    function getDateDisplay(sSelector) {
        return $(sSelector).text();
    }
    function getDateValue(sSelector) {
        return $(sSelector).val();
    }
    function getDateField(sSelector, format) {
        var date = getDateValue(sSelector);
        format = getDateFormat($(sSelector), format);
        var value = moment.utc(date, format).format();

        return { Name: $(sSelector).attr("data-filed-name"), Value: value + " " };
    }
    function setDate(value, sSelector) {
        if (value != null) {
            var format = getDateFormat($(sSelector));
            $(sSelector).val(moment(value.FormattedDate).format(format));
        }
    }
    function getDateRangeField(sSelector, format) {
        var $el = $(sSelector);

        var dtp = $el.data('daterangepicker');

        format = getDateFormat($el, format);

        var startDate = moment.utc(dtp.startDate, format).format();

        var endDate = moment.utc(dtp.endDate, format).format();

        return { Name: $el.attr("data-filed-name"), Value: { Start: startDate, End: endDate, TypeName: "DatePeriod" } };
    }
    function setDateRange(value, sSelector) {
        var drp = $(sSelector).data('daterangepicker');
        var format = getDateFormat($(sSelector));
        if (IsNullOrUndefined(value))
            return;
        if (IsNullOrUndefined(value.StartFormattedDate) == false) {
            var formated = moment(value.StartFormattedDate).format(format);
            if (IsNullOrUndefined(drp) == false)
                drp.setStartDate(formated);
            $(sSelector).attr("data-selected-start-date", formated);
        }
        if (IsNullOrUndefined(value.EndFormattedDate) == false) {
            var formated = moment(value.EndFormattedDate).format(format);
            if (IsNullOrUndefined(drp) == false)
                drp.setEndDate(formated);
            $(sSelector).attr("data-selected-end-date", formated);
        }
    }

    function setDateDisplay(value, sSelector) {
        if (IsNullOrUndefined(value))
            return "";

        if (IsNullOrWhiteSpace(sSelector) == false)
            $(sSelector).text(returnDateFormated(value));
        else
            return returnDateFormated(value);
    }
    function getDateRangeDisplay(sSelector) {
        var el = $(sSelector);
        var dateRange = new Object();

        var sStart = el.attr("data-selected-start-date");
        var sEnd = el.attr("data-selected-end-date");

        if (IsNullOrWhiteSpace(sStart))
            sStart = "";
        if (IsNullOrWhiteSpace(sEnd))
            sEnd = "";

        dateRange.Start = sStart;
        dateRange.End = sEnd;
        dateRange.TypeName = "DatePeriod";

        return dateRange;
    }
    function setDateRangeDisplay(value, sSelector) {
        if (IsNullOrUndefined(value))
            return "";

        var text = "";
        var sStart = "?";
        var sEnd = "?";

        if (IsNullOrUndefined(value.Start) == false) {
            sStart = returnDateFormated(value.Start)
        }

        if (IsNullOrUndefined(value.End) == false) {
            sEnd = returnDateFormated(value.End);
        }

        text = sStart + " - " + sEnd;

        if (IsNullOrWhiteSpace(sSelector) == false) {
            var $el = $(sSelector);
            $el.text(text);

            if (sStart.length > 1)
                $el.attr("data-selected-start-date", sStart);

            if (sEnd.length > 1)
                $el.attr("data-selected-end-date", sEnd);
        } else
            return text;
    }

    function getUnitOfTimeName(unitOfTime) {
        if (IsNullOrUndefined(unitOfTime) || isNumber(unitOfTime) == false)
            return "";

        if (unitOfTime == 0)
            return "MiliSeconds";
        else if (unitOfTime == 1)
            return "Seconds";
        else if (unitOfTime == 2)
            return "Minutes";
        else if (unitOfTime == 3)
            return "Hours";
        else if (unitOfTime == 4)
            return "Days";
        else if (unitOfTime == 5)
            return "Months";
        else if (unitOfTime == 6)
            return "Years";
        else
            return "";
    }
    function getText(el) {
        return $(el).text();
    }
    function getValue(sSelector) {
        return $(sSelector).val();
    }
    function getValueField(sSelector) {
        if ($(sSelector).attr('data-money') == 'true') {
            var value = AutoNumeric.getLocalized(sSelector);
            return { Name: $(sSelector).attr("data-filed-name"), Value: value };
        }
        return { Name: $(sSelector).attr("data-filed-name"), Value: getValue(sSelector) };
    }
    function setValueDisplay(value, oSelector) {
        if (IsNullOrUndefined(value))
            return;
        var el = $(oSelector);
        var displayValue = value;
        if (IsNullOrUndefined(el) == false) {
            var format = $(el).attr("data-format-pattern");
            if (IsNullOrWhiteSpace(format) == false) {
                var formated = numeral(displayValue);
                displayValue = formated.format(format);
            }
        }

        el.text(value);

    }
    function setValue(value, sSelector) {
        setValue(value, sSelector, false);
    }
    function setValue(value, sSelector, bDefault) {
        if (!IsNullOrUndefined(value)) {

            //if (typeof $(sSelector).attr('type') != 'undefined') {
            //    if ($(sSelector).attr('type') === 'number') {
            //        value = numberWithCommas(value);
            //    }
            //}

            $(sSelector).val(value);
            var sId = $(sSelector).attr('id')
            if ($('#' + sId + "-mlt").length)
                $('#' + sId + "-mlt").first().val(value);
            if (bDefault) {
                $(sSelector).attr("data-default-value", value);
            }
            if ($(sSelector).attr('data-money') == "true") {

                var an = AutoNumeric.getAutoNumericElement('input[name="' + $(sSelector).attr('name') + '"][data-money]');
                if (an === undefined || an === null)
                    an = new AutoNumeric(sSelector, value, autoNumericOptionsEuro);

                an.set(value);
            }
            TryCatchWraper(function () {
                $(sSelector).keyup();
            });
        } else {
            TryCatchWraper(function () {
                $(sSelector).val(null);
                $(sSelector).keyup();
            });

            if ($(sSelector).attr("data-representing-type") == "Amount") {
                TryCatchWraper(function () {
                    $(sSelector + '.money2').autoNumeric('init');
                    //var m = new AutoNumeric(sSelector + '.money2', autoNumericOptionsEuro);
                });

                TryCatchWraper(function () {
                    $(sSelector + '.money4').autoNumeric('init', { mDec: 4 });
                    //var m = new AutoNumeric(sSelector + '.money2', autoNumericOptionsEuro);
                });
            }

        }
    }
    function setLookupDisplay(value, oSelector, bReslove, sUrl) {
        var resolve = false;
        if (IsNullOrUndefined(bReslove) == false)
            resolve = true;
        if (IsNullOrUndefined(value))
            return "";
        var text = "";
        if (value.Name)
            text = value.Name;
        else
            text = value.Id;

        if (IsNullOrWhiteSpace(oSelector) == false) {
            $(oSelector).text(text);
            TryCatchWraper(function () {
                $(oSelector).attr("data-selected-id", value.Id);
                $(oSelector).attr("data-selected-type-name", value.TypeName);
            });

            TryCatchWraper(function () {
                var $link = $(oSelector).parent().find("div.field-link span a");
                var href = $link.attr("data-href");
                if (IsNullOrWhiteSpace(href) == false && href != "#");
                $link.attr("href", sRootUrl + href + value.Id);
            });

            TryCatchWraper(function () {
                var $link = $(oSelector).parent().find("div.field-popup span a");
                var dataUrl = $link.attr("data-url");
                if (IsNullOrWhiteSpace(dataUrl) == false && dataUrl != "#");
                $link.attr("data-url", sRootUrl + dataUrl + value.Id);
            });

        }

        if (resolve == true && IsNullOrWhiteSpace(value.LocalId) == false && IsNullOrWhiteSpace(value.TypeName) == false && (value.Id != 0 || value.Id != "0")) {
            var url = sUrl;

            if (IsNullOrWhiteSpace(url))
                url = sRootUrl + "api/" + value.TypeName + "/" + value.LocalId;

            webApiGet(url, function returnText(rData) {

                if (rData != null) {
                    text = (IsNullOrUndefined(rData.Name) ? "" : rData.Name);
                } else {
                    text = "---";
                }

                if (IsNullOrWhiteSpace(oSelector) == false)
                    oSelector.text(text);
            }, false, false);
        }

        if (IsNullOrWhiteSpace(oSelector))
            return text;
    }
    function setLookup(reference, sSelector) {
        if (!IsNullOrUndefined(reference)) {
            var data = null;
            if (reference.SerializedValue)
                data = JSON.parse(reference.SerializedValue);
            if (IsNullOrUndefined(data)) {
                $(sSelector).attr("data-selected-id", reference.LocalId);
                $(sSelector).attr("data-selected-number", reference.Number);
                if (IsNullOrWhiteSpace(reference.TypeName) == false && reference.TypeName != "NullReference") {
                    $(sSelector).attr("data-type-name", reference.TypeName);
                    $(sSelector).attr("data-selected-type", reference.TypeName);
                }
                $(sSelector).attr("data-default-value", reference.Name);
                $(sSelector).val(reference.LocalId);
                data = reference.LocalId;
                if (IsNullOrEmpty(reference.Name)) {
                    if (reference.TypeName != "NullReference" && IsNullOrWhiteSpace(reference.TypeName) == false) {
                        $.ajax({
                            type: "GET",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            url: sRootUrl + "api/" + reference.TypeName + "/" + reference.LocalId + "?Fields=Name",
                            headers: GetWebApiHeaders(),
                            success: function (dat, textStatus, xhr) {
                                $(sSelector).typeahead('val', dat.Name).focus();
                                $(sSelector).attr("data-default-value", dat.Name);
                                $(sSelector).val(dat.Name);
                                data = dat.Name;
                            },
                            error: function (xhr, textStatus, errorThrown) {
                            }
                        });
                    } else if (reference.LocalId == "0") {
                        $(sSelector).val("");
                        data = "";
                    }
                } else {
                    $(sSelector).attr("data-default-value", reference.Name);
                    $(sSelector).val(reference.Name);
                    data = reference.Name;
                }
            } else {
                $(sSelector).attr("data-selected-id", data.LocalId);
                $(sSelector).attr("data-selected-number", data.Number);
                if (IsNullOrWhiteSpace(data.TypeName) == false && data.TypeName != "NullReference") {
                    $(sSelector).attr("data-type-name", data.TypeName);
                    $(sSelector).attr("data-selected-type", data.TypeName);
                }
                $(sSelector).attr("data-default-value", data.Name + " - " + data.Number);
                $(sSelector).val(data.Name + " - " + data.Number);
                data = data.Name + " - " + data.Number;
            }
            TryCatchWraper(function () {
                $(sSelector).typeahead('val', data).blur();
            });
            var dName = $(sSelector).attr("data-name");
            setTimeout(function () {
                $("input.tt-input[name='" + dName + "']").keyup();
            }, 200);

            TryCatchWraper(function () {
                $(sSelector).keyup();
            });
        } else {
            clearLookup(sSelector);
        }
    }
    function getLookupDisplayed(sSelector) {
        var el = $(sSelector);

        var reference = new Object();
        reference.Id = el.attr("data-selected-id");
        reference.TypeName = el.attr("data-selected-type-name");

        return reference;
    }
    function getLookup(sSelector, sTypeName) {
        var reference = new Object();
        reference.Id = $(sSelector).attr("data-selected-id");
        if (IsNullOrUndefined(reference.Id))
            reference.Id = "0";
        var name = $(sSelector).attr("data-selected-value");
        if (IsNullOrWhiteSpace(name))
            reference.Name = $(sSelector).val();
        else
            reference.Name = name;
        if (!IsNullOrUndefined(sTypeName))
            reference.TypeName = sTypeName;
        else if (!IsNullOrUndefined($(sSelector).attr("data-selected-type")))
            reference.TypeName = $(sSelector).attr("data-selected-type");
        else
            reference.TypeName = $(sSelector).attr("data-type-name");

        if (IsNullOrWhiteSpace(reference.TypeName))
            reference.TypeName = "NullReference";

        var number = $(sSelector).attr("data-selected-number");
        if (IsNullOrWhiteSpace(number))
            reference.Number = "";
        else
            reference.Number = number;

        return reference;
    }

    function getLookupField(sSelector, sTypeName) {
        var reference = getLookup(sSelector, sTypeName);
        return { Name: $(sSelector).attr("data-filed-name"), Value: JSON.stringify(reference) };
    }
    function setBooleanDisplay(value, oSelector) {
        var orgValue = value;

        if (value == null)
            value = GetLclText("Undefined");
        else if (value)
            value = GetLclText("Yes");
        else
            value = GetLclText("No");

        if (IsNullOrWhiteSpace(oSelector) == false) {
            $(oSelector).text(value);
            $(oSelector).attr("data-string-value", orgValue.toString());
        } else
            return value;
    }
    function setBoolean(value, sSelector) {
        if (IsNullOrUndefined($(sSelector)[0]))
            return;

        var rdbId = "#" + $(sSelector)[0].id;

        $(rdbId + '-Null').prop('checked', false);
        $(rdbId + '-No').prop('checked', false);
        $(rdbId + '-Yes').prop('checked', false);

        if (value || typeof (value) == 'boolean') {
            $(sSelector).val(value);

            if (value == 'Yes' || (typeof (value) == 'boolean' && value == true) || value.toString().toLowerCase() == "true") {
                $(rdbId + '-Yes').prop('checked', true);
            } else {
                $(rdbId + '-No').prop('checked', true);
            }
        } else {
            $(rdbId + '-Null').prop('checked', true);
        }
    }
    function getBooleanDisplay(sSelector) {
        return $(sSelector).attr("data-string-value");
    }
    function getBoolean(sSelector) {
        return getValue(sSelector);
    }
    function getBooleanField(sSelector) {
        return { Name: $(sSelector).attr("data-filed-name"), Value: getBoolean(sSelector) };
    }
    function getQuantityDisplay(sSelector) {
        var el = $(sSelector);

        var quantity = new Object();
        quantity.UnitOfMeasureId = null;
        quantity.Amount = null;
        quantity.UnitOfMeasure = "";

        var amount = el.attr("data-amount");
        TryCatchWraper(function () { quantity.Amount = parseFloat(amount); });

        var unitOfMeasureName = el.attr("data-unit-of-measure-name");
        if (IsNullOrWhiteSpace(unitOfMeasureName) == false)
            quantity.UnitOfMeasure = unitOfMeasureName;

        var unitOfMeasureId = el.attr("data-unit-of-measure-id");
        if (IsNullOrWhiteSpace(unitOfMeasureId) == false)
            quantity.UnitOfMeasureId = unitOfMeasureId;

        return quantity;
    }
    function getQuantity(sSelector) {
        var quantity = new Object();
        quantity.UnitOfMeasureId = null;
        quantity.Amount = null;
        quantity.UnitOfMeasure = "";

        var amount = $(sSelector).val();
        var startIndex = 0;
        var pattern = "'";
        while (amount.indexOf(pattern, startIndex) != -1) {
            amount = amount.slice(1, -1);
        }

        TryCatchWraper(function () { quantity.Amount = parseFloat(amount); });

        var unitOfMeasureName = $(sSelector).attr("data-unit-of-measure-name");
        if (IsNullOrUndefined(unitOfMeasureName) == false)
            quantity.UnitOfMeasure = unitOfMeasureName;

        var unitOfMeasureId = $(sSelector).attr("data-unit-of-measure-id");
        if (IsNullOrUndefined(unitOfMeasureId) == false)
            quantity.UnitOfMeasureId = unitOfMeasureId;


        return quantity;
    }
    function setQuantity(value, sSelector) {
        var $el = $(sSelector);

        $el.val(value.Amount);

        if (IsNullOrWhiteSpace(value.UnitOfMeasure) == false)
            $el.attr("data-unit-of-measure-name", value.UnitOfMeasure);

        if (IsNullOrUndefined(value.UnitOfMeasureId) == false)
            $el.attr("data-unit-of-measure-id", value.UnitOfMeasureId);

        TryCatchWraper(function () {
            $(sSelector).keyup();
        });
    }
    function GetQuantityUnitOfMeasureList(el, fOnData) {
        $el = $(el);
        if (IsNullOrUndefined($el))
            return null;
        var cListUrl = $el.attr("data-url");
        if (IsNullOrWhiteSpace(cListUrl))
            return;
        if (startsWithTwo(cListUrl, "http") == false)
            cListUrl = sRootUrl + cListUrl;
        if (IsNullOrWhiteSpace(cListUrl) == false) {
            webApiGet(cListUrl, fOnData, false);
        } else { //TODO Fail back to space or application

        }
    }
    function onQuantityUnitOfMeasureClick(sSelector, sValue, sText) {
        if (IsNullOrUndefined(sValue) == false)
            $(sSelector).attr('data-unit-of-measure-id', sValue);
        if (IsNullOrUndefined(sText) == false)
            $(sSelector).attr('data-unit-of-measure-name', sText);
        $(sSelector + 'Code').text(sText);
    }

    function getQuantityField(sSelector) {
        var quantity = getQuantity(sSelector)
        return { Name: $(sSelector).attr("data-filed-name"), Value: JSON.stringify(quantity) }
    }
    function setQuantityDisplay(value, oSelector) {
        if (IsNullOrUndefined(value))
            return "";
        var el = $(oSelector);
        var displayValue = value.Amount;
        if (IsNullOrUndefined(el) == false) {
            var format = $(el).attr("data-format-pattern");
            if (IsNullOrWhiteSpace(format) == false) {
                var formated = numeral(displayValue);
                displayValue = formated.format(format);
            }
        }

        el.attr("data-amount", value.Amount);
        if (IsNullOrWhiteSpace(value.UnitOfMeasure) == false)
            el.attr("data-unit-of-measure-name", value.UnitOfMeasure);

        if (IsNullOrUndefined(value.UnitOfMeasureId) == false)
            el.attr("data-unit-of-measure-id", value.UnitOfMeasureId);

        if (IsNullOrWhiteSpace(value.UnitOfMeasure) == false) {
            var text = displayValue + " " + value.UnitOfMeasure;
            if (IsNullOrWhiteSpace(oSelector) == false)
                el.text(text);
            else
                return text;
        } else if (IsNullOrUndefined(value.UnitOfMeasureId)) {
            var text = displayValue + " " + ((IsNullOrWhiteSpace(value.UnitOfMeasure)) ? "" : value.UnitOfMeasure);
            if (IsNullOrWhiteSpace(oSelector) == false)
                el.text(text);
            else
                return text;
        } else {
            var url = sRootUrl + "api/query/GetAllClassificationValues?Filter=(Id%20eq%20" + value.UnitOfMeasureId + "L)&NoCache=true&LangCode=" + LangCode;

            webApiGet(url, function returnText(rData) {
                var text = "";
                if (rData != null && rData[0] != null) {
                    text = displayValue + " " + rData[0].Name;
                } else {
                    text = displayValue;
                }

                if (IsNullOrWhiteSpace(oSelector) == false)
                    el.text(text);
                else
                    return text;
            }, true, false);
        }
    }
    function getAmountDisplayed(sSelector) {
        var el = $(sSelector);

        var currencyAmount = new Object();
        currencyAmount.Amount = el.attr("data-amount");
        currencyAmount.Code = el.attr("data-currency-code");
        currencyAmount.CurrencyId = el.attr("data-currency-id");

        return currencyAmount;
    }
    function getAmount(sSelector) {
        var currencyAmount = new Object();
        currencyAmount.Amount = $(sSelector).val();
        TryCatchWraper(function () {
            var an = new AutoNumeric(sSelector);
            currencyAmount.Amount = an.getNumber();
            //currencyAmount.Amount = parseFloat($(sSelector).autoNumeric("get"));
        });
        currencyAmount.Code = $(sSelector).attr("data-currency-code");
        var currencyId = $(sSelector).attr("data-currency-id");
        if (IsNullOrWhiteSpace(currencyId) == false)
            currencyAmount.CurrencyId = currencyId;

        TryCatchWraper(function () { $(sSelector).autoNumeric('destroy'); });
        return currencyAmount;
    }

    function getAmountField(sSelector) {
        var currencyAmount = getAmount(sSelector)
        return { Name: $(sSelector).attr("data-filed-name"), Value: JSON.stringify(currencyAmount) }
    }
    function setAmount(value, sSelector) {
        if (!IsNullOrUndefined(value)) {
            console.log(value);
            $(sSelector).val(value.Amount);
            $(sSelector).attr("data-default-value", value.Amount);
            //TryCatchWraper(function () {
            //    $(sSelector).autoNumeric('init');
            //    $(sSelector).autoNumeric('set', value.Amount);
            //});
            $(sSelector).attr("data-currency-code", value.Code);

            var lId = value.CurrencyId;
            if (IsNullOrUndefined(lId) == false)
                $(sSelector).attr('data-currency-id', lId);
            TryCatchWraper(function () {
                var sSelectorCode = "#" + $(sSelector)[0].id + "Code";
                $(sSelectorCode).text(getCurrencyNameFromCode(value.Code));
            });

            TryCatchWraper(function () {
                $(sSelector).keyup();
            });
        }
    }
    function getCurrencyNameFromCode(code) {
        if (code == 840 || code == "840" || code == "USD")
            return "USD";
        else if (code == 978 || code == "978" || code == "EUR")
            return "EUR";
        else if (code == 941 || code == "941" || code == "RSD")
            return "RSD";
        else
            return code;
    }
    function setAmountDisplay(value, oSelector) {
        if (IsNullOrUndefined(value))
            return "";
        var el = $(oSelector);
        var format = getCurrencyFormat(el);
        var displayValue = value.Amount;
        if (IsNullOrWhiteSpace(format) == false) {
            var amount = numeral(displayValue);
            displayValue = amount.format(format);
        }

        var currencyName = getCurrencyNameFromCode(value.Code);
        if (IsNullOrWhiteSpace(oSelector) == false) {

            el.text(displayValue + " " + currencyName);
            el.attr("data-amount", value.Amount);
            el.attr("data-currency-id", value.CurrencyId);
            el.attr("data-currency-code", value.Code);
            el.attr("data-currency-name", currencyName);

        }
        else
            return displayValue + " " + currencyName;
    }
    function setNumberDisplay(value, oSelector, decimalPlaces) {
        if (IsNullOrUndefined(value))
            return "";
        var el = $(oSelector);
        var format = getCurrencyFormat(el, null, decimalPlaces);
        var displayValue = value;
        if (IsNullOrWhiteSpace(format) == false) {
            var amount = numeral(displayValue);
            displayValue = amount.format(format);
        }

        if (IsNullOrWhiteSpace(oSelector) == false)
            el.text(displayValue);
        else
            return displayValue
    }

    function getRatingField(sSelector) {
        sSelector = "#" + $(sSelector).attr("id") + "-star";
        return { Name: $(sSelector).attr("data-filed-name"), Value: JSON.stringify(getRating(sSelector)) };
    }
    function getRatingDisplay(sSelector) {
        return $(sSelector).attr("data-rating");
    }
    function setRating(value, sSelector) {
        if (IsNullOrWhiteSpace(sSelector) == false) {
            sSelector = $(sSelector).attr("id").replace("-star", "");
            sSelector = "#" + sSelector + "-star";
            $(sSelector).attr("data-rating", value.Value);
            $(sSelector).starrr();
        } else {
            return value.Value;
        }
    }

    function onRangeChange(sender) {
        sId = $(sender).attr("id").replace("-range", "");

        var elm = $("#" + sId);
        if (elm.length == 0)
            return;

        var value = $(sender).val();
        elm.val(value);

        if (!IsNullOrUndefined(value) && !IsNullOrEmpty(value))
            elm.attr("data-edit", "true");
        else
            elm.attr("data-edit", "false");

        var isValid = $(elm).valid();

        onInputChange($(elm), isValid);
    }
    function setRange(value, sSelector) {
        if (!IsNullOrUndefined(value)) {
            sId = "#" + $(sSelector).attr("id").replace("-range", "");
            if (IsNullOrUndefined(value.Value)) {
                $(sId).val(value.Value);
                $(sId + "-range").val(value.Value);
            } else {
                $(sId).val(value);
                $(sId + "-range").val(value);
            }
        }
    }
    function setRangeDisplay(value, sSelector) {
        if (IsNullOrUndefined(value))
            return "";
        if (IsNullOrWhiteSpace(sSelector) == false)
            $(sSelector).text(value.Value);
        else
            return value.Value;
    }
    function getRangeDisplay(sSelector) {
        var range = new Object();
        range.Value = $(sSelector).text();

        return range;
    }
    function getRange(sSelector) {
        var sId = "#" + $(sSelector).attr("id").replace("-range", "");

        var range = new Object();
        var el = $(sId);
        range.Value = el.val();
        TryCatchWraper(function () { range.Value = parseFloat(range.Value); });
        el = $(sId + "-range");
        var min = el.attr("min");
        if (IsNullOrWhiteSpace(min) == false)
            TryCatchWraper(function () { range.Min = parseFloat(min); });

        var max = el.attr("max");
        if (IsNullOrWhiteSpace(max) == false)
            TryCatchWraper(function () { range.Max = parseFloat(max); });

        var step = el.attr("step");
        if (IsNullOrWhiteSpace(step) == false)
            TryCatchWraper(function () { range.Step = parseFloat(step); });

        return range;
    }
    function getRangeField(sSelector) {
        var sId = "#" + $(sSelector).attr("id").replace("-range", "");
        var range = getRange(sId);
        return { Name: $(sSelector).attr("data-filed-name"), Value: JSON.stringify(range) };
    }
    function isString(myVar) {
        if (typeof myVar === 'string' || myVar instanceof String)
            return true;
        else
            return false;
    }

    function onFormSubmit(form, e, fOnData, bShowAlertOnSuccess, bShowAlertOnFailure, fOnError) {
        if (IsNullOrUndefined(bShowAlertOnSuccess))
            bShowAlertOnSuccess = true;
        if (IsNullOrUndefined(bShowAlertOnFailure))
            bShowAlertOnFailure = true;
        var formId = $(form).attr("Id");
        e.preventDefault();
        ShowPageLoader();
        if (!validateForm(document.getElementById(formId))) {
            ClosePageLoader();
            return false;
        }
        var i = new Object();

        var objectid = $(form).attr("objectid");

        if (IsNullOrWhiteSpace(objectid) == false)
            i.ObjectId = objectid;

        i.Data = $(form).serialize();
        i.Name = $(form).attr("dataname");

        i.Fields = getFormFields(form, true);

        var dataObject = new Object();
        dataObject.Id = i.ObjectId;
        $.each(i.Fields, function (index, value) {
            dataObject[value.Name] = value.Value;
        });

        var url = sRootUrl + $(form).attr("posturl");

        webApiPost(url, dataObject, fOnData, bShowAlertOnSuccess, bShowAlertOnFailure, fOnError, true, formId);

        return false;
    }

    function getFormFields(elForm, bAll) {
        var fields = new Array();
        var sInputSelector = "input[data-edit='true'][data-child-id='']" + ", " + "div.collection.display-field-container[data-partially-edit='true']";
        if (bAll)
            sInputSelector = "input" + ", " + "div.collection.display-field-container[data-partially-edit='true']";

        $(elForm).find(sInputSelector).each(function (index) {
            getFormFieldsValues(this, fields);
        });

        if ($("div[data-dropzone='true']").length > 0) {
            getFromDropzone(fields);
        }

        if ($("table[data-jqGrid='true']").length > 0) {
            getjQGridData(fields);
        }

        return fields;
    }
    function getFormFieldsValues(el, fields) {
        var type = $(el).attr('type');
        var id = $(el).attr('id');
        if ((type == "text" || type == "number" || type == "hidden")) {


            var value = getFormElementFiledValue(el);

            if (fields && IsNullOrUndefined(value) == false && IsNullOrUndefined(value.Name) == false)
                fields.push(value);

            return value;
        }
    }

    function getjQGridData(fields) {

        var gridId = $("table[data-jqGrid='true']").attr("id");
        var name = $("table[data-jqGrid='true']").attr("data-type-name");

        var f = new Object();
        f.Name = name;
        f.FieldType = "ItemCollection";
        f.IsEnumerable = true;

        var items = [];
        items.length = 0;
        var inx = 0;

        var gridModel = jQuery("#" + gridId).jqGrid('getGridParam', 'data');
        var columnNames = $("#" + gridId).jqGrid('getGridParam', 'colNames');

        $.each(gridModel, function (index, value) {
            var p = new Object();

            //p.Fields = new Array();
            $.each(columnNames, function (ind, val) {
                if (!IsNullOrEmpty(val) && !IsNullOrWhiteSpace(val) && val != "Actions") {
                    //p.Fields.push({ Name: val, Value: value[val] });
                    p[val] = value[val];
                }
            });

            items[inx] = p;
            inx++;
        });

        if (items.length > 0) {
            f.Value = items;
            fields.push(f);
        }
    }

    function getFromDropzone(fields) {

        var f = new Object();
        f.Name = "DocumentItems";
        f.FieldType = "ItemCollection";
        f.DataType = "AttachmentCollection";
        f.Id = $("div[data-dropzone='true']").attr("data-refers-to-id");

        f.IsEnumerable = true;
        var items = [];
        items.length = 0;
        var inx = 0;

        fields.push({ Name: "Object_Id", Value: $("div[data-dropzone='true']").attr("data-refers-to-id") });
        //fields.push({ Name: "RefersTo_Id", Value: $("div[data-dropzone='true']").attr("data-refers-to-id") });
        //fields.push({ Name: "RefersTo_TypeName", Value: $("div[data-dropzone='true']").attr("data-refers-to-type-name") });

        $("div[data-dropzone='true']").find(".dz-preview").each(function (index) {
            var el = $(this);
            if (!IsNullOrUndefined(el) && !IsNullOrUndefined($(this).attr("data-mimetype"))) {

                var p = new Object();
                //p.Fields = new Array();

                var dropZone = new Object();
                dropZone.DocumentName = $(el.find(".dz-filename[data-dz-name]"))[0].innerText;
                dropZone.DocumentSize = $(el.find(".dz-size[data-dz-size]"))[0].innerText;
                dropZone.DocumentMimeType = $(this).attr("data-mimetype");
                dropZone.Description = $(this).find(".dz-description").find("#description").val();
                dropZone.ObjectId = $(this).attr("data-objectid");

                //p.Fields.push({ Name: "Document", Value: JSON.stringify(dropZone) });
                p = dropZone;

                items[inx] = p;
                inx++;
            }
        });

        if (items.length > 0) {
            f.Value = items;
            fields.push(f);
        }
    }

    function getFormElementFiledValue(el) {

        var represent = $(el).attr('data-representing-type');

        if (!represent)
            return getValueField(el);

        var id = $(el).attr('id');

        var value = null;

        if (IsNullOrUndefined(id))
            return value;

        var sSelector = "#" + id;

        if (represent == "Amount")
            value = getAmountField(sSelector);
        else if (represent == "Lookup")
            value = getLookupField(sSelector);
        else if (represent == "Boolean")
            value = getBooleanField(sSelector);
        else if (represent == "Select")
            value = getSelectedField(sSelector);
        else if (represent == "Collection")
            value = getCollectionField(sSelector);
        else if (represent == "Rating")
            value = getRatingField(sSelector);
        else if (represent == "Range")
            value = getRangeField(sSelector);
        else if (represent == "Date")
            value = getDateField(sSelector);
        else if (represent == "DatePeriod")
            value = getDateRangeField(sSelector);
        else if (represent == "TimePeriod")
            value = getTimeRangeField(sSelector);
        else if (represent == "Quantity")
            value = getQuantityField(sSelector);
        else
            value = getValueField(sSelector);

        return value;
    }

    function deleteObject(el, fOnSuccess) {
        swalCall({
            title: "Löschen",
            text: "Soll der Datensatz wirklich gelöscht werden?",
            type: "warning",
            showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Übernehmen", closeOnConfirm: true,
            cancelButtonText: "Abbrechen"

        },
            function() { return webApiDelete(el, $(el).attr("data-type"), fOnSuccess) },
            function () {}
        )
    }

    function swalCall(options, fnSuccess, fnError) {
        if (fnError === undefined || fnError === null)
            fnError = function () { };
        if (fnSuccess === undefined || fnSuccess === null)
            fnSuccess = function () { };
        swal(options, fnSuccess, fnError);
    }

    function onSelectChange(sender, sId) {
        if (!sId) {
            sId = $(sender).attr("id").replace("-ddl", "");
        }
        var elm = $("#" + sId);
        if (!elm)
            return;
        elm.attr("data-edit", "true");
        elm.val($(sender).val());
    }

    function getSelectedField(sSelector) {
        var el = $(sSelector + "-ddl");
        if (el.length != 0) {
            sValue = el.val();
            sSelector = sSelector + "-ddl"
        }
        else
            sValue = getSelected(sSelector);

        if (sValue == null || sValue == 'undefined')
            sValue = null;

        //var typeName = $(sSelector).attr("data-type-name");
        //if (IsNullOrWhiteSpace(typeName) == false) {
        //    var reference = new Object();
        //    reference.TypeName = typeName;
        //    reference.Id = sValue;

        //    return {
        //        Name: $(sSelector).attr("data-filed-name"), Value: JSON.stringify(reference)
        //    }
        //} else {
        return {
            Name: $(sSelector).attr("data-filed-name") + "Id", Value: sValue
        }
        //};
    }

    function getSelected(sSelector) {
        sId = sSelector.replace("-ddl", "");
        return $(sId).val();
    }

    function initializeDropZone(idDropZone, idPreview, refersToId, refersToTypeName) {
        var firstTime = myAttachZone == null;

        if (!firstTime) {
            myAttachZone.destroy();
        }
        
        var previewNode = document.querySelector("#" + idPreview);
        if (previewNode != null && previewNode != 'undefined') {
            previewNode.id = "";
            var previewTemplate = previewNode.parentNode.innerHTML;
            previewNode.parentNode.removeChild(previewNode);

            var maxfiles = null;

            try {
                 maxfiles = parseInt($("#" + idDropZone).attr('data-maxfiles'));
            } catch(ex) {

            }            
            if (maxfiles == undefined || maxfiles == NaN)
                maxfiles = null;

            myAttachZone = new Dropzone(document.querySelector("#" + idDropZone), {
                url: sRootUrl + "document/insert/contentstream?Tag=" + refersToTypeName,
                addRemoveLinks: true,
                dictRemoveFile: "Löschen",
                addOpenLinks: true,
                dictOpenLink: "Offnen",
                sendFileId: true,
                clickable: "#" + idDropZone,
                acceptedFiles: "image/jpeg,image/png,image/jpg",
                addDescription: true,
                objectTypeName: refersToTypeName,
                previewTemplate: previewTemplate,
                thumbnailHeight: 120,
                thumbnailWidth: 140,
                thumbnailMethod: 'crop',
                maxFiles: maxfiles,
                dictRemoveFileConfirmation: 'Question'
            });

            $("#addFile-" + idDropZone).click(function () {
               $("#" + idDropZone).click();
            });
        }
       

        myAttachZone.on("addedfile", function (file) {

            if (file.status == "added") {
                file.previewElement.setAttribute("data-mimetype", file.type);
                file.previewElement.setAttribute("data-objectid", file.id);
                file.previewElement.setAttribute("data-isnew", IsNullOrUndefined(file.isnew));

                $('#iddropzone').attr("data-refers-to-id", refersToId);
                $('#iddropzone').attr("data-refers-to-type-name", refersToTypeName);

                var fileNew = refersToId == 0 ? true : false;

                var eventArgs = new Object();
                eventArgs.FileIsNew = fileNew;
                eventArgs.RefersToTypeName = refersToTypeName;
                eventArgs.FileName = file.name;
                eventArgs.ObjectId = file.id;

                $(document).trigger("fileUpload:addedFile", eventArgs);
            }
        });
        myAttachZone.on("maxfilesreached", function (file) {
            myAttachZone.removeEventListeners();
            $("#" + idDropZone).removeClass("dz-clickable");
            $("#" + idDropZone).unbind();
            $("#addFile-" + idDropZone).prop('disabled', true);
        });


    }

    function FillDocumentDropzone(dcDocument, idDropZone, refersToId, refersToTypeName) {

        if (!IsNullOrEmpty(dcDocument)) {

            var data = dcDocument;

            if (!IsNullOrUndefined(data)) {
                $.each(data, function (index, value) {

                    var mockfilee = {
                        name: value.DocumentName, size: setFileSize(value.DocumentSize),
                        type: value.DocumentMimeType, id: value.ObjectId, status: "added",
                        description: value.Description, accepted: true
                    };

                    myAttachZone.files.push(mockfilee);
                    myAttachZone.emit("addedfile", mockfilee);
                    if (isImage(value.DocumentName)) {
                        myAttachZone.createThumbnailFromUrl(mockfilee, sRootUrl + "document/download/contentstream?Tag=" + refersToTypeName + "&Number=" + value.ObjectId);
                    }
                    myAttachZone.emit("complete", mockfilee);
                    myAttachZone._updateMaxFilesReachedClass();

                    $('#' + idDropZone).attr("data-refers-to-id", refersToId);
                    $('#' + idDropZone).attr("data-refers-to-type-name", refersToTypeName);

                });
            }
        }
    }

    function callSwal(accepted, rejected) {
        swalCall({
            title: "Löschen",
            text: "Soll der Datensatz wirklich gelöscht werden?",
            type: "warning",
            showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Übernehmen", closeOnConfirm: true,
            cancelButtonText: "Abbrechen"
        },
            accepted,
            rejected
        )
    }

    return {
        getWebApi: function (sUri, fOnData, bShowError, bAsync) {
            webApiGet(sUri, fOnData, bShowError, bAsync);
        },

        postWebApiOnError: function (header, xhr, textStatus, errorThrown, bShowAlert, fOnError, sUri, closeModal) {
            webApiPostOnError(header, xhr, textStatus, errorThrown, bShowAlert, fOnError, sUri, closeModal);
        },

        postWebApi: function (sUri, oData, fOnData, bShowAlertOnSuccess, bShowAlertOnFailure, fOnError, bCloseModalDialog, sFormId, isAsync) {
            webApiPost(sUri, oData, fOnData, bShowAlertOnSuccess, bShowAlertOnFailure, fOnError, bCloseModalDialog, sFormId, isAsync);
        },

        postWebApiOnSuccess: function (data, textStatus, xhr, bShowAlert, fOnData, bCloseModalDialog, sUri, sFormId) {
            webApiPostOnSuccess(data, textStatus, xhr, bShowAlert, fOnData, bCloseModalDialog, sUri, sFormId);
        },

        postWebApiInSuccessFalse: function (header, data, bShowAlert) {
            webApiPostOnSuccessFalse(header, data, bShowAlert);
        },

        deleteWebApi: function (el, objectType, fOnSuccess, fOnError) {
            webApiDelete(el, objectType, fOnSuccess, fOnError);
        },

        openModalForm: function (el) {
            openModalDialog(el);
        },

        validateFormApp: function (elForm) {
            return validateForm(elForm);
        },

        pageLoaderShow: function () {
            ShowPageLoader();
        },

        pageLoaderClose: function () {
            ClosePageLoader();
        },

        modalDialogClose: function (text) {
            closeModalDialog(text);
        },

        setFormApp: function (elForm, oData, behDefValues) {
            setForm(elForm, oData, behDefValues);
        },

        onFormSubmitApp: function (form, e, fOnData, bShowAlertOnSuccess, bShowAlertOnFailure, fOnError) {
            onFormSubmit(form, e, fOnData, bShowAlertOnSuccess, bShowAlertOnFailure, fOnError);
        },
        deleteObjectApp: function (el, fOnSuccess) {
            deleteObject(el, fOnSuccess);
        },
        onSelectChangeApp: function (sender, sId) {
            onSelectChange(sender, sId);
        },
        initializeDropZoneApp: function (idDropZone, idPreview, refersToId, refersToTypeName) {
            initializeDropZone(idDropZone, idPreview, refersToId, refersToTypeName);
        },
        fillDropZoneApp: function (dcDocument, idDropZone, refersToId, refersToTypeName) {
            FillDocumentDropzone(dcDocument, idDropZone, refersToId, refersToTypeName);
        },
        windowResize: function () {
            windowResize();
        },
        callSwallApp: function (accepted, rejected) {
            callSwal(accepted, rejected);
        }
    }

}());


function IsNullOrEmpty(value) {
    return (IsNullOrUndefined(value) || value == "");
}
function IsNullOrWhiteSpace(value) {
    return (IsNullOrUndefined(value) || !/\S/.test(value));
}

function IsNullOrUndefined(sValue) {
    if (typeof (sValue) != "undefined" && sValue != null)
        return false;
    else
        return true;
}

var setGridOptions = (function () {
    function setUpGrid(gridId, pagerId, colModel, width, height, rowsPerPage, fetchGridData, customButtonAddRow, editUrl) {

        var widthGrid = width;
        var widthParent = $("#" + gridId).parent().width();

        if (Number(widthParent) != 0)
            widthGrid = widthParent;

        var gridHeight = $(window).innerHeight() - 280;

        if (height != 0)
            gridHeight = height;

        $("#" + gridId).jqGrid({
            // data: mydata,
            editurl: 'clientArray',
            datatype: "local",
            colModel: colModel,
            width: widthGrid,
            height: gridHeight,
            autowidth: true,
            pgbuttons: false,
            pginput: false, 
            shrinkToFit: true,
            ondblClickRow: function (rowId) {

                if (IsNullOrUndefined(editUrl))
                    return;

                var rowData = jQuery(this).getRowData(rowId);

                if (IsNullOrUndefined(rowData))
                    return;

                var IdEdit = 0;

                if (typeof rowData.Id === 'undefined')
                    return;

                var btn = document.createElement("button");
                var url = editUrl + rowData.Id;
                btn.setAttribute('data-url', url);

                publicApp.openModalForm(btn);

            },
            rowNum: rowsPerPage,
            pager: "#jqGridPager",
        });

        delSettings = {
            afterShowForm: function ($form) {
                // delete button: "#dData", cancel button: "#eData"
                $("#dData", $form.parent()).click();
            }
        };

        // append data to grid
        fetchGridData();

        $("#" + gridId).jqGrid('filterToolbar', {
            // JSON stringify all data from search, including search toolbar operators
            stringResult: true,
            // instuct the grid toolbar to show the search options
            searchOperators: true,
            searchOnEnter: false
        });

        $("#" + gridId).navGrid("#" + pagerId, { edit: false, add: false, del: false, refresh: false, view: false, search: false }, {}, {}, delSettings);


        $("#" + gridId).inlineNav('#' + pagerId,
            // the buttons to appear on the toolbar of the grid
            {
                edit: false,
                add: false,
                del: true,
                save: false,
                cancel: false,
                editParams: {
                    keys: false,
                },
                addParams: {
                    keys: false,
                    position: "last"
                },

            }
        );

        if (!IsNullOrUndefined(customButtonAddRow)) {
            $("#" + customButtonAddRow).on("click", function (event) {
                $("#" + gridId).jqGrid('addRow', parameters);
            });

            var parameters = {
                rowID: "new_row",
                initdata: {},
                position: "last",
                useDefValues: false,
                useFormatter: false,
                addRowParams: { extraparam: {} }
            };
        }

        //$("#" + gridId).jqGrid('setGridHeight', heightParent);

        $("#" + gridId).trigger("resize");

        $(window).resize(function () {
            if (height == 0)
                $("#" + gridId).jqGrid('setGridHeight', $(window).innerHeight() - 280);
        });

    }

    function deleteRows(gridId) {
        var rowIds = $('#' + gridId).jqGrid('getDataIDs');

        for (var i = 0, len = rowIds.length; i < len; i++) {
            var currRow = rowIds[i];
            $('#' + gridId).jqGrid('delRowData', currRow);
        }
    }

    function deleteGridRows(gridId, el) {

        swal({
            title: "Löschen?",
            text: "Soll der Datensatz wirklich gelöscht werden?",
            type: "warning",
            showCancelButton: true, confirmButtonColor: "#DD6B55",
            confirmButtonText: "Übernehmen", closeOnConfirm: true,
            cancelButtonText: "Abbrechen"
        }, function () {
            var elem = $(el);
            var idRow = elem.parent().parent().attr("id");

            $('#' + gridId).jqGrid('delRowData', idRow);
        });
        
    }

    return {
        setUpGrid: function (gridId, pagerId, colModel, width, height, rowsPerPage, fetchGridData, customButtonAddRow,editUrl) {
            setUpGrid(gridId, pagerId, colModel, width, height, rowsPerPage, fetchGridData, customButtonAddRow,editUrl);
        },
        deleteRows: function (gridId) {
            deleteRows(gridId);
        },
        deleteRowById: function (gridId, rowId) {
            deleteGridRows(gridId, rowId); 
        }
    };

}());

(function ($) {
    $.fn.floatLabels = function (options) {

        var self = this;
        var settings = $.extend({}, options);

        function registerEventHandlers() {
            self.on('input:not([data-float-label-set]) keyup change', 'input, textarea', function () {
                actions.swapLabels(this);
            });
        }

        var actions = {
            initialize: function () {
                self.each(function () {
                    var $this = $(this);
                    var $label = $this.children('label');
                    var $field = $this.find('input,textarea').first();

                    var isSet = $field.attr('data-float-label-set');

                    if (isSet)
                        return false;

                    if ($this.children().first().is('label')) {
                        $this.children().first().remove();
                        $this.append($label);
                    }

                    var placeholderText = ($field.attr('placeholder') && $field.attr('placeholder') != $label.text()) ? $field.attr('placeholder') : $label.text();

                    $label.data('placeholder-text', placeholderText);
                    $label.data('original-text', $label.text());

                    if ($field.val() == '') {
                        $field.addClass('empty');
                    }

                    $field.attr('data-float-label-set', 'true');

                    return true;
                });
            },
            swapLabels: function (field) {
                var $field = $(field);
                var $label = $(field).siblings('label').first();
                var isEmpty = Boolean($field.val());

                if (isEmpty) {
                    $field.removeClass('empty');
                    $label.text($label.data('original-text'));
                }
                else {
                    $field.addClass('empty');
                    $label.text($label.data('placeholder-text'));
                }
            }
        }

        function init() {
            registerEventHandlers();

            actions.initialize();

            self.each(function () {
                actions.swapLabels($(this).find('input,textarea').first());
            });
        }
        init();


        return this;
    };
})(jQuery);

function logConsole(msg, error) {
    if (typeof console != "undefined") {
        console.log(msg, error);
    }
}

function TryCatchWraper(func) {
    try { func(); } catch (ex) { logConsole("TryCatchWraper", ex); }
}

function setFocus(sSelector) {
    var $fF = $(sSelector);
    if ($fF) {
        if ($fF.hasClass('typeahead2') && $fF.parent().find(".tt-input").length) {
            $fF = $fF.parent().find(".tt-input").first();
        }
        if ($fF.hasClass('selectTwo')) {
            TryCatchWraper(function () {
                $fF.select2('open');
            });
        }
        else {
            $fF.focus();
        }
    }
}
function setFileSize(filesize) {
    var size = filesize.split(' ');

    switch (size[1].toLowerCase()) {
        case 'tb':
            return 1000000000000 * Number(size[0]);

        case 'gb':
            return 1000000000 * Number(size[0]);

        case 'mb':
            return 1000000 * Number(size[0]);

        case 'kb':
            return 1000 * Number(size[0]);

        case 'b':
            return 1 * Number(size[0]);
    }

}

function getExtension(filename) {
    var parts = filename.split('.');
    return parts[parts.length - 1];
}

function isImage(filename) {
    var ext = getExtension(filename);
    switch (ext.toLowerCase()) {
        case 'jpg':
        case 'gif':
        case 'bmp':
        case 'png':
        case 'tif':
        case 'svg':

            return true;
    }
    return false;
}
var iGloablLocalId = 0;
function getLocalId() {
    var prefix = '_' + Math.random().toString(36).substr(2, 9);

    var d = new Date(),
        m = d.getMilliseconds() + "",
        u = ++d + m + (++iGloablLocalId === 10000 ? (iGloablLocalId = 1) : iGloablLocalId);

    return prefix + u;
}

function onTextAreaChange(sender) {
    sId = $(sender).attr("id").replace("-mlt", "");

    var elm = $("#" + sId);
    if (elm.length == 0)
        return;

    value = $(sender).val();
    elm.val(value);

    if (!IsNullOrUndefined(value) && !IsNullOrEmpty(value))
        elm.attr("data-edit", "true");
    else
        elm.attr("data-edit", "false");
}

function TryCatchWraper(func) {
    try { func(); } catch (ex) { logConsole("TryCatchWraper", ex); }
}
