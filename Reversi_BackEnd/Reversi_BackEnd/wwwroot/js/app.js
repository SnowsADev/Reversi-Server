"use strict";

var ModuleTemplate = function ($) {
  var _configMap = {}; //initialize function

  var initModule = function initModule() {
    return true;
  };

  return {
    initModule: initModule
  };
}(jQuery);

var SPA = function ($) {
  var _configMap = {}; //initialize function

  var initModule = function initModule() {
    return true;
  };

  return {
    initModule: initModule
  };
}(jQuery);

SPA.Data = function ($) {
  var _configMap = {
    endpoint: "../api/game.json",
    environment: ""
  }; //initialize function

  var initModule = function initModule(environment) {
    _configMap.environment = environment;
    return true;
  };

  var getSpellen = function getSpellen() {
    var result;

    if (environment === "production") {
      result = $.ajax({
        url: endpoint,
        succes: function succes() {
          return data;
        },
        error: function error() {
          SPA.feedbackModule.toonErrorBericht("error", "Failed request to server.");
        },
        type: "POST"
      });
    }

    return result;
  };

  return {
    initModule: initModule,
    getSpellen: getSpellen
  };
}(jQuery);

SPA.Model = function ($) {
  var _configMap = {}; //initialize function

  var initModule = function initModule() {
    return true;
  };

  return {
    initModule: initModule
  };
}(jQuery);

SPA.Reversi = function ($) {
  var _configMap = {}; //initialize function

  var initModule = function initModule() {
    return true;
  };

  return {
    initModule: initModule
  };
}(jQuery);

SPA.feedbackModule = function ($) {
  var _configMap = {}; //initialize function

  var initModule = function initModule() {
    return true;
  };

  var toonSuccesBericht = function toonSuccesBericht() {
    var message = "Mike wil deelnemen aan jouw spel.Geef akkoord.";
    popup_widget.show("succes", message);
  };

  var toonErrorBericht = function toonErrorBericht(type, error) {
    popup_widget.show(type, error);
  };

  return {
    toonSuccesBericht: toonSuccesBericht,
    toonErrorBericht: toonErrorBericht,
    initModule: initModule
  };
}(jQuery);

var popup_widget = function () {
  function show(type, message) {
    var color = "red";
    var imgPath = "/src/img/Error_Icon.png"; //changes colour + imgSource depending on the parameter 'succes'

    if (type === "succes") {
      color = "green";
      imgPath = "/src/img/Succes_Icon.png";
    } //main container + children creation


    var el = document.createElement("div");
    el.setAttribute("id", "popup_container");
    var topContainer = document.createElement("div");
    var hideButton = document.createElement("input");
    hideButton.type = "button";
    hideButton.value = "X";
    var mainContent = document.createElement("div"); // append children to main container

    el.appendChild(topContainer);
    topContainer.appendChild(hideButton);
    el.appendChild(mainContent);
    document.body.appendChild(el); // styling containers + exit button

    el.style = "width: 1000px; height: 150px; border: 2px solid black; background-color: " + color + ";";
    topContainer.style = "display: inline-block; height: 30px; width: 100%;";
    hideButton.style = "border: 2px solid black; width: 30px;  height: 30px; display: inline; float: right; margin-right: 5px; margin-top: 5px; color: " + color + ";";

    hideButton.onclick = function () {
      hidePopup(el);
    }; //Main content (includes Icon and Message)
    //var mainContent = document.getElementById("main_content");


    mainContent.style = "height: 100px; width: 100%; word-break: break-all; overflow:hidden;"; //Icon

    var warningIMG = document.createElement("img");
    warningIMG.src = imgPath;
    warningIMG.style = "width: 75px; height: 75px; float:left; margin-left: 20px;"; //message

    var message_content = document.createElement("H3");
    message_content.textContent = message;
    message_content.style = "color: black; float: left; margin-left: 40px; margin-top- 35px;"; //append Children

    mainContent.appendChild(warningIMG);
    mainContent.appendChild(message_content); //succes message expension

    if (type === "succes") {
      el.style.height = "200px";
      var footer = document.createElement("footer");
      footer.style = "height: 70px;";
      el.appendChild(footer); //create & style button elements

      var acceptButton = document.createElement("input");
      acceptButton.style = "width: 100px;  height: 30px; border: 2px solid black; float: right; margin-right: 10px;";
      acceptButton.type = "button";
      acceptButton.value = "Akkoord";
      var declineButton = document.createElement("input");
      declineButton.style = "width: 100px;  height: 30px; border: 2px solid black; float: right; margin-right: 10px;";
      declineButton.type = "button";
      declineButton.value = "Weigeren"; // append button children to footer

      footer.appendChild(acceptButton);
      footer.appendChild(declineButton);
    }

    _store(message);
  } // sets main container.visiblity to hidden


  function hidePopup(el) {
    var popupContainer = el;
    popupContainer.style.visibility = "hidden";
  } // stores messages in session


  function _store(message) {
    if (sessionStorage.msg) {
      var _msgArray = JSON.parse(sessionStorage.msg);

      _msgArray.push(message);

      sessionStorage.msg = JSON.stringify(_msgArray);
    } else {
      sessionStorage.msg = JSON.stringify([message]);
    } // if there are > 10 messages in session array shift array


    var msgArray = JSON.parse(sessionStorage.msg);

    if (msgArray.length > 10) {
      while (msgArray.length > 10) {
        msgArray.shift();
      }

      sessionStorage.msg = JSON.stringify(msgArray);
    }
  }

  return {
    show: show
  };
}();

console.log("Dit hoort aan het einde van het bestand te komen!");