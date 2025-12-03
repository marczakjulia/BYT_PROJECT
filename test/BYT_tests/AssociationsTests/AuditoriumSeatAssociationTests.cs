using BYT_Entities.Enums;
using BYT_Entities.Models;

namespace TestByt;

public class AuditoriumSeatAssociationTests
{
    private Auditorium _auditorium;

    [SetUp]
    public void SetUp()
    {
        _auditorium = new Auditorium("Main Hall", AuditoriumScreenType.IMAX, AuditoriumSoundsSystem.Stereo, 1);
        for (var i = 0; i < 12; i++)
        {
            var seat = new Seat($"{i:D2}A", SeatType.Normal, i);
            _auditorium.SetSeat(seat);
        }
    }

    [Test]
    public void SeatSetAuditoriumAssignsAuditorium()
    {
        var seat = new Seat("99Z", SeatType.VIP, 100);
        seat.SetAuditorium(_auditorium);

        Assert.That(seat.Auditorium, Is.EqualTo(_auditorium));
        Assert.Contains(seat, new List<Seat>(_auditorium.Seats));
    }

    [Test]
    public void Seat_SetAuditorium_ThrowsIfAlreadyAssigned()
    {
        var seat = new Seat("99Z", SeatType.VIP, 100);
        seat.SetAuditorium(_auditorium);

        var anotherAuditorium =
            new Auditorium("Second Hall", AuditoriumScreenType.IMAX, AuditoriumSoundsSystem.DolbyAtmos, 2);
        var ex = Assert.Throws<InvalidOperationException>(() => seat.SetAuditorium(anotherAuditorium));
        Assert.That(ex.Message, Is.EqualTo("Seat is already assigned to an auditorium."));
    }

    [Test]
    public void Seat_RemoveAuditorium_ClearsAssociation()
    {
        var seat = new Seat("99Z", SeatType.VIP, 100);
        seat.SetAuditorium(_auditorium);

        seat.RemoveAuditorium();

        Assert.IsNull(seat.Auditorium);
        Assert.IsFalse(_auditorium.Seats.Contains(seat));
    }

    [Test]
    public void Auditorium_SetSeat_AddsSeatAndUpdatesSeatAuditorium()
    {
        var seat = new Seat("99Z", SeatType.VIP, 100);
        _auditorium.SetSeat(seat);

        Assert.Contains(seat, new List<Seat>(_auditorium.Seats));
        Assert.That(seat.Auditorium, Is.EqualTo(_auditorium));
    }

    [Test]
    public void Auditorium_RemoveSeat_ThrowsIfBelowMinimum()
    {
        var seats = new List<Seat>(_auditorium.Seats);
        foreach (var seat in seats.Take(seats.Count - 12)) _auditorium.RemoveSeat(seat);

        var remainingSeat = new List<Seat>(_auditorium.Seats)[0];
        var ex = Assert.Throws<InvalidOperationException>(() => _auditorium.RemoveSeat(remainingSeat));
        Assert.That(ex.Message, Is.EqualTo("Cannot remove seat. Auditorium must have at least 12 seats."));
    }
}