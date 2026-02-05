FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["OrleansAuction.Client/OrleansAuction.Client.csproj", "OrleansAuction.Client/"]
COPY ["OrleansAuction.Shared/OrleansAuction.Shared.csproj", "OrleansAuction.Shared/"]
RUN dotnet restore "OrleansAuction.Client/OrleansAuction.Client.csproj"
COPY . .
WORKDIR "/src/OrleansAuction.Client"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrleansAuction.Client.dll"]