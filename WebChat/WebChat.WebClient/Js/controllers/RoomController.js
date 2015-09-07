var app = app || {};

app.roomController = (function () {
    function RoomController(model){
        this.model = model;
    }

    RoomController.prototype.getAllRooms = function () {
      this.model.getAllRooms()
          .then(function (data) {
              console.log(data);
              console.log("Here are the rooms");
          }, function (error) {
              console.log(error);
              console.log(error.responseText);
          });
    };

    RoomController.prototype.createRoom = function () {
        // TODO: get values for the room!

        this.model.createRoom("123456", "Public", 22, "GabrielaObshtata")
            .then(function (data) {
                console.log(data);
            }, function (err) {
                console.log(err);
                console.log(err.responseText);
            })
    };

    RoomController.prototype.joinRoom = function () {
        //TODO: Get the room id

        this.model.joinRoom(5)
            .then(function (data) {
                console.log(data);
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
        load: function(model) {
            return new RoomController(model);
        }
    }
}());