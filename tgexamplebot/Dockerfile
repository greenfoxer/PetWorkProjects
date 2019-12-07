FROM mcr.microsoft.com/dotnet/core/runtime:2.1 as base

COPY publish/ app/

RUN mkdir -m 777 -p /app/data
 
VOLUME /app/data

ENTRYPOINT ["dotnet", "app/GShopPriceBot.dll"]
