# AS-Auth.Net

Servicio de autenticación construido con .NET 8 que valida credenciales contra una base de datos MySQL y expone un endpoint REST de inicio de sesión. El objetivo del repositorio es servir como base para la autenticación de otras aplicaciones ASP.NET Core y demostrar la orquestación tanto en contenedores Docker como en Kubernetes.

## Arquitectura

- **Auth.Api (Minimal API)**: Exposición HTTP de `/api/auth/login`, health checks (`/health/live` y `/health/ready`) y arranque de dependencias. No ejecuta redirección HTTPS en modo *Development* para simplificar las pruebas en contenedores.
- **Auth.Business**: Capa de negocio con entidades (`User`), contratos de entrada/salida y `AuthService`, encargado de validar credenciales y delegar en el repositorio.
- **Auth.Data**: Persistencia con Entity Framework Core y Pomelo MySQL. Define `AuthDbContext`, configuración fluida de la entidad `users`, repositorio `UserRepository`, hashing de contraseñas (BCrypt) y la migración inicial `20241113000119_InitialCreate` que crea la tabla y un usuario administrador seed.
- **WebApp.UI**: Aplicación MVC de ejemplo que puede consumir el servicio (actualmente sin integración directa de login).
- **Infraestructura**:
  - `docker/`: Dockerfile de la API y `docker-compose.yml` para levantar API + MySQL con health checks.
  - `k8s/`: Manifiestos para desplegar MySQL (StatefulSet), la API (Deployment + HPA) y secretos de conexión.

## Requisitos previos

- SDK de .NET 8 instalado localmente.
- Docker Desktop, si se desea ejecutar la solución en contenedores.
- `kubectl` y un clúster compatible, si se probará el despliegue en Kubernetes.

## Ejecución local (sin contenedores)

1. Configura la cadena de conexión en `src/Auth.Api/appsettings.Development.json` (por defecto apunta a `localhost:3306`).
2. Asegúrate de tener una instancia MySQL disponible con las credenciales configuradas.
3. Desde la raíz del repositorio ejecuta:

	```powershell
	dotnet build
	dotnet run --project src/Auth.Api/Auth.Api.csproj
	```
4. La API escuchará en `http://localhost:5186` (o el puerto asignado por el `launchSettings.json`).

### Usuario seed

Al primer arranque, el initializer crea un usuario administrador:

- **Usuario**: `admin`
- **Contraseña**: `Admin123$`

Se recomienda cambiar la contraseña tras las pruebas.

## Ejecución con Docker

1. Construye y levanta los servicios:

	```powershell
	docker compose -f docker/docker-compose.yml up --build -d
	```

2. Verifica el estado:

	```powershell
	docker compose -f docker/docker-compose.yml ps
	```

3. Health check de preparación:

	```powershell
	Invoke-RestMethod http://localhost:8080/health/ready
	```

4. Prueba de login:

	```powershell
	Invoke-RestMethod -Uri "http://localhost:8080/api/auth/login" -Method Post -ContentType "application/json" -Body '{"username":"admin","password":"Admin123$"}'
	```

5. Finaliza los contenedores cuando termines:

	```powershell
	docker compose -f docker/docker-compose.yml down
	```

## Despliegue en Kubernetes (resumen)

1. Crea los secretos necesarios (usuario/contraseña, cadenas de conexión) aplicando `k8s/auth-secrets.yaml` tras ajustar los valores.
2. Despliega MySQL y la API:
	```bash
	kubectl apply -f k8s/mysql-statefulset.yaml
	kubectl apply -f k8s/auth-api-deployment.yaml
	kubectl apply -f k8s/auth-api-hpa.yaml
	```
3. Expone el servicio según la estrategia elegida (LoadBalancer, Ingress, Port-forward).
4. Ejecuta los mismos health checks y pruebas de login apuntando a la dirección publicada.

## Endpoints principales

| Método | Ruta                 | Descripción                                   |
|--------|----------------------|-----------------------------------------------|
| POST   | `/api/auth/login`    | Valida usuario y contraseña, devuelve `isValid` y mensaje. |
| GET    | `/health/live`       | Health check simple (siempre responde 200).   |
| GET    | `/health/ready`      | Verifica conectividad a la base de datos.     |

## Pruebas

Actualmente no hay pruebas automatizadas. El comando `dotnet test` se incluye para comprobar que la solución compila y permitir futuras suites.

## Próximos pasos sugeridos

- Añadir pruebas unitarias para `AuthService` y el repositorio.
- Implementar endpoints para registro/gestión de usuarios.
- Integrar la WebApp.UI con el microservicio.
- Automatizar despliegues y migraciones en el pipeline CI/CD.