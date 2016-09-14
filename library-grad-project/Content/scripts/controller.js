(function () {
    var app = angular.module("LibraryApp", ["ngCookies"]);

    app.controller('LibraryController', function ($scope, $http, $filter, $cookies) {
        $scope.books = [];
        $scope.reservations = [];
        var self = this;
        $scope.loading = true;
        $scope.myReservations = []
        this.isBookReserved = function (bookId, date) {
            //debugger;
            return $scope.reservations.some(function (el) {
                // If reservation for that book
                if (el.BookId === bookId) {
                    // Check if it's in date range
                    var reservationStart = new Date(el.ReservationStart.split("/").reverse().join("-"));
                    var reservationEnd = new Date(el.ReservationEnd.split("/").reverse().join("-"));
                    var bookingDate = new Date(date.split("/").reverse().join("-"));
                    return bookingDate >= reservationStart && bookingDate <= reservationEnd;
                }
            });
        };

        this.loadBooks = function () {
            $http.get("/api/books").then(function (results) {
                $scope.books = results.data;
                self.cookiesLogin();
                $scope.loading = false;
            });
        };

        this.loadReservations = function () {
            $http.get("/api/reservations").then(function (results) {
                $scope.reservations = results.data;
            });
        };

        /*
        this.loadMyReservations = function () {
            $http.get("/api/users/"+$scope.localUser.Id).then(function (results) {
                console.log(results);
                $scope.myReservations = results.data.Reservations;
            });
        };
        */
        this.setUpForm = function () {
            $("#datepickerFrom, #datepickerTo").datepicker({
                dateFormat: "dd/mm/yy",
                beforeShowDay: function (date) {
                    var dateString = jQuery.datepicker.formatDate("dd/mm/yy", date);
                    
                    var booked = self.isBookReserved($scope.books[$scope.selectedBook].Id, dateString);
                    if (booked) {
                        return [false, "", "unAvailable"];
                    }
                    else {
                        return [true, "", "Available"];
                    }
                },
                todayHighlight: true
            });
        };

        this.loadReservations();
        this.loadBooks();

        this.changeSelected = function (index) {
            if ($scope.selectedBook === index) {
                $scope.selectedBook = -1;
            }
            else {
                $scope.selectedBook = index;
                self.setUpRating();
            }
            self.clearReservationForm();
        };

        this.makeReservation = function () {
            $scope.reservation.BookId = $scope.books[$scope.selectedBook].Id;
            $scope.reservation.ReservationStart = $filter("date")($scope.reservation.ReservationStart, "dd/MM/yyyy");
            $scope.reservation.ReservationEnd = $filter("date")($scope.reservation.ReservationEnd, "dd/MM/yyyy");
            $scope.reservation.UserId = $scope.localUser.Id;
            $http.post("/api/reservations", $scope.reservation).success(function () {
                console.log("success");
                self.loadReservations();
            }).error(function () {
                console.log("failure");
            });

            $scope.reservation = {};
        };

        this.clearReservationForm = function () {
            $scope.reservation = {};
            //document.getElementById("reservationButton").click();
            document.getElementById("reservation").classList.remove("in");
        };

        this.addBook = function () {            
            var tempBook = $scope.newBook;
            
            $http.post("/api/books", tempBook).success(function () {
                console.log("success");
                self.loadBooks();
                $scope.newBook = {};
            }).error(function () {
                console.log("failure");
            });            
        };

        // Try login from cookies
        this.cookiesLogin = function () {
            if ($cookies.get("loginName")) {
                var data = {};
                data.Name = $cookies.get("loginName");
                data.SessionKey = $cookies.get("sessionKey");

                $http.post("/api/login", data).success(function (data) {
                    $scope.loggedIn = true;
                    $scope.localUser = JSON.parse(data);
                    console.log("Cookies login suceeeded");
                }).error(function () {
                    console.log("Cookies login failed");
                });                
            }
        };

        this.handleLogin = function (event) {
            if (event.target.name === "Login") {
                $http.post("/api/login", $scope.user).success(function (data) {
                    data = JSON.parse(data);
                    $scope.loggedIn = true;
                    $scope.localUser = data;
                    $cookies.put("loginName", data.Name);
                    $cookies.put("sessionKey",data.SessionKey);
                }).error(function () {
                    console.log("failure");
                    alert("Failed to login");
                });
            }
            else {
                console.log($scope.user);
                $http.post("/api/users", $scope.user).success(function (data) {
                    $scope.loggedIn = true;
                    $scope.localUser = data;
                }).error(function () {
                    console.log("failure");
                    alert("Failed to register");
                });
            }
        };

        this.submitRating = function (rating) {
            console.log(rating + " for " + $scope.books[$scope.selectedBook] + "by " + $scope.localUser);
            var data = {};
            data.BookId = $scope.books[$scope.selectedBook].Id;
            data.Rating = rating;
            data.UserId = $scope.localUser.Id;
            $http.put("/api/ratings", data).success(function () {
                console.log("success");
            }).error(function () {
                console.log("failure");
                alert("Failed to register");
            });
        };

        this.setUpRating = function () {
            // Remove stars
            for (i = 1; i < 6; i++) {
                document.getElementById("star-" + i).removeAttribute("checked");
            }
            // Get personal rating
            $http.get("/api/ratings/"+$scope.localUser.Id).success(function (data) {
                if (data !== null) {
                    var rating = data.filter(function (el) {
                        return el.BookId === $scope.books[$scope.selectedBook].Id;
                    });
                    if (rating.length > 0)
                    {
                        document.getElementById("star-" + rating[0].Rating).setAttribute("checked", "checked");
                    }
                }
            }).error(function () {
                console.log("failure");
            });

            // Calculate total book rating
            $http.get("/api/ratings").success(function (data) {
                console.log(data);
                if (data !== null) {
                    var ratings = data.filter(function (el) {
                        return el.BookId === $scope.books[$scope.selectedBook].Id;
                    });
                    if (ratings.length > 0) {
                        var sum = 0;
                        ratings.forEach(function (el) {
                            sum += el.Rating;
                        });
                        console.log("total rating: " + sum);
                        $scope.totalRating = sum;
                        $scope.totalRates = ratings.length;
                    }
                    else {
                        $scope.totalRating = 0;
                        $scope.totalRates = 0;
                    }

                }
            }).error(function () {
                console.log("failure");
            });
        };

        this.isDatePast = function (date) {
            var today = Date.parse(new Date());
            var otherDate = date.split("/");
            otherDate = Date.parse(new Date(otherDate[2], otherDate[1]-1, otherDate[0]));
            return today > otherDate;
        };

        this.removeReservation = function (id) {
            $http.delete("/api/reservations/" + id).success(function () {
                console.log("success");
                $scope.reservations = $scope.reservations.filter(function(el) {
                    if (el.Id !== id) {
                        return true;
                    }
                    return false;
            });
            });
        }
    });
})();
