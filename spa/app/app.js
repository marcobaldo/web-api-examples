angular.module('spa', ['spa.Vote', 'spa.Auth']).config(function ($routeProvider, $locationProvider) {
    $locationProvider.html5Mode(false);
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
        redirectTo: '/vote'
    });
}).run(function ($rootScope, $location, User) {
    $rootScope.$on('$routeChangeStart', function (event, next, current) {
        if (!User.IsAuthenticated && next.templateUrl != 'app/login.tpl.html') {
            $location.path('/login');
        }
    });
});

angular.module('spa.Auth', ['spa.Auth.Controllers', 'spa.Auth.Services']);
angular.module('spa.Auth.Controllers', []).controller('AuthCtrl', function ($scope, $location, User) {
    User.checkStatus(function (response) {
        User.IsAuthenticated = response.IsAuthenticated;
        User.Principal = response.Principal;
        if (response.IsAuthenticated) {
            $location.path('/votes');
        }
    });

    // Sign in
    var credentials = $scope.credentials = {};
    $scope.signin = function () {
        User.login(credentials.Username, credentials.Password, function (response) {
            User.IsAuthenticated = response.IsAuthenticated;
            User.Principal = response.Principal;
            if (response.IsAuthenticated) {
                $location.path('/votes');
            }
        });
    };
});

angular.module('spa.Auth.Services', []).factory('User', function ($http) {
    return {
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