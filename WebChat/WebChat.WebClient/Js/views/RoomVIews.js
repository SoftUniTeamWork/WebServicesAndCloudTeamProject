var app = app || {};

app.roomView = (function () {
    function RoomView(controller, selector) {
        this.controller = controller;
        this.selector = selector;

    }

    RoomView.prototype.getAllRooms = function ( data) {
        var _this = this;
        $.get('Templates/showAllRooms.html', function (template) {
            var outputHtml = Mustache.render(template, data);
            $('#container').html(outputHtml);
        })
    };


    RoomView.prototype.createRoom = function () {
        var _this = this;
        $.get('Templates/createRoom.html', function (template) {
            var outputHtml = Mustache.render(template);
            $('#container').html(outputHtml);
        }).then(function () {
            $('#create-room').click(function () {
                var name = $('#login-username').val();
                var password = $('#roomPassword').val();
                var type = $('#roomType').val();
                var size = $('#roomSize').val();

                $.sammy(function () {
                    this.trigger('createRoom', {
                        Name : name,
                        Password: password,
                        Type: type,
                        Size: size})
                });
                return false;
            });
        }).done();

    };

    return {
        load: function (controller, selector) {
            return new RoomView(controller, selector);
        }
    }
}());