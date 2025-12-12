using BYT_Entities.Enums;
using BYT_Entities.Models;

public class SeatTests
{
    private Auditorium _auditorium;
    private Cinema c1;

    [SetUp]
    public void Setup()
    {
        c1 = new Cinema(1, "C1", "A", "1", "a@a.com", "10-20");
        _auditorium = new Auditorium("Salon 1", AuditoriumScreenType._2D, AuditoriumSoundsSystem.Stereo, 1,c1);

        if (_auditorium.Seats.Any())
        {
            var seatsToRemove = _auditorium.Seats.ToList();
            foreach (var seat in seatsToRemove) _auditorium.RemoveSeat(seat);
        }

        for (var i = 0; i < 15; i++)
        {
            var seat = new Seat($"{i:D2}A", SeatType.Normal, i + 1);
            _auditorium.SetSeat(seat);
        }
    }

    [Test]
    public void SeatCreatedProperly()
    {
        var seat = new Seat("05A", SeatType.Normal, 1);
        seat.SetAuditorium(_auditorium);

        Assert.That(seat.Code, Is.EqualTo("05A"));
        Assert.That(seat.Type, Is.EqualTo(SeatType.Normal));
        Assert.That(seat.Auditorium, Is.EqualTo(_auditorium));
    }

    [Test]
    public void SeatInvalidCode()
    {
        Assert.Throws<ArgumentException>(() => new Seat("BADCODE", SeatType.Normal, 1));
        Assert.Throws<ArgumentException>(() => new Seat("11a", SeatType.VIP, 1));

        Assert.DoesNotThrow(() => new Seat("11A", SeatType.VIP, 1));
        Assert.DoesNotThrow(() => new Seat("99Z", SeatType.Normal, 2));
    }

    [Test]
    public void SeatCanExistWithoutAuditorium()
    {
        var seat = new Seat("05A", SeatType.Normal, 1);

        Assert.That(seat.Code, Is.EqualTo("05A"));
        Assert.That(seat.Type, Is.EqualTo(SeatType.Normal));
        Assert.That(seat.Auditorium, Is.Null);

        seat.SetAuditorium(_auditorium);
        Assert.That(seat.Auditorium, Is.EqualTo(_auditorium));
    }

    [Test]
    public void AssignToAuditoriumCreatesReverseConnection()
    {
        var seat = new Seat("05A", SeatType.Normal, 1);

        seat.SetAuditorium(_auditorium);

        Assert.That(seat.Auditorium, Is.EqualTo(_auditorium));
        Assert.That(_auditorium.Seats.Contains(seat), Is.True);
    }

    [Test]
    public void RemoveFromAuditoriumRemovesReverseConnection()
    {
        var seat = new Seat("05A", SeatType.Normal, 1);
        seat.SetAuditorium(_auditorium);

        seat.RemoveAuditorium();

        Assert.That(seat.Auditorium, Is.Null);
        Assert.That(_auditorium.Seats.Contains(seat), Is.False);
    }

    [Test]
    public void CannotAssignToAuditoriumWhenAlreadyAssigned()
    {
        var seat = new Seat("05A", SeatType.Normal, 1);
        var anotherAuditorium =
            new Auditorium("Salon 2", AuditoriumScreenType._3D, AuditoriumSoundsSystem.DolbyAtmos, 2,c1);

        seat.SetAuditorium(_auditorium);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            seat.SetAuditorium(anotherAuditorium));
        Assert.That(exception.Message, Does.Contain("already assigned"));
    }
}