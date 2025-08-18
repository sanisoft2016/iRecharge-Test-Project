# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy solution and all project files explicitly
COPY CustRewardMgtSys.sln ./
COPY CustRewardMgtSys.API/*.csproj CustRewardMgtSys.API/
COPY CustRewardMgtSys.API/*.csproj CustRewardMgtSys.API/
COPY CustRewardMgtSys.API/*.csproj CustRewardMgtSys.API/
COPY CustRewardMgtSys.API/*.csproj CustRewardMgtSys.API/
COPY CustRewardMgtSys.API/*.csproj CustRewardMgtSys.API/
COPY CustRewardMgtSys.API/*.csproj CustRewardMgtSys.API/

# Copy the full source
COPY . .

# Restore and publish
RUN dotnet restore
RUN dotnet publish CustRewardMgtSys.API/CustRewardMgtSys.API.csproj -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CustRewardMgtSys.API.dll"]


