FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY RentBikeApi/RentBikeApi.csproj RentBikeApi/
COPY RentBikeApi.Domain/RentBikeApi.Domain.csproj RentBikeApi.Domain/
COPY RentBikeApi.Infrastructure/RentBikeApi.Infrastructure.csproj RentBikeApi.Infrastructure/
RUN dotnet restore RentBikeApi/RentBikeApi.csproj

COPY . .
RUN dotnet publish RentBikeApi/RentBikeApi.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

RUN mkdir -p /app/storage/cnh /app/logs && chown -R $APP_UID /app/storage /app/logs
USER $APP_UID

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "RentBikeApi.dll"]