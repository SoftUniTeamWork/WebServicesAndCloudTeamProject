var app = app || {};

(function () {
    var baseUrl = "http://localhost:22520/api/";

    var headers = app.headers.load();
    var requester = app.requester.load();

    var userModel = app.userModel.load(baseUrl, requester, headers);
    var tagModel = app.tagModel.load(baseUrl, requester, headers);
    var roomModel = app.roomModel.load(baseUrl, requester, headers);
    var messageModel = app.messageModel.load(baseUrl, requester, headers);


    var userView = app.userView.load(userController, '#wrapper');
    var roomView = app.roomView.load(roomController, "#bs-example-navbar-collapse-1");

    var userController = app.userController.load(userModel, userView);
    var TagController = app.tagController.load(tagModel);
    var roomController = app.roomController.load(roomModel, roomView);
    var messageController = app.messageController.load(messageModel);

    userController.attachEventHandlers();

    app.router = Sammy(function () {
        var selector = '#wrapper';

        this.get('#/login', function () {
           // $.get('Templates/Login.html', function (template) {
             //   var outerHtml = Mustache.render(template);
               // $(selector).html(outerHtml);
           // })
            userView.loadLogin();

        });

        this.get('#/register', function () {
            $.get('Templates/register.html', function (template) {
                var outputHtml = Mustache.render(template);
                $(selector).html(outputHtml);
            })
        });

        this.get('#/home', function () {
           userController.home(selector);
        });

        this.get('#/showallrooms', function () {
            roomController.getAllRooms();
        });


        this.get('#/create', function () {
            roomView.createRoom(selector);
        });

        this.get('#/room/:id', function () {
            alert(this.params['id']);
            roomView.room(selector);
        });
        this.bind('createRoom', function (e, data) {
            roomController.createRoom(data);
        });

        this.bind('joinRoom', function (e, data) {
            roomController.joinRoom(data);
        });
    });

    app.router.run('#/login')
}());