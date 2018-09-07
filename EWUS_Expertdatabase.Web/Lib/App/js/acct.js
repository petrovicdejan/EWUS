function logConsole(msg, error) {
    if (typeof console != "undefined") {
        console.log(msg, error);
    }
}
Ready.prototype.Run = function () {
    var len = this.callbacks.length; for (var i = 0; i < len; i++) { var func = this.callbacks[i]; try { func(); } catch (e) { logConsole("ready internal run", e); } }
};
try {
    if (ready != null) {
        ready.Run();
    };
}
catch (e) {
    
};
function TryCatchWraper(func) {
    try { func(); } catch (ex) { logConsole("TryCatchWraper", ex); }
}
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

function onInputChange(elem) {
    var icon = $(elem).prev("i");
    var isValid = elem[0].checkValidity();
    var isEdit = $(elem).val() != '';
    icon.attr('valid', isValid);

    if (icon.hasClass("fa-terminal")) {
        if (isValid && isEdit)
            icon.removeClass('fa-terminal').addClass('fa-check');
        else
            icon.removeClass('fa-check').addClass('fa-terminal');
    }

    if (isValid && isEdit) {
        icon.css('color', '#5cb85c');
    }
    else {
        icon.css('color', '#999');
    }

    if (!isValid && isEdit)
        icon.css('color', '#d9534f');

    if (isEdit) {
        $(elem).attr('data-edit', 'true');
        icon.attr('data-edit', 'true');
    }
    else {
        $(elem).attr('data-edit', 'false');
        icon.attr('data-edit', 'false');
    }

    
}

function setUpFloatLabels() {
    $("input:not([data-float-label-set])[type='email']").on("change paste keyup blur mousedown", function () {
        if ($(this).hasClass("date2")==false)
            onInputChange(this);
    });

    $("input:not([data-float-label-set])[type='text']").on("change paste keyup blur mousedown", function () {
        if ($(this).hasClass("date2") == false)
            onInputChange(this);
    });

    $("textarea:not([data-float-label-set])").on("change paste keyup blur mousedown", function () {
        if ($(this).hasClass("date2") == false)
            onInputChange(this);
    });

    $('.float-label-control').floatLabels();
}
function setUpModelForm() {
     
    TryCatchWraper(function () { setUpFloatLabels(); });
    TryCatchWraper(function () { $('.dropdown-toggle').dropdown();});
    TryCatchWraper(function () { $('.money2').autoNumeric('init'); });
    TryCatchWraper(function () { $('.money4').autoNumeric('init', { mDec: 4 }); });
    TryCatchWraper(function () { $(".starrr").starrr(); });
    TryCatchWraper(function () {
        $('.field-date input').datepicker({
            format: "dd.mm.yyyy",
            autoclose: true,
            disableEntry: false,
            orientation: "bottom auto",
            keyboardNavigation: true,
            disableTouchKeyboard: false
        });
    });
    TryCatchWraper(function () { setUpTypeHead(); });
}
