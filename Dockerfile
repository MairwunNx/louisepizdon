FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY Louisepizdon/Louisepizdon.csproj Louisepizdon/
RUN dotnet restore Louisepizdon/Louisepizdon.csproj

# Copy everything else and build
COPY Louisepizdon/ Louisepizdon/
WORKDIR /src/Louisepizdon
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set timezone support
ENV TZ=Europe/Moscow
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

ENTRYPOINT ["dotnet", "Louisepizdon.dll"]