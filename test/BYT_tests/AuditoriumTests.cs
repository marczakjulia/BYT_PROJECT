using BYT_Entities.Enums;
using BYT_Entities.Models;

namespace TestByt;

public class AuditoriumTests
{
    private Auditorium _auditorium;
    private Movie _movie;

    [SetUp]
    public void Setup()
    {
        var tempSeatsList = new List<Seat>();
        for (var i = 0; i < 15; i++)
            tempSeatsList.Add(null);

        _auditorium = new Auditorium(
            "Salon 1",
            AuditoriumScreenType._2D,
            AuditoriumSoundsSystem.Stereo,
            tempSeatsList,
            1
        );
        _auditorium.Seats.Clear();
        for (var i = 0; i < 15; i++)
            _auditorium.Seats.Add(new Seat($"{i:D2}A", SeatType.Normal, _auditorium, i + 1));

        _movie = new Movie(1, "Movie 1", "Turkey", 105, "Agree to Julia", "Mazhar Altincay", AgeRestrictionType.PG13);
    }

    [Test]
    public void AuditoriumCreatedProperly()
    {
        Assert.That(_auditorium.Name, Is.EqualTo("Salon 1"));
        Assert.That(_auditorium.AuditoriumScreenType, Is.EqualTo(AuditoriumScreenType._2D));
        Assert.That(_auditorium.SoundSystem, Is.EqualTo(AuditoriumSoundsSystem.Stereo));
        Assert.That(_auditorium.Seats.Count, Is.EqualTo(15));
    }

    [Test]
    public void AuditoriumNameEmpty()
    {
        Assert.Throws<ArgumentException>(() => _auditorium.Name = "");
    }

    [Test]
    public void Auditorium_SetInvalidScreenType_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => _auditorium.AuditoriumScreenType = (AuditoriumScreenType)999);
    }

    [Test]
    public void AuditoriumHasLessThan12Seats()
    {
        var seats = new List<Seat>();
        var tempAuditorium = new Auditorium("Temp", AuditoriumScreenType._3D, AuditoriumSoundsSystem.DTS,
            new List<Seat>(new Seat[12]), 5);

        for (var i = 0; i < 10; i++)
            seats.Add(new Seat($"{i:D2}A", SeatType.Normal, tempAuditorium, i + 1));

        Assert.Throws<ArgumentException>(() =>
            new Auditorium("Mini Salon", AuditoriumScreenType._2D, AuditoriumSoundsSystem.Stereo, seats, 2));
    }

    [Test]
    public void AuditoriumAddSeat()
    {
        var newSeat = new Seat("15A", SeatType.VIP, _auditorium, 16);
        _auditorium.AddSeat(newSeat);
        Assert.That(_auditorium.Seats.Contains(newSeat));
    }

    [Test]
    public void AuditoriumAddNullSeat()
    {
        Assert.Throws<ArgumentException>(() => _auditorium.AddSeat(null!));
    }

    [Test]
    public void AuditoriumRemoveSeatWorks()
    {
        var seatToRemove = _auditorium.Seats[0];
        _auditorium.RemoveSeat(seatToRemove);
        Assert.That(!_auditorium.Seats.Contains(seatToRemove));
    }

}