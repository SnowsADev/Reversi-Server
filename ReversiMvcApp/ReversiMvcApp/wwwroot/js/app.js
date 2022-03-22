"use strict";

var _interopRequireDefault = require("@babel/runtime/helpers/interopRequireDefault");

var _regenerator = _interopRequireDefault(require("@babel/runtime/regenerator"));

var _defineProperty2 = _interopRequireDefault(require("@babel/runtime/helpers/defineProperty"));

var _asyncToGenerator2 = _interopRequireDefault(require("@babel/runtime/helpers/asyncToGenerator"));

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

  var maakZetAsync = /*#__PURE__*/function () {
    var _ref = (0, _asyncToGenerator2["default"])( /*#__PURE__*/_regenerator["default"].mark(function _callee(row, column) {
      var result, requestBody;
      return _regenerator["default"].wrap(function _callee$(_context) {
        while (1) {
          switch (_context.prev = _context.next) {
            case 0:
              if (SPA.Reversi.isAanDeBeurt()) {
                _context.next = 2;
                break;
              }

              return _context.abrupt("return");

            case 2:
              if (!(_configMap.environment === "production")) {
                _context.next = 7;
                break;
              }

              requestBody = {
                rijZet: row,
                kolomZet: column,
                spelToken: SPA.Reversi._spel.ID,
                spelerToken: _configMap.spelerId
              };
              return _context.abrupt("return", $.ajax({
                type: "PUT",
                url: _configMap.endpoint + "/Zet",
                data: JSON.stringify(requestBody),
                //headers
                dataType: "json",
                contentType: "application/json",
                accepts: "application/json",
                success: function success(data) {
                  SPA.Reversi._spel = data;
                  SPA.Reversi.show();
                  return true;
                },
                error: function error(err) {
                  console.log(err);
                  SPA.feedbackModule.toonErrorBericht(err.status + ": " + err.responseJSON.title);
                }
              }));

            case 7:
              if (!(SPA.Reversi._spel === undefined || SPA.Reversi._spel === null)) {
                _context.next = 11;
                break;
              }

              _context.next = 10;
              return getSpellenAsync();

            case 10:
              this.Reversi._spel = _context.sent;

            case 11:
              if (SPA.Reversi.zetMogelijk(row, column)) {
                SPA.Reversi._spel.Bord[row][column] = SPA.Reversi._spel.AandeBeurt;
                SPA.Reversi._spel.AandeBeurt = SPA.Reversi._spel.AandeBeurt === 1 ? 2 : 1;
                SPA.Reversi.show();
              }

            case 12:
            case "end":
              return _context.stop();
          }
        }
      }, _callee, this);
    }));

    return function maakZetAsync(_x, _x2) {
      return _ref.apply(this, arguments);
    };
  }();

  var getSpellenAsync = /*#__PURE__*/function () {
    var _ref2 = (0, _asyncToGenerator2["default"])( /*#__PURE__*/_regenerator["default"].mark(function _callee2() {
      return _regenerator["default"].wrap(function _callee2$(_context2) {
        while (1) {
          switch (_context2.prev = _context2.next) {
            case 0:
              if (!(_configMap.environment === "production")) {
                _context2.next = 4;
                break;
              }

              return _context2.abrupt("return", $.ajax({
                type: "GET",
                url: _configMap.endpoint + "/".concat(_configMap.spelId),
                // Headers
                dataType: "json",
                contentType: "application/json",
                crossDomain: true,
                success: function success(data) {
                  result = data;
                },
                error: function error(err) {
                  console.log(err);
                  SPA.feedbackModule.toonErrorBericht(err.status + ": " + err.responseJSON.title);
                }
              }));

            case 4:
              if (!(SPA.Reversi._spel === null || SPA.Reversi._spel === undefined)) {
                _context2.next = 8;
                break;
              }

              return _context2.abrupt("return", $.ajax({
                dataType: "json",
                contentType: "application/json",
                type: "GET",
                url: "api/game.json",
                async: true,
                success: function success(data) {
                  return data;
                },
                error: function error(err) {
                  console.error(err);
                  SPA.feedbackModule.toonErrorBericht("Failed request to server.");
                }
              }));

            case 8:
              return _context2.abrupt("return", SPA.Reversi._spel);

            case 9:
            case "end":
              return _context2.stop();
          }
        }
      }, _callee2);
    }));

    return function getSpellenAsync() {
      return _ref2.apply(this, arguments);
    };
  }();

  var passMoveAsync = /*#__PURE__*/function () {
    var _ref3 = (0, _asyncToGenerator2["default"])( /*#__PURE__*/_regenerator["default"].mark(function _callee3() {
      var result, requestBody;
      return _regenerator["default"].wrap(function _callee3$(_context3) {
        while (1) {
          switch (_context3.prev = _context3.next) {
            case 0:
              if (!SPA.Reversi.isAanDeBeurt()) {
                _context3.next = 2;
                break;
              }

              return _context3.abrupt("return");

            case 2:
              if (!(_configMap.environment === "production")) {
                _context3.next = 8;
                break;
              }

              requestBody = {
                spelToken: SPA.Reversi._spel.ID,
                spelerToken: _configMap.spelerId
              };
              _context3.next = 6;
              return $.ajax({
                dataType: "json",
                contentType: "application/json",
                accepts: "application/json",
                crossDomain: true,
                type: "PUT",
                url: _configMap.endpoint + "/Pass",
                data: JSON.stringify(requestBody),
                async: true,
                success: function success(data) {
                  result = data;
                  SPA.Reversi.show();
                  return true;
                },
                error: function error(err) {
                  console.log(err);
                  SPA.feedbackModule.toonErrorBericht(err.status + ": " + err.responseJSON.title);
                }
              });

            case 6:
              _context3.next = 10;
              break;

            case 8:
              // Development
              if (SPA.Reversi._spel === undefined || SPA.Reversi._spel === null) {
                SPA.Reversi._spel = getSpellenAsync();
              }

              if (SPA.Reversi._possibleMoves.length === 0) {
                SPA.Data._spel.AandeBeurt = SPA.Data._spel.AandeBeurt === 1 ? 2 : 1;
                result = true;
              }

            case 10:
              return _context3.abrupt("return", result);

            case 11:
            case "end":
              return _context3.stop();
          }
        }
      }, _callee3);
    }));

    return function passMoveAsync() {
      return _ref3.apply(this, arguments);
    };
  }();

  var terugNaarOverzicht = function terugNaarOverzicht() {
    if (_configMap.environment === "production") {
      $.ajax({
        type: "GET",
        url: "/Spellen/",
        error: function error(err) {
          console.log(err);
          SPA.feedbackModule.toonErrorBericht(err.status + ": " + err.responseJSON.title);
        }
      });
    }
  };

  return (0, _defineProperty2["default"])({
    initModule: initModule,
    getSpellenAsync: getSpellenAsync,
    maakZetAsync: maakZetAsync,
    passMoveAsync: passMoveAsync,
    getSpelerId: getSpelerId,
    terugNaarOverzicht: terugNaarOverzicht
  }, "terugNaarOverzicht", terugNaarOverzicht);
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
  var ID;
  var AandeBeurt = 0;
  var Bord = [];
  var Spelers = [];

  var aandeBeurtAsString = function aandeBeurtAsString() {};

  var nietAandeBeurt = function nietAandeBeurt() {}; //initialize function


  var initModule = function initModule(jsonData) {
    return true;
  };

  return {
    initModule: initModule,
    ID: ID,
    AandeBeurt: AandeBeurt,
    Bord: Bord,
    Spelers: Spelers,
    aandeBeurtAsString: aandeBeurtAsString,
    nietAandeBeurt: nietAandeBeurt
  };
}(jQuery); // const signalR = require("~/dist/signalr/signalr.min.js");


SPA.Reversi = function ($) {
  var _configMap = {
    signalRHub: "/SpelHub",
    environment: "development"
  };

  var initModule = function initModule() {
    SPA.Reversi._spel = null;
    SPA.Reversi._currentSpeler = null;
    SPA.Reversi._isWaitingForPlayers = false;

    if (_configMap.environment === "production") {
      //SignalR setup
      var connection = new signalR.HubConnectionBuilder().withUrl(_configMap.signalRHub).build(); //Refresh when a move is made

      connection.on("ReceiveSpelZetGemaakt", refreshSpel); //Show join requests

      connection.on("ReceiveJoinRequest", function (message, spelId, spelerId) {
        var _data2;

        var data = (_data2 = {
          spelerId: spelerId
        }, (0, _defineProperty2["default"])(_data2, "spelerId", spelerId), (0, _defineProperty2["default"])(_data2, "spelId", spelId), _data2);
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

      connection.on("ReceiveErrorMessage", function (message) {
        SPA.feedbackModule.toonErrorBericht(message);
      });
      connection.start().then(function () {
        show();
      })["catch"](function (err) {
        SPA.feedbackModule.toonErrorBericht(err.toString());
        return console.error(err.toString());
      });
    } else {
      show();
    }
  };

  var refreshSpel = /*#__PURE__*/function () {
    var _ref5 = (0, _asyncToGenerator2["default"])( /*#__PURE__*/_regenerator["default"].mark(function _callee4() {
      return _regenerator["default"].wrap(function _callee4$(_context4) {
        while (1) {
          switch (_context4.prev = _context4.next) {
            case 0:
              _context4.next = 2;
              return SPA.Data.getSpellenAsync();

            case 2:
              SPA.Reversi._spel = _context4.sent;
              show();

            case 4:
            case "end":
              return _context4.stop();
          }
        }
      }, _callee4);
    }));

    return function refreshSpel() {
      return _ref5.apply(this, arguments);
    };
  }();

  var show = /*#__PURE__*/function () {
    var _show = (0, _asyncToGenerator2["default"])( /*#__PURE__*/_regenerator["default"].mark(function _callee5() {
      var spelers, applicationUserId, numberOfEmptySpaces;
      return _regenerator["default"].wrap(function _callee5$(_context5) {
        while (1) {
          switch (_context5.prev = _context5.next) {
            case 0:
              if (!(SPA.Reversi._spel === null || SPA.Reversi._spel === undefined)) {
                _context5.next = 4;
                break;
              }

              _context5.next = 3;
              return SPA.Data.getSpellenAsync();

            case 3:
              SPA.Reversi._spel = _context5.sent;

            case 4:
              if (SPA.Reversi._spel.Spelers.length < 2) {
                SPA.Reversi._isWaitingForPlayers = true;
                showWachtenOpSpelersbericht();
              }

              spelers = SPA.Reversi._spel.Spelers;
              applicationUserId = SPA.Data.getSpelerId();
              spelers.forEach(function (speler) {
                if (speler.Id === applicationUserId) {
                  SPA.Reversi._currentSpeler = speler;
                }
              });
              showScoreboard();

              if (!(SPA.Reversi._spel.Bord === undefined || SPA.Reversi._spel.Bord === null)) {
                _context5.next = 12;
                break;
              }

              SPA.feedbackModule.toonErrorBericht("Failed request to server.");
              return _context5.abrupt("return");

            case 12:
              showBord();
              SPA.Reversi._possibleMoves = calcAllPossibleMoves();
              numberOfEmptySpaces = calcEmptySpaces();

              if (!(numberOfEmptySpaces === 0 || SPA.Reversi._spel.Afgelopen === 1)) {
                _context5.next = 18;
                break;
              }

              // Spel is afgelopen
              showResultScreen();
              return _context5.abrupt("return");

            case 18:
              if (SPA.Reversi._possibleMoves.length === 0) {
                alert('No Moves Possible');
                SPA.Reversi.passMove();
              }

            case 19:
            case "end":
              return _context5.stop();
          }
        }
      }, _callee5);
    }));

    function show() {
      return _show.apply(this, arguments);
    }

    return show;
  }();

  var showWachtenOpSpelersbericht = function showWachtenOpSpelersbericht() {
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
    elWrapper.className = "scoreboard";

    if (SPA.Reversi._spel.Spelers.length < 2) {
      elWrapper.className = "scoreboard__wrapper--disabled";
    }

    var elAanDeBeurt = document.createElement("header"); // elAanDeBeurt.className = "scoreboard__aanDeBeurt";

    elAanDeBeurt.textContent = "Aan zet: ".concat(AanDeBeurtToString());
    var elDivider = document.createElement("div");
    elDivider.className = "scoreboard__div";
    var elPuntenWit = document.createElement("section");
    elPuntenWit.textContent = "Zwart: ".concat(SPA.Reversi._aantalWit); // elPuntenWit.className = "scoreboard__punten";

    var elPuntenZwart = document.createElement("section"); // elPuntenZwart.className = "scoreboard__punten";

    elPuntenZwart.textContent = "Wit: ".concat(SPA.Reversi._aantalZwart);
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

  var showBord = function showBord() {
    var Bord = SPA.Reversi._spel.Bord;
    var bordGrootte = Bord.length; //Wrapper

    var elBord = document.createElement("div");
    elBord.className = "bord__wrapper";

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

        showFiche(i, j, Bord[i][j], elBordDataCell);
        elBordRow.appendChild(elBordDataCell);
      }

      elTableBody.appendChild(elBordRow);
    }

    var currentTable = document.getElementById('bord__wrapper');

    if (currentTable !== null) {
      currentTable.parentNode.removeChild(currentTable);
    }

    elBord.appendChild(elTable);
    document.body.appendChild(elBord);
  };

  var showFiche = function showFiche(row, column, kleur) {
    var elBordDataCell = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : null;
    var Bord = SPA.Reversi._spel.Bord;

    if (elBordDataCell === null) {
      var elBordDataCellId = String.fromCharCode(row + 65) + column;
      elBordDataCell = document.getElementById(elBordDataCellId);
    }

    if (kleur > 0) {
      elBordDataCell.innerHTML = '';
      var elSteen = document.createElement("div");

      switch (kleur) {
        case 1:
          elBordDataCell.setAttribute("bezetDoor", 1); //Witte steen

          elSteen.className = "bord__fiche--wit";
          break;

        case 2:
          elBordDataCell.setAttribute("bezetDoor", 2); //Witte steen

          elSteen.className = "bord__fiche--zwart";
          break;

        default:
          break;
      }

      elBordDataCell.appendChild(elSteen);
    }
  };

  var showResultScreen = function showResultScreen() {
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

  var handleTableCellOnClick = /*#__PURE__*/function () {
    var _ref6 = (0, _asyncToGenerator2["default"])( /*#__PURE__*/_regenerator["default"].mark(function _callee6(e) {
      var cell, row, column;
      return _regenerator["default"].wrap(function _callee6$(_context6) {
        while (1) {
          switch (_context6.prev = _context6.next) {
            case 0:
              e.preventDefault(); // console.log(SPA.Reversi._isWaitingForPlayers);
              // console.log(!isAanDeBeurt());

              if (!SPA.Reversi._isWaitingForPlayers) {
                _context6.next = 3;
                break;
              }

              return _context6.abrupt("return");

            case 3:
              if (isAanDeBeurt()) {
                _context6.next = 5;
                break;
              }

              return _context6.abrupt("return");

            case 5:
              cell = e.target;
              row = parseInt(e.path[1].id);
              column = cell.id.charCodeAt(0) - 65;

              if (!SPA.Reversi._possibleMoves.includes(cell.id)) {
                _context6.next = 11;
                break;
              }

              _context6.next = 11;
              return SPA.Data.maakZetAsync(row, column).then(function () {
                refreshSpel();
              });

            case 11:
            case "end":
              return _context6.stop();
          }
        }
      }, _callee6);
    }));

    return function handleTableCellOnClick(_x3) {
      return _ref6.apply(this, arguments);
    };
  }();

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

        if (zetMogelijk(row, column)) {
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

  var zetMogelijk = function zetMogelijk(rijZet, kolomZet) {
    var zetMogelijk = false;
    var Bord = SPA.Reversi._spel.Bord;
    var AandeBeurt = SPA.Reversi._spel.AandeBeurt; //checks if position is on the board

    if (Bord.length <= rijZet || Bord[1].length <= kolomZet) return false; //Checks if position is NOT already taken

    var idToFind = String.fromCharCode(kolomZet + 65) + rijZet;
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
    return true;
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
    zetMogelijk: zetMogelijk,
    isAanDeBeurt: isAanDeBeurt,
    refreshSpel: refreshSpel,
    showFiche: showFiche
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
      elButtonDecline.className = "popup__btn--decline";
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