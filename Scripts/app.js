var EmployeeApp = angular.module('EmployeeApp', ['ngResource', 'ngRoute']).
    config(function ($routeProvider) {
        $routeProvider.
            when('/', { controller: ListCtrl, templateUrl: 'list.html' }).
            when('/new', { controller: CreateCtrl, templateUrl: 'details.html' }).
            when('/edit/:editId', { controller: EditCtrl, templateUrl: 'details.html' }).
            otherwise({ redirectTo: '/' });
    });

EmployeeApp.factory('EmployeeResource', function ($resource) {
    return $resource('/api/Employee/:id', { id: '@id' }, { 'update': { method: 'PUT' } });
});


var EditCtrl = function ($scope, $location, $routeParams, EmployeeResource) {
    $scope.action = "Update";
    var id = $routeParams.editId;
    $scope.newEmployee = EmployeeResource.get({ id: id });

    $scope.save = function () {
        EmployeeResource.update({ id: id }, $scope.newEmployee, function () {
            $location.path('/');
        });
    };

};


var CreateCtrl = function ($scope, $location, EmployeeResource) {
    $scope.action = "Add";
    $scope.save = function () {
        EmployeeResource.save($scope.newEmployee, function () {
            $location.path('/');
        });
    };
};


var ListCtrl = function ($scope, $location, EmployeeResource) {
    $scope.search = function () {
        EmployeeResource.query({ q: $scope.query, sort: $scope.sort_order, desc: $scope.sort_desc,  limit: $scope.limit, offset: $scope.offset },
            function (items) {
                var cnt = items.length;
                $scope.no_more = cnt < 5;
                $scope.employees = $scope.items.concat(items);
            }
        );
    };

    $scope.sort_by = function (ord) {
        if ($scope.sort_order == ord) { $scope.sort_desc = !$scope.sort_desc; }
        else { $scope.sort_desc = false; }
        $scope.sort_order = ord;
        $scope.reset();
    };

    $scope.do_show = function (asc, col) {
        return (asc != $scope.sort_desc) && ($scope.sort_order == col);
    };


    $scope.delete = function () {
        var id = this.employee.Id;
        EmployeeResource.delete({ id: id }, function () {
            $('#emp_'+ id).fadeOut();
        });
    };

    $scope.reset = function () {
        $scope.offset = 0;
        $scope.items = [];
        $scope.search();
    };

    $scope.show_more = function () { return !$scope.no_more; };

    $scope.limit = 5;
    $scope.sort_order = 'FName';
    $scope.sort_desc = false;
    $scope.reset();
};