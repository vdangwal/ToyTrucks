FROM mcr.microsoft.com/dotnet/sdk:5.0 AS debug
ENV ASPNETCORE_URLS=http://+:80  

RUN apt-get update
RUN apt-get install -y unzip
# dls a sh script from ../getvsdbgsh and pipes it to /bin/sh for execution and provides a few arguments, most importantly the /vsdbg which is where the remote debugger will be extracted to
RUN curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l ~/vsdbg


WORKDIR /orderapiwork

COPY ["Services/Orders/Orders.Api/Orders.Api.csproj", "Services/Orders/Orders.Api/"]
COPY ["BuildingBlocks/Messaging/Messaging.csproj", "BuildingBlocks/Messaging/"]
RUN dotnet restore Services/Orders/Orders.Api/Orders.Api.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish Services/Orders/Orders.Api/Orders.Api.csproj --no-restore -o out -c Release
#--no-launch-profile means it doesnt access launchSettings.json
ENTRYPOINT ["dotnet", "run", "-p", "Services/Orders/Orders.Api/", "--no-launch-profile"] 

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0 as prod

WORKDIR /app/
#COPY --from=Debug /out/ /app/
COPY --from=debug /orderapiwork/out/ /app/
RUN chmod +x /app/ 
CMD dotnet Orders.Api.dll





