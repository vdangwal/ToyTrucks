
@REM echo 'Cleaning projects...'
@REM dotnet clean .\src\Services\Basket\Basket.Api\
@REM dotnet clean .\src\Services\Catalog\Catalog.Api\
@REM dotnet clean .\src\Services\Discount\Discount.Grpc\
@REM dotnet clean .\src\Services\Orders\Orders.Api\

echo 'Building projects...'
dotnet build .\src\Services\Basket\Basket.Api\
dotnet build .\src\Services\Catalog\Catalog.Api\
dotnet build .\src\Services\Discount\Discount.Grpc\
dotnet build .\src\Services\Orders\Orders.Api\