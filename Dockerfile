FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY arm-hello-world/*.csproj ./
RUN dotnet restore -r linux-arm

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out -r linux-arm

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.10-buster-slim-arm32v7
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "arm-hello-world.dll"]