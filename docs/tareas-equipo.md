# Tareas del equipo



| Módulo / Entregable                       | Actividad principal                                       | Responsable     | Evidencia (commit / PR)        | Estado | Notas |
|------------------------------------------|-----------------------------------------------------------|-----------------|--------------------------------|--------|-------|
| Frontend WebApp.UI                       | Vista y controlador de Login, consumo de Auth.Api        | Sara            | `feat/webapp-ui-sara`          | ✔️ Implementado | Frontend completo y funcionando; assets y controlador presentes |
| Docker front                             | webapp-ui.Dockerfile                                      | Sara            | `feat/webapp-ui-sara`          | ✔️ Implementado | Dockerfile en `docker/` |
| K8s front                                | webapp-deployment.yaml, webapp-service.yaml              | Sara            | `feat/webapp-ui-sara`          | ✔️ Implementado | Manifests para frontend en `k8s/` |
| Backend Auth.Api                         | Modelo User, DbContext, AuthController /api/auth/login   | Alejandro       | `feat/auth-api-alejo`          | ✔️ Implementado | Backend en la rama de Alejandro (intencional) |
| Docker back                              | auth-api.Dockerfile                                       | Alejandro       | `feat/auth-api-alejo`          | ✔️ Implementado | Dockerfile backend en `docker/` (nota: había duplicado en otra rama) |
| K8s back + BD                            | authapi-*.yaml, sqldb-*.yaml                              | Alejandro       | `feat/auth-api-alejo`          | ✔️ Implementado | Manifests de auth-api y MySQL están en la rama de backend |
| Definición de arquitectura 3-tier        | Documento arquitectura.md y diagrama                     | Sara            | `docs/arquitectura.md`         | ✔️ Documento presente | `docs/arquitectura.md` existe; revisar si centralizar en `main` |
| Pruebas integradas en Kubernetes         | Pruebas de login end-to-end                               | Sara & Alejandro| tests/e2e + workflow (feat/webapp-ui-sara) | ❗ Pendiente (en feat/webapp-ui-sara) | Añadí tests mínimos y workflow en `feat/webapp-ui-sara`; mover a `develop` o crear PR |
