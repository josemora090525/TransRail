using TransRail.Infrastructure.Persistence.Json;

namespace TransRail.Tests.Infrastructure;

public sealed class JsonStorageDefaultPathTests
{
    [Fact]
    public async Task DefaultStorage_WritesIntoProjectJsonFolder()
    {
        var storage = new JsonStorage();
        var fileName = $"json-storage-path-test-{Guid.NewGuid():N}.json";
        var expectedPath = Path.Combine(FindSolutionRoot(), "TransRail.Infrastructure", "Persistence", "Json", fileName);

        try
        {
            await storage.SaveAsync(fileName, new[] { "ok" });

            Assert.True(File.Exists(expectedPath));
        }
        finally
        {
            if (File.Exists(expectedPath))
            {
                File.Delete(expectedPath);
            }
        }
    }

    private static string FindSolutionRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current is not null)
        {
            if (File.Exists(Path.Combine(current.FullName, "TransRail.sln")))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new DirectoryNotFoundException("No se encontró la raíz de la solución TransRail.");
    }
}
