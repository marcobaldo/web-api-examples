angular.module('spa', ['spa.Vote', 'spa.Create', 'spa.Auth', 'spa.Directives', 'spa.Services']).config(function ($routeProvider, $locationProvider, $httpProvider) {
    $routeProvider.when('/login', {
        templateUrl: 'app/login.tpl.html',
        controller: 'AuthCtrl'
    });
    $routeProvider.when('/vote', {
        templateUrl: 'app/vote/vote.tpl.html',
        controller: 'VoteCtrl'
    });
    $routeProvider.when('/create', {
        templateUrl: 'app/create/create.tpl.html',
        controller: 'CreateCtrl'
    });
    $routeProvider.otherwise({
        redirectTo: '/login'
    });

    $locationProvider.html5Mode(false);
    $httpProvider.responseInterceptors.push('onCompleteInterceptor');
}).run(function ($rootScope, $location, User, BusyService, $http, onStartInterceptor) {
    $http.defaults.transformRequest.push(onStartInterceptor);
    $rootScope.$on('$routeChangeStart', function (event, next, current) {
        if (!User.IsAuthenticated && next.templateUrl != 'app/login.tpl.html') {
            $location.path('/login');
            return;
        }
    });

    $rootScope.$watch(function() {
        return BusyService.IsBusy();
    }, function(value) {
        $rootScope.isBusy = value;
    });
}).factory('onStartInterceptor', function (BusyService) {
    return function (data, headersGetter) {
        BusyService.RequestCount++;
        return data;
    };
}).factory('onCompleteInterceptor', function (BusyService, delayedPromise) {
    return function (promise) {
        var decrementRequestCount = function (response) {
            BusyService.RequestCount--;
            return response;
        };

        // Normally we would just chain on to the promise but ...
        //return promise.then(decrementRequestCount, decrementRequestCount);
        return delayedPromise(promise, 500).then(decrementRequestCount, decrementRequestCount);
    };
}).factory('delayedPromise', function ($q, $timeout) {
    return function (promise, delay) {
        var deferred = $q.defer();
        var delayedHandler = function () {
            $timeout(function () { deferred.resolve(promise); }, delay);
        };
        promise.then(delayedHandler, delayedHandler);
        return deferred.promise;
    };
});;

angular.module('spa.Auth', ['spa.Auth.Controllers', 'spa.Auth.Services']);
angular.module('spa.Auth.Controllers', []).controller('AuthCtrl', function ($scope, $location, User) {
    $scope.isCheckingAuthenticatinStatus = true;
    $scope.isSigningIn = false;

    User.checkStatus(function (response) {
        User.HasAttemptedLogin = true;
        User.IsAuthenticated = response.IsAuthenticated;
        User.Principal = response.Principal;

        if (response.IsAuthenticated) {
            $location.path('/vote');
            return;
        }

        $scope.isCheckingAuthenticatinStatus = false;
    });

    // Sign in
    var credentials = $scope.credentials = {};
    $scope.signin = function () {
        $scope.isSigningIn = true;
        User.login(credentials.Username, credentials.Password, function (response) {
            $scope.isSigningIn = false;

            User.IsAuthenticated = response.IsAuthenticated;
            User.Principal = response.Principal;
            if (response.IsAuthenticated) {
                $location.path('/vote');
            }
        });
    };
});

angular.module('spa.Auth.Services', []).factory('User', function ($http) {
    return {
        HasAttemptedLogin: false,
        IsAuthenticated: false,
        Principal: null,
        login: function (username, password, callback) {
            $http.post('/api/sessions/login', $.param({ Username: username, Password: password }), {
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).success(callback);
        },
        logout: function (callback) {
            $http.post('/api/sessions/logout').success(callback);
        },
        checkStatus: function (callback) {
            $http.get('/api/sessions/status').success(callback);
        }
    };
});