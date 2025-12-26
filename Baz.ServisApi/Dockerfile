#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["bazkurum/Baz.ServisApi/Baz.KurumServiceApi.csproj", "bazkurum/Baz.ServisApi/"]
COPY ["bazkurum/Baz.Service/Baz.Service.csproj", "bazkurum/Baz.Service/"]
COPY packages /src/packages
RUN dotnet restore "bazkurum/Baz.ServisApi/Baz.KurumServiceApi.csproj" --source /src/packages --source https://api.nuget.org/v3/index.json
COPY bazkurum/Baz.ServisApi bazkurum/Baz.ServisApi
COPY bazkurum/Baz.Service bazkurum/Baz.Service
WORKDIR "/src/bazkurum/Baz.ServisApi"
RUN dotnet build "Baz.KurumServiceApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Baz.KurumServiceApi.csproj" -c Release -o /app/publish

FROM base AS final

# Change timezone to local time
ENV TZ=Europe/Istanbul
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Baz.KurumServiceApi.dll"]
