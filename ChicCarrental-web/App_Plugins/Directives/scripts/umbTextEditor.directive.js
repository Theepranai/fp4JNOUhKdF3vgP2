
angular.module("umbraco.directives")
    .directive('umbTextEditor', function (dialogService, entityResource, mediaHelper) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: '/App_Plugins/Directives/umb-text-editor.html',
            require: "ngModel",
            link: function (scope, element, attr, ctrl) {           
                
                ctrl.$render = function () {
                    scope.node = {
                        view: 'rte',
                        config: {
                            //editor: {
                            //    //toolbar: ["code", "undo", "redo", "cut", "styleselect", "bold", "italic", "alignleft", "aligncenter", "alignright", "bullist", "numlist", "link", "umbmediapicker", "umbmacro", "table", "umbembeddialog"],
                            //    stylesheets: [],
                            //    dimensions: { height: 200}
                            //}
                        },
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