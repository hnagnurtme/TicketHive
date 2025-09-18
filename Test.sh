#!/bin/bash

# 1️⃣ Register user (ignore jq parse)
echo "=== Registering user ==="
curl -s -X POST http://localhost:5180/api/auth/register \
-H "Content-Type: application/json" \
-d '{
  "email": "anh@example.com",
  "password": "Password123!",
  "fullName": "Nguyen An",
  "phoneNumber": "+84901234567"
}' || true

echo -e "\n"

# 2️⃣ Login user & get token
echo "=== Logging in user ==="
TOKEN=$(curl -s -X POST http://localhost:5180/api/auth/login \
-H "Content-Type: application/json" \
-d '{
  "email": "anh@example.com",
  "password": "Password123!"
}' | jq -r '.token')

echo "Token: $TOKEN"
echo -e "\n"

# 3️⃣ Call protected endpoint
echo "=== Calling protected endpoint ==="
curl -s -X GET http://localhost:5180/api/user/profile \
-H "Authorization: Bearer $TOKEN" | jq .
