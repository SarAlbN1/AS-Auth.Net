End-to-end tests (E2E)

This folder contains a minimal end-to-end test for the authentication flow.

run-login.sh
- Simple bash script that sends a POST to /api/auth/login on the configured base URL.
- Defaults:
  - BASE_URL=http://localhost:8081 (webapp-ui mapped by docker-compose)
  - E2E_USERNAME=admin
  - E2E_PASSWORD=Admin123$

Local run examples:

# using docker-compose (starts services and maps webapp to localhost:8081)
# make sure docker-compose is running the stack
BASE_URL=http://localhost:8081 ./tests/e2e/run-login.sh

# using Minikube: if services are exposed via NodePort or ingress, set BASE_URL accordingly
BASE_URL=http://<minikube-ip>:<nodeport> ./tests/e2e/run-login.sh

CI notes:
- The CI workflow `./.github/workflows/e2e-k8s.yml` (if present) runs the script after deploying manifests to the test cluster.
