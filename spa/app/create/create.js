angular.module('spa.Create', ['spa.Create.Controllers', 'spa.Services', '$strap.directives']);
angular.module('spa.Create.Controllers', []).controller('CreateCtrl', function ($scope, $location, Polls, Choices, User) {
    $scope.addChoice = function (poll) {
        poll.Choices.push({
            Label: $scope.newChoice.Label
        });
        $scope.resetChoice();
    };
    
    $scope.resetChoice = function () {
        $scope.newChoice = {
            Label: ''
        };
    }

    $scope.resetPoll = function () {
        $scope.newPoll = {
            Title: '',
            EndTime: {
                Date: '',
                Time: ''
            },
            Choices: [],
            AllowEdit: false,
            MaxVotes: 0
        };
    };
    $scope.resetPoll();

    $scope.createPoll = function () {
        var et = $scope.newPoll.EndTime;
        if (et != null && et.Date != null && et.Time != null && et.Date != '' && et.Time != '') {
            var closing = new Date($scope.newPoll.EndTime.Date);
            var time = timeParser($scope.newPoll.EndTime.Time);
            closing.setHours(time.getHours());
            closing.setMinutes(time.getMinutes());
            
            $scope.newPoll.Closing = closing;
        }

        var poll = $scope.newPoll;
        if (poll.Title == '') {
            alert('Please set a title');
            return;
        }

        if (poll.Closing == null) {
            alert('Please set poll closing time');
            return;
        }
        
        if (poll.Choices.length < 2) {
            alert('Please add at least two options');
            return;
        }

        poll.MaxVotes = parseInt(poll.MaxVotes, 10);
        if (!isInt(poll.MaxVotes)) {
            alert('Please provide a proper number for maximum votes');
            return;
        }

        var pollToCreate = {
            Title: poll.Title,
            Closing: poll.Closing,
            Choices: poll.Choices,
            AllowEdit: poll.AllowEdit,
            MaxVotes: poll.MaxVotes,
            CreatedBy: {
                Id: User.Principal.Id
            },
            Status: 0
        };

        Polls.add(pollToCreate, function(response) {
            if (response.Success) {
                $location.path('/vote');
            }
        })
    };

    var isInt = function(n) {
        return typeof n === 'number' && n % 1 == 0;
    };
    
    var timeParser = function(stringTime) {
        var d = new Date();
        var time = stringTime.match(/(\d+)(?::(\d\d))?\s*(P?)/);
        d.setHours(parseInt(time[1]) + (time[3] ? 12 : 0));
        d.setMinutes(parseInt(time[2]) || 0); //console.log( d );
        return d;
    }
});