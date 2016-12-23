$(document).ready(function () {
    $.ajaxSetup({ timeout: 10000 })
})

function Search($scope) {
    var uri = "/Mind/GetSearchResultsAjax";
    $scope.searchstring = "";
    var expSearchString = new RegExp("[0-9, ,a-z]{2,25}", "i");
    $scope.items = [];
    var blockNo = 0;
    var retry = 0;
    $scope.no_blocks = 0;
    $scope.noMoreResults = false;
    $scope.loading = false;

    $scope.loadMore = function () {
        if (!$scope.noMoreResults) {
            $("#searchStatus").html();
            $("#loadingImg").show();
            $.getJSON(uri, { pageNumber: ++blockNo, searchString: $scope.searchstring })
                .done(function (data) {
                    $scope.noMoreResults = data.EndOfRecords;
                    UpdateModel(data.ResultData);
                })
                .fail(function (jqXHR, textStatus, err) {
                    console.log('Failed: ' + textStatus + "   " + err);
                    $("#loadingImg").hide();
                    blockNo--;
                    if (textStatus == 'timeout')
                        $("#searchStatus").html('Request Timed Out');
                    else {
                        $("#searchStatus").html(textStatus);
                    }
                    $scope.loading = false;
                }).complete(function () {
                    console.log("Complete:"+blockNo);
                    if ($(document).height() < $(window).height() && !$scope.noMoreResults && !$scope.loading && ++retry<=2) {
                        $scope.loading = true;
                        $scope.loadMore();
                    }
                    if ($scope.no_blocks > blockNo) {
                        $scope.loading = true;
                        $scope.loadMore();
                    }
                    if ($scope.items[0]) {
                       // $("#collapse" + $scope.items[0].MID).attr("class", "panel-collapse in");
                    }
                });
        }
    };
   
    $scope.startLoading = function () {
        $scope.loading = true;
        retry = 0;
        window.location.hash = "";
        $scope.noMoreResults = false;
        $("#searchStatus").html("");
        blockNo = 0;
        $scope.no_blocks = 0;
        $scope.items = [];
        window.location.hash = "q=" + $scope.searchstring + "&p=1";
        $scope.loadMore();
    }

    //$scope.callback = function () {
    //    if ($(document).height() < $(window).height() + 200 && !$scope.noMoreResults && !$scope.loading) {
    //        $scope.loadMore();
    //    }
    //    $("#collapse" + $scope.items[0].MID).attr("class", "panel-collapsein in");
    //}

    $(document).ready(function () {
        var hashValue = window.location.hash.substring(1);
        var expSearch = new RegExp("q=?([0-9, ,a-z]+)&", "i");
        var expPage = new RegExp("&p=?([0-9]+)", "i");
        var query = expSearch.exec(hashValue);
        var page = expPage.exec(hashValue);
        if (page) {
            $scope.no_blocks = page[1];
        }
        if (query) {
            $scope.searchstring = query[1];
            $scope.loading = true;
            window.location.hash = "";
            $("#searchStatus").html("");
            window.location.hash = "q=" + $scope.searchstring + "&p=1";
            $scope.loadMore();
        }
    });

    window.onscroll = function myfunction() {
        if ($(window).scrollTop() >= $(document).height() - $(window).height()-50 && !$scope.noMoreResults && !$scope.loading)
        {
            $scope.loading = true;
            $scope.loadMore();
        }
        else if ($scope.noMoreResults) {
            $("#searchStatus").html('End of results');
        }
    }

    //$scope.AddArchitect = function (id) {
    //    console.log(id);
    //}

    function UpdateModel(data) {
        if (Object.keys(data).length > 0) {
            // On success, 'data' contains a list of products.
            $.each(data, function (key, item) {
                // Add a list item for the product.
                $scope.items.push(item);
                $scope.$apply();
            });
            window.location.hash = "q=" + $scope.searchstring + "&p="+blockNo;
            $("#loadingImg").hide();
            $scope.loading = false;
        }
        if($scope.noMoreResults) {
            $("#loadingImg").hide(0);
            $scope.loading = false;
            $("#searchStatus").html('End of results');
        }
        else {
            $("#searchStatus").html('Scroll down to load more');
        }
    }
}

angular.module('searchresult', []);


