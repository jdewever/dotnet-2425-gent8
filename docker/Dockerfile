FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

COPY / ./

ENV NODE_ENV=production
RUN curl -fsSL https://deb.nodesource.com/setup_22.x | bash - && apt-get install -y nodejs
RUN cd ./Rise.Client && npm i && npm run build:prod && cd ..

RUN dotnet restore
RUN dotnet publish ./Rise.Server -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App

COPY --from=build-env /App/out .

RUN apt-get update && apt-get install -y --no-install-recommends \
    fonts-dejavu \
    fonts-liberation \
    fonts-noto \
    && rm -rf /var/lib/apt/lists/

# Create the logs directory and the initial log file
RUN mkdir -p /App/logs && touch /App/logs/log-20241218.log

ENV PATH="$PATH:/root/.dotnet/tools"

ENTRYPOINT ["dotnet", "Rise.Server.dll"]
