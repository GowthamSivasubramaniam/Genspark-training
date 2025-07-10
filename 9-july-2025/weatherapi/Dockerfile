FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
 
WORKDIR /app


COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:5200
EXPOSE 5200
ENTRYPOINT ["dotnet", "weatherapi.dll"]