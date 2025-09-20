#!/bin/bash

# ===============================================
# RunProject.sh
# Build and run the TicketHive.Api project
# ===============================================

# Build the project
echo "Building TicketHive.Api..."
dotnet build src/TicketHive.Api/TicketHive.Api.csproj

# Check if build succeeded
if [ $? -ne 0 ]; then
  echo "Build failed. Exiting..."
  exit 1
fi

# Run the project with dotnet watch
echo "Running TicketHive.Api with dotnet watch..."
dotnet run --project src/TicketHive.Api/TicketHive.Api.csproj run