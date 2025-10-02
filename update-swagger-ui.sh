#!/bin/bash

# Update Swagger UI script for TicketHive
# This script downloads the latest Swagger UI files

echo "🔄 Updating Swagger UI files..."

SWAGGER_UI_VERSION="5.9.0"
DOCS_DIR="/Users/anhnon/CODE/TicketHive/docs/swagger-ui"

# Create directory if it doesn't exist
mkdir -p "$DOCS_DIR"

# Download Swagger UI files
echo "📥 Downloading Swagger UI v$SWAGGER_UI_VERSION..."

curl -o "$DOCS_DIR/swagger-ui-bundle.js" "https://unpkg.com/swagger-ui-dist@$SWAGGER_UI_VERSION/swagger-ui-bundle.js"
curl -o "$DOCS_DIR/swagger-ui.css" "https://unpkg.com/swagger-ui-dist@$SWAGGER_UI_VERSION/swagger-ui.css"
curl -o "$DOCS_DIR/swagger-ui-standalone-preset.js" "https://unpkg.com/swagger-ui-dist@$SWAGGER_UI_VERSION/swagger-ui-standalone-preset.js"
curl -o "$DOCS_DIR/favicon-32x32.png" "https://unpkg.com/swagger-ui-dist@$SWAGGER_UI_VERSION/favicon-32x32.png"
curl -o "$DOCS_DIR/favicon-16x16.png" "https://unpkg.com/swagger-ui-dist@$SWAGGER_UI_VERSION/favicon-16x16.png"

echo "✅ Swagger UI files updated successfully!"
echo "📁 Files saved to: $DOCS_DIR"
echo "🌐 GitHub Pages URL: https://hnagnurtme.github.io/TicketHive/swagger-ui/"

# Make the script executable
chmod +x "$0"