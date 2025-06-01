FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["ReadNest.Application/ReadNest.Application.csproj", "ReadNest.Application/"]
COPY ["ReadNest.Domain/ReadNest.Domain.csproj", "ReadNest.Domain/"]
COPY ["ReadNest.Infrastructure/ReadNest.Infrastructure.csproj", "ReadNest.Infrastructure/"]
COPY ["ReadNest.Shared/ReadNest.Shared.csproj", "ReadNest.Shared/"]
COPY ["ReadNest.WebAPI/ReadNest.WebAPI.csproj", "ReadNest.WebAPI/"]

RUN dotnet restore "ReadNest.WebAPI/ReadNest.WebAPI.csproj"

COPY . .

WORKDIR /src/ReadNest.WebAPI
RUN dotnet build "ReadNest.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ReadNest.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8386;http://+:28383
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8386
EXPOSE 28383

COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "ReadNest.WebAPI.dll" ]
