# Arquitectura del sistema de autenticación (AS-Auth.Net)

## 1. Vista general

El sistema implementa una arquitectura de **3 capas (3-tier)**:

1. **Presentación (Front):** `WebApp.UI` (ASP.NET Core MVC).
2. **Lógica de negocio (Back):** `Auth.Api` (ASP.NET Core Web API).
3. **Datos:** `SQL Server` desplegado en Kubernetes.

TODO: agregar diagrama y explicación del flujo de login.

## 2. Flujo de login (alto nivel)

1. Usuario ingresa usuario y contraseña en `WebApp.UI`.
2. `WebApp.UI` envía una petición HTTP `POST /api/auth/login` a `Auth.Api`.
3. `Auth.Api` consulta la base de datos y valida las credenciales.
4. `Auth.Api` responde `login OK` o `login NO OK`.
5. `WebApp.UI` muestra el mensaje correspondiente en la interfaz.

TODO: detallar endpoints, modelos y respuestas.
