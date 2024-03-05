FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Car.Auction.Management.System.Web/Car.Auction.Management.System.Web.csproj", "Car.Auction.Management.System.Web/"]
RUN dotnet restore "Car.Auction.Management.System.Web/Car.Auction.Management.System.Web.csproj"
COPY . .
WORKDIR "/src/Car.Auction.Management.System.Web"
RUN dotnet build "Car.Auction.Management.System.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Car.Auction.Management.System.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Car.Auction.Management.System.Web.dll"]
