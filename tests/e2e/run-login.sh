#!/usr/bin/env bash
set -euo pipefail

# Simple end-to-end login test script.
# Usage: BASE_URL=http://host:port ./run-login.sh
# By default it will try http://localhost:8081 (webapp-ui mapped by docker-compose)

BASE_URL=${BASE_URL:-http://localhost:8081}
# Login page (form) and API endpoint
LOGIN_PAGE="$BASE_URL/Account/Login"
API_ENDPOINT="$BASE_URL/api/auth/login"

USERNAME=${E2E_USERNAME:-admin}
PASSWORD=${E2E_PASSWORD:-'Admin123$'}

echo "Running E2E login test against $BASE_URL"

# Try direct API login first (faster, reliable for CI)
echo "-> POST $API_ENDPOINT"
HTTP_STATUS=$(curl -s -o /tmp/e2e_response.json -w "%{http_code}" -X POST \
  -H "Content-Type: application/json" \
  -d "{\"Username\": \"${USERNAME}\", \"Password\": \"${PASSWORD}\"}" \
  "$API_ENDPOINT")

echo "HTTP status: $HTTP_STATUS"
if [ "$HTTP_STATUS" -ne 200 ]; then
  echo "API login failed (status $HTTP_STATUS). Response:"
  cat /tmp/e2e_response.json
  exit 2
fi

# Check JSON for success field (LoginResponse: { Success, Message } )
if command -v jq >/dev/null 2>&1; then
  SUCCESS=$(jq -r '.Success // .success // false' /tmp/e2e_response.json)
  echo "Parsed Success: $SUCCESS"
  if [ "$SUCCESS" != "true" ]; then
    echo "Login result indicates failure. Full response:" && cat /tmp/e2e_response.json
    exit 3
  fi
else
  echo "jq not available: doing basic check for 'Success' string in response"
  if ! grep -q "Success" /tmp/e2e_response.json; then
    echo "Response does not contain expected Success field:" && cat /tmp/e2e_response.json
    exit 4
  fi
fi

echo "E2E login test: OK"
rm -f /tmp/e2e_response.json
exit 0
