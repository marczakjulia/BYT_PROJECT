using BYT_Entities.Enums;
using BYT_Entities.Interfaces;
using BYT_Entities.Models;
using NUnit.Framework;

namespace TestByt;

public class AuditoriumTests
{
    private Auditorium _auditorium;
    private Movie _movie;
    private Cinema c1;

    [SetUp]
    public void Setup()
    {
        c1 = new Cinema(1, "C1", "A", "1", "a@a.com", "10-20");

        _auditorium = new Auditorium(
            "Salon 1",
            AuditoriumScreenType._2D,
            AuditoriumSoundsSystem.Stereo,
            1,
            c1
        );

        for (var i = 0; i < 15; i++)
        {
            var seat = new Seat($"{i:D2}A", SeatType.Normal, i + 1);
            _auditorium.SetSeat(seat);
        }

        _movie = new NormalCut(
            id: 1,
            title: "Movie 1",
            countryOfOrigin: "Turkey",
            length: 105,
            description: "Agree to Julia",
            director: "Mazhar Altincay",
            ageRestriction: AgeRestrictionType.PG13,
            genres: new IGenreType[]
            {
                new ComedyMovie("satire")
            },
            reRelease: new Rerelease(
                1,
                "REASON",
                new DateTime(2025, 6, 10),
                true
            )
        );
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
        Assert.Throws<ArgumentException>(() =>
            _auditorium.AuditoriumScreenType = (AuditoriumScreenType)999);
    }

    [Test]
    public void AuditoriumCanBeCreatedWithLessThan12SeatsInitially()
    {
        Assert.DoesNotThrow(() =>
        {
            var minimalAuditorium = new Auditorium(
                "Mini Salon",
                AuditoriumScreenType._2D,
                AuditoriumSoundsSystem.Stereo,
                2,
                c1
            );

            Assert.That(minimalAuditorium.Seats.Count, Is.EqualTo(0));
        });
    }

    [Test]
    public void AuditoriumAddSeatToAuditorium()
    {
        var newSeat = new Seat("15A", SeatType.VIP, 16);
        _auditorium.SetSeat(newSeat);

        Assert.That(_auditorium.Seats.Contains(newSeat));
        Assert.That(newSeat.Auditorium, Is.EqualTo(_auditorium));
    }

    [Test]
    public void AuditoriumAddSeatToAuditoriumWithNull()
    {
        Assert.Throws<ArgumentException>(() => _auditorium.SetSeat(null!));
    }

    [Test]
    public void AuditoriumRemoveNonExistentSeatThrowsException()
    {
        var nonExistentSeat = new Seat("99Z", SeatType.Normal, 999);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            _auditorium.RemoveSeat(nonExistentSeat));

        Assert.That(exception.Message, Does.Contain("not found"));
    }
}
