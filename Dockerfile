FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base 

USER root 

# Update and upgrade openssl to patched version 
RUN apk update && \ 
	apk add --no-cache --upgrade openssl && \ 
	: 

USER $APP_UID 
WORKDIR /app 

# Build stage (same as before) 
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build 
ARG BUILD_CONFIGURATION=Release 

WORKDIR /src 
COPY ["CleverConversion.csproj", "."] 
RUN dotnet restore "./CleverConversion.csproj" 

COPY . .
WORKDIR "/src/." 
RUN dotnet build "./CleverConversion.csproj" -c $BUILD_CONFIGURATION -o /app/build 

# Publish stage (same) 
FROM build AS publish 
ARG BUILD_CONFIGURATION=Release 
RUN dotnet publish "./CleverConversion.csproj" -c $BUILD_CONFIGURATION -o /app/publish

# Final runtime 
FROM base AS final 
WORKDIR /app 

COPY --from=publish /app/publish .
EXPOSE 8080 
ENTRYPOINT ["dotnet", "CleverConversion.dll"]