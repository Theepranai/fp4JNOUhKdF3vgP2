'use strict';

angular.module("umbraco").controller('cars.controller',
    function ($scope, $routeParams, $filter, $http, notificationsService, navigationService, $anchorScroll, $location) {

    	$scope.data = {};

    	if ($routeParams.id > 0) {
            $http.get("backoffice/cars/carsapi/get/" + $routeParams.id)
			  .then(function (response) {
			  	$scope.data = response.data;
			  });
        }

        $scope.save = function () {
            if ($routeParams.id > 0) {
                $http.post("backoffice/cars/carsapi/edit/", $scope.data)
                    .then(function successCallback(response) {
                        notificationsService.success("Success", "Save");
                        $location.path('/cars/cars');
                    }, function errorCallback(response) {
                        notificationsService.error("Error", "Save");
                    });
            } else {
                $http.put("backoffice/cars/carsapi/create/", $scope.data)
                    .then(function successCallback(response) {
                        notificationsService.success("Success", "Save");
                    }, function errorCallback(response) {
                        notificationsService.error("Error", "Save");
                    });
            }

        }

    });