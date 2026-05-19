using TransRail.Application.Interfaces;
using TransRail.Domain.Entities;
using TransRail.Domain.Enums;
using TransRail.Domain.Rules;
using TransRail.Domain.Structures;

namespace TransRail.Application.Services;

public sealed class BoletoService
{
    private readonly IBoletoRepository _repository;
    private readonly ListaDoblementeEnlazada<Boleto> _historial = new();

    public BoletoService(IBoletoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Boleto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var all = await _repository.GetAllAsync(cancellationToken);
        foreach (var boleto in all)
        {
            if (_historial.Find(x => x.CodigoBoleto.Equals(boleto.CodigoBoleto, StringComparison.OrdinalIgnoreCase)) is null)
            {
                _historial.AddLast(boleto);
            }
        }

        return all;
    }

    public async Task<(bool Ok, string Error)> ComprarAsync(Boleto boleto, CancellationToken cancellationToken = default)
    {
        var validacion = ValidadorBoleto.Validar(boleto);
        if (!validacion.EsValido)
        {
            return (false, validacion.Error);
        }

        await _repository.UpsertAsync(boleto, cancellationToken);
        _historial.AddLast(boleto);
        return (true, string.Empty);
    }

    public IReadOnlyCollection<Boleto> HistorialEnOrden()
    {
        return _historial.ToArray();
    }

    public IReadOnlyCollection<Boleto> HistorialReverso()
    {
        return _historial.EnumerarReversa().ToArray();
    }

    public Task<Boleto?> GetByCodigoAsync(string codigoBoleto, CancellationToken cancellationToken = default)
    {
        return _repository.GetByCodigoAsync(codigoBoleto, cancellationToken);
    }

    public Task DeleteAsync(string codigoBoleto, CancellationToken cancellationToken = default)
    {
        _historial.Remove(x => x.CodigoBoleto.Equals(codigoBoleto, StringComparison.OrdinalIgnoreCase));
        return _repository.DeleteAsync(codigoBoleto, cancellationToken);
    }

    public decimal CalcularPrecio(int distanciaKm, TipoBoleto tipoBoleto)
    {
        return CalculadoraPrecioBoleto.Calcular(distanciaKm, tipoBoleto);
    }
}
