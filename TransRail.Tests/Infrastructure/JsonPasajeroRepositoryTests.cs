using TransRail.Domain.Entities;
using TransRail.Infrastructure.Persistence.Json;
using TransRail.Infrastructure.Repositories;

namespace TransRail.Tests.Infrastructure;

public sealed class JsonPasajeroRepositoryTests
{
    [Fact]
    public async Task UpsertAndDelete_PersistChangesInJson()
    {
        var tempDirectory = Path.Combine(Path.GetTempPath(), $"transrail-tests-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tempDirectory);

        try
        {
            var storage = new JsonStorage(tempDirectory);
            var repository = new JsonPasajeroRepository(storage);
            var pasajero = new Pasajero
            {
                CodigoUsuario = "PAS-TEST-001",
                NombreCompleto = "Pasajero Test",
                NumeroDocumento = "123",
                Correo = "pasajero@test.local",
                Contrasena = "demo123"
            };

            await repository.UpsertAsync(pasajero);

            var persistedAfterUpsert = await storage.LoadAsync<Pasajero>("pasajeros.json");
            Assert.Contains(persistedAfterUpsert, x => x.CodigoUsuario == "PAS-TEST-001");

            await repository.DeleteAsync("PAS-TEST-001");

            var persistedAfterDelete = await storage.LoadAsync<Pasajero>("pasajeros.json");
            Assert.DoesNotContain(persistedAfterDelete, x => x.CodigoUsuario == "PAS-TEST-001");
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }
}
