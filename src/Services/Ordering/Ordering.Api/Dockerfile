FROM mcr.microsoft.com/dotnet/sdk:5.0 AS debug
ENV ASPNETCORE_URLS=http://+:4406

RUN apt-get update
RUN apt-get install -y unzip

RUN curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l ~/vsdbg

WORKDIR /orderapiwork
COPY ["Services/Ordering/Ordering.Api/Ordering.Api.csproj", "Services/Ordering/Ordering.Api/"]
COPY ["Services/Ordering/Ordering.Application/Ordering.Application.csproj", "Services/Ordering/Ordering.Application/"]
COPY ["Services/Ordering/Ordering.Domain/Ordering.Domain.csproj", "Services/Ordering/Ordering.Domain/"]
COPY ["Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj", "Services/Ordering/Ordering.Infrastructure/"]

RUN dotnet restore Services/Ordering/Ordering.Api/Ordering.Api.csproj
COPY . ./


RUN dotnet publish Services/Ordering/Ordering.Api/Ordering.Api.csproj -o out -c Release 
#WORKDIR /orderapiwork/Services/Ordering/Ordering.Api

ENTRYPOINT [ "dotnet", "run", "--no-launch-profile"] 

# # # # # Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0 as prod

WORKDIR /app/

COPY --from=debug /out/ /app/
RUN chmod +x /app/ 
CMD ["dotnet", "Ordering.Api.dll"]
