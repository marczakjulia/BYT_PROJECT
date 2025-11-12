using BYT_Entities.Enums;
using BYT_Entities.Models;

public class SeatTests
{
    private Auditorium _auditorium;

    [SetUp]
    public void Setup()
    {
        var tempSeatsList = new List<Seat>();
        for (var i = 0; i < 15; i++) tempSeatsList.Add(null);

        _auditorium = new Auditorium("Salon 1", AuditoriumScreenType._2D, AuditoriumSoundsSystem.Stereo, tempSeatsList,
            1);
        _auditorium.Seats.Clear();

        for (var i = 0; i < 15; i++)
            // 00A,01A,...,14A
            _auditorium.Seats.Add(new Seat($"{i:D2}A", SeatType.Normal, _auditorium, i + 1));
    }

    [Test]
    public void SeatCreatedProperly()
    {
        var seat = new Seat("05A", SeatType.Normal, _auditorium, 1);
        Assert.That(seat.Code, Is.EqualTo("05A"));
        Assert.That(seat.Type, Is.EqualTo(SeatType.Normal));
        Assert.That(seat.Auditorium, Is.EqualTo(_auditorium));
    }

    [Test]
    public void SeatInvalidCode()
    {
        Assert.Throws<ArgumentException>(() => new Seat("BADCODE", SeatType.Normal, _auditorium, 1));
        Assert.Throws<ArgumentException>(() => new Seat("11a", SeatType.VIP, _auditorium, 1));
    }

    [Test]
    public void SeatNullAuditorium()
    {
        Assert.Throws<ArgumentException>(() => new Seat("05A", SeatType.Normal, null!, 1));
    }
}