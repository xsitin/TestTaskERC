FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AreaAccountApi/AreaAccountApi.csproj", "AreaAccountApi/"]
COPY ["AreaAccountData/AreaAccountData.csproj", "AreaAccountData/"]
RUN dotnet restore "AreaAccountApi/AreaAccountApi.csproj"
COPY . .
WORKDIR "/src/AreaAccountApi"
RUN dotnet build "AreaAccountApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AreaAccountApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AreaAccountApi.dll"]
