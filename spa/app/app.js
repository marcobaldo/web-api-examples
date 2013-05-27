angular.module('spa', ['spa.Vote', 'spa.Auth']).config(function ($routeProvider, $locationProvider) {
    $locationProvider.html5Mode(false);
    $routeProvider.when('/login', {
        templateUrl: 'app/login.tpl.html',
        controller: 'AuthCtrl'
    });
    $routeProvider.when('/vote', {
        templateUrl: 'app/vote/index.tpl.html',
        controller: 'VoteCtrl'
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
angular.module('spa.Auth.Controllers', []).controller('AuthCtrl', function ($scope, User) {
    // Sign in
    var credentials = $scope.credentials = {};
    $scope.signin = function () {
        User.login(credentials.username, credentials.password, function(response) {
            console.log(response);
        });
    };
});

angular.module('spa.Auth.Services', []).factory('User', function ($http) {
    return {
        IsAuthenticated: false,
        Principal: null,
        login: function (username, password, callback) {
            $http.post('/api/sessions/login', {
                username: username,
                password: password
            }).success(callback);
        },
        logout: function (callback) {
            $http.post('/api/sessions/logout').success(callback);
        }
    };
});