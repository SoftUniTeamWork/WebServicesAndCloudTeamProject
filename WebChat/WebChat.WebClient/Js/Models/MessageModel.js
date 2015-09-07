var app = app || {};

app.messageModel = (function () {
   function MessageModel(baseUrl, requester, headers) {
       this.baseUrl = baseUrl;
       this.requester = requester;
       this.headers = headers;
   }

    MessageModel.prototype.postMessage = function (text, roomId) {

        var serviceUrl = this.baseUrl + 'rooms/' + roomId + '/messages';
        var data = {Text: text};
        return this.requester.post(serviceUrl, this.headers.getHeaders(), data);
    };

    MessageModel.prototype.getAllMessages = function (roomId) {
        var serviceUrl = "message?roomId=" + roomId;

        return this.requester.get(serviceUrl, this.headers.getHeaders());
    }

    return {
        load: function (model) {
            return new MessageModel(model);
        }
    }
}());