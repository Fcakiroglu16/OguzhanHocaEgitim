FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RabbitMqApp.WorkerService/RabbitMqApp.WorkerService.csproj", "RabbitMqApp.WorkerService/"]
RUN dotnet restore "RabbitMqApp.WorkerService/RabbitMqApp.WorkerService.csproj"
COPY . .
WORKDIR "/src/RabbitMqApp.WorkerService"
RUN dotnet build "./RabbitMqApp.WorkerService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RabbitMqApp.WorkerService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RabbitMqApp.WorkerService.dll"]
