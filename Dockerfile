FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS builder
WORKDIR /src
COPY Louisepizdon/Louisepizdon.csproj ./Louisepizdon/
RUN dotnet restore ./Louisepizdon/Louisepizdon.csproj
COPY Louisepizdon/ ./Louisepizdon/
COPY .version ./.version
ARG APP_VERSION
ARG BUILD_TIME
RUN : "${APP_VERSION:=$(cat ./.version 2>/dev/null || echo 0.0.0)}" && \
    : "${BUILD_TIME:=$(date -u +%Y%m%d-%H%M%S)}" && \
    dotnet publish ./Louisepizdon/Louisepizdon.csproj \
      -c Release \
      -o /app/publish \
      --no-restore \
      -p:Version=${APP_VERSION} \
      -p:InformationalVersion="${APP_VERSION}+${BUILD_TIME}"

FROM mcr.microsoft.com/dotnet/runtime:9.0-alpine
WORKDIR /app
RUN apk --no-cache add ca-certificates tzdata && \
    update-ca-certificates && \
    adduser -D -s /bin/sh louisepizdon
COPY --from=builder /app/publish .
USER louisepizdon
ENTRYPOINT ["dotnet", "Louisepizdon.dll"]
