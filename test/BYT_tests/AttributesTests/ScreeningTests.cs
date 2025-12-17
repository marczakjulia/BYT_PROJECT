using BYT_Entities.Enums;
using BYT_Entities.Interfaces;
using BYT_Entities.Models;
using NUnit.Framework;

namespace TestByt;

public class ScreeningTests
{
    private Auditorium _auditorium;
    private Movie _movie;
    private Cinema c1;

    private static IEnumerable<IGenreType> Genres() =>
        new IGenreType[] { new ComedyMovie("satire") };

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
            1,
            "Movie 1",
            "Turkey",
            105,
            "Agree to Julia",
            "Mazhar Altincay",
            AgeRestrictionType.PG13,
            Genres(),
            new Rerelease(1, "REASON", new DateTime(2025, 6, 10), true)
        );
    }

    [Test]
    public void MovieIsNull()
    {
        Assert.Throws<ArgumentException>(() =>
            new Screening(
                1,
                null!,
                _auditorium,
                DateTime.Now.Date.AddDays(1),
                new TimeSpan(14, 30, 0),
                ScreeningFormat._2D,
                ScreeningVersion.Subtitles
            ));
    }

    [Test]
    public void AuditoriumIsNull()
    {
        Assert.Throws<ArgumentException>(() =>
            new Screening(
                1,
                _movie,
                null!,
                DateTime.Now.Date.AddDays(1),
                new TimeSpan(14, 30, 0),
                ScreeningFormat._2D,
                ScreeningVersion.Original
            ));
    }

    [Test]
    public void DateIsInPast()
    {
        Assert.Throws<ArgumentException>(() =>
            new Screening(
                1,
                _movie,
                _auditorium,
                DateTime.Now.Date.AddDays(-1),
                new TimeSpan(14, 30, 0),
                ScreeningFormat._2D,
                ScreeningVersion.Original
            ));
    }

    [Test]
    public void StartTimeIsInPast()
    {
        var pastTime = DateTime.Now.TimeOfDay.Subtract(TimeSpan.FromHours(1));

        Assert.Throws<ArgumentException>(() =>
            new Screening(
                1,
                _movie,
                _auditorium,
                DateTime.Now.Date,
                pastTime,
                ScreeningFormat._2D,
                ScreeningVersion.Original
            ));
    }

    [Test]
    public void AuditoriumHasLessThan12Seats_ShouldStillBeCreatable()
    {
        var tempAuditorium = new Auditorium(
            "Temp",
            AuditoriumScreenType.IMAX,
            AuditoriumSoundsSystem.DolbyAtmos,
            2,
            c1
        );

        for (var i = 0; i < 10; i++)
        {
            tempAuditorium.SetSeat(new Seat($"{i:D2}A", SeatType.Normal, i + 1));
        }

        Assert.DoesNotThrow(() =>
            new Auditorium(
                "Mini Salon",
                AuditoriumScreenType._2D,
                AuditoriumSoundsSystem.Stereo,
                3,
                c1
            ));
    }

    [Test]
    public void ScreeningCreationWithValidParameters()
    {
        var screening = new Screening(
            1,
            _movie,
            _auditorium,
            DateTime.Now.Date.AddDays(1),
            new TimeSpan(14, 30, 0),
            ScreeningFormat._2D,
            ScreeningVersion.Original
        );

        Assert.That(screening.Movie, Is.EqualTo(_movie));
        Assert.That(screening.Auditorium, Is.EqualTo(_auditorium));
        Assert.That(screening.Status, Is.EqualTo(ScreeningStatus.Planned));
    }

    [Test]
    public void CreateScreeningGeneratesTickets()
    {
        var screening = new Screening(
            1,
            _movie,
            _auditorium,
            DateTime.Now.Date.AddDays(1),
            new TimeSpan(14, 30, 0),
            ScreeningFormat._2D,
            ScreeningVersion.Original
        );

        screening.CreateTickets(25.00m);

        Assert.That(screening.TicketsBySeatCode.Count, Is.EqualTo(15));
        Assert.That(screening.Status, Is.EqualTo(ScreeningStatus.Planned));

        foreach (var seat in _auditorium.Seats)
        {
            Assert.That(screening.TicketsBySeatCode.ContainsKey(seat.Code));
            Assert.That(screening.TicketsBySeatCode[seat.Code].Price, Is.EqualTo(25.00m));
            Assert.That(screening.TicketsBySeatCode[seat.Code].Status, Is.EqualTo(TicketStatus.Available));
        }
    }
}
