'use strict';

angular.module("umbraco").controller('branches.car-edit.controller',
    function ($scope, $http, notificationsService) {

        $scope.items = {};
        $scope.car = {};
        $scope.selectnew = true;
        $scope.model = $scope.dialogData;

        if ($scope.model.Car_ID) {
            $scope.selectnew = false;
        } else {
            $http.get("backoffice/cars/carsapi/getlist/-1")
                .then(function (response) {
                    $scope.car = response.data;
                });
        }

        $scope.items = {};
        $scope.car = {};

        $scope.save = function () {
            if ($scope.model.name) {
                $http.post("backoffice/branches/branchesapi/caredit/", $scope.model)
                    .then(function successCallback(response) {
                        notificationsService.success("Update Success", "Save");
                    }, function errorCallback(response) {
                        notificationsService.error("Error", "Save");
                    });
            } else {
                $http.put("backoffice/branches/branchesapi/carcreate/", $scope.model)
                    .then(function successCallback(response) {
                        notificationsService.success("Create Success", "Save");
                    }, function errorCallback(response) {
                        notificationsService.error("Create Error", "Save");
                    });
            }
        }

    });
