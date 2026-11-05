# Entregable — Web API REST Semana 12

## Tema y problema

**Veterinaria API** centraliza el registro de mascotas y la agenda de atención de una clínica veterinaria. Evita duplicar identificadores de chip, programar a una mascota dos veces en el mismo horario y borrar el historial de citas por error.

## Modelo de datos

- **Mascota**: `MascotaId`, nombre, especie, edad y código de chip.
- **Cita**: `CitaId`, fecha/hora, motivo, veterinario y `MascotaId`.
- Relación **1:N**: una mascota puede tener varias citas; una cita pertenece a una mascota.

El script [database/Veterinaria.sql](database/Veterinaria.sql) crea las tablas, restricciones, claves y datos de ejemplo. El acceso desde controladores se hace directamente con `VeterinariaContext` y LINQ async.

## Reglas de negocio

1. El código de chip de la mascota es único (`409 Conflict`).
2. No se elimina una mascota que posea citas (`409 Conflict`).
3. Una mascota no puede tener dos citas en la misma fecha y hora (`409 Conflict`).

Las anotaciones `[Required]`, `[StringLength]` y `[Range]` activan la respuesta automática `400 Bad Request` de `[ApiController]`. La navegación `Cita.Mascota` es nullable y `Mascota.Citas` tiene `[JsonIgnore]`, por lo que un POST de cita solo requiere `mascotaId` y no se genera un ciclo JSON.

## Endpoints y pruebas

| Método | Ruta | Resultado principal |
|---|---|---|
| GET | `/api/mascotas?especie=Perro` | 200 |
| GET | `/api/mascotas/{id}` | 200 / 404 |
| GET | `/api/mascotas/{id}/citas` | 200 / 404 |
| POST | `/api/mascotas` | 201 / 400 / 409 |
| PUT | `/api/mascotas/{id}` | 204 / 400 / 404 / 409 |
| DELETE | `/api/mascotas/{id}` | 204 / 404 / 409 |
| GET | `/api/citas?desde=2026-11-01` | 200 |
| GET | `/api/citas/{id}` | 200 / 404 |
| POST | `/api/citas` | 201 / 400 / 409 |
| PUT | `/api/citas/{id}` | 204 / 400 / 404 / 409 |
| DELETE | `/api/citas/{id}` | 204 / 404 |

El archivo [VeterinariaApi.http](VeterinariaApi.http) contiene solicitudes listas para documentar cada código solicitado (200, 201, 204, 400, 404 y 409) una vez definida la URL de la API publicada.

## Despliegue y seguridad

Proveedor de API: **pendiente de configurar con una cuenta del estudiante**. Base de datos: **SQL Server administrado**, compatible con `Microsoft.EntityFrameworkCore.SqlServer`. La conexión se debe guardar exclusivamente en la variable de entorno `ConnectionStrings__Veterinaria`; no se incluye en este repositorio.

- Repositorio: pendiente de creación.
- Swagger publicado: pendiente de despliegue; usar `https://<host>/swagger`.

## Conclusiones

La API demuestra una arquitectura REST basada en controladores, EF Core y LINQ asíncrono. Las validaciones protegen la calidad de datos y las reglas de negocio devuelven códigos HTTP consistentes. El despliegue debe incluir tanto la aplicación como SQL Server en la nube y validar todos los endpoints contra la URL publicada antes de entregar capturas.
