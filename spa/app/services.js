angular.module('spa.Services', []);
angular.module('spa.Services').factory('Polls', function ($http) {
    return {
        add: function(poll, callback) {
            $http.post('/api/polls/add', poll).success(callback);
        },
        get: function (callback) {
            $http.get('/api/polls/all').success(callback);
        }
    };
});

angular.module('spa.Services').factory('Choices', function ($http, User) {
    return {
        add: function (poll, label, callback) {
            var choice = {
                Label: label,
                AddedBy: {
                    Id: 1
                },
                PollId: poll.Id
            };

            $http.post('/api/choices/post', choice).success(callback);
        },
        vote: function (choice, callback) {
            var castVote = {
                ChoiceId: choice.Id,
                UserId: User.Principal.Id
            };
            $http.post('/api/choices/vote', castVote).success(callback);
            return;
        }
    };
});

angular.module('spa.Services').factory('BusyService', function() {
    return {
        RequestCount: 0,
        IsBusy: function() {
            return this.RequestCount > 0;
        }
    };
});