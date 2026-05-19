using TransRail.Domain.Structures;

namespace TransRail.Tests.Structures;

public sealed class ArbolAvlTests
{
    [Fact]
    public void Insert_Produces_Sorted_InOrder()
    {
        var avl = new ArbolAvl<int>();
        avl.Insert(30);
        avl.Insert(10);
        avl.Insert(20);
        avl.Insert(50);
        avl.Insert(40);

        var ordered = avl.ToArray();

        Assert.Equal(new[] { 10, 20, 30, 40, 50 }, ordered);
        Assert.Equal(5, avl.Count);
    }

    [Fact]
    public void Remove_Removes_Element()
    {
        var avl = new ArbolAvl<int>();
        avl.Insert(1);
        avl.Insert(2);
        avl.Insert(3);

        var removed = avl.Remove(2);

        Assert.True(removed);
        Assert.False(avl.Contains(2));
        Assert.Equal(new[] { 1, 3 }, avl.ToArray());
    }
}

