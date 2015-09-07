var app = app || {};

app.headers = (function() {
    function Headers() {
    }

    Headers.prototype.getHeaders = function (useSessionToken) {
        var headers = {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': 'http://localhost:22520/'
        };

        if (sessionStorage['sessionToken'] && useSessionToken) {
            headers['X-Parse-Session-Token'] = sessionStorage['sessionToken'];
        }

        return headers;
    };

    return {
        load : function () {
            return new Headers();
        }
    }
}());