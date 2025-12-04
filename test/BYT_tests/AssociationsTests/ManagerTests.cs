namespace TestByt;

using BYT_Entities.Models;


public class ManagerAssociationTests
{
    [SetUp]
    public void SetUp()
    {
        Manager.ClearManagers();
    }

    [Test]
    public void SetSupervisor_CreatesReverseConnection()
    {
        var m1 = new Manager(1, "Sales", 5000, 0.1);
        var m2 = new Manager(2, "Sales", 4500, 0.1);

        m2.SetSupervisor(m1);

        Assert.That(m2.GetSupervisor(), Is.EqualTo(m1));
        Assert.That(m1.GetSubordinates().Contains(m2));
    }

    [Test]
    public void AddSubordinate_CreatesReverseConnection()
    {
        var m1 = new Manager(1, "Sales", 5000, 0.1);
        var m2 = new Manager(2, "Sales", 4500, 0.1);

        m1.AddSubordinate(m2);

        Assert.That(m1.GetSubordinates().Contains(m2));
        Assert.That(m2.GetSupervisor(), Is.EqualTo(m1));
    }

    [Test]
    public void RemoveSupervisor_RemovesReverseConnection()
    {
        var m1 = new Manager(1, "Sales", 5000, 0.1);
        var m2 = new Manager(2, "Sales", 4500, 0.1);

        m2.SetSupervisor(m1);
        m2.SetSupervisor(null);

        Assert.That(m2.GetSupervisor(), Is.Null);
        Assert.That(!m1.GetSubordinates().Contains(m2));
    }

    [Test]
    public void RemoveSubordinate_RemovesReverseConnection()
    {
        var m1 = new Manager(1, "Sales", 5000, 0.1);
        var m2 = new Manager(2, "Sales", 4500, 0.1);

        m1.AddSubordinate(m2);
        m1.RemoveSubordinate(m2);

        Assert.That(!m1.GetSubordinates().Contains(m2));
        Assert.That(m2.GetSupervisor(), Is.Null);
    }

    [Test]
    public void ManagerCannotBeOwnSupervisor()
    {
        var m = new Manager(1, "Sales", 5000, 0.1);
        var ex = Assert.Throws<InvalidOperationException>(() => m.SetSupervisor(m));
        Assert.That(ex.Message, Does.Contain("cannot supervise themselves"));
    }
}
