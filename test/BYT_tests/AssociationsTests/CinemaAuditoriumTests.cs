using BYT_Entities.Models;
using BYT_Entities.Enums;

namespace TestByt;

public class CinemaAuditoriumTests
{
    private Cinema c1;
    private Cinema c2;
    private Auditorium a1;

    [SetUp]
    public void Setup()
    {
        Cinema.ClearCinemas();
        Auditorium.ClearAuditorium();
        
        c1 = new Cinema(1, "C1", "A", "1", "a@a.com", "10-20");
        c2 = new Cinema(2, "C2", "B", "2", "b@b.com", "10-20");

        a1 = new Auditorium("Hall A", AuditoriumScreenType._2D, AuditoriumSoundsSystem.DolbyAtmos, id: 10,c1);
    }

    [Test]
    public void AddAuditorium_ShouldSetReverseConnection()
    {
        c1.AddAuditorium(a1);

        Assert.IsTrue(c1.GetAuditoriums().Contains(a1));
        Assert.AreEqual(c1, a1.Cinema);
    }

    [Test]
    public void RemoveAuditorium_ShouldRemoveReverseConnection()
    {
        c1.AddAuditorium(a1);

        c1.RemoveAuditorium(a1);

        Assert.IsFalse(c1.GetAuditoriums().Contains(a1));
        Assert.IsNull(a1.Cinema);
    }

    [Test]
    public void AddingAuditoriumToAnotherCinema_ShouldThrow()
    {
        c1.AddAuditorium(a1);

        Assert.Throws<InvalidOperationException>(() => c2.AddAuditorium(a1));
    }

    [Test]
    public void AddAuditorium_ShouldThrow_WhenNull()
    {
        Assert.Throws<ArgumentException>(() => c1.AddAuditorium(null!));
    }

    [Test]
    public void RemoveAuditorium_ShouldThrow_WhenNull()
    {
        Assert.Throws<ArgumentException>(() => c1.RemoveAuditorium(null!));
    }

    [Test]
    public void AddSameAuditoriumTwice_ShouldNotDuplicate()
    {
        c1.AddAuditorium(a1);
        int before = c1.GetAuditoriums().Count;

        c1.AddAuditorium(a1);  // ignored by stop logic

        int after = c1.GetAuditoriums().Count;
        Assert.AreEqual(before, after);
    }

    [Test]
    public void RemoveAuditorium_NotAssigned_ShouldDoNothing()
    {
        Assert.DoesNotThrow(() => c1.RemoveAuditorium(a1));
    }

    [Test]
    public void DestroyCinema_ShouldDeleteAuditoriums_AndUnlinkReverse()
    {
        c1.AddAuditorium(a1);

        Assert.AreEqual(c1, a1.Cinema);
        Assert.IsTrue(Cinema.GetCinemas().Contains(c1));

        c1.Destroy();

        Assert.IsFalse(Cinema.GetCinemas().Contains(c1));

        Assert.IsFalse(Auditorium.GetAll().Contains(a1));

        Assert.IsNull(a1.Cinema);
    }
}
