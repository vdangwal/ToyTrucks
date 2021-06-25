
@REM echo 'Cleaning projects...'



@REM dotnet clean .\src\Services\Basket\Basket.Api\
@REM dotnet clean .\src\Services\Catalog\Catalog.Api\
@REM dotnet clean .\src\Services\Discount\Discount.Grpc\
@REM dotnet clean .\src\Services\Orders\Orders.Api\

echo 'Deleting all obj subfolders...'
 
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S obj') DO RMDIR /S /Q "%%G"
echo 'Deleting all bin subfolders...'
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S bin') DO RMDIR /S /Q "%%G"
@REM echo 'Building projects...'
@REM dotnet build .\src\Services\Basket\Basket.Api\
@REM dotnet build .\src\Services\Catalog\Catalog.Api\
@REM dotnet build .\src\Services\Discount\Discount.Grpc\
@REM dotnet build .\src\Services\Orders\Orders.Api\