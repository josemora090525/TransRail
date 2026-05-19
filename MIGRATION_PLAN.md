# Plan de Migracion desde rama `Prueba` a `TransRail` (.NET 10)

## 1) Arbol final de proyectos y carpetas

```txt
TransRail
├── TransRail.sln
├── ARCHITECTURE.md
├── MIGRATION_PLAN.md
├── TransRail.Presentation
│   ├── Forms
│   ├── Presenters
│   ├── Views
│   ├── Controls
│   ├── Theme
│   └── Resources
├── TransRail.Application
│   ├── Services
│   ├── DTOs
│   ├── Interfaces
│   ├── UseCases
│   └── Validators
├── TransRail.Domain
│   ├── Entities
│   ├── ValueObjects
│   ├── Enums
│   ├── Structures
│   └── Rules
├── TransRail.Infrastructure
│   ├── Persistence
│   │   └── Json
│   ├── Repositories
│   ├── Seed
│   └── Config
└── TransRail.Tests
    ├── Domain
    ├── Application
    └── Structures
```

## 2) Conservar, mover, renombrar o eliminar

### Conservar (como referencia funcional)
- Conceptos de modulos de `Prueba`:
  - trenes, rutas, estaciones, horarios, usuarios.
- Reglas academicas de estructuras de datos.

### Mover (migrar progresivamente)
- Logica de negocio que hoy vive en formularios de `ProyectoEstructuras` hacia:
  - `TransRail.Application/Services`
  - `TransRail.Domain/Rules`

### Renombrar / unificar
- Namespaces legacy:
  - `Proyecto*`, `ProyectoEstructuras*`, `Modulo*`
- Namespace objetivo unico:
  - `TransRail.*`

### Eliminar de version activa
- Codigo Java legado en `src/` y binarios Java asociados.
- Formularios duplicados legacy no migrados a MVP.

## 3) Entidades principales y propiedades

- `Usuario` (base):
  - `IdInterno`, `CodigoUsuario`, `NombreCompleto`, `NumeroDocumento`, `Correo`, `Contrasena`, `Rol`
- `Administrador`, `Empleado`, `Pasajero`
- `Tren`:
  - `CodigoTren`, `NumeroOperativo`, `Nombre`, `CapacidadVagones`, `Kilometraje`, `EnCirculacion`
- `Vagon`:
  - `CodigoVagon`, `CodigoTren`, `Tipo`, `Capacidad`, `PesoMaximoKg`
- `Ruta`:
  - `CodigoRuta`, `CodigoEstacionOrigen`, `CodigoEstacionDestino`, `DistanciaKm`, `Activa`
- `Estacion`:
  - `CodigoEstacion`, `Nombre`, `Ciudad`
- `Horario`:
  - `CodigoHorario`, `CodigoTren`, `CodigoRuta`, `Fecha`, `HoraSalida`, `HoraLlegada`
- `Boleto`:
  - `CodigoBoleto`, `CodigoPasajero`, `CodigoHorario`, `TipoBoleto`, `Precio`, `FechaCompraUtc`
- `Equipaje`, `Pago`

## 4) Interfaces de repositorio

- `IUsuarioRepository`, `IPasajeroRepository`, `IEmpleadoRepository`
- `ITrenRepository`, `IVagonRepository`
- `IRutaRepository`, `IEstacionRepository`, `IHorarioRepository`
- `IBoletoRepository`
- `IJsonStorage`

## 5) Estructura de datos por modulo

- Usuarios/autenticacion:
  - `TablaHash<TKey, TValue>` conceptual, repositorio JSON como almacenamiento.
- Trenes en circulacion:
  - `ListaCircular<Tren>`
- Boletos:
  - `ListaDoblementeEnlazada<Boleto>`
- Rutas/estaciones:
  - `Grafo<string>` + Dijkstra
- Vagones/equipaje:
  - `Pila<T>`
- Abordaje:
  - `ColaPrioridad<Pasajero>`
- Horarios:
  - `ArbolAvl<Horario>` con orden por `Fecha + HoraSalida + CodigoTren`

## 6) Persistencia JSON por modulo

En runtime se usa carpeta `data` en salida de Presentation:

- `usuarios.json`
- `pasajeros.json`
- `empleados.json`
- `trenes.json`
- `vagones.json`
- `rutas.json`
- `estaciones.json`
- `horarios.json`
- `boletos.json`

## 7) Implementacion de horarios con AVL

- Clave compuesta: `HorarioKey(Fecha, HoraSalida, CodigoTren)`.
- `Horario` implementa `IComparable<Horario>` usando `HorarioKey`.
- `HorarioService` mantiene indice `ArbolAvl<Horario>`.
- Busquedas por tren:
  - filtro sobre recorrido ordenado del AVL.

## 8) UI/UX TransRail en WinForms

- Tema global: `TransRailTheme`.
- Presentacion por MVP:
  - Forms: capturan y muestran.
  - Presenters: coordinan.
  - Services: ejecutan logica.
- Regla:
  - no exponer IDs internos.
  - usar codigos operativos (`TR-001`, `RUT-001`, `HOR-...`).

## 9) Estado de fases (actualizado)

- Fase 1: completada.
  - arquitectura por capas, estructuras academicas y modulos base.
- Fase 2: completada.
  - CRUD restantes en UI, login por rol pasajero, abordaje, equipaje, pagos JSON.
- Fase 3: completada.
  - reorganizacion modular de `Forms`, `Presenters`, `Views` y UseCases implementados en `Application/UseCases`.

Pendiente opcional (mejora futura):
- seguridad avanzada (hash de contrasenas) y migracion a DB relacional.
