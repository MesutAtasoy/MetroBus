FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Applications/Application.Data/Application.Data.csproj", "Application.Data/"]
RUN dotnet restore "Applications/Application.Data/Application.Data.csproj"
COPY . .
WORKDIR "/src/Application.Data"
RUN dotnet build "Application.Data.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Application.Data.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Application.Data.dll"]
