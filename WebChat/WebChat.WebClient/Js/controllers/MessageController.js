var app = app || {};

app.messageController = (function () {
   function MessageController(model) {
        this.model = model;
    }

    MessageController.prototype.postMessage = function () {
        //TODO: get text and roomId.
        this.model.postMessage()
            .then(function (data) {
                console.log(data);
            }, function (err) {
                console.log(err);
            })
    };


    MessageController.prototype.getAllMessages= function () {
        //TODO: get text and roomId.
        this.model.getAllMessages()
            .then(function (data) {
                console.log(data);
            }, function (err) {
                console.log(err);
            })
    };

    return {
        load: function (model) {
        return new MessageController(model);
        }
    }
}());