angular.module("umbraco.directives")
    .directive('umbMediaPicker', function (dialogService, entityResource, mediaHelper) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: '/App_Plugins/Directives/umb-media-picker.html',
            require: "ngModel",
            link: function (scope, element, attr, ctrl) {

                ctrl.$render = function () {
                    var val = parseInt(ctrl.$viewValue);
                    if (!isNaN(val) && angular.isNumber(val) && val > 0) {
                        entityResource.getById(val, "Media").then(function (item) {

                            if (!item.thumbnail) {
                                
                                item.thumbnail = item.metaData.MediaPath;//mediaHelper.resolveFileFromEntity(item, true);
                                console.log(item);
                            }
                            scope.node = item;
                           
                        });
                    } else {
                        scope.node = "";
                    }
                };

                scope.openMediaPicker = function () {
                    dialogService.mediaPicker({
                        callback: populatePicture 
                    });
                }

                scope.removePicture = function () {
                    scope.node = undefined;
                    updateModel(0);
                }

                function populatePicture(item) {
                    scope.node = item;
                    updateModel(item.id);
                }

                function updateModel(id) {
                    ctrl.$setViewValue(id);

                }
            }
        };
    });