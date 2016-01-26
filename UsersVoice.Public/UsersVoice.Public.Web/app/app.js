angular.module('userVoice', ["ngRoute", "kendo.directives"])
	.config(['$routeProvider',
	  function($routeProvider) {
		  $routeProvider.
			  when('/', {
				  templateUrl: '/app/views/Main.html'
			  }).
			  when('/areas/:areaId/create', {
					templateUrl: '/app/views/SubmitIdea.html',
					controller: 'SubmitIdeaDetailController'
			  }).
			  when('/ideas/:ideaId', {
				  templateUrl: '/app/views/IdeaDetail.html',
				  controller: 'IdeaDetailController'
			  })
			  .when("/login", {
			  	templateUrl: "/app/views/Login.html",
			  	controller:"LoginController"
			  });

	  }])
	.controller('AreasController', function ($scope, $rootScope, $http, LoginService) {
	    var instance = this;

	    this.selectedArea = null;

	    this.initialize = function () {
	        $http({
	            method: 'GET',
	            url: "/api/areas/",
	            dataType: "json"
	        }).then(function successCallback(response) {
	            $scope.areas = response.data;
	        }, function errorCallback(response) {
	            debugger;
	        });
	    };
        
	    $scope.isSelected = function (area) {
	        return instance.selectedArea === area;
	    };

	    $scope.onClickOnArea = function (area, $event) {
	        instance.selectedArea = area;
	        $rootScope.$broadcast('areaClicked', area);
	        $event.preventDefault();
	    };

	    $scope.isLoading = function () {
	        return null === $scope.areas || undefined === $scope.areas;
	    };

		this.initialize();
	})
	.controller('IdeasController', function ($scope, $http, $location, LoginService) {
        var instance = this;

        instance.hasLoadedIdeas = false;

        $scope.source = null;

        this.read = function (url, pageSize) {
            instance.hasLoadedIdeas = false;
            $scope.source = null;

            $http({
                method: 'GET',
                url: url,
                data: { pageSize: pageSize||20 },
                dataType: "json"
            }).then(function successCallback(response) {
                $scope.source = response.data;
                instance.hasLoadedIdeas = ($scope.source !== null);
            }, function errorCallback(response) {
                debugger;
            });
        };

		this.initialize = function () {
		    $scope.$on('areaClicked', function (event, args) {
		        var url = "/api/areas/" + args.Id + "/ideas?sortBy=CreationDate&sortDirection=DESC";
		        $scope.selectedArea = args;
		        instance.read(url, 20);
		    });

		    instance.read('/api/ideas?sortBy=CreationDate&sortDirection=DESC', 20);
		};

		$scope.canAddIdea = function () {
			return (LoginService.isLogged() && true === instance.hasLoadedIdeas && $scope.selectedArea);
		};

		$scope.addNewIdeaToArea = function() {
			$location.path("/areas/" + $scope.selectedArea.Id + "/create");
		};

        $scope.isLoading = function() {
            return !instance.hasLoadedIdeas;
        };

		this.initialize();
	})
	.controller('VotingIdeaController', function ($scope, $http, $routeParams, $rootScope, VotingService, LoginService) {
		var self = this;
		var user = LoginService.getUser();

        this.checkHasVoted = function() {
            VotingService.hasVoted($routeParams.ideaId).then(function (result) {
                $scope.hasVoted = result.data;
            });
        };

		this.initialize = function() {
		    self.checkHasVoted();
		};

		$scope.submitVotes = function (numberOfVotes) {
			VotingService.voteForIdea($routeParams.ideaId, numberOfVotes);
			$rootScope.$broadcast(numberOfVotes ? 'ideaVoted' : 'ideaUnvoted', numberOfVotes);
		}

		$rootScope.$on("userchanged", function(event, user) {
		    self.checkHasVoted();
		});

		$scope.canVote = function () {
			return (user !== null);
		};

		this.initialize();
	})
	.controller('IdeaDetailController', function ($scope, $http, $routeParams, LoginService) {
	    var instance = this,
            user = LoginService.getUser();

        this.readModel = function() {
            $http({
                method: 'GET',
                url: "/api/ideas/" + $routeParams.ideaId + '?_t=' + new Date().getTime(),
                cache: false
            }).then(function successCallback(response) {
                $scope.model = response.data;
            }, function errorCallback(response) {
            });
        };

	    this.initialize = function () {
	        instance.readModel();

	        $scope.$on('ideaVoted', function (event, args) {
	            $scope.model.TotalPoints += args;
	        });
	        $scope.$on('ideaUnvoted', function (event, args) {
	            setTimeout(function() { instance.readModel(); }, 300);
	        });
	        $scope.$on('ideaCommented', function (event, args) {
	            $scope.model.Comments = $scope.model.Comments || [];
	            $scope.model.Comments.push(args);
	            $scope.model.TotalComments++;
	        });
	       
	    };

        $scope.canViewNewCommentBox = function() {
            return (user !== null);
        };

        this.initialize();
	})
    .controller('IdeaCommentsController', function ($scope,$rootScope, $http, $routeParams, LoginService) {
        var instance = this,
            user = LoginService.getUser();

        this.initialize = function () {
            instance.resetComment();
        };

        this.resetComment = function () {
            if (!user) {
                return;
            }

            $scope.newComment = {
                Text: '',
                IdeaId: $routeParams.ideaId,
                Author: user.CompleteName,
                AuthorId: user.Id,
                CreationDate: new Date()
            };
        };

        $scope.sendComment = function () {
            if (!$scope.canSendComment()) {
                return;
            }

            $http({
                method: 'POST',
                url: "/api/ideas/" + $routeParams.ideaId + '/comment',
                data: $scope.newComment
            }).then(function successCallback(response) {
                $rootScope.$broadcast('ideaCommented', $scope.newComment);
                
                instance.resetComment();
                
            }, function errorCallback(response) {
                instance.resetComment();
            });
        };

        $scope.canSendComment = function () {
            return $scope.newComment && $scope.newComment.Text && 0 != $scope.newComment.Text.trim().length;
        };

        this.initialize();
    })
	.controller('SubmitIdeaDetailController', function ($scope, $routeParams, $http, LoginService) {
		var self = this;

		$scope.model = {
			title: '',
			description: '',
			success: false,
			validationError: false
		};

		this.validate = function() {
			return $scope.model.title && $scope.model.description;
		}

		this.cleanForm = function() {
			$scope.model.title = '';
			$scope.model.description = '';
		}

		$scope.submitIdea = function () {
			var isLogged = LoginService.isLogged();

			if (!isLogged)
				return;

			var user = LoginService.getUser();

			if (!self.validate()) {
				$scope.model.validationError = true;
				return;
			}

			$http({
				method: 'POST',
				url: '/api/ideas/',
				data: {
					AreaId: $routeParams.areaId,
					Title: $scope.model.title,
					Description: $scope.model.description,
					AuthorID: user.Id,
					Author: user.FirstName
				}
			}).then(function successCallback(response) {
				$scope.model.validationError = false;
				$scope.model.success = true;
				self.cleanForm();
			}, function errorCallback(response) {
				
			});
		};
	});
