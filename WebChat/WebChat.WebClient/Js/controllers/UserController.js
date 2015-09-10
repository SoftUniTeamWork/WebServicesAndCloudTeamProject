var app = app || {};

app.userController = (function() {
    function UserController(model) {
        this.model = model;
    }

    UserController.prototype.register = function () {
        this.model.register("NagabrielaPi4kata", "123456", "123456", "asd@asd.bg")
            .then(function (data) {
                console.log("success");
                console.log(data);
            }, function (error) {
                console.log("error");
                console.log(error.responseText)
            })


    };

    UserController.prototype.login = function() {
        $('#sign-in').on('click', function () {
            var data = {
                Username: $('#login-username').val(),
                Password: $('#login-password').val(),
                'grant_type': "Password"
            };
            console.log(data);
            this.model.login(data)
                .then(function (data) {
                    setUserToStorage(data);
                }, function (err) {
                    console.log(err);
                    console.log(err.responseText);
                    console.log('error');

                })
        })
    };

    UserController.prototype.attachEventHandlers = function () {
        var selector = '#wrapper';
        attachLoginHandler.call(this, selector);
    }

    var attachLoginHandler = function (selector) {
        var _this = this;
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
        sessionStorage['Username'] = data.username;
    }

    function clearUserFromStorage() {
        delete sessionStorage['Username'];
        delete sessionStorage['authorize'];
        delete sessionStorage['Username'];

    }
    return {
        load: function(model) {
            return new UserController(model);
        }
    }
}());