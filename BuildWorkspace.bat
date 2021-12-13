
@REM echo 'Cleaning projects...'
@REM dotnet clean .\src\Services\Basket\Basket.Api\
@REM dotnet clean .\src\Services\Catalog\Catalog.Api\
@REM dotnet clean .\src\Services\Discount\Discount.Grpc\
@REM dotnet clean .\src\Services\Orders\Orders.Api\

echo 'Building projects...'
dotnet build .\src\BuildingBlocks\EventBus.Messages\
dotnet build .\src\Services\Discount\Discount.Grpc\
dotnet build .\src\Services\Catalog\Catalog.Api\
dotnet build .\src\Services\Basket\Basket.Api\
dotnet build .\src\Services\Orders\Orders.Api\
dotnet build .\src\Security\Identity\
@REM dotnet build .\src\Services\Inventory\Inventory.Api\
@REM dotnet build .\src\ApiGateways\Shopping.Aggregator\
dotnet build .\src\ApiGateways\OcelotApi\
dotnet build .\src\Frontends\Mvc\Web\
 