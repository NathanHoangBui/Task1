(function ()
{
    angular.module("Task1_SelfPractice_1").controller('task3', [
        "$scope",
        "$http",
        function (
            $scope,
            $http,
        )
        {
            var getAllProductFinished = function ()
            {
                return $http({
                        method: 'GET',
                        url: '/api/product/get-category'
                }).then(function successCallback(response)
                {
                        $scope.products = response.data;
                },      function errorCallback(response)
                {
                    // called asynchronously if an error occurs
                    // or server returns response with an error status.
                });
            };
            var getAllProduct = function ()
            {
                return $http({
                    method: 'GET',
                    url: '/api/product/get-task3part2'
                }).then(function successCallback(response)
                {
                       $scope.products = response.data;
                },     function errorCallback(response)
                {
                    // called asynchronously if an error occurs
                    // or server returns response with an error status.
                });
            };
            $scope.updateOption = function ()
            {
                if ($scope.optionValue == "1")
                {
                     return getAllProductFinished();
                }
                else if ($scope.optionValue == "2")   
                {
                     return getAllProduct();
                }
                else
                {
                     return $q.resolved();
                }
            };
            $scope.init = function ()
            {
                $scope.optionValue = "1";
                return $scope.updateOption();
            }
        }

    ])

})();
