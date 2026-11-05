# Veterinaria API

API REST para registrar mascotas y sus citas veterinarias. Una mascota puede tener muchas citas y cada cita pertenece a una mascota.

## Reglas de negocio

- El `CodigoChip` es único por mascota.
- No se elimina una mascota que tenga citas.
- Una mascota no puede tener dos citas a la misma fecha y hora.

Las reglas devuelven **409 Conflict**. Las anotaciones de datos devuelven **400 Bad Request** automáticamente gracias a `[ApiController]`.

## Base de datos primero

Ejecute `database/Veterinaria.sql` en PostgreSQL. Para regenerar el contexto y entidades mediante Scaffold:

```powershell
dotnet ef dbcontext scaffold "<cadena-de-conexion>" Npgsql.EntityFrameworkCore.PostgreSQL --context VeterinariaContext --context-dir Data --output-dir Models --force
```

En la nube, configure `ConnectionStrings__Veterinaria` como variable de entorno; no suba la cadena real al repositorio.

## Ejecutar

```powershell
dotnet run
```

Abra `/swagger`. Endpoints: `api/mascotas`, `api/mascotas/{id}`, `api/mascotas/{id}/citas`, `api/citas`, `api/citas/{id}`. Filtros: `api/mascotas?especie=Perro` y `api/citas?desde=2026-11-01`.

El CORS permite explícitamente `https://veterinaria-web.example.com`; reemplácelo por el dominio real del frontend antes del despliegue. Para Render, el proyecto incluye un `Dockerfile` y `render.yaml`.
