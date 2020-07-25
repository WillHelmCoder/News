(function ($) {
    var timeOut;
    var alertsContainer;

    $.fn.showAlert = function (message, classes) {
        alertsContainer = this;

        alertsContainer.css("height", "auto");

        if (alertsContainer.is(':animated')) { return; }
        alertsContainer.html('<div id="alert-box" class="alert pointer p-relative"></div>');

        var theAlert = $("#alert-box");
        theAlert.html(message).addClass(classes);

        theAlert.append("<i class='fa fa-sort-desc' aria-hidden='true'></i>")

        if (alertsContainer.is(':animated') || timeOut != null) { return; }

        timeOut = null;
        timeOut = this.fadeAlert(9000);
    }

    $.fn.fadeAlert = function (time) {
        setTimeout(function () {
            alertsContainer.stop(true, true).animate({ height: 0 }, 400, function () {
                alertsContainer.html("");
                timeOut = null;
            });
        }, time);
    }
}(jQuery));