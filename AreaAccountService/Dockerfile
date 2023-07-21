FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["AreaAccountService/AreaAccountService.csproj", "AreaAccountService/"]
RUN dotnet restore "AreaAccountService/AreaAccountService.csproj"
COPY . .
WORKDIR "/src/AreaAccountService"
RUN dotnet build "AreaAccountService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AreaAccountService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AreaAccountService.dll"]
