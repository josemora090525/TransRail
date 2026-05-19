# TransRail - Sistema de Gestión de Trenes de Transporte de Pasajeros

## 1. Descripción general

**TransRail** es un proyecto académico desarrollado como una aplicación de escritorio para la gestión de trenes de transporte de pasajeros. El sistema busca representar, de forma práctica, una situación problema relacionada con la administración de operaciones ferroviarias: gestión de usuarios, trenes, vagones, rutas, estaciones, horarios, boletos, equipaje, pagos y abordaje de pasajeros.

El proyecto aplica estructuras de datos propias como parte central de la solución. No se limita a usar listas genéricas del lenguaje, sino que incorpora estructuras académicas implementadas en C# para resolver problemas concretos del dominio.

La situación problema que guía el sistema es:

> ¿Cómo puede un sistema de gestión de trenes de transporte de pasajeros asegurar el control de las cargas y descargas de los trenes de transporte, a su vez que apoya las labores administrativas?

Desde esa necesidad, TransRail organiza las operaciones principales de una empresa ferroviaria de pasajeros:

- Inicio de sesión por roles.
- Gestión administrativa de trenes, vagones, estaciones, rutas y horarios.
- Gestión de pasajeros y empleados.
- Compra y validación de boletos.
- Registro de equipaje.
- Control de abordaje por prioridad.
- Cálculo de rutas y distancias entre estaciones.
- Persistencia inicial en archivos JSON.

## 2. Tipo de proyecto

TransRail es una **aplicación de escritorio** construida con **C# y WinForms sobre .NET 10**.

No es una aplicación web, no usa React, Angular ni servicios externos. La solución está pensada para ejecutarse localmente, con interfaz gráfica de escritorio y almacenamiento en archivos JSON.

El enfoque del proyecto es académico, por lo que su objetivo principal es demostrar cómo se pueden aplicar estructuras de datos en una situación realista de gestión ferroviaria.

## 3. Tecnologías usadas

| Tecnología | Uso dentro del proyecto |
|---|---|
| C# | Lenguaje principal del sistema |
| .NET 10 | Plataforma de ejecución |
| WinForms | Interfaz gráfica de escritorio |
| xUnit | Pruebas unitarias |
| JSON | Persistencia inicial de datos |
| System.Text.Json | Lectura y escritura de archivos JSON |
| MVP | Patrón de presentación para separar formularios y lógica |
| Clean Architecture ligera | Organización por capas |
| SOLID | Principios de diseño aplicados en servicios, interfaces y repositorios |

## 4. Objetivo del sistema

El objetivo de TransRail es ofrecer una solución funcional para administrar un sistema de transporte ferroviario de pasajeros, manteniendo una arquitectura clara y defendible académicamente.

El sistema permite:

- Administrar trenes y vagones.
- Crear estaciones y rutas entre ciudades.
- Calcular recorridos y distancias usando grafos.
- Gestionar horarios de viaje.
- Registrar pasajeros y empleados.
- Vender boletos.
- Registrar pagos.
- Controlar equipaje.
- Gestionar abordaje de pasajeros según prioridad.
- Persistir la información en JSON.

## 5. Arquitectura general

La solución activa está ubicada en:

```txt
TransRail/
```

La solución principal es:

```txt
TransRail/TransRail.sln
```

El proyecto está organizado con una **Clean Architecture ligera**, separando responsabilidades en cinco proyectos:

```txt
TransRail
├── TransRail.Domain
├── TransRail.Application
├── TransRail.Infrastructure
├── TransRail.Presentation
└── TransRail.Tests
```

### 5.1 Regla de dependencias

La regla principal es que el dominio no depende de la interfaz ni de la persistencia.

```txt
Presentation -> Application, Infrastructure, Domain
Infrastructure -> Application, Domain
Application -> Domain
Domain -> sin dependencias externas del sistema
```

Esto permite que la lógica principal del negocio se mantenga separada de WinForms y de JSON.

## 6. Capas del proyecto

### 6.1 TransRail.Domain

Contiene el corazón del negocio. Aquí viven las entidades, enumeraciones, reglas y estructuras de datos.

Ubicación:

```txt
TransRail.Domain/
```

Carpetas principales:

```txt
Entities/
Enums/
Rules/
Structures/
```

Entidades principales:

- `Usuario`
- `Administrador`
- `Empleado`
- `Pasajero`
- `Tren`
- `Vagon`
- `Ruta`
- `Estacion`
- `Horario`
- `Boleto`
- `Equipaje`
- `Pago`

Regla importante:

El proyecto `Domain` no debe conocer WinForms, JSON ni repositorios concretos. Su función es representar el negocio y las estructuras académicas.

### 6.2 TransRail.Application

Contiene los servicios, interfaces, DTOs y casos de uso. Esta capa coordina las operaciones del sistema.

Ubicación:

```txt
TransRail.Application/
```

Carpetas principales:

```txt
DTOs/
Interfaces/
Services/
UseCases/
```

Servicios principales:

- `AuthService`
- `UsuarioService`
- `PasajeroService`
- `EmpleadoService`
- `TrenService`
- `VagonService`
- `RutaService`
- `EstacionService`
- `HorarioService`
- `BoletoService`
- `EquipajeService`
- `PagoService`
- `AbordajeService`

Casos de uso principales:

- `LoginUseCase`
- `ManageTrainUseCase`
- `ManageWagonUseCase`
- `ManageRouteUseCase`
- `ManageStationUseCase`
- `ManageScheduleUseCase`
- `ManagePassengerUseCase`
- `ManageEmployeeUseCase`
- `TicketPurchaseUseCase`
- `RegisterPaymentUseCase`
- `ManageBoardingQueueUseCase`
- `LuggageOperationsUseCase`
- `PassengerPortalUseCase`

Las interfaces de repositorio también se definen aquí para mantener desacoplada la persistencia:

- `IUsuarioRepository`
- `IPasajeroRepository`
- `IEmpleadoRepository`
- `ITrenRepository`
- `IVagonRepository`
- `IRutaRepository`
- `IEstacionRepository`
- `IHorarioRepository`
- `IBoletoRepository`
- `IEquipajeRepository`
- `IPagoRepository`
- `IJsonStorage`

### 6.3 TransRail.Infrastructure

Contiene la persistencia concreta del sistema. Actualmente usa archivos JSON.

Ubicación:

```txt
TransRail.Infrastructure/
```

Carpetas principales:

```txt
Persistence/Json/
Repositories/
Seed/
```

Archivos JSON principales:

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

Repositorios JSON:

- `JsonUsuarioRepository`
- `JsonPasajeroRepository`
- `JsonEmpleadoRepository`
- `JsonTrenRepository`
- `JsonVagonRepository`
- `JsonRutaRepository`
- `JsonEstacionRepository`
- `JsonHorarioRepository`
- `JsonBoletoRepository`
- `JsonEquipajeRepository`
- `JsonPagoRepository`

La clase `DataSeeder` carga datos iniciales para poder probar el sistema desde el primer arranque.

### 6.4 TransRail.Presentation

Contiene la interfaz gráfica de escritorio construida con WinForms.

Ubicación:

```txt
TransRail.Presentation/
```

Carpetas principales:

```txt
Forms/
Presenters/
Views/
Controls/
Theme/
Resources/
```

Organización de formularios:

```txt
Forms/
├── Auth/
├── Menus/
├── Operations/
│   ├── Boarding/
│   ├── Employees/
│   ├── Luggage/
│   ├── Passengers/
│   ├── Routes/
│   ├── Schedules/
│   ├── Stations/
│   ├── Tickets/
│   ├── Trains/
│   └── Wagons/
└── PassengerPortal/
```

Esta capa usa el patrón **MVP**:

- `Forms`: muestran datos, capturan entradas y eventos.
- `Views`: definen contratos de pantalla.
- `Presenters`: coordinan la comunicación entre pantalla y casos de uso.
- `Services` y `UseCases`: ejecutan la lógica de aplicación.

### 6.5 TransRail.Tests

Contiene pruebas unitarias con xUnit.

Ubicación:

```txt
TransRail.Tests/
```

Pruebas implementadas:

- Estructuras de datos.
- Reglas de dominio.
- Persistencia JSON.
- Servicios críticos.

Ejemplos:

- `TablaHashTests`
- `GrafoTests`
- `ArbolAvlTests`
- `HorarioKeyTests`
- `ValidadorBoletoTests`
- `ValidadorAbordajeTests`
- `JsonPasajeroRepositoryTests`

## 7. Patrones aplicados

### 7.1 Clean Architecture ligera

Se usa para separar:

- Dominio.
- Casos de uso.
- Persistencia.
- Presentación.

Esto mejora la mantenibilidad y permite cambiar la persistencia en el futuro sin reescribir la lógica principal.

### 7.2 MVP en WinForms

WinForms funciona con eventos, por eso se usa MVP en lugar de MVC puro.

Flujo típico:

```txt
Formulario
↓
Presenter
↓
UseCase o Service
↓
Repositorio
↓
JSON
```

Ejemplo para crear un tren:

```txt
TrainManagementForm
↓
TrainPresenter
↓
ManageTrainUseCase / TrenService
↓
ITrenRepository
↓
JsonTrenRepository
↓
trenes.json
```

Ejemplo para compra de pasajero:

```txt
PassengerRoutesForm
↓
PassengerRoutesPresenter
↓
PassengerPortalUseCase
↓
RutaService / HorarioService / BoletoService
↓
Repositorios JSON
```

### 7.3 Repository Pattern

Los repositorios ocultan cómo se almacenan los datos. Actualmente se guardan en JSON, pero las interfaces permiten migrar más adelante a SQLite, MySQL o PostgreSQL.

### 7.4 DTOs

Los DTOs se usan para transportar información entre capas sin exponer directamente toda la entidad o detalles internos.

## 8. Estructuras de datos aplicadas

El proyecto usa estructuras de datos propias dentro de `TransRail.Domain/Structures`.

### 8.1 Tabla hash

Clase:

```txt
TablaHash<TKey, TValue>
```

Uso dentro del sistema:

- Búsqueda rápida de usuarios.
- Apoyo conceptual para autenticación.
- Búsqueda por código operativo.

Justificación:

La tabla hash permite consultar información por clave con alta eficiencia promedio.

### 8.2 Lista circular

Clase:

```txt
ListaCircular<T>
```

Uso dentro del sistema:

- Gestión de trenes en circulación.
- Recorrido cíclico de trenes activos.

Justificación:

En un sistema ferroviario, los trenes pueden operar de forma continua en ciclos de circulación.

### 8.3 Lista doblemente enlazada

Clase:

```txt
ListaDoblementeEnlazada<T>
```

Uso dentro del sistema:

- Historial de boletos.
- Recorrido hacia adelante y hacia atrás.

Justificación:

Permite navegar compras o registros anteriores y posteriores sin depender de índices rígidos.

### 8.4 Grafo

Clase:

```txt
Grafo<TNodo>
```

Uso dentro del sistema:

- Representación de estaciones.
- Conexiones entre ciudades.
- Cálculo de ruta más corta con Dijkstra.

Matriz base de distancias:

```txt
A-B = 30
A-C = 40
A-D = 50
A-F = 50
D-E = 20
E-F = 65
F-G = 80
G-H = 30
G-I = 145
C-I = 80
C-J = 120
C-K = 110
```

Estaciones base:

```txt
A = Medellín
B = Bogotá
C = Cali
D = Barranquilla
E = Cartagena
F = Bucaramanga
G = Pereira
H = Manizales
I = Cúcuta
J = Santa Marta
K = Pasto
```

Justificación:

Las rutas ferroviarias son naturalmente un grafo, porque cada estación puede conectarse con varias estaciones.

### 8.5 Pila

Clase:

```txt
Pila<T>
```

Uso dentro del sistema:

- Gestión de equipaje por vagón de carga.
- Simulación de carga y descarga tipo LIFO.

Justificación:

El último equipaje cargado puede ser el primero en descargarse, dependiendo de la operación del vagón.

### 8.6 Cola de prioridad

Clase:

```txt
ColaPrioridad<T>
```

Uso dentro del sistema:

- Abordaje de pasajeros.

Prioridades consideradas:

- Personas con discapacidad.
- Adultos mayores.
- Pasajeros premium.
- Pasajeros ejecutivos.
- Pasajeros estándar.

Justificación:

El abordaje no debe depender únicamente del orden de llegada, sino de reglas de prioridad.

### 8.7 Árbol AVL

Clase:

```txt
ArbolAvl<T>
```

Uso dentro del sistema:

- Gestión de horarios.

Clave de orden:

```txt
Fecha + HoraSalida + CodigoTren
```

Justificación:

El AVL mantiene los horarios balanceados, permitiendo búsquedas y recorridos ordenados de forma eficiente.

## 9. Lógica de negocio

### 9.1 Autenticación

El usuario ingresa correo y contraseña. El sistema valida sus credenciales y abre el menú correspondiente según el rol:

- Administrador.
- Empleado.
- Pasajero.

### 9.2 Rol administrador

El administrador puede gestionar:

- Trenes.
- Vagones.
- Estaciones.
- Rutas.
- Horarios.
- Pasajeros.
- Empleados.
- Boletos.
- Abordaje.
- Equipaje.

Este rol representa las labores administrativas completas del sistema.

### 9.3 Rol empleado

El empleado puede operar módulos funcionales del día a día:

- Rutas.
- Horarios.
- Pasajeros.
- Boletos.
- Equipaje.
- Abordaje.

Este rol está pensado para atención y operación.

### 9.4 Rol pasajero

El pasajero tiene un portal propio, diferente al CRUD administrativo.

Flujo principal:

```txt
Mis datos
↓
Rutas disponibles
↓
Equipaje
↓
Método de pago
↓
Confirmar compra
```

En este flujo el pasajero puede:

- Actualizar sus datos personales.
- Buscar origen y destino.
- Seleccionar una ruta y horario.
- Registrar equipaje de mano y equipaje documentado.
- Elegir tipo de boleto y método de pago.
- Confirmar la compra.
- Ver el resumen del boleto.

### 9.5 Compra de boleto

La compra del boleto depende de:

- Pasajero autenticado.
- Ruta seleccionada.
- Horario seleccionado.
- Tipo de boleto.
- Distancia calculada.
- Método de pago.
- Equipaje registrado.

El precio se calcula a partir de la distancia y el tipo de boleto.

### 9.6 Abordaje

El abordaje usa una cola de prioridad. Esto permite llamar primero a pasajeros con mayor prioridad según reglas definidas.

### 9.7 Equipaje

El equipaje se registra asociado al boleto y a un vagón de carga. La lógica permite construir una pila de equipajes por vagón.

## 10. Identificadores del sistema

El sistema diferencia entre:

- `IdInterno`: identificador técnico, no visible para el usuario.
- `Código operativo`: identificador visible y usado en pantalla.

Ejemplos:

```txt
TR-001
VG-001
RUT-A-B
HOR-2026-001
BOL-001
EQ-001
```

Esta decisión evita mostrar identificadores técnicos de base de datos y permite usar códigos más comprensibles para el usuario.

## 11. Persistencia JSON

La persistencia actual se realiza mediante archivos JSON.

Ubicación:

```txt
TransRail.Infrastructure/Persistence/Json/
```

Cada módulo tiene su archivo:

```txt
usuarios.json
pasajeros.json
empleados.json
trenes.json
vagones.json
rutas.json
estaciones.json
horarios.json
boletos.json
equipajes.json
pagos.json
```

Ventajas para el proyecto académico:

- Es fácil ver los datos guardados.
- No requiere instalar un motor de base de datos.
- Permite demostrar persistencia de forma clara.
- Facilita una migración futura a base de datos usando las interfaces existentes.

## 12. Datos iniciales

El sistema carga datos base al iniciar mediante `DataSeeder`.

Credenciales:

```txt
Administrador
Correo: admin@transrail.local
Contraseña: admin123

Empleado
Correo: empleado@transrail.local
Contraseña: empleado123

Pasajero
Correo: pasajero@transrail.local
Contraseña: pasajero123
```

Horarios de ejemplo:

```txt
HOR-2026-001: A -> B, 08:30 - 09:15, 30 km
HOR-2026-002: A -> D, 10:20 - 11:20, 50 km
HOR-2026-003: C -> I, 14:00 - 15:35, 80 km
HOR-2026-004: A -> C, 07:10 - 08:05, 40 km
HOR-2026-005: I -> D, ruta calculada con Dijkstra
```

## 13. Cómo ejecutar el proyecto

Abrir una terminal en:

```powershell
cd c:\Users\jmora\Downloads\TransRail\SistemaGestionTrenes
```

Restaurar dependencias:

```powershell
dotnet restore TransRail\TransRail.sln
```

Compilar:

```powershell
dotnet build TransRail\TransRail.sln
```

Ejecutar:

```powershell
dotnet run --project TransRail\TransRail.Presentation
```

Ejecutar pruebas:

```powershell
dotnet test TransRail\TransRail.sln
```

Si la compilación falla porque la aplicación está abierta y bloquea archivos DLL:

```powershell
Get-Process -Name TransRail.Presentation -ErrorAction SilentlyContinue | Stop-Process -Force
```

## 14. Flujo de prueba recomendado

### 14.1 Prueba como administrador

1. Iniciar sesión como administrador.
2. Crear o consultar un tren.
3. Crear o consultar un vagón.
4. Ver estaciones.
5. Consultar rutas y distancias.
6. Gestionar horarios.
7. Crear un pasajero.
8. Crear un boleto.
9. Registrar equipaje.
10. Probar abordaje.

### 14.2 Prueba como empleado

1. Iniciar sesión como empleado.
2. Consultar horarios.
3. Consultar rutas.
4. Crear pasajero.
5. Vender boleto.
6. Registrar equipaje.
7. Encolar pasajero para abordaje.
8. Llamar al siguiente pasajero.

### 14.3 Prueba como pasajero

1. Iniciar sesión como pasajero.
2. Actualizar datos personales.
3. Buscar ruta por origen y destino.
4. Seleccionar horario.
5. Registrar equipaje.
6. Seleccionar método de pago.
7. Confirmar compra.
8. Ver resumen y boleto generado.

## 15. Organización visual

La interfaz se construye con WinForms y usa:

- Menús por rol.
- Formularios por módulo.
- Panel lateral de navegación.
- Botones con iconos.
- Tema visual centralizado.
- Carga de recursos desde `Resources`.
- Formularios embebidos dentro de la misma ventana de menú.

Clases importantes:

- `TransRailTheme`
- `TransRailImages`
- `TransRailButton`
- `TransRailGridStyler`
- `TransRailFormLayout`
- `WorkspaceMenuFormBase`

## 16. Estado actual del proyecto

El proyecto se encuentra funcional en la solución nueva `TransRail`.

Estado actual:

- Arquitectura por capas implementada.
- MVP aplicado en WinForms.
- Persistencia JSON funcional.
- Módulos principales implementados.
- Portal de pasajero implementado.
- Rutas con grafo y Dijkstra.
- Horarios con árbol AVL.
- Abordaje con cola de prioridad.
- Equipaje con pila.
- Pruebas unitarias incluidas.

## 17. Proyecto legado

En la raíz del repositorio todavía existen carpetas y archivos heredados del proyecto anterior:

```txt
Modulo*
Códigos/
src/
ProyectoEstructuras.sln
ProyectoEstructuras.csproj
```

La versión activa y recomendada para ejecutar es:

```txt
TransRail/TransRail.sln
```

El código legado se conserva como referencia histórica, pero la solución final refactorizada está en `TransRail/`.

## 18. Mejoras futuras

Aunque el proyecto ya es funcional para entrega académica, se pueden proponer mejoras futuras:

- Aplicar hash seguro a contraseñas.
- Migrar JSON a SQLite, MySQL o PostgreSQL.
- Agregar reportes administrativos.
- Agregar exportación de boletos.
- Mejorar validaciones de formularios.
- Implementar control más avanzado de disponibilidad de sillas.
- Agregar auditoría de operaciones.
- Agregar pruebas de interfaz.

## 19. Conclusión

El desarrollo de TransRail permitió implementar estructuras de datos en una situación problema relacionada con la gestión de trenes de transporte de pasajeros.

Cada estructura fue usada según su utilidad:

- La tabla hash apoya búsquedas rápidas y autenticación.
- La lista circular representa trenes en circulación.
- La lista doblemente enlazada permite manejar historial de boletos.
- El grafo representa estaciones y rutas, permitiendo calcular distancias con Dijkstra.
- La pila modela operaciones de equipaje.
- La cola de prioridad permite organizar el abordaje de pasajeros.
- El árbol AVL organiza horarios con una clave cronológica compuesta.

El resultado es una aplicación de escritorio académicamente defendible, con separación por capas, patrón MVP, persistencia JSON y una lógica de negocio alineada con el problema planteado.
