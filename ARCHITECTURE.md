# TransRail - Arquitectura (estado actual)

## Solucion activa

- `TransRail.sln`
- `TransRail.Presentation` (WinForms + MVP)
- `TransRail.Application` (Services + UseCases + Interfaces)
- `TransRail.Domain` (entidades, reglas y estructuras academicas)
- `TransRail.Infrastructure` (persistencia JSON y repositorios)
- `TransRail.Tests` (xUnit)

## Regla de dependencias

- Presentation -> Application, Domain, Infrastructure
- Application -> Domain
- Infrastructure -> Application, Domain
- Domain -> (sin dependencias de UI o Infrastructure)

## Estructuras de datos implementadas

- `TablaHash<TKey, TValue>`: busqueda rapida por clave.
- `ListaCircular<T>`: trenes en circulacion.
- `ListaDoblementeEnlazada<T>`: historial de boletos.
- `Grafo<TNodo>`: rutas y estaciones + Dijkstra.
- `Pila<T>`: vagones/equipaje en orden LIFO.
- `ColaPrioridad<T>`: abordaje por prioridad.
- `ArbolAvl<T>`: horarios ordenados por clave cronologica compuesta.

## Persistencia JSON

Archivos de runtime en `TransRail.Presentation/bin/.../data`:

- `usuarios.json`
- `pasajeros.json`
- `empleados.json`
- `trenes.json`
- `vagones.json`
- `rutas.json`
- `estaciones.json`
- `horarios.json`
- `boletos.json`
- `equipajes.json`
- `pagos.json`

## Capas de aplicacion

- `Services`: logica de aplicacion reusable por UI.
- `UseCases`: orquestacion por escenario funcional:
  - autenticacion (`LoginUseCase`)
  - operacion ferroviaria (`ManageTrain/Route/Station/Schedule/WagonUseCase`)
  - personas (`ManagePassenger/EmployeeUseCase`)
  - venta y pago (`TicketPurchaseUseCase`, `RegisterPaymentUseCase`)
  - abordaje y equipaje (`ManageBoardingQueueUseCase`, `LuggageOperationsUseCase`)

## Organizacion Presentation

- `Forms/Auth`, `Forms/Menus`, `Forms/Operations/*`
- `Presenters/*` por modulo
- `Views/*` por modulo

## Estado de cierre de fases

1. CRUD principal de modulos implementado.
2. Abordaje de pasajeros implementado con cola de prioridad.
3. Equipaje implementado con persistencia JSON y pila por vagon.
4. Formularios auxiliares completados sin placeholders.
5. Reorganizacion modular de Forms/Views/Presenters aplicada.

## Mejoras opcionales futuras

1. Hash de contrasenas y hardening de seguridad.
2. Migrar persistencia a SQLite/PostgreSQL manteniendo interfaces de Application.
