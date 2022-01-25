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

  var toonSuccesBericht = function toonSuccesBericht(message) {
    popup_widget.show("succes", message);
  };

  var toonErrorBericht = function toonErrorBericht(message) {
    popup_widget.show("error", message);
  };

  return {
    toonSuccesBericht: toonSuccesBericht,
    toonErrorBericht: toonErrorBericht,
    initModule: initModule
  };
}(jQuery);

var popup_widget = function ($) {
  function show(type, message) {
    var iconPath = ""; //main container + children creation

    var elMainContainer = document.createElement("div"); // el.setAttribute("id", "popup_container");
    //changes colour + imgSource depending on the parameter 'succes'        

    if (type === "succes") {
      elMainContainer.setAttribute("class", "popup popup--state-success");
      iconPath = "/images/Succes_Icon.png";
    } else {
      elMainContainer.setAttribute("class", "popup popup--state-warning");
      iconPath = "/images/Error_Icon.png";
    }

    var elHeader = document.createElement("header");
    var elButtonClosePopup = document.createElement("input");
    elButtonClosePopup.type = "button";
    elButtonClosePopup.value = "X";
    elButtonClosePopup.className = "popup__button--close";
    var elMain = document.createElement("main");
    elMain.setAttribute("class", "popup__main"); // append children to main container

    elMainContainer.appendChild(elHeader);
    elHeader.appendChild(elButtonClosePopup);
    elMainContainer.appendChild(elMain);
    document.body.appendChild(elMainContainer); // styling containers + exit button

    elHeader.setAttribute("class", "popup__header");

    elButtonClosePopup.onclick = function () {
      hidePopup(elMainContainer);
    }; //Main content (includes Icon and Message)
    //Icon


    var elImageStateIcon = document.createElement("img");
    elImageStateIcon.src = iconPath;
    elImageStateIcon.className = "popup__icon"; //message

    var elParagraphMessageContent = document.createElement("span");
    elParagraphMessageContent.textContent = message; //append Children

    elMain.appendChild(elImageStateIcon);
    elMain.appendChild(elParagraphMessageContent); //succes message expension

    if (type === "succes") {
      var elFooter = document.createElement("footer");
      elFooter.className = "popup__footer";
      elMainContainer.appendChild(elFooter); //create & style button elements

      var elButtonAccept = document.createElement("input");
      elButtonAccept.className = "is-active";
      elButtonAccept.type = "button";
      elButtonAccept.value = "Akkoord";
      var elButtonDecline = document.createElement("input"); // elButtonDecline.className = "popup__button--choice";

      elButtonDecline.type = "button";
      elButtonDecline.value = "Weigeren"; // append button children to footer

      elFooter.appendChild(elButtonAccept);
      elFooter.appendChild(elButtonDecline);
    }

    _store(message);
  } // sets main container.visiblity to hidden


  function hidePopup(el) {
    el.classList.add("popup--state-closing");
    setTimeout(function () {
      el.remove();
    }, 500);
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
}(jQuery);