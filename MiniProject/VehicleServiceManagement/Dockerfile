FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
 
WORKDIR /src
 
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
 
COPY *.csproj ./
RUN dotnet restore
 
COPY . ./
RUN dotnet publish -c Release -o /app/publish
 
 
FROM mcr.microsoft.com/dotnet/sdk:9.0
 
WORKDIR /app
 
RUN apt-get update && apt-get install -y postgresql-client
 
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
 
COPY --from=build /app/publish .
 
COPY . .
 
COPY entrypoint.sh .
RUN chmod +x ./entrypoint.sh
 
RUN mkdir -p /app/BookingImages 
 
ENV ASPNETCORE_ENVIRONMENT=Development
 
EXPOSE 80
 
ENTRYPOINT ["./entrypoint.sh"]