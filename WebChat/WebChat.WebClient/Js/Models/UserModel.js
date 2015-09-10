var app = app || {};

app.userModel = (function () {
    function UserModel(baseUrl, requester, headers) {
        this.baseUrl= baseUrl + "user/";
        this.requester=requester;
        this.headers = headers;
    }

    UserModel.prototype.login = function (data) {
        var serviceUrl = this.baseUrl + "login";
        return this.requester.post(serviceUrl,
            this.headers.getHeaders(), data);
    };
    
    UserModel.prototype.register= function (data) {
        var serviceUrl = this.baseUrl + "Register";
                var headers = this.headers.getHeaders();

        return this.requester.post(serviceUrl, headers, data);
    };

    UserModel.prototype.postMessage = function (text) {
        var serviceUrl = this.baseUrl + 'Message';
        var data = {Text:text};

        return this.requester.post(serviceUrl, this.headers.getHeaders(), data);
    }

    return{
        load: function (baseUrl, requester, headers) {
            return new UserModel(baseUrl,requester, headers);
        }
    }
}());