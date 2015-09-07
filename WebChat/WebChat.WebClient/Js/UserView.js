var app = app || {}

app.userView = (function () {
   function UserView(controller, selector) {
       this.controller = controller;
       this.selector = selector;

   }

    UserView.prototype.loadLogin = function () {
        $(this.selector).load('Templates/Login.html');
    }

    return {
        load: function (controller, selector) {
            return new UserView(controller, selector);
        }
    }
}());