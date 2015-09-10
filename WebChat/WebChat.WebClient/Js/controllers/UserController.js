var app = app || {};

app.userController = (function() {
    function UserController(model, views) {
        this.model = model;
        this.views = views;
    }

    UserController.prototype.register = function (data) {
        var _this = this;
        this.model.register(data)
            .then(function (data) {
                console.log("success");
                console.log(data);
                setUserToStorage(data);
                var uri = window.location.toString();
                if (uri.indexOf("?") > 0) {
                    var clean_uri = uri.substring(0, uri.indexOf("?"));
                    window.history.replaceState({}, document.title, clean_uri + "WebChat.WebClient/Index.html#/home");
                }
            }, function (error) {
                console.log("error");
                console.log(error.responseText)
            })
    };

    UserController.prototype.home = function (selector) {
        var data = {
          Username: sessionStorage["Username"]
        };
        this.views.loadHome(selector, data);
    };

    UserController.prototype.login = function(data) {
            this.model.login(data)
                .then(function (data) {
                    setUserToStorage(data);
                    window.history.replaceState({}, document.title, "WebChat.WebClient/Index.html#/home");
                }, function (err) {
                    console.log(err);
                    console.log(err.responseText);
                    console.log('error');

                })
    };

    UserController.prototype.attachEventHandlers = function () {
        var selector = '#wrapper';
        attachLoginHandler.call(this, selector);
        attachRegisterHandler.call(this, selector);
    }

    var attachLoginHandler = function (selector) {
        var _this = this;
        $(selector).on('click', '#sign-in', function () {
            var data = {
                Username: $('#login-username').val(),
                Password: $('#login-password').val(),
                'grant_type': "Password"
            };
            console.log(data);
            _this.login(data);
        })
    };

    var attachRegisterHandler = function (selector) {
        var _this = this;
        $(selector).on('click', '#register', function () {
            var data = {
                Username: $('#first_name').val(),
                Email: $('#email').val(),
                Password: $('#password').val(),
                ConfirmPassword: $('#password_confirmation').val()
            };
            console.log(data);
            _this.register(data);
        })
    }

    UserController.prototype.postMessage = function() {
        this.model.postMessage()
            .then(function (data) {
                console.log(data)
            }, function (error) {
                console.log(err.responseText);
            })
    };

    function setUserToStorage(data) {
        sessionStorage['Username'] = data.username;
        sessionStorage['authorize'] ='Bearer ' + data.access_token;
    }

    function clearUserFromStorage() {
        delete sessionStorage['Username'];
        delete sessionStorage['authorize'];
        delete sessionStorage['Username'];

    }
    return {
        load: function(model, views) {
            return new UserController(model, views);
        }
    }
}());