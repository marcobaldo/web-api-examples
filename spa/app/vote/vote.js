angular.module('spa.Vote', ['spa.Vote.Controllers', 'spa.Vote.Services']);
angular.module('spa.Vote.Controllers', []).controller('VoteCtrl', function ($scope, Polls, Choices) {

    var polls = $scope.polls = [];
    var refs = $scope.refs = {};

    // Init
    Polls.get(function (data) {
        _.each(data, function (el) {
            polls.push(el);

            _.each(el.Choices, function (choice) {
                choice.ShowMoreInfo = false;

                var addedBy = choice.AddedBy;
                if (!_.has(addedBy, '$ref')) {
                    refs['$u' + addedBy.$id] = addedBy;
                } else {
                    choice.AddedBy = refs['$u' + addedBy.$ref];
                }
            });
        });
    });

    // Voting
    $scope.vote = function (poll, choice) {
        var toggle = !choice.Selected;
        if (toggle && ($scope.getVotes(poll) >= poll.MaxVotes)) {
            alert('Cannot vote more than ' + poll.MaxVotes);
            return;
        }

        choice.Selected = toggle;
    };
    $scope.showMoreInfo = function (choice) {
        choice.ShowMoreInfo = !choice.ShowMoreInfo;
    };

    // Add choice
    $scope.newChoice = {
        Label: '',
        Selected: false
    };
    $scope.addChoice = function (poll) {
        if (!poll.AllowEdit) {
            alert('You are not allowed to add choices to this poll');
            return;
        }

        if ($scope.newChoice.Label == '') {
            return;
        }

        var choice = angular.copy($scope.newChoice);
        Choices.add(poll, choice.Label, function (data) {
            if (data.Success) {
                data.Choice.ShowMoreInfo = false;

                var addedBy = data.Choice.AddedBy;
                if (!_.has(addedBy, '$ref')) {
                    refs['$u' + addedBy.$id] = addedBy;
                } else {
                    data.Choice.AddedBy = refs['$u' + addedBy.$ref];
                }
                poll.Choices.push(data.Choice);
            }
        });
        $scope.newChoice.Label = '';
    };

    //  Helpers
    $scope.getVotes = function (poll) {
        var voted = 0;
        _.each(poll.Choices, function (el, list) {
            if (el.Selected) {
                voted++;
            }
        });

        return voted;
    };
});

angular.module('spa.Vote.Services', []);
angular.module('spa.Vote.Services').factory('Polls', function ($http) {
    return {
        get: function (callback) {
            $http.get('/api/polls').success(callback);
        }
    };
});

angular.module('spa.Vote.Services').factory('Choices', function ($http) {
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
        }
    };
});