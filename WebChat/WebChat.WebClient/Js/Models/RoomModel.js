var app = app || {};

app.roomModel = (function () {
    function RoomModel(baseUrl, requester, headers) {
            this.baseUrl = baseUrl + 'rooms/';
            this.requester = requester;
            this.headers = headers;
    }

    RoomModel.prototype.getAllRooms = function () {

        return this.requester
            .get(this.baseUrl, this.headers.getHeaders());
    };

    RoomModel.prototype.createRoom = function (password, type, size, name) {
        var data = {
            Password: password,
            Type: type,
            Size: size,
            Name: name
        };
        return this.requester.post(this.baseUrl, this.headers.getHeaders(), data);
    };


    RoomModel.prototype.joinRoom = function (roomId) {
        var serviceUrl = this.baseUrl + roomId + '/join';
        return this.requester.post(serviceUrl, this.headers.getHeaders());
    };


    RoomModel.prototype.quitRoom = function (roomId) {
        var serviceUrl = this.baseUrl + roomId + '/quit';
        return this.requester.post(this.baseUrl, this.headers.getHeaders());
    }

    return {
        load : function (baseUrl, requester, headers) {
            return new RoomModel(baseUrl, requester, headers);
        }
    }
}());