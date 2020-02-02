'use strict';

angular.module("umbraco").controller('transection.controller',
    function ($scope, $routeParams, $filter, $http, dialogService, notificationsService, navigationService, $anchorScroll, $location) {

        $scope.data = {};
        $scope.items = {};
        $scope.loading = true;
        $scope.detail = false;
        $scope.name = "Transection " + $routeParams.id;
        var id = $routeParams.id.split('-');
        $scope.fristload = function () {
            $scope.loading = true;
            $http.get("backoffice/Transections/TransectionsApi/get?year=" + id[0] + "&month="+id[1])
                .then(function (response) {
                    $scope.items = response.data;
                    $scope.loading = false;
                    $scope.detail = false;
                });
        };
        $scope.fristload();
        $scope.showdetail = function (x) {
            $scope.detail = true;
            $scope.loading = true;
            $http.get("backoffice/Transections/TransectionsApi/getdetail/"+x)
                .then(function (response) {
                    $scope.transection = response.data;
                    $scope.loading = false;
                    $scope.detail = true;
                });
        }

        $scope.testx = function (str) {
            var x = str.toFixed(3) + 'x';
            return x.slice(0, -2);
        }

        //if ($routeParams.id > 0) {
        //    $scope.fristload();
        //    $scope.addnew = {
        //        Branch_ID: $routeParams.id
        //    }
        //}

        //$scope.openEditDialog = function (x) {
        //    // open a custom dialog
        //    dialogService.open({
        //        // set the location of the view
        //        template: "/App_Plugins/branches/dialog/add-car.html",
        //        // pass in data used in dialog
        //        dialogData: x,
        //        // function called when dialog is closed
        //        callback: $scope.fristload()
        //    });
        //};

    });
