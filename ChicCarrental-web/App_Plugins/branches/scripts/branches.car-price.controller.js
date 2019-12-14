'use strict';

angular.module("umbraco").controller('branches.car-price.controller',
    function ($scope, $routeParams, $filter, $http, notificationsService, navigationService, $anchorScroll, $location) {

        $scope.data = {};
        $scope.name = {};
        $scope.items = {};
        

        $scope.id = $routeParams.id;

        $scope.addnew = {
            car_branch_id: $scope.id
        }

        if ($routeParams.id) {
            $http.get("backoffice/branches/branchesapi/price/" + $routeParams.id)
                .then(function (response) {
                    $scope.items = response.data;
                    $scope.name = response.data[0].Branch_Name + ' - ' + response.data[0].name;
                });
        }

        $scope.edit = function (x) {
            $scope.model = x;
            console.log(x);
        }

        $scope.cancel = function () {
            $scope.model = false;
        }

        $scope.save = function () {
            if ($scope.model.price_id > 0) {
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
