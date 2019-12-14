angular.module("umbraco.directives")
    .directive('umbDatetimePicker', function (dialogService, entityResource, mediaHelper) {
          return {
            restrict: 'E',
            replace: true,
              templateUrl: '/App_Plugins/Directives/umb-datetime-picker.html',
            require: "ngModel",
            link: function (scope, element, attr, ctrl) {           
                
                ctrl.$render = function () {
                    scope.node = {
                        view: 'datepicker',
                        value: ctrl.$viewValue
                    };
                };

                scope.$watch('node', function () {
                    if (scope.node != undefined) {
                        ctrl.$setViewValue(scope.node.value);
                    }
                }, true);        
            }
        };
    });

angular.module("umbraco").filter('dateFormat', function () {
    return function (input) {
        if (!input || !input.length) { return; }
        return input.substring(0, 16).replace('T', ' ');
    };
});