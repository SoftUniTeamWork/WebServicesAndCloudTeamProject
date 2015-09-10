var app = app || {};

app.roomView = (function () {
    function RoomView(controller, selector) {
        this.controller = controller;
        this.selector = selector;

    }

    RoomView.prototype.joinRoom = function (data) {
        var _this = this;
        $.get('Templates/joinRoom.html', function (template) {
            var outputHtml = Mustache.render(template, data);
            $('#conmtainer').html(outputHtml);
        })
    };

    RoomView.prototype.getAllRooms = function ( data) {
        var _this = this;
        $.get('Templates/showAllRooms.html', function (template) {
            var outputHtml = Mustache.render(template, data);
            $('#container').html(outputHtml);
        }).then(function () {
            $("#container").on('click', '.room', function () {
                var room = $(this).parent().parent();

                console.log(room);
                $.sammy(function () {
                    this.trigger('joinRoom', room)
                });
                return false;
            });
        });
    };

    RoomView.prototype.room = function (data) {
        var _this = this;
        $.get("Templates/room.html", function (template) {
            var room = {
                Name: data.find($('h3')),
                Type: data.find($('p'))
            };
            console.log(room);
            var outputHtml = Mustache.render(template);
            $('#container').html(outputHtml);
        })
    }


    RoomView.prototype.createRoom = function () {
        var _this = this;
        $.get('Templates/createRoom.html', function (template) {
            var outputHtml = Mustache.render(template);
            $('#container').html(outputHtml);
        }).then(function () {
            $('#create-room').click(function () {
                var name = $('#roomName').val();
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
        })

    };

    return {
        load: function (controller, selector) {
            return new RoomView(controller, selector);
        }
    }
}());