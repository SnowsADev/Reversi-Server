"use strict";

function _typeof(o) { "@babel/helpers - typeof"; return _typeof = "function" == typeof Symbol && "symbol" == typeof Symbol.iterator ? function (o) { return typeof o; } : function (o) { return o && "function" == typeof Symbol && o.constructor === Symbol && o !== Symbol.prototype ? "symbol" : typeof o; }, _typeof(o); }
function _defineProperty(obj, key, value) { key = _toPropertyKey(key); if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }
function _toPropertyKey(t) { var i = _toPrimitive(t, "string"); return "symbol" == _typeof(i) ? i : String(i); }
function _toPrimitive(t, r) { if ("object" != _typeof(t) || !t) return t; var e = t[Symbol.toPrimitive]; if (void 0 !== e) { var i = e.call(t, r || "default"); if ("object" != _typeof(i)) return i; throw new TypeError("@@toPrimitive must return a primitive value."); } return ("string" === r ? String : Number)(t); }
var Game = function ($) {
  var _configMap = {
    environment: "development",
    spelToken: null,
    spelerToken: null,
    backendAdress: "https://localhost:44378"
  };
  var _gameState = null;

  //initialize function
  var init = function init() {
    var env = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : 'development';
    var spelerToken = arguments.length > 1 ? arguments[1] : undefined;
    var spelToken = arguments.length > 2 ? arguments[2] : undefined;
    _configMap.environment = env;
    _configMap.spelerToken = spelerToken;
    _configMap.spelToken = spelToken;
    Game.Template.init();
    Game.API.init();
    Game.Data.init();
    Game.Reversi.init();
    Game.Stats.init();
    console.log("Game initialized");
    console.log("Game running in ".concat(env, " mode"));
    refresh().then(function () {
      return _createInterface();
    });
  };
  var refresh = function refresh() {
    var url = getEnvironment() === "development" ? "api/game.json" : _getEndpointUrl() + "/" + getSpelToken();
    return Game.Data.get(url).then(function (data) {
      var gameState = getEnvironment() === "development" ? data : JSON.parse(data);
      setGameState(gameState);
    })["catch"](function (e) {
      console.error(e);
    });
  };
  var _createInterface = function _createInterface() {
    Game.API.showRelevantNews($("#swiper-container"));
    Game.Stats.showCharts($("#charts-container"), 'stats');
    Game.Reversi.show("#main-content");
    console.log("created interface!");
  };
  var _updateInterface = function _updateInterface() {
    Game.Stats.showCharts($("#charts-container"), 'stats');
    Game.Reversi.show("#main-content");
  };
  var getEnvironment = function getEnvironment() {
    return _configMap.environment;
  };
  var getSpelerToken = function getSpelerToken() {
    return _configMap.spelerToken;
  };
  var getSpelToken = function getSpelToken() {
    return _configMap.spelToken;
  };
  var _getEndpointUrl = function _getEndpointUrl() {
    switch (getEnvironment()) {
      case "development":
        return "api/game.json";
      case "production":
        return _configMap.backendAdress + "/api/spel";
    }
  };
  var getSpeler = function getSpeler() {
    return null;
  };
  var getGameState = function getGameState() {
    if (_gameState === null) {
      refresh();
    }
    return _gameState;
  };
  var setGameState = function setGameState(gameState) {
    _gameState = gameState;
    _updateInterface();
  };
  return {
    init: init,
    getEnvironment: getEnvironment,
    getSpelerToken: getSpelerToken,
    getSpelToken: getSpelToken,
    getSpeler: getSpeler,
    getGameState: getGameState,
    setGameState: setGameState,
    refresh: refresh
  };
}(jQuery);
Game.API = function () {
  var swiper;
  var init = function init() {
    console.log("Game.API initialized");
  };
  var _getNewsFromAPI = function _getNewsFromAPI() {
    switch (Game.getEnvironment()) {
      case "development":
        return Game.Data.get("api/api.json");
      case "production":
        var source = "https://newsapi.org/v2/top-headlines";
        var params = {
          today: new Date().toISOString().split('T')[0],
          sortBy: "popularity",
          country: "nl",
          apiKey: "8b95393f1cc34b22b5d9f17892e4099e"
        };
        var URL = "".concat(source, "?from=").concat(params.today, "&sortBy=").concat(params.sortBy, "&country=").concat(params.country, "&apiKey=").concat(params.apiKey);
        return Game.Data.get(URL);
    }
  };
  var _parseTemplate = function _parseTemplate(data) {
    return Game.Template.parseTemplate("artiekelen", data);
  };
  var _updateDOM = function _updateDOM(parsedTemplate, container) {
    $("#swiper-container").append(parsedTemplate);
    new Swiper('#swiper-container', {
      // Optional parameters
      direction: 'vertical',
      loop: true,
      mousewheel: true,
      slidesPerView: 2,
      pagination: {
        el: ".swiper-pagination",
        clickable: true,
        type: "progressbar"
      }
    });
  };
  var showRelevantNews = function showRelevantNews(container) {
    _getNewsFromAPI().then(function (data) {
      var parsedTemplate = _parseTemplate(data);
      _updateDOM(parsedTemplate, container);
    });
  };
  return {
    init: init,
    showRelevantNews: showRelevantNews
  };
}();
Game.Data = function () {
  var _configMap = {
    signalRHub: "/SpelHub"
  };
  var connection = null;
  var init = function init() {
    // setup signalR connection
    if (Game.getEnvironment() === "production") {
      connection = new signalR.HubConnectionBuilder().withUrl(_configMap.signalRHub).build();

      //Refresh when a move is made
      connection.on("ReceiveRefreshGameNotification", _handleRefreshNotification);
      connection.on("ReceiveJoinRequest", _showJoinRequest);
      connection.on("ReceiveErrorMessage", _showErrorMessage);
      connection.start().then(function () {
        return console.log("SignalR started successfully");
      })["catch"](function (e) {
        return console.error(e.toString());
      });
    }
    console.log("Game.Data initialized");
  };
  var _showErrorMessage = function _showErrorMessage(message) {
    console.debug("Error! :\"".concat(message, "\""));
    var templateData = {
      success: false,
      image: "/images/Error_Icon.png",
      content: message
    };
    var parsedTemplate = Game.Template.parseTemplate("notificatie", templateData);
    $(".notification-container").append(parsedTemplate).ready(function () {
      $(".popup .popup__btn--decline").click(_closeNotification);
      $(".popup .popup__button--close").click(_closeNotification);
    });
  };
  var _showJoinRequest = function _showJoinRequest(message, spelId, spelerId) {
    console.debug("join request message: " + message);
    var templateData = _defineProperty(_defineProperty({
      success: true,
      image: Game.getEnvironment() === "development" ? "" : "~/" + "images/Succes_Icon.png",
      content: message,
      spelerId: spelerId
    }, "spelerId", spelerId), "spelId", spelId);
    var parsedTemplate = Game.Template.parseTemplate("notificatie", templateData);
    $(".notification-container").append(parsedTemplate).ready(function () {
      $(".popup .popup__btn--accept").click(_acceptJoinRequest);
      $(".popup .popup__btn--decline").click(_closeNotification);
      $(".popup .popup__button--close").click(_closeNotification);
    });
  };
  var _handleRefreshNotification = function _handleRefreshNotification() {
    console.debug("Refresh notification");
    Game.refresh();
  };
  var _acceptJoinRequest = function _acceptJoinRequest(e) {
    e.preventDefault();
    console.debug("accept noti");
    //This function is called when the request is accepted. Thus $(this) will get the element from which the function is called.
    var request = $(this).closest(".popup");
    if (Game.getEnvironment() !== "development") {
      connection.invoke("SendJoinRequestResult", request[0].dataset.spelid, request[0].dataset.spelerid, true).then(function () {
        _closeNotification(e);
      }).then(function () {
        return Game.refresh();
      })["catch"](function (err) {
        return console.error(err.toString());
      });
    } else {
      _closeNotification(e);
    }
  };
  var _closeNotification = function _closeNotification(e) {
    e.preventDefault();
    var el = $(e.currentTarget).closest(".popup");
    el.addClass("popup--state-closing");
    setTimeout(function () {
      el.remove();
    }, 500);
  };
  var get = function get(url) {
    return new Promise(function (resolve, reject) {
      return $.ajax({
        url: url,
        success: function success(data) {
          return resolve(data);
        },
        error: function error(e) {
          return reject(e);
        }
      });
    });
  };
  var put = function put(url, data) {
    return new Promise(function (resolve, reject) {
      return $.ajax({
        url: url,
        contentType: "application/json",
        type: "PUT",
        data: JSON.stringify(data),
        success: function success(data) {
          return resolve(data);
        },
        error: function error(e) {
          console.error(e);
          reject(e);
        }
      });
    });
  };
  return {
    init: init,
    get: get,
    put: put
  };
}();
Game.Reversi = function () {
  var _isWaitingForPlayers = true;
  var init = function init() {
    console.log("Game.Reversi initialized ");
  };
  var show = function show(parent) {
    var gameState = Game.getGameState();
    _isWaitingForPlayers = gameState.Spelers.length < 2;
    if (Game.getGameState() === null) {
      console.error("GameState was not found");
      return;
    }
    var isDisabled = _isWaitingForPlayers || gameState.SpelIsAfgelopen;
    _createBord(parent, isDisabled);
    if (gameState.SpelIsAfgelopen) {
      _createAfgelopenNotificatie(parent);
    }
  };
  var _createAfgelopenNotificatie = function _createAfgelopenNotificatie(parent) {
    var gameState = Game.getGameState();
    var isWinnaar;
    switch (Game.getEnvironment()) {
      case "development":
        isWinnaar = true;
        break;
      default:
        var kleur = gameState.Spelers.find(function (s) {
          return s.Id === Game.getSpelerToken();
        }).Kleur;
        var stats = Game.Stats.getStats();
        isWinnaar = kleur === 1 ? stats.aantalWit > stats.aantalZwart : stats.aantalZwart > stats.aantalWit;
        break;
    }
    var html = Game.Template.parseTemplate("afgelopen", {
      isWinnaar: isWinnaar
    });
    var container = $(parent);
    if (container.find('.afgelopen').length > 0) {
      container.find('.afgelopen').replaceWith(html);
    } else {
      container.append(html);
    }
  };
  var _createBord = function _createBord(parent, isDisabled) {
    var html = Game.Template.parseTemplate("speelbord", {
      disabled: isDisabled,
      bord: Game.getGameState().Bord
    });
    var container = $(parent);
    if (container.find('.bord__wrapper, .bord__wrapper--disabled').length > 0) {
      container.find('.bord__wrapper, .bord__wrapper--disabled').replaceWith(html);
    } else {
      container.append(html);
    }
    if (!isDisabled) {
      _addHandlers();
    }
  };
  var _addHandlers = function _addHandlers() {
    var cells = $("#bord__table td");
    cells.click(_handleOnClick);
    cells.mouseenter(_HandleMouseMovement);
    cells.mouseleave(_HandleMouseMovement);
  };
  var _handleOnClick = function _handleOnClick(e) {
    e.preventDefault();
    if (_isWaitingForPlayers || _isAanDeBeurt() === false) return;
    var cell = $(this);
    var row = parseInt(cell[0].parentNode.id);
    var column = parseInt(cell[0].id);
    if (_moveIsPossible(row, column)) {
      if (Game.getEnvironment() !== "development") {
        Game.Data.put("/api/spel/zet", {
          RijZet: row,
          KolomZet: column,
          SpelToken: Game.getSpelToken(),
          SpelerToken: Game.getSpelerToken()
        }).then(function (data) {
          Game.setGameState(JSON.parse(data));
        });
      }
    }
  };
  var _moveIsPossible = function _moveIsPossible(row, column) {
    return Game.getGameState().MogelijkeZetten.some(function (obj) {
      return obj.x === row && obj.y === column;
    });
  };
  var _HandleMouseMovement = function _HandleMouseMovement(e) {
    var cell = $(this);
    var row = parseInt(cell[0].parentNode.id);
    var column = parseInt(cell[0].id);
    if (_isWaitingForPlayers || !_isAanDeBeurt()) return;
    switch (e.type) {
      case "mouseenter":
        if (_moveIsPossible(row, column)) {
          cell.addClass("bord__td--active");
        }
        break;
      case "mouseleave":
        cell.removeClass("bord__td--active");
        break;
    }
  };
  var _isAanDeBeurt = function _isAanDeBeurt() {
    if (Game.getEnvironment() === "development") {
      return true;
    }
    return Game.getGameState().AandeBeurt === Game.getGameState().Spelers.find(function (speler) {
      return speler.Id === Game.getSpelerToken();
    }).Kleur;
  };
  return {
    init: init,
    show: show
  };
}();
Game.Stats = function () {
  var _configMap = {
    containerId: "chart-container",
    stonesPerPlayerChartData: {
      chart: null,
      numberOfBlackPieces: 0,
      numberOfWhitePieces: 0
    },
    turn: {
      player: 1,
      count: 0
    }
  };
  var init = function init() {
    console.log("Game.Stats initialized");
  };
  var showCharts = function showCharts(container, id) {
    //Insert templates into container
    var stonesPerPlayerChart = Game.Template.parseTemplate("stats", {
      "id": id
    });
    container.append(stonesPerPlayerChart);

    //Draw the charts on templates
    _createCharts(id, "");
  };
  var updateStats = function updateStats() {
    return new Promise(function (resolve, reject) {
      var gameState = Game.getGameState();
      if (gameState !== null) {
        _updatePieces(gameState.Bord);
        resolve();
        return;
      }
      reject();
      return;
    });
  };
  var _updatePieces = function _updatePieces(board) {
    if (Array.isArray(board) === false) {
      console.error("Board is not an array");
      console.debug(board);
      return;
    }
    var white = 0;
    var black = 0;
    board.forEach(function (row) {
      row.forEach(function (cell) {
        if (cell === 1) {
          white++;
        }
        if (cell === 2) {
          black++;
        }
      });
    });
    _configMap.stonesPerPlayerChartData.numberOfBlackPieces = black;
    _configMap.stonesPerPlayerChartData.numberOfWhitePieces = white;
  };
  var _createCharts = function _createCharts(chartA, chartB) {
    updateStats().then(function () {
      var sppChart = _configMap.stonesPerPlayerChartData.chart; // stones per player chart
      if (sppChart !== null && sppChart !== undefined) {
        _configMap.stonesPerPlayerChartData.chart.destroy();
      }
      sppChart = new Chart(chartA, {
        type: 'pie',
        data: {
          labels: ['White', 'Black'],
          datasets: [{
            label: 'Stenen per speler',
            data: [_configMap.stonesPerPlayerChartData.numberOfWhitePieces, _configMap.stonesPerPlayerChartData.numberOfBlackPieces],
            backgroundColor: ['rgb(255,255,255)', 'rgb(0,0,0)'],
            borderWidth: 0
          }]
        },
        options: _getChartOptions('Stenenverdeling')
      });
      _configMap.stonesPerPlayerChartData.chart = sppChart;
      $("#" + chartA).show();
    })["catch"](function (e) {
      console.error(e);
      $("#" + chartA).hide();
    });
  };
  var _getChartOptions = function _getChartOptions(chartTitle) {
    return {
      responsive: true,
      plugins: {
        legend: {
          position: 'top',
          onClick: function onClick(e) {
            return e.stopPropagation();
          },
          labels: {
            color: 'rgb(0,0,0)',
            family: 'Arial'
          }
        },
        title: {
          display: true,
          text: chartTitle,
          color: 'rgb(0,0,0)',
          font: {
            size: 20,
            weight: 'normal',
            family: 'Arial'
          }
        }
      }
    };
  };
  var getStats = function getStats() {
    return {
      aantalZwart: _configMap.stonesPerPlayerChartData.numberOfBlackPieces,
      aantalWit: _configMap.stonesPerPlayerChartData.numberOfWhitePieces
    };
  };
  return {
    init: init,
    updateStats: updateStats,
    showCharts: showCharts,
    getStats: getStats
  };
}();
Game.Template = function () {
  var init = function init() {
    console.log("Game.Template initialized");
  };
  function getTemplate(templateName) {
    return spa_templates.templates[templateName];
  }
  function parseTemplate(templateName, data) {
    var template = getTemplate(templateName);
    return template(data);
  }
  return _defineProperty(_defineProperty(_defineProperty({
    init: init,
    getTemplate: getTemplate
  }, "getTemplate", getTemplate), "parseTemplate", parseTemplate), "parseTemplate", parseTemplate);
}();