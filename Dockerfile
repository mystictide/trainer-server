FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["trainer-server/trainer.server.csproj", "trainer-server/"]
RUN dotnet restore "trainer-server/trainer.server.csproj"
COPY . .
WORKDIR "/src/trainer-server"
RUN dotnet build "trainer.server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "trainer.server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=https://+:747
ENV ASPNETCORE_HTTP_PORT=https://+:747
EXPOSE 747
ENTRYPOINT ["dotnet", "trainer.server.dll", "--urls", "https://+:747"]