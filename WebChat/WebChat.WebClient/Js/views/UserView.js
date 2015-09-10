var app = app || {};

app.userView = (function () {
   function UserView(controller, selector) {
       this.controller = controller;
       this.selector = selector;

   }

    UserView.prototype.loadLogin = function () {
        $(this.selector).load('Templates/Login.html');
    };

    UserView.prototype.loadHome = function (selector, data) {
        $.get('Templates/home.html', function (template) {
            var outputHtml = Mustache.render(template, data);
            $(selector).html(outputHtml);
        })
    }

    return {
        load: function (controller, selector) {
            return new UserView(controller, selector);
        }
    }
}());