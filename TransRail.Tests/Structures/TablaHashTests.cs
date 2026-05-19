using TransRail.Domain.Structures;

namespace TransRail.Tests.Structures;

public sealed class TablaHashTests
{
    [Fact]
    public void AddOrUpdate_And_TryGetValue_Work_Correctly()
    {
        var table = new TablaHash<string, int>(7);
        table.AddOrUpdate("A", 1);
        table.AddOrUpdate("B", 2);
        table.AddOrUpdate("A", 3);

        Assert.True(table.TryGetValue("A", out var valueA));
        Assert.Equal(3, valueA);
        Assert.True(table.TryGetValue("B", out var valueB));
        Assert.Equal(2, valueB);
        Assert.Equal(2, table.Count);
    }

    [Fact]
    public void Remove_Removes_Key()
    {
        var table = new TablaHash<string, string>(7);
        table.AddOrUpdate("K1", "V1");

        var removed = table.Remove("K1");

        Assert.True(removed);
        Assert.False(table.ContainsKey("K1"));
        Assert.Equal(0, table.Count);
    }
}

