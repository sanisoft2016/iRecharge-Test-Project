# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy the solution file and restore dependencies
COPY *.sln .
COPY iRechargeTestProject.API/iRechargeTestProject.API.csproj iRechargeTestProject.API/
COPY iRechargeTestProject.Application/iRechargeTestProject.Application.csproj iRechargeTestProject.Application/
COPY iRechargeTestProject.Domain/iRechargeTestProject.Domain.csproj iRechargeTestProject.Domain/
COPY iRechargeTestProject.Infrastructure/iRechargeTestProject.Infrastructure.csproj iRechargeTestProject.Infrastructure/
RUN dotnet restore

# Copy the entire source code and build the application
COPY . .
RUN dotnet publish iRechargeTestProject.API/iRechargeTestProject.API.csproj -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "iRechargeTestProject.API.dll"]
