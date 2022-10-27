FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Libraries/MetroBus.IOC/MetroBus.IOC.csproj", "MetroBus.IOC/"]
RUN dotnet restore "Libraries/MetroBus.IOC/MetroBus.IOC.csproj"
COPY . .
WORKDIR "/src/MetroBus.IOC"
RUN dotnet build "MetroBus.IOC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MetroBus.IOC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MetroBus.IOC.dll"]
