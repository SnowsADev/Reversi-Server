let ModuleTemplate = (function ($) {
    var _configMap = {};

    //initialize function
    var initModule = function () {
        return true;
    }

    return {
        initModule: initModule
    };
})(jQuery);

let SPA = (function ($) {
    var _configMap = {};

    //initialize function
    var initModule = function () {
        return true;
    }

    return {
        initModule: initModule


    };
})(jQuery);

SPA.Data = (function ($) {
    var _configMap = {
        endpoint: "../api/game.json",
        environment: String.
    };  

    //initialize function
    var initModule = function (environment) {
        _configMap.environment = environment;
        return true;
    }

    var getSpellen = (function () {
        var result;
        if (environment === "production") {
            result = $.ajax({
                url: endpoint,
                succes: (function () { return data }),
                error: (function () {
                    SPA.feedbackModule.toonErrorBericht("error", "Failed request to server.")
                }),
                type: "POST"
            });
        }

        return result;
    });



    return {
        initModule: initModule,
        getSpellen: getSpellen
    };
})(jQuery);

SPA.Model = (function ($) {
    var _configMap = {};

    //initialize function
    var initModule = function () {
        return true;
    }

    return {
        initModule: initModule
    };
})(jQuery);

SPA.Reversi = (function ($) {
    var _configMap = {};

    //initialize function
    var initModule = function () {
        return true;
    }

    return {
        initModule: initModule
    };
})(jQuery);

SPA.feedbackModule = (function ($) {
    var _configMap = {};

    //initialize function
    var initModule = function () {
        return true;
    }

    var toonSuccesBericht = function () {
        var message = "Mike wil deelnemen aan jouw spel.Geef akkoord.";
        popup_widget.show("succes", message)
    }
    var toonErrorBericht = function (type, error) {
        popup_widget.show(type, error)
    }


    return {
        toonSuccesBericht: toonSuccesBericht,
        toonErrorBericht: toonErrorBericht,
        initModule: initModule
    }
})(jQuery);