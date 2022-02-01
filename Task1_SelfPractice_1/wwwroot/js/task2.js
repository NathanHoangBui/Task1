(function ()
{
    angular.module("Task1_SelfPractice_1").controller('task2', [
        "$scope",
        "$http",
        function (
            $scope,
            $http,
        )
        {
            var getProduct = function ()
            {
                return $http({
                    method: 'GET',
                    url: '/api/product/get-category'
                }).then(function successCallback(response)
                {
                    $scope.products = response.data;
                }, function errorCallback(response)
                {
                    // called asynchronously if an error occurs
                    // or server returns response with an error status.
                });
            }

            $scope.init = function ()
            {
                return getProduct();
            }


        }
    ])

})();
