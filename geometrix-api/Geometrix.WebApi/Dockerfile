FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files for restore
COPY Geometrix.sln global.json ./
COPY Geometrix.AppHost/Geometrix.AppHost.csproj Geometrix.AppHost/
COPY Geometrix.Application/Geometrix.Application.csproj Geometrix.Application/
COPY Geometrix.Domain/Geometrix.Domain.csproj Geometrix.Domain/
COPY Geometrix.Infrastructure/Geometrix.Infrastructure.csproj Geometrix.Infrastructure/
COPY Geometrix.ServiceDefaults/Geometrix.ServiceDefaults.csproj Geometrix.ServiceDefaults/
COPY Geometrix.WebApi/Geometrix.WebApi.csproj Geometrix.WebApi/

RUN dotnet restore Geometrix.WebApi/Geometrix.WebApi.csproj

# Copy full source and build
COPY . .
RUN dotnet publish Geometrix.WebApi/Geometrix.WebApi.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Geometrix.WebApi.dll"]
