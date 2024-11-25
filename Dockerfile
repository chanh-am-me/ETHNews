# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

COPY . /src

WORKDIR /src

RUN dotnet nuget add source "https://pkgs.dev.azure.com/tgbots/Telegram.Bot/_packaging/release/nuget/v3/index.json" -n Telegram.Bot
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

COPY --from=build /app .

ENTRYPOINT ["dotnet", "Web.dll"]