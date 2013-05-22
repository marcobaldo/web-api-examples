angular.module('spa', ['spa.Vote']).config(function ($routeProvider, $locationProvider) {
    $locationProvider.html5Mode(false);
    $routeProvider.when('/vote', {
        templateUrl: 'app/vote/index.tpl.html',
        controller: 'VoteCtrl'
    });
    $routeProvider.otherwise({
        redirectTo: '/vote'
    });
});

angular.module('spa.Auth', []).factory('User', function ($http) {
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