(function () {
    var app = angular.module("LibraryApp", []);

    app.controller('LibraryController', function ($scope, $http, $filter) {
        $scope.books = [];
        $scope.reservations = [];
        var self = this;
        $scope.loading = true;

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
                $scope.loading = false;
            });
        };

        this.loadReservations = function () {
            $http.get("/api/reservations").then(function (results) {
                $scope.reservations = results.data;
            });
        };

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
            }
            self.clearReservationForm();
        };

        this.makeReservation = function () {
            $scope.reservation.BookId = $scope.books[$scope.selectedBook].Id;
            $scope.reservation.ReservationStart = $filter("date")($scope.reservation.ReservationStart, "dd/MM/yyyy");
            $scope.reservation.ReservationEnd = $filter("date")($scope.reservation.ReservationEnd, "dd/MM/yyyy");

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
    });
})();
