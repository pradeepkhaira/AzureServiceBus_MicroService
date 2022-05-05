#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BurgerKingOrder.MicroService/BurgerKingOrder.MicroService.csproj", "BurgerKingOrder.MicroService/"]
RUN dotnet restore "BurgerKingOrder.MicroService/BurgerKingOrder.MicroService.csproj"
COPY . .
WORKDIR "/src/BurgerKingOrder.MicroService"
RUN dotnet build "BurgerKingOrder.MicroService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BurgerKingOrder.MicroService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BurgerKingOrder.MicroService.dll"]