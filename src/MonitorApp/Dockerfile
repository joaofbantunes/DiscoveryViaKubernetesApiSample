FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-chiseled AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
WORKDIR /src
COPY ["src/MonitorApp/MonitorApp.csproj", "src/MonitorApp/"]
RUN dotnet restore "src/MonitorApp/MonitorApp.csproj"
COPY . .
WORKDIR "/src/src/MonitorApp"
RUN dotnet build "MonitorApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MonitorApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MonitorApp.dll"]
