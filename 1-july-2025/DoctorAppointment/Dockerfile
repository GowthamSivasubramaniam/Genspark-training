# Use .NET 9 SDK base image (preview)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy and build the app
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish
# Use runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose the default port
EXPOSE 8080

ENTRYPOINT ["dotnet", "DoctorAppointment.dll"]
