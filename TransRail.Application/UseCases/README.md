UseCases en estado actual:

- Auth:
  - `LoginUseCase`
- Operacion ferroviaria:
  - `ManageTrainUseCase`
  - `ManageRouteUseCase`
  - `ManageStationUseCase`
  - `ManageScheduleUseCase`
  - `ManageWagonUseCase`
- Gestion de personas:
  - `ManagePassengerUseCase`
  - `ManageEmployeeUseCase`
- Venta y pagos:
  - `TicketPurchaseUseCase`
  - `RegisterPaymentUseCase`
- Abordaje y equipaje:
  - `ManageBoardingQueueUseCase`
  - `LuggageOperationsUseCase`

Los `Services` siguen siendo el motor de dominio/aplicacion, y los UseCases orquestan operaciones listas para ser consumidas por Presenters o Forms.
