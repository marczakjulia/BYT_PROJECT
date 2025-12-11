using BYT_Entities.Models;

namespace TestByt;

public class ManagerTests
{
    private Manager boss;
    private Manager lead;
    private Manager worker;

    [SetUp]
    public void Setup()
    {
        Manager.ClearManagers();

        boss = new Manager(1, "HQ", 9000, 0.2);
        lead = new Manager(2, "IT", 6000, 0.1);
        worker = new Manager(3, "IT", 4000, 0.05);
    }

    [Test]
    public void SetSupervisor_ShouldCreateReverseConnection()
    {
        lead.SetSupervisor(boss);

        Assert.AreEqual(boss, lead.GetSupervisor());
        Assert.IsTrue(boss.GetSubordinates().Contains(lead));
    }

    [Test]
    public void RemoveSupervisor_ShouldRemoveReverseConnection()
    {
        lead.SetSupervisor(boss);

        lead.SetSupervisor(null);

        Assert.IsNull(lead.GetSupervisor());
        Assert.IsFalse(boss.GetSubordinates().Contains(lead));
    }

    [Test]
    public void AddSubordinate_ShouldCreateReverseConnection()
    {
        boss.AddSubordinate(lead);

        Assert.AreEqual(boss, lead.GetSupervisor());
        Assert.IsTrue(boss.GetSubordinates().Contains(lead));
    }

    [Test]
    public void RemoveSubordinate_ShouldRemoveReverseConnection()
    {
        boss.AddSubordinate(lead);

        boss.RemoveSubordinate(lead);

        Assert.IsNull(lead.GetSupervisor());
        Assert.IsFalse(boss.GetSubordinates().Contains(lead));
    }

    [Test]
    public void ChangingSupervisor_ShouldRemoveOldAndSetNewLinks()
    {
        lead.SetSupervisor(boss);
        lead.SetSupervisor(worker);

        Assert.AreEqual(worker, lead.GetSupervisor());
        Assert.IsFalse(boss.GetSubordinates().Contains(lead));
        Assert.IsTrue(worker.GetSubordinates().Contains(lead));
    }

    [Test]
    public void SetSupervisor_ShouldThrow_WhenSupervisorIsSelf()
    {
        Assert.Throws<InvalidOperationException>(() => boss.SetSupervisor(boss));
    }

    [Test]
    public void AddSubordinate_ShouldThrow_WhenAddingSelf()
    {
        Assert.Throws<InvalidOperationException>(() => boss.AddSubordinate(boss));
    }

    [Test]
    public void AddSubordinateTwice_ShouldThrow()
    {
        boss.AddSubordinate(lead);

        Assert.Throws<InvalidOperationException>(() => boss.AddSubordinate(lead));
    }

    [Test]
    public void RemoveNonExistingSubordinate_ShouldThrow()
    {
        Assert.Throws<InvalidOperationException>(() => boss.RemoveSubordinate(lead));
    }

    [Test]
    public void SetSameSupervisorTwice_ShouldThrow()
    {
        lead.SetSupervisor(boss);

        Assert.Throws<InvalidOperationException>(() => lead.SetSupervisor(boss));
    }

    [Test]
    public void AddSubordinate_ShouldThrow_WhenNull()
    {
        Assert.Throws<ArgumentException>(() => boss.AddSubordinate(null!));
    }

    [Test]
    public void RemoveSubordinate_ShouldThrow_WhenNull()
    {
        Assert.Throws<ArgumentException>(() => boss.RemoveSubordinate(null!));
    }
}
