'use strict';

angular.module("umbraco").controller('branches.car-price.controller',
    function ($scope, $routeParams, $filter, $http, notificationsService, navigationService, $anchorScroll, $location) {

        $scope.data = {};
        $scope.name = {};
        $scope.items = {};
        $scope.editform = false;

        $scope.fristload = function () {
            $http.get("backoffice/branches/branchesapi/price/" + $routeParams.id)
                .then(function (response) {
                    $scope.items = response.data;
                    $scope.name = response.data[0].Branch_Name + ' - ' + response.data[0].name;
                });
        }
        if ($routeParams.id) {
            $scope.fristload();
        }

        $scope.edit = function (x) {
            $scope.model = x;
            $scope.editform = true;
        }

        $scope.addnew = function () {
            $scope.model = {
                car_branch_id: $routeParams.id
            };
            $scope.editform = true;
        }

        $scope.cancel = function () {
            if ($scope.editform) {
                $scope.editform = false;
            } else {
                location.hash = '#/branches/branches/car/' + $scope.items[0].Branch_ID;
            }
           
        }

        $scope.save = function () {

            if ($scope.model.price_id) {
                $http.post("backoffice/branches/branchesapi/pricedit/", $scope.model)
                    .then(function successCallback(response) {
                        notificationsService.success("Update Success", "Save");
                    }, function errorCallback(response) {
                        notificationsService.error("Error", "Save");
                    });
            } else {
                $http.put("backoffice/branches/branchesapi/pricecreate/", $scope.model)
                    .then(function successCallback(response) {
                        notificationsService.success("Create Success", "Save");
                    }, function errorCallback(response) {
                        notificationsService.error("Create Error", "Save");
                    });
            }
        }

    });
