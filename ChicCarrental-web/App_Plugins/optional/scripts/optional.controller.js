'use strict';

angular.module("umbraco").controller('optional.controller',
    function ($scope, $routeParams, $filter, $http, notificationsService, navigationService, $anchorScroll, $location) {

    	$scope.data = {};

        if ($routeParams.id > 0) {
            $http.get("backoffice/optional/optionalApi/get/" + $routeParams.id)
			  .then(function (response) {
			  	$scope.data = response.data;
			  });
        }

        $scope.save = function () {
            if ($routeParams.id > 0) {
                $http.post("backoffice/optional/optionalApi/edit/", $scope.data)
                    .then(function successCallback(response) {
                        notificationsService.success("Success", "Save");
                        navigationService.syncTree({ tree: 'optional', path: [-1,-1], forceReload: true }); 
                    }, function errorCallback(response) {
                        notificationsService.error("Error", "Save");
                    });
            } else {
                $http.put("backoffice/optional/optionalApi/create/", $scope.data)
                    .then(function successCallback(response) {
                        notificationsService.success("Success", "Save");
                        navigationService.syncTree({ tree: 'optional', path: [-1,-1], forceReload: true });
                        location.hash = "#/optional/";
                    }, function errorCallback(response) {
                        notificationsService.error("Error", "Save");
                    });
            }

        }

    });