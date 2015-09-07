var app = app || {};

app.tagController = (function () {
    function TagController(model) {
        this.model = model;
    }

    TagController.prototype.GetAllTags = function () {
        this.model.getAlltags()
            .then(function (data) {
                console.log(data);
            }, function (err) {
                console.log(err);
            })
    };


    TagController.prototype.GetById = function () {
        //TODO: get id
        this.model.getTagById(5)
            .then(function (data) {
                console.log(data);
            }, function (err) {
                console.log(err);
            })
    };


    TagController.prototype.deleteById = function () {
        //TODO: get id
        this.model.deleteById(5)
            .then(function (data) {
                console.log(data);
            }, function (err) {
                console.log(err);
            })
    };

    TagController.prototype.createTag = function () {
        //TODO: get the data
        var data = {};
        this.model.createTag(data)
            .then(function (data) {
                console.log(data);
            }, function (err) {
                console.log(err);
            })
    };

    return {
        load: function (model) {
            return new TagController(model);
        }
    }
}());