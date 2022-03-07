"use strict";

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

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
    endpoint: "/api/spel",
    environment: ""
  };
  var _data = null; //initialize function

  var initModule = function initModule(environment) {
    var endpoint = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : "/api/spel";
    var spelerId = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : null;
    var spelId = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : null;
    _configMap.environment = environment;
    _configMap.endpoint = endpoint;
    _configMap.spelerId = spelerId;
    _configMap.spelId = spelId;
    return true;
  };

  var getSpelerId = function getSpelerId() {
    return _configMap.spelerId;
  };

  var makeMove = function makeMove(row, column) {
    if (!SPA.Reversi.isAanDeBeurt()) return;
    var result;

    if (_configMap.environment === "production") {
      var requestBody = {
        rijZet: row,
        kolomZet: column,
        spelToken: SPA.Reversi._spel.ID,
        spelerToken: _configMap.spelerId
      };
      result = $.ajax({
        dataType: "json",
        contentType: "application/json",
        accepts: "application/json",
        crossDomain: true,
        type: "PUT",
        url: _configMap.endpoint + "/Zet",
        data: JSON.stringify(requestBody),
        async: false,
        success: function success(data) {
          SPA.Reversi._spel = data;
          SPA.Reversi.show();
          sendRefreshGameNotification();
          return true;
        },
        error: function error(err) {
          console.log(err);
          SPA.feedbackModule.toonErrorBericht(err.status + ": " + err.responseJSON.title);
        }
      });
    } else {
      if (this._spel === undefined || this._spel === null) {
        this._spel = getSpellen();
      }

      if (SPA.Reversi.movePossible(row, column)) {
        SPA.Data._spel.Bord[row][column] = SPA.Data._spel.AandeBeurt;
        SPA.Data._spel.AandeBeurt = SPA.Data._spel.AandeBeurt === 1 ? 2 : 1;
        result = true;
      }
    }

    return result;
  };

  var sendRefreshGameNotification = function sendRefreshGameNotification() {
    SPA.Reversi._connection.invoke('SendRefreshGameNotification', _configMap.spelerId, _configMap.spelId)["catch"](function (err) {
      SPA.feedbackModule.toonErrorBericht(err.toString());
      return console.error(err.toString());
    });
  };

  var getSpellen = function getSpellen() {
    var result;

    if (_configMap.environment === "production") {
      $.ajax({
        dataType: "json",
        contentType: "application/json",
        crossDomain: true,
        type: "GET",
        url: _configMap.endpoint + "/".concat(_configMap.spelId),
        async: false,
        success: function success(data) {
          result = data;
        },
        error: function error(err) {
          console.log(err);
          SPA.feedbackModule.toonErrorBericht(err.status + ": " + err.responseJSON.title);
        }
      });
    } else if (this._spel === null || this._spel === undefined) {
      $.ajax({
        dataType: "json",
        contentType: "application/json",
        type: "GET",
        url: "api/game.json",
        async: false,
        success: function success(data) {
          result = data;
        },
        error: function error() {
          SPA.feedbackModule.toonErrorBericht("Failed request to server.");
        }
      });
    } else {
      return this._spel;
    }

    return result;
  };

  var passMove = function passMove() {
    if (!SPA.Reversi.isAanDeBeurt()) return;
    var result;

    if (_configMap.environment === "production") {
      var requestBody = {
        spelId: SPA.Reversi._spel.ID,
        spelerId: _configMap.spelerId
      };
      result = $.ajax({
        dataType: "json",
        contentType: "application/json",
        accepts: "application/json",
        crossDomain: true,
        type: "POST",
        url: _configMap.endpoint + "/Pass",
        data: JSON.stringify(requestBody),
        async: false,
        success: function success(data) {
          SPA.Reversi._spel = data;
          SPA.Reversi.show();
          sendRefreshGameNotification();
          return true;
        },
        error: function error(err) {
          console.log(err.toString());
          SPA.feedbackModule.toonErrorBericht(err.status + ": " + err.responseJSON.title);
        }
      });
    } else {
      if (this._spel === undefined || this._spel === null) {
        this._spel = getSpellen();
      }

      if (SPA.Reversi._possibleMoves.length === 0) {
        SPA.Data._spel.AandeBeurt = SPA.Data._spel.AandeBeurt === 1 ? 2 : 1;
        result = true;
      }
    }

    return result;
  };

  var terugNaarOverzicht = function terugNaarOverzicht() {
    if (_configMap.environment === "production") {
      window.location.href = window.location.origin + '/Spellen/';
    }
  };

  return {
    initModule: initModule,
    getSpellen: getSpellen,
    makeMove: makeMove,
    passMove: passMove,
    getSpelerId: getSpelerId,
    terugNaarOverzicht: terugNaarOverzicht
  };
}(jQuery);

SPA.feedbackModule = function ($) {
  var _configMap = {}; //initialize function

  var initModule = function initModule() {
    return true;
  };

  var toonSuccesBericht = function toonSuccesBericht(message) {
    var data = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : null;
    popup_widget.show("succes", message, data);
  };

  var toonErrorBericht = function toonErrorBericht(error) {
    popup_widget.show("error", error);
  };

  return {
    toonSuccesBericht: toonSuccesBericht,
    toonErrorBericht: toonErrorBericht,
    initModule: initModule
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
}(jQuery); // const signalR = require("~/dist/signalr/signalr.min.js");


SPA.Reversi = function ($) {
  var _configMap = {
    signalRHub: "/SpelHub"
  };

  var initModule = function initModule() {
    SPA.Reversi._spel = null;
    SPA.Reversi._currentSpeler = null;
    SPA.Reversi._isWaitingForPlayers = false; //SignalR setup

    SPA.Reversi._connection = new signalR.HubConnectionBuilder().withUrl(_configMap.signalRHub).build(); //Refresh when a move is made

    SPA.Reversi._connection.on("ReceiveRefreshGameNotification", refreshSpel); //Show join requests


    SPA.Reversi._connection.on("ReceiveJoinRequest", function (message, spelId, spelerId) {
      var _data2;

      var data = (_data2 = {
        spelerId: spelerId
      }, _defineProperty(_data2, "spelerId", spelerId), _defineProperty(_data2, "spelId", spelId), _data2);
      SPA.feedbackModule.toonSuccesBericht(message, data);
      $('.popup__btn--accept').each(function (index, element) {
        if ($(element).attr("onClick") === undefined) {
          $(element).on("click", function () {
            connection.invoke("SendJoinRequestResult", data.spelId, data.spelerId, true)["catch"](function (err) {
              return console.error(err.toString());
            });
          });
        }
      });
    }); //Show errors


    SPA.Reversi._connection.on("ReceiveErrorMessage", function (message) {
      SPA.feedbackModule.toonErrorBericht(message);
    }); //start game


    SPA.Reversi._connection.start().then(function () {
      show();
    })["catch"](function (err) {
      SPA.feedbackModule.toonErrorBericht(err.toString());
      return console.error(err.toString());
    });
  };

  var refreshSpel = function refreshSpel() {
    SPA.Reversi._spel = SPA.Data.getSpellen();
    show();
  };

  var show = function show() {
    if (SPA.Reversi._spel === null || SPA.Reversi._spel === undefined) {
      SPA.Reversi._spel = SPA.Data.getSpellen();
    }

    if (SPA.Reversi._spel.Spelers.length < 2) {
      SPA.Reversi._isWaitingForPlayers = true;
      createWachtenOpSpelersbericht();
    }

    var spelers = SPA.Reversi._spel.Spelers;
    var applicationUserId = SPA.Data.getSpelerId();
    spelers.forEach(function (speler) {
      if (speler.Id === applicationUserId) {
        SPA.Reversi._currentSpeler = speler;
      }
    });
    showScoreboard();

    if (SPA.Reversi._spel.Bord === undefined || SPA.Reversi._spel.Bord === null) {
      SPA.feedbackModule.toonErrorBericht("Failed request to server.");
      return;
    }

    createBord();
    SPA.Reversi._possibleMoves = calcAllPossibleMoves();
    var numberOfEmptySpaces = calcEmptySpaces();

    if (numberOfEmptySpaces === 0 || SPA.Reversi._spel.Afgelopen === 1) {
      // Spel is afgelopen
      createResultScreen();
      return;
    }

    if (SPA.Reversi._possibleMoves.length === 0) {
      SPA.Data.passMove();
    }
  };

  var createWachtenOpSpelersbericht = function createWachtenOpSpelersbericht() {
    var BestaandBericht = document.getElementById("spel__wachtenOpSpelersBericht");

    if (BestaandBericht !== null) {
      document.body.removeChild(BestaandBericht);
    }

    var elWaitingMessage = document.createElement("div");
    elWaitingMessage.id = "spel__wachtenOpSpelersBericht";
    var elWaitingMessageText = document.createElement("span");
    elWaitingMessageText.textContent = "Aan het wachten op spelers...";
    elWaitingMessage.appendChild(elWaitingMessageText); //Loading Icon

    var elLoadingIcon = document.createElement("div");
    elLoadingIcon.className = "lds-roller";
    var elEmptyDiv;

    for (var i = 0; i < 8; i++) {
      elEmptyDiv = document.createElement("div");
      elLoadingIcon.appendChild(elEmptyDiv);
    }

    elWaitingMessage.appendChild(elLoadingIcon);
    document.body.appendChild(elWaitingMessage);
  };

  var showScoreboard = function showScoreboard() {
    SPA.Reversi._aantalZwart = 0;
    SPA.Reversi._aantalWit = 0; //Calc score

    SPA.Reversi._spel.Bord.forEach(function (rij) {
      rij.forEach(function (kolom) {
        switch (kolom) {
          case 1:
            SPA.Reversi._aantalWit++;
            break;

          case 2:
            SPA.Reversi._aantalZwart++;
            break;
        }
      });
    });

    var elWrapper = document.createElement("div");
    elWrapper.id = "scoreboard__wrapper";

    if (SPA.Reversi._spel.Spelers.length < 2) {
      elWrapper.className = "scoreboard__wrapper--disabled";
    }

    var elAanDeBeurt = document.createElement("div");
    elAanDeBeurt.id = "scoreboard__aanDeBeurt";
    elAanDeBeurt.textContent = "Aan zet: ".concat(AanDeBeurtToString());
    var elDivider = document.createElement("div");
    elDivider.className = "scoreboard__div";
    var elPuntenWit = document.createElement("div");
    elPuntenWit.textContent = "Zwart: ".concat(SPA.Reversi._aantalZwart);
    elPuntenWit.id = "scoreboard__punten--wit";
    elPuntenWit.className = "scoreboard__punten";
    var elPuntenZwart = document.createElement("div");
    elPuntenZwart.id = "scoreboard__punten--wit";
    elPuntenZwart.className = "scoreboard__punten";
    elPuntenZwart.textContent = "Wit: ".concat(SPA.Reversi._aantalWit);
    elWrapper.appendChild(elAanDeBeurt);
    elWrapper.appendChild(elDivider);
    elWrapper.appendChild(elPuntenWit);
    elWrapper.appendChild(elPuntenZwart);
    var currentScoreboard = document.querySelector('#scoreboard__wrapper');

    if (currentScoreboard !== null) {
      currentScoreboard.parentNode.removeChild(currentScoreboard);
    }

    document.body.appendChild(elWrapper);
  };

  var createBord = function createBord() {
    var Bord = SPA.Reversi._spel.Bord;
    var bordGrootte = Bord.length; //Wrapper

    var elBord = document.createElement("div");
    elBord.id = "bord__wrapper";

    if (SPA.Reversi._spel.Spelers.length < 2) {
      elBord.className = "bord__wrapper--disabled";
    } //Table


    var elTable = document.createElement("table");
    elTable.id = "bord__table";
    elTable.classList = ["table table-bordered"];
    var elTableBody = document.createElement("tbody");
    elTable.appendChild(elTableBody);
    var elBordRow;
    var elBordDataCell;

    for (var i = 0; i < bordGrootte; i++) {
      elBordRow = document.createElement("tr");
      elBordRow.id = i;
      elBordRow.setAttribute("scope", "row");

      for (var j = 0; j < bordGrootte; j++) {
        elBordDataCell = document.createElement("td");
        elBordDataCell.id = String.fromCharCode(j + 65) + i; // elBordDataCell.textContent = elBordDataCell.id;

        elBordDataCell.setAttribute("bezetDoor", 0); //Niet bezet

        elBordDataCell.addEventListener('mouseenter', handleVisualZetMogelijk);
        elBordDataCell.addEventListener('mouseleave', handleVisualZetMogelijk);
        elBordDataCell.onclick = handleTableCellOnClick; // Stenen toevoegen

        createStone(elBordDataCell, i, j);
        elBordRow.appendChild(elBordDataCell);
      }

      elTableBody.appendChild(elBordRow);
    }

    var currentTable = document.querySelector('#bord__wrapper');

    if (currentTable !== null) {
      currentTable.parentNode.removeChild(currentTable);
    }

    elBord.appendChild(elTable);
    document.body.appendChild(elBord);
  };

  var createStone = function createStone(elBordDataCell, row, column) {
    var Bord = SPA.Reversi._spel.Bord;

    if (Bord[row][column] > 0) {
      var elSteen = document.createElement("div");
      elSteen.className = "bord__steen";

      switch (Bord[row][column]) {
        case 1:
          elBordDataCell.setAttribute("bezetDoor", 1); //Witte steen

          elSteen.classList.add("bord__steen--wit");
          break;

        case 2:
          elBordDataCell.setAttribute("bezetDoor", 2); //Witte steen

          elSteen.classList.add("bord__steen--zwart");
          break;

        default:
          break;
      }

      elBordDataCell.appendChild(elSteen);
    }
  };

  var createResultScreen = function createResultScreen() {
    SPA.Reversi._spel.Bord.forEach(function (rij) {
      rij.forEach(function (kolom) {
        switch (kolom) {
          case 0:
            SPA.Reversi._aantalLeeg++;
            break;

          case 1:
            SPA.Reversi._aantalWit++;
            break;

          case 2:
            SPA.Reversi._aantalZwart++;
            break;
        }
      });
    });

    var elOverlay = document.createElement('div');
    elOverlay.id = 'page__overlay';
    var elResults = document.createElement('div');
    elResults.id = 'page__results';
    var elHeaderSpan = document.createElement('span');
    var overzichtBericht = ' heeft het spel gewonnen!';

    if (SPA.Reversi._aantalWit > SPA.Reversi._aantalZwart) {
      overzichtBericht = "Wit" + overzichtBericht;
    } else if (SPA.Reversi._aantalZwart > SPA.Reversi._aantalWit) {
      overzichtBericht = "Zwart" + overzichtBericht;
    } else {
      overzichtBericht = "Gelijkspel!";
    }

    elHeaderSpan.innerHTML = overzichtBericht;
    var elTerugNaarOverzichtButton = document.createElement('button');
    elTerugNaarOverzichtButton.onclick = SPA.Data.terugNaarOverzicht;
    elTerugNaarOverzichtButton.innerHTML = 'Terug naar overzicht';
    elResults.appendChild(elHeaderSpan);
    elResults.appendChild(elTerugNaarOverzichtButton);
    elOverlay.appendChild(elResults);
    document.body.appendChild(elOverlay);
  };

  var handleTableCellOnClick = function handleTableCellOnClick(e) {
    e.preventDefault();
    if (SPA.Reversi._isWaitingForPlayers) return;
    if (!isAanDeBeurt()) return;
    var cell = e.target;
    var row = parseInt(e.path[1].id);
    var column = cell.id.charCodeAt(0) - 65;

    if (SPA.Reversi._possibleMoves.includes(cell.id)) {
      SPA.Data.makeMove(row, column);
      SPA.Reversi.show(); // SPA.Reversi._possibleMoves = calcAllPossibleMoves();
    }
  };

  var handleVisualZetMogelijk = function handleVisualZetMogelijk(e) {
    var cell = e.target;
    if (SPA.Reversi._isWaitingForPlayers) return;
    if (isAanDeBeurt() === false) return;

    switch (e.type) {
      case "mouseleave":
        cell.classList.remove("bord__td--active");
        break;

      case "mouseenter":
        // let row = cell.id.charCodeAt(0) - 66;
        // let column = parseInt(e.path[1].id);
        if (SPA.Reversi._possibleMoves.includes(cell.id)) {
          cell.classList.add("bord__td--active");
        }

        break;
    }
  };

  var calcAllPossibleMoves = function calcAllPossibleMoves() {
    var id;
    var result = [];
    var rows = $("#bord__table tr");
    rows.each(function () {
      $('td').each(function () {
        id = $(this).attr("id");
        var row = parseInt(id.charAt(1));
        var column = $(this).attr("id").charCodeAt(0) - 65;

        if (movePossible(row, column)) {
          result.push(id);
        }
      });
    });
    return result;
  };

  var calcEmptySpaces = function calcEmptySpaces() {
    var aantal = 0; //Calc score

    SPA.Reversi._spel.Bord.forEach(function (rij) {
      rij.forEach(function (kolom) {
        if (kolom === 0) {
          aantal++;
        }
      });
    });

    return aantal;
  };

  var movePossible = function movePossible(rijZet, kolomZet) {
    var zetMogelijk = false;
    var Bord = SPA.Reversi._spel.Bord;
    var AandeBeurt = SPA.Reversi._spel.AandeBeurt; //checks if position is on the board

    if (Bord.length <= rijZet || Bord[1].length <= kolomZet) return false; //Checks if position is NOT already taken

    var columnChar = String.fromCharCode(kolomZet + 65);
    var idToFind = columnChar + rijZet;
    var cell = $("#bord__table #".concat(idToFind)).get(0);

    if (cell.getAttribute("bezetDoor") > 0) {
      return false;
    } //checks all positions around chosen position


    var startCheckRijPositie = rijZet - 1;
    var startCheckKolomPositie = kolomZet - 1;
    var eindCheckrijPositie = rijZet + 1;
    var eindCheckKolomPositie = kolomZet + 1; //check if starting Iterations And end Iterations arent < 0 OR bigger than board size

    if (startCheckKolomPositie < 0) startCheckKolomPositie = 0;
    if (startCheckRijPositie < 0) startCheckRijPositie = 0;
    if (eindCheckrijPositie >= Bord.length) eindCheckrijPositie = rijZet;
    if (eindCheckKolomPositie >= Bord[1]) eindCheckKolomPositie = kolomZet;

    for (var rij = startCheckRijPositie; rij <= eindCheckrijPositie; rij++) {
      for (var kolom = startCheckKolomPositie; kolom <= eindCheckKolomPositie; kolom++) {
        //If position has a different colour around it
        if (Bord[rij][kolom] == nietAandeBeurt()) {
          //Rij + Column the opposite colour that is found above
          var rijVerschil = rij - rijZet;
          var kolomVerschil = kolom - kolomZet;
          var kPos = kolom + kolomVerschil;
          var rPos = rij + rijVerschil; //kPos & rPos is the starting position

          while (rPos >= 0 && rPos < Bord.length && kPos >= 0 && kPos < Bord[1].length) {
            //stop when empty space
            if (Bord[rPos][kPos] == 0) break; //if discs are surrounded by this move

            if (Bord[rPos][kPos] == AandeBeurt) {
              zetMogelijk = true;
              break;
            }

            rPos += rijVerschil;
            kPos += kolomVerschil;
          }
        }
      }
    }

    return zetMogelijk;
  };

  var nietAandeBeurt = function nietAandeBeurt() {
    var AandeBeurt = SPA.Reversi._spel.AandeBeurt;
    return AandeBeurt === 1 ? 2 : 1;
  };

  var isAanDeBeurt = function isAanDeBeurt() {
    return SPA.Reversi._spel.AandeBeurt === SPA.Reversi._currentSpeler.Kleur;
  };

  var AanDeBeurtToString = function AanDeBeurtToString() {
    var SpelerAanZet = SPA.Reversi._spel.AandeBeurt;

    if (SpelerAanZet == 1) {
      return "Wit";
    }

    return "Zwart";
  };

  return {
    initModule: initModule,
    show: show,
    movePossible: movePossible,
    isAanDeBeurt: isAanDeBeurt,
    refreshSpel: refreshSpel
  };
}(jQuery);

var popup_widget = function ($) {
  function show(type, message) {
    var data = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : null;
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
      elMainContainer.dataset["data"] = data;
      var elFooter = document.createElement("footer");
      elFooter.className = "popup__footer";
      elMainContainer.appendChild(elFooter); //create & style button elements

      var elButtonAccept = document.createElement("input");
      elButtonAccept.className = "popup__btn--accept is-active";
      elButtonAccept.type = "button";
      elButtonAccept.value = "Akkoord"; // elButtonAccept.onclick = () => {
      //     connection.invoke("SendJoinRequestResult", data.spelID, data.spelerId, true).catch(function (err) {
      //         return console.error(err.toString());
      //     });
      // };

      var elButtonDecline = document.createElement("input");
      elButtonDecline.className = "popup__button--decline";
      elButtonDecline.type = "button";
      elButtonDecline.value = "Weigeren"; // elButtonDecline.onclick = () => {
      //     connection.invoke("SendJoinRequestResult", data.spelID, data.spelerId, false).catch(function (err) {
      //         return console.error(err.toString());
      //     });
      // };
      // append button children to footer

      elFooter.appendChild(elButtonAccept);
      elFooter.appendChild(elButtonDecline);
    }

    _store(message);
  }

  var acceptJoinRequest = function acceptJoinRequest(Data) {}; // sets main container.visiblity to hidden


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