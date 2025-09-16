FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy solution and project files for restore caching
COPY TicketHive.sln .
COPY src/TicketHive.Api/TicketHive.Api.csproj src/TicketHive.Api/
COPY src/TicketHive.Application/TicketHive.Application.csproj src/TicketHive.Application/
COPY src/TicketHive.Domain/TicketHive.Domain.csproj src/TicketHive.Domain/
COPY src/TicketHive.Infrastructure/TicketHive.Infrastructure.csproj src/TicketHive.Infrastructure/
COPY tests/TicketHive.Tests/TicketHive.Tests.csproj tests/TicketHive.Tests/
RUN dotnet restore TicketHive.sln

# Copy the rest of the source and publish the API
COPY . .
WORKDIR /app/src/TicketHive.Api
# Restore again after full source copy to ensure consistency
RUN dotnet restore
RUN dotnet publish -c Release -o /app/out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
ENV ASPNETCORE_URLS=http://+:8080 \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "TicketHive.Api.dll"]
