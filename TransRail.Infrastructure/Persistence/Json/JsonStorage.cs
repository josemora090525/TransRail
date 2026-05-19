using System.Text.Json;
using TransRail.Application.Interfaces;

namespace TransRail.Infrastructure.Persistence.Json;

public sealed class JsonStorage : IJsonStorage
{
    private readonly string _basePath;
    private readonly JsonSerializerOptions _serializerOptions;

    public JsonStorage(string? basePath = null)
    {
        _basePath = basePath ?? ResolveDefaultBasePath();
        Directory.CreateDirectory(_basePath);
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    public async Task<IReadOnlyCollection<T>> LoadAsync<T>(string moduleFileName, CancellationToken cancellationToken = default)
    {
        var fullPath = GetFullPath(moduleFileName);
        if (!File.Exists(fullPath))
        {
            return Array.Empty<T>();
        }

        await using var stream = File.OpenRead(fullPath);
        var result = await JsonSerializer.DeserializeAsync<List<T>>(stream, _serializerOptions, cancellationToken);
        return result ?? new List<T>();
    }

    public async Task SaveAsync<T>(string moduleFileName, IReadOnlyCollection<T> items, CancellationToken cancellationToken = default)
    {
        var fullPath = GetFullPath(moduleFileName);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

        await using var stream = File.Create(fullPath);
        await JsonSerializer.SerializeAsync(stream, items, _serializerOptions, cancellationToken);
    }

    private string GetFullPath(string moduleFileName)
    {
        return Path.Combine(_basePath, moduleFileName);
    }

    private static string ResolveDefaultBasePath()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current is not null)
        {
            var solutionPath = Path.Combine(current.FullName, "TransRail.sln");
            if (File.Exists(solutionPath))
            {
                return Path.Combine(current.FullName, "TransRail.Infrastructure", "Persistence", "Json");
            }

            current = current.Parent;
        }

        return Path.Combine(AppContext.BaseDirectory, "data");
    }
}
