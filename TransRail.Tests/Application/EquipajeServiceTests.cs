using TransRail.Application.Interfaces;
using TransRail.Application.Services;
using TransRail.Domain.Entities;

namespace TransRail.Tests.Application;

public sealed class EquipajeServiceTests
{
    [Fact]
    public async Task ConstruirPilaPorVagonAsync_CargaSoloEquipajesDelVagon()
    {
        var repo = new InMemoryEquipajeRepository();
        var service = new EquipajeService(repo);

        await service.UpsertAsync(new Equipaje { CodigoEquipaje = "EQ-001", CodigoVagonCarga = "VG-01", CodigoBoleto = "BOL-1", PesoKg = 20 });
        await service.UpsertAsync(new Equipaje { CodigoEquipaje = "EQ-002", CodigoVagonCarga = "VG-01", CodigoBoleto = "BOL-2", PesoKg = 18 });
        await service.UpsertAsync(new Equipaje { CodigoEquipaje = "EQ-003", CodigoVagonCarga = "VG-02", CodigoBoleto = "BOL-3", PesoKg = 10 });

        var pila = await service.ConstruirPilaPorVagonAsync("VG-01");

        Assert.Equal(2, pila.Count);
        Assert.Equal("EQ-002", pila.Peek().CodigoEquipaje);
    }

    private sealed class InMemoryEquipajeRepository : IEquipajeRepository
    {
        private readonly List<Equipaje> _items = new();

        public Task<IReadOnlyCollection<Equipaje>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyCollection<Equipaje>>(_items.ToArray());
        }

        public Task<Equipaje?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_items.FirstOrDefault(x => x.CodigoEquipaje.Equals(codigo, StringComparison.OrdinalIgnoreCase)));
        }

        public Task UpsertAsync(Equipaje entity, CancellationToken cancellationToken = default)
        {
            _items.RemoveAll(x => x.CodigoEquipaje.Equals(entity.CodigoEquipaje, StringComparison.OrdinalIgnoreCase));
            _items.Add(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(string codigo, CancellationToken cancellationToken = default)
        {
            _items.RemoveAll(x => x.CodigoEquipaje.Equals(codigo, StringComparison.OrdinalIgnoreCase));
            return Task.CompletedTask;
        }

        public Task<IReadOnlyCollection<Equipaje>> GetByCodigoBoletoAsync(string codigoBoleto, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyCollection<Equipaje>>(
                _items.Where(x => x.CodigoBoleto.Equals(codigoBoleto, StringComparison.OrdinalIgnoreCase)).ToArray());
        }

        public Task<IReadOnlyCollection<Equipaje>> GetByCodigoVagonCargaAsync(string codigoVagonCarga, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyCollection<Equipaje>>(
                _items.Where(x => x.CodigoVagonCarga.Equals(codigoVagonCarga, StringComparison.OrdinalIgnoreCase)).ToArray());
        }
    }
}
