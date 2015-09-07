var app = app || {};

(function () {
    var baseUrl = "http://localhost:22520/api/";

    var headers = app.headers.load();
    var requester = app.requester.load();

    var userModel = app.userModel.load(baseUrl, requester, headers);
    var tagModel = app.tagModel.load(baseUrl, requester, headers);
    var roomModel = app.roomModel.load(baseUrl, requester, headers);
    var messageModel = app.messageModel.load(baseUrl, requester, headers);

    var userController = app.userController.load(userModel);
    var TagController = app.tagController.load(tagModel);
    var roomController = app.roomController.load(roomModel);
    var messageController = app.messageController.load(messageModel);

    var userView = app.userView.load(userController, '#wrapper');
    app.router = Sammy(function () {
        var selector = '#wrapper';

        this.get('#/', function () {
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
        })
    });

    app.router.run('#/')
}());