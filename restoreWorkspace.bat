 

echo 'Building projects...'
dotnet restore .\src\BuildingBlocks\EventBus.Messages\
dotnet restore .\src\Services\Discount\Discount.Grpc\
dotnet restore .\src\Services\Catalog\Catalog.Api\
dotnet restore .\src\Services\Basket\Basket.Api\
dotnet restore .\src\Services\Orders\Orders.Api\
dotnet build .\src\Security\Identity\
 
dotnet restore .\src\ApiGateways\Shopping.Aggregator\
dotnet restore .\src\ApiGateways\OcelotApi\
dotnet restore .\src\Frontends\Mvc\Web\
 