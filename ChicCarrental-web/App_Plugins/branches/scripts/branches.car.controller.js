'use strict';

angular.module("umbraco").controller('branches.car.controller',
    function ($scope, $routeParams, $filter, $http, dialogService, notificationsService, navigationService, $anchorScroll, $location) {

        $scope.data = {};
        $scope.items = {};
        $scope.loading = true;
       
        $scope.fristload = function () {
            $scope.loading = true;
            $http.get("backoffice/branches/branchesapi/car/" + $routeParams.id)
                .then(function (response) {
                    $scope.items = response.data;
                    $scope.loading = false;
                });
        };

        if ($routeParams.id > 0) {
            $scope.fristload();
            $scope.addnew = {
                Branch_ID: $routeParams.id
            }
        }

        $scope.openEditDialog = function (x) {
            // open a custom dialog
            dialogService.open({
                // set the location of the view
                template: "/App_Plugins/branches/dialog/add-car.html",
                // pass in data used in dialog
                dialogData: x,
                // function called when dialog is closed
                callback: $scope.fristload()
            });
        };

    });
