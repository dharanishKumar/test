function Search($scope) {

    SearchState = {
        query: "",
        option:0,
        results : [],
        endOfResults : false,
        pageNo : 0,
        selected : "",
        retryNo : 0
    };

    $scope.searchOptions = [
        { Name: 'AD', Value: '0',PlaceHolder:'MID or Name' },
        { Name: 'Portal', Value: '1',PlaceHolder:'Search' }
    ];

    $scope.searchOption = $scope.searchOptions[0];

    if (history.replaceState === undefined) {
        var History = window.History;
        var State = History.getState();
    }

    $(document).ready(function () {
        $.ajaxSetup({ timeout: 30000 });
        if (history.replaceState !== undefined) {
            ResultsRecreate(history.state);
        }
        else {
            ResultsRecreate(State.data);
        }
        
    });

    var uri = "/Mind/GetSearchResultsAjax";
    $scope.searchstring = "";
    var expSearchString = new RegExp("^[0-9, ,a-z]{3,25}$", "i");
    $scope.items = [];
    var blockNo = 0;
    var retry = 0;
    var searchquery = "";
    $scope.noMoreResults = false;
    $scope.loading = false;

    $scope.loadMore = function () {
        if (!$scope.noMoreResults) {
            $("#searchStatus").html();
            if (retry!=0) {
                $("#loadingImg").show();
            }
            if (!angular.isString($scope.searchstring) && retry > 0) {
                searchquery = SearchState.query;
            }
            else if(angular.isString($scope.searchstring)){
                searchquery = $scope.searchstring;
            }
            else {
                searchquery = "";
            }
            if (searchquery.length > 0) {
                $.getJSON(uri, { pageNumber: ++blockNo, searchString: searchquery, option: $scope.searchOption.Value, cache: false })
                .done(function (data) {
                    $scope.noMoreResults = data.EndOfRecords;
                    if (data.TotalRecordCount>0) {
                        UpdateModel(data.ResultData);
                    }
                    else {
                        UpdateModel(data.ResultData);
                        $("#searchStatus").html('No results');
                    }
                })
                .error(function () {
                })
                .fail(function (jqXHR, textStatus, err) {
                    LoadingImgHide();
                    blockNo--;
                    if (textStatus == 'timeout')
                        $("#searchStatus").html('Request Timed Out');
                    else {
                        $("#searchStatus").html(textStatus);
                    }
                    $scope.loading = false;
                })
                .complete(function () {
                    if ($(document).height() < $(window).height() + 20 && !$scope.noMoreResults && !$scope.loading && ++retry <= 2 && $scope.items.length>0) {
                        $("#searchStatus").html();
                        LoadingImgHide();
                        $scope.loading = true;
                        $scope.loadMore();
                    }
                    else {
                        LoadingImgHide();
                        retry = 1;
                    }
                });
            }
            else {
                LoadingImgHide();
                $scope.loading = false;
            }
        }
    };

    $scope.startLoading = function () {
        $scope.loading = true;
        $(".loadingmain").show();
        retry = 0;
        $scope.noMoreResults = false;
        blockNo = 0;
        $scope.loadMore();
    }

    window.onscroll = function () {
        if ($(window).scrollTop() >= $(document).height() - $(window).height()-50 && !$scope.noMoreResults && !$scope.loading) {
            $scope.loading = true;
            $scope.loadMore();
        }
        else if ($scope.noMoreResults) {
            $("#searchStatus").html('End of results');
        }
    }

    $scope.AddArchitect = function (id) {
        SearchState.selected = id;
        SaveSearchState();
    }

    function ResultsRecreate(stateData) {
        //if state exists for this url
        if (stateData) {
            FillUsingStateData(stateData);
        }
        //if url contains a search query and a saved state doesnt exist
        else {
            FillUsingQueryString();
        }
    }

    function FillUsingStateData(stateData) {
        if (stateData.results) {
            if (stateData.results)
                $scope.items = stateData.results;
            if (stateData.query)
                $scope.searchstring = stateData.query;
            if (stateData.option) {
                $scope.searchOption = $scope.searchOptions[stateData.option];
            }
            if (stateData.endOfResults)
                $scope.noMoreResults = stateData.endOfResults;
            if (stateData.pageNo)
                blockNo = stateData.pageNo;
            if (stateData.retryNo) {
                retry = stateData.retryNo;
            }
            $scope.$apply();
            if ($scope.noMoreResults) {
                $("#searchStatus").html('End of results');
            }
            else {
                $("#searchStatus").html('Scroll down to load more');
            }
            if (stateData.selected) {
                $("#collapse" + stateData.selected).attr("class", "panel-collapse in");
            }
        }
        else {
            FillUsingQueryString();
        }
    }

    function FillUsingQueryString() {
        if (angular.isString(getUrlVars()["q"])) {
            var URLquery = getUrlVars()["q"].replace(/\+/g," ");
            $scope.$apply(function () {
                $scope.searchstring = URLquery;
            });
            if (expSearchString.test(URLquery)) {
                $scope.searchstring = URLquery;
                $scope.startLoading();
            }
            else {
                $(architectSearch.query).val(URLquery);
            }
        }
    }

    function SaveSearchState() {
        if (!(typeof($scope.searchstring) === "undefined")) {
            SearchState.query = $scope.searchstring;
        }
        SearchState.endOfResults = $scope.noMoreResults;
        SearchState.results = $scope.items;
        SearchState.pageNo = blockNo;
        SearchState.retryNo = retry;
        SearchState.option = $scope.searchOption.Value;
        if (history.replaceState!==undefined) {
            history.replaceState(SearchState, null, "?q=" + SearchState.query.replace(/\ /g, "+"));
        }
        else {
            History.replaceState(SearchState, null, "?q=" + SearchState.query.replace(/\ /g, "+"));
        }
    }

    function UpdateModel(data) {
        if (retry==0) {
            $scope.items = [];
        }
        if (data.length > 0) {
            $scope.items=$scope.items.concat(data);
            LoadingImgHide();
            $scope.loading = false;
        }
        SaveSearchState();
        $scope.$apply();
        if ($scope.noMoreResults) {
            LoadingImgHide();
            $scope.loading = false;
            $("#searchStatus").html('End of results');
        }
        else {
            $("#searchStatus").html('Scroll down to load more');
        }
    }

    function LoadingImgHide() {
        $("#loadingImg").hide();
        $(".loadingmain").hide();
    }

    $(document).on('click', '[data-toggle=collapse]', function (e) {

        var Mid = $(this).data('mid');

        //To modify the text on add button to Add or Update. 
        var btnId = "#addbtn" + Mid;
        $.ajax("/Mind/IsAlreadyAMember", {
            type: "POST",
            data: { mId: Mid },
            dataType: "json"
        }).done(function (data) {
            if (data) {
                $(btnId).text("Update");
            }
            else {
                $(btnId).text("Add");
            }
        });

    // Load the image at the time when name is clicked

        $('.profileImg').attr('src', '/Images/fadingblocks.gif');
        var Mid = $(this).data('mid');

        $.ajax({
            type: 'GET',
            url: '/Mind/GetImageAjax/',
            data: { mid: Mid },
            dataType: 'json',
            timeout: 20000,
            statusCode: {
                200: function (response) {
                    // $('.profileImg').attr('src', 'data:image/jpg;base64,' + response.responseText);
                    if (response.responseText != "")
                        $('.profileImg').attr('src', 'data:image/jpg;base64,' + response.responseText);
                    else
                        $('.profileImg').attr('src', '/Images/users/user-male.jpg');
                }
            },
            error: function (objRequest, errortype) {
                if (errortype == 'timeout') {
                    $('.profileImg').attr('alt', 'Timeout');
                }
            }
        })
    })
}

angular.module('searchresult', []);



