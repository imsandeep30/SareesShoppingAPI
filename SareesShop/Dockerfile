# -------- RUNTIME --------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# -------- BUILD --------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files
COPY *.csproj ./
COPY ../OnlineMedicineBookingApplication.Application/*.csproj ../OnlineMedicineBookingApplication.Application/
COPY ../OnlineMedicineBookingApplication.Domain/*.csproj ../OnlineMedicineBookingApplication.Domain/
COPY ../OnlineMedicineBookingApplication.Infrastructure/*.csproj ../OnlineMedicineBookingApplication.Infrastructure/

RUN dotnet restore

# Copy everything
COPY . .
RUN dotnet publish -c Release -o /app/publish

# -------- FINAL --------
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SareesShop.dll"]