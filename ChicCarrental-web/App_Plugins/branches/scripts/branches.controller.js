'use strict';

angular.module("umbraco").controller('branches.controller',
    function ($scope, $routeParams, $filter, $http, notificationsService, navigationService, $anchorScroll, $location) {

    	$scope.data = {};

    	if ($routeParams.id > 0) {
    		$http.get("backoffice/branches/branchesapi/get/" + $routeParams.id)
			  .then(function (response) {
			  	$scope.data = response.data;
			  });
        }

        $scope.save = function () {
            if ($routeParams.id > 0) {
                $http.post("backoffice/branches/branchesapi/edit/", $scope.data)
                    .then(function successCallback(response) {
                        notificationsService.success("Success", "Save");
                    }, function errorCallback(response) {
                        notificationsService.error("Error", "Save");
                    });
            } else {
                $http.put("backoffice/branches/branchesapi/create/", $scope.data)
                    .then(function successCallback(response) {
                        notificationsService.success("Success", "Save");
                    }, function errorCallback(response) {
                        notificationsService.error("Error", "Save");
                    });
            }

        }

    });