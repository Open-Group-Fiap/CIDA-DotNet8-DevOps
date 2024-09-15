FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

COPY *.sln ./
COPY CIDA.Api/*.csproj ./CIDA.Api/
COPY CIDA.Data/*.csproj ./CIDA.Data/
COPY CIDA.Domain/*.csproj ./CIDA.Domain/
RUN dotnet restore

COPY CIDA.Api/. ./CIDA.Api/
COPY CIDA.Data/. ./CIDA.Data/
COPY CIDA.Domain/. ./CIDA.Domain/

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "CIDA.Api.dll"]
