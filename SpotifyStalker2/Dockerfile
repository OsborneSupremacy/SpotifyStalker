#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SpotifyStalker2/SpotifyStalker2.csproj", "SpotifyStalker2/"]
COPY ["Spotify.Object/Spotify.Object.csproj", "Spotify.Object/"]
COPY ["Spotify.Interface/Spotify.Interface.csproj", "Spotify.Interface/"]
COPY ["SpotifyStalker.Interface/SpotifyStalker.Interface.csproj", "SpotifyStalker.Interface/"]
COPY ["Spotify.Model/Spotify.Model.csproj", "Spotify.Model/"]
COPY ["SpotifyStalker.Model/SpotifyStalker.Model.csproj", "SpotifyStalker.Model/"]
COPY ["SpotifyStalker.Service/SpotifyStalker.Service.csproj", "SpotifyStalker.Service/"]
RUN dotnet restore "SpotifyStalker2/SpotifyStalker2.csproj"
COPY . .
WORKDIR "/src/SpotifyStalker2"
RUN dotnet build "SpotifyStalker2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SpotifyStalker2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SpotifyStalker2.dll"]