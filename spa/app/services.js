angular.module('spa.Services', []);
angular.module('spa.Services').factory('Polls', function ($http) {
    return {
        get: function (callback) {
            $http.get('/api/polls').success(callback);
        }
    };
});

angular.module('spa.Services').factory('Choices', function ($http) {
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
            console.log(arguments);
            console.log(User);
            return;
            var castVote = {
                ChoiceId: choice.Id,
                UserId: User.Principal.$id
            };
            $http.post('/api/choices/vote', castVote).success(callback);
        }
    };
});