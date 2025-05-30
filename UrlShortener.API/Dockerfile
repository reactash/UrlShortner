# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest and publish
COPY . ./
RUN dotnet publish -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build /app/out ./

# Expose port 80
EXPOSE 80

# Tell ASP.NET Core to listen on port 80
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "UrlShortener.API.dll"]
