var app = app || {};

app.tagModel = (function () {
    function TagModel(baseUrl, requester, headers){
        this.baseUrl = baseUrl + 'tags';
        this.headers = headers;
        this.requester = requester;
    }

    TagModel.prototype.getAlltags = function () {
        return this.requester.get(this.baseUrl,this.headers.getHeaders());
    };

    TagModel.prototype.getTagById = function (id) {
        var serviceUrl = this.baseUrl + '/' + id;

        return this.requester.get(serviceUrl,this.headers.getHeaders());
    };

    TagModel.prototype.deleteTagById = function (id) {
        var serviceUrl = this.baseUrl + '?tagId='+ id;
        return this.requester.remove(serviceUrl, this.headers.getHeaders());
    };

    TagModel.prototype.createTag= function (data) {
        var serviceUrl = this.baseUrl;
        return this.requester.post(serviceUrl, this.headers.getHeaders(), data);
    };

    return {
        load: function (baseUrl, requester, headers) {
            return new TagModel(baseUrl, requester, headers);
        }
    }
}());