FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

COPY / ./

ENV NODE_ENV=production
RUN curl -fsSL https://deb.nodesource.com/setup_22.x | bash - && apt-get install -y nodejs
RUN cd ./Rise.Client && npm i && npm run build:prod && cd ..

RUN dotnet tool install --global dotnet-ef \
&& dotnet tool install --global dotnet-format

#ENV PATH="$PATH:/root/.dotnet/tools"
#Code analysis aan de hand van dotnet format
#WORKDIR /App
#RUN dotnet format /App/Rise.sln --verify-no-changes  --verbosity diagnostic
# Test projects
#WORKDIR /App/Rise.Client.Tests
#RUN dotnet test --no-build

# Code coverage
# WORKDIR /App/Rise.Client.Tests
# RUN dotnet test --no-build --collect:"XPlat Code Coverage"
# RUN mv /root/.microsoft/coverage /App/coverage

WORKDIR /App
RUN dotnet restore
RUN dotnet publish ./Rise.Server -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:8.0

COPY --from=build-env /App/out .

RUN apt-get update && apt-get install -y --no-install-recommends \
    fonts-dejavu \
    fonts-liberation \
    fonts-noto \
    && rm -rf /var/lib/apt/lists/

ENV PATH="$PATH:/root/.dotnet/tools"

ENTRYPOINT ["dotnet", "Rise.Server.dll"]
