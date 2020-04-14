FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/sdk:3.1
WORKDIR /app
COPY --from=build-env /app/out .

LABEL kind="weather"

RUN mkdir /root/.dotnet/tools
ENV PATH="/root/.dotnet/tools:${PATH}"
RUN dotnet tool install -g dotnet-counters

ENTRYPOINT ["dotnet", "weatherapp-route-to-code.dll", "--urls=http://*:80"]
