using TransRail.Application.DTOs;
using TransRail.Application.Interfaces;
using TransRail.Application.Services;
using TransRail.Domain.Entities;

namespace TransRail.Tests.Application;

public sealed class AuthServiceTests
{
    [Fact]
    public async Task LoginAsync_ReturnsSuccess_ForValidCredentials()
    {
        var repo = new FakeUsuarioRepository(new Usuario[]
        {
            new Administrador
            {
                CodigoUsuario = "USR-ADM-TEST",
                NombreCompleto = "Admin Test",
                NumeroDocumento = "999",
                Correo = "admin@test.local",
                Contrasena = "1234"
            }
        });

        var auth = new AuthService(repo);
        var result = await auth.LoginAsync(new LoginRequestDto("admin@test.local", "1234"));

        Assert.True(result.Exitoso);
        Assert.NotNull(result.Rol);
    }

    private sealed class FakeUsuarioRepository : IUsuarioRepository
    {
        private readonly List<Usuario> _usuarios;

        public FakeUsuarioRepository(IEnumerable<Usuario> usuarios)
        {
            _usuarios = usuarios.ToList();
        }

        public Task<IReadOnlyCollection<Usuario>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyCollection<Usuario>>(_usuarios);
        }

        public Task<Usuario?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
        {
            var item = _usuarios.FirstOrDefault(x => x.CodigoUsuario.Equals(codigo, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(item);
        }

        public Task<Usuario?> GetByCorreoAsync(string correo, CancellationToken cancellationToken = default)
        {
            var item = _usuarios.FirstOrDefault(x => x.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(item);
        }

        public Task UpsertAsync(Usuario entity, CancellationToken cancellationToken = default)
        {
            _usuarios.RemoveAll(x => x.CodigoUsuario.Equals(entity.CodigoUsuario, StringComparison.OrdinalIgnoreCase));
            _usuarios.Add(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(string codigo, CancellationToken cancellationToken = default)
        {
            _usuarios.RemoveAll(x => x.CodigoUsuario.Equals(codigo, StringComparison.OrdinalIgnoreCase));
            return Task.CompletedTask;
        }
    }
}

