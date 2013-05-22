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