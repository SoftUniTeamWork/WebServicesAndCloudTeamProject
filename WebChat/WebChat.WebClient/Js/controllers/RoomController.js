var app = app || {};

app.roomController = (function () {
    function RoomController(model, views){
        this.model = model;
        this.views = views
    }

    RoomController.prototype.getAllRooms = function () {
      var _this= this;
      this.model.getAllRooms()
          .then(function (data) {
              console.log(data);
              _this.views.getAllRooms(data);
          }, function (error) {
              console.log(error);
              console.log(error.responseText);
          });
    };

    RoomController.prototype.createRoom = function (data) {
        var _this = this;
        this.model.createRoom(data)
            .then(function (data) {
                console.log(data);
            }, function (err) {
                console.log(err);
                console.log(err.responseText);
            })
    };

    RoomController.prototype.joinRoom = function (data) {
            var _this = this;
        this.model.joinRoom(data.attr('id'))
            .then(function (_data) {
                window.history.replaceState({}, document.title, "WebChat.WebClient/Index.html#/room/" + data.attr('id'));
                _this.views.room(data);
            }, function (err) {
                console.log(err);
                console.log(err.responseText);
            })
    };

    RoomController.prototype.quitRoom = function () {
      //TODO: get roomid

        this.model.quitRoom(5)
            .then(function (data) {
                console.log(data);
            }, function (err) {
                console.log(err);
                console.log(err.responseText);
            })
    };

    return {
        load: function(model, views) {
            return new RoomController(model, views);
        }
    }
}());