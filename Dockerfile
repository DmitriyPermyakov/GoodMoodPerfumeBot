#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GoodMoodPerfumeBot/GoodMoodPerfumeBot.csproj", "GoodMoodPerfumeBot/"]
RUN dotnet restore "GoodMoodPerfumeBot/GoodMoodPerfumeBot.csproj"
COPY . .
WORKDIR "/src/GoodMoodPerfumeBot"
RUN dotnet build "GoodMoodPerfumeBot.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS migrations
WORKDIR /src
COPY ["ApplyMigrations/ApplyMigrations.csproj", "ApplyMigrations/"]
RUN dotnet restore "ApplyMigrations/ApplyMigrations.csproj"
COPY . .
WORKDIR "/src/ApplyMigrations"
RUN dotnet build "ApplyMigrations.csproj" -c Release -o /app/ApplyMigrations

FROM build AS publish
RUN dotnet publish "GoodMoodPerfumeBot.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /ApplyMigrations
COPY --from=ApplyMigrations /app/ApplyMigrations .


WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GoodMoodPerfumeBot.dll"]