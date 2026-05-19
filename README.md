# TransRail (.NET 10 + WinForms + Clean ligera + MVP)

## Documentacion completa

La documentacion academica y tecnica detallada del proyecto esta en:

- [`DOCUMENTACION_PROYECTO.md`](DOCUMENTACION_PROYECTO.md)

## Requisitos

- .NET SDK 10 (`dotnet --version`)

## Comandos

```powershell
dotnet restore TransRail.sln
dotnet build TransRail.sln
dotnet test TransRail.sln
dotnet run --project TransRail.Presentation
```

Si `dotnet build` falla por DLL bloqueada, cierra la app abierta y ejecuta:

```powershell
Get-Process -Name TransRail.Presentation -ErrorAction SilentlyContinue | Stop-Process -Force
```

## Credenciales de seed

- Admin:
  - correo: `admin@transrail.local`
  - contrasena: `admin123`
- Empleado:
  - correo: `empleado@transrail.local`
  - contrasena: `empleado123`
- Pasajero:
  - correo: `pasajero@transrail.local`
  - contrasena: `pasajero123`

## Estado actual

- Arquitectura por capas lista y ejecutable.
- Persistencia JSON por modulo.
- MVP funcional para:
  - Login
  - Menu Admin/Empleado/Pasajero
  - Gestion de Trenes
  - Gestion de Vagones
  - Gestion de Estaciones
  - Gestion de Rutas (calculo ruta minima con Dijkstra)
  - Gestion de Horarios (orden cronologico interno)
  - Gestion de Pasajeros
  - Gestion de Empleados
  - Gestion de Boletos (incluye calculo de precio)
  - Gestion de Abordaje (cola de prioridad)
  - Gestion de Equipaje (pila por vagon)
  - Registro de pagos JSON
- Formularios auxiliares (crear/buscar/eliminar/listar/modificar) redirigidos a modulos principales, sin placeholders.
- Estructura organizada por modulos:
  - `Forms/Auth|Menus|Operations/*`
  - `Presenters/*`
  - `Views/*`
- UseCases implementados en `TransRail.Application/UseCases/*` y enlazados en `AppServices`.
- Semilla de rutas con matriz A-K:
  - A-B 30, A-C 40, A-D 50, A-F 50
  - D-E 20, E-F 65, F-G 80, G-H 30
  - G-I 145, C-I 80, C-J 120, C-K 110

## Sobre codigo operativo

La UI usa codigos operativos visibles (`TR-001`, `VG-001`, `HOR-2026-001`, etc.) porque son identificadores de negocio que el usuario puede reconocer y escribir.

El `IdInterno` (GUID) sigue existiendo para persistencia interna, pero no se muestra en pantalla.
