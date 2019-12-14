angular.module("umbraco.directives")
    .directive('umbMutiMediaPicker', function (dialogService, entityResource, mediaHelper) {
          return {
            restrict: 'E',
            replace: true,
              templateUrl: '/App_Plugins/Directives/umb-muti-media-picker.html',
            require: "ngModel",
            link: function (scope, element, attr, ctrl) {

                scope.array = null;

                ctrl.$render = function () {
                    if(ctrl.$viewValue != null && ctrl.$viewValue != undefined){
                    var valx = ctrl.$viewValue;
                    scope.array= JSON.parse("[" + valx + "]");
                        entityResource.getByIds(scope.array, "Media").then(function (item) {
                            for (var key in item) {
                                        if (!item[key].thumbnail) {
                                            item[key].thumbnail = mediaHelper.resolveFileFromEntity(item[key],false);
                                        }
                                }
                             scope.node = item;
                        });
                    }
                };

                scope.openMediaPicker = function () {
                    dialogService.mediaPicker({
                        multiPicker: true,
                        callback: populatePicture 
                    });
                }

                scope.removePicture = function (id) {
                     var myEl = angular.element(document.querySelector('#div'+id ));
                        myEl.remove();
                    scope.array.splice(scope.array.indexOf(id),1);
                    updateModel(scope.array.toString());
                }

                function populatePicture(item) {
                    scope.node = item;
                    console.log(item);
                    var obj = [];
                    for (var k in item) {
                        obj.push(item[k].id);
                    }
                    updateModel(obj.toString());
                }

                function updateModel(id) {
                    ctrl.$setViewValue(id);
                }
            }
        };
});