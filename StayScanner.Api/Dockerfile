# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 7146

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StayScanner.Api/StayScanner.Api.csproj", "StayScanner.Api/"]
COPY ["Hotel.Application/Hotel.Application.csproj", "Hotel.Application/"]
COPY ["Hotel.Infrastructure/Hotel.Infrastructure.csproj", "Hotel.Infrastructure/"]
COPY ["Hotel.Domain/Hotel.Domain.csproj", "Hotel.Domain/"]
COPY ["Report.Application/Report.Application.csproj", "Report.Application/"]
COPY ["Report.Infrastructure/Report.Infrastructure.csproj", "Report.Infrastructure/"]
COPY ["Report.Domain/Report.Domain.csproj", "Report.Domain/"]
RUN dotnet restore "./StayScanner.Api/StayScanner.Api.csproj"
COPY . .
WORKDIR "/src/StayScanner.Api"
RUN dotnet build "./StayScanner.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./StayScanner.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StayScanner.Api.dll"]