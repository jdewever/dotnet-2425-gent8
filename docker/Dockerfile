FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

COPY ../ ./

RUN curl -fsSL https://deb.nodesource.com/setup_22.x | bash - && apt-get install -y nodejs
RUN cd ./Rise.Client && npm i && npm run build && cd ..

RUN dotnet restore
RUN dotnet publish ./Rise.Server -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /App
COPY --from=build-env /App/out .
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

ENTRYPOINT ["dotnet", "Rise.Server.dll"]
