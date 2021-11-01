echo 'running projects...'
@REM dotnet build .\src\BuildingBlocks\EventBus.Messages\
@REM dotnet build .\src\Services\Discount\Discount.Grpc\
start /d "C:\Code\Core\Work\ProductAppMicroservices\src\Services\Catalog\Catalog.Api\Catalog.Api.csproj" dotnet run
start /d "C:\Code\Core\Work\ProductAppMicroservices\src\Services\Basket\Basket.Api\Basket.Api.csproj" dotnet run
@REM dotnet run --project .\src\Services\Catalog\Catalog.Api\Catalog.Api.csproj
@REM dotnet run --project .\src\Services\Basket\Basket.Api\Basket.Api.csproj
@REM dotnet run --project .\src\Security\Identity\Identity.csproj
@REM dotnet build .\src\Services\Orders\Orders.Api\
@REM dotnet build .\src\Services\Inventory\Inventory.Api\
@REM dotnet build .\src\ApiGateways\Shopping.Aggregator\
@REM dotnet build .\src\ApiGateways\OcelotApi\
@REM dotnet run --project .\src\Frontends\Mvc\Web\Web.csproj