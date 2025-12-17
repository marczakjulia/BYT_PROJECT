using BYT_Entities.Enums;
using BYT_Entities.Interfaces;
using BYT_Entities.Models;
using NUnit.Framework;

namespace TestByt;

public class ScreeningTicketQualifiedAssociationTests
{
    private Auditorium _auditorium;
    private Screening _screening;
    private Seat _seat1;
    private Seat _seat2;
    private Cinema c1;

    [SetUp]
    public void Setup()
    {
        c1 = new Cinema(1, "C1", "A", "1", "a@a.com", "10-20");

        _auditorium = new Auditorium(
            "Aud1",
            AuditoriumScreenType.IMAX,
            AuditoriumSoundsSystem.DolbyAtmos,
            2,
            c1
        );

        _seat1 = new Seat("01A", SeatType.Normal, 1);
        _seat2 = new Seat("01B", SeatType.VIP, 2);

        _seat1.SetAuditorium(_auditorium);
        _seat2.SetAuditorium(_auditorium);

        var genres = new IGenreType[]
        {
            new HorrorMovie(6)
        };

        Movie movie = new NormalCut(
            id: 1,
            title: "Inception",
            countryOfOrigin: "USA",
            length: 100,
            description: "Sci-Fi",
            director: "Nolan",
            ageRestriction: null,
            genres: genres,
            reRelease: new Rerelease(
                1,
                "REASON",
                new DateTime(2025, 6, 10),
                true
            )
        );

        _screening = new Screening(
            1,
            movie,
            _auditorium,
            DateTime.Now.AddDays(1),
            TimeSpan.FromHours(18),
            ScreeningFormat._2D,
            ScreeningVersion.Original
        );

        _screening.CreateTickets(10.0m);
    }

    [Test]
    public void TicketsBySeatCodeShouldContainAllSeats()
    {
        Assert.That(_screening.TicketsBySeatCode.ContainsKey("01A"));
        Assert.That(_screening.TicketsBySeatCode.ContainsKey("01B"));
    }

    [Test]
    public void AddTicketShouldAddTicketCorrectly()
    {
        var newSeatCode = "01C";
        var newSeat = new Seat(newSeatCode, SeatType.Accessible, 3);
        newSeat.SetAuditorium(_auditorium);

        var newTicket = new Ticket(15m, 3);

        _screening.AddTicket(newSeatCode, newTicket);

        Assert.That(_screening.TicketsBySeatCode.ContainsKey(newSeatCode));
        Assert.That(_screening.TicketsBySeatCode[newSeatCode], Is.EqualTo(newTicket));
    }

    [Test]
    public void RemoveTicketShouldRemoveTicketCorrectly()
    {
        _screening.RemoveTicket("01A");

        Assert.That(_screening.TicketsBySeatCode.ContainsKey("01A"), Is.False);
    }

    [Test]
    public void AddingDuplicateTicketShouldThrowException()
    {
        var ticket = new Ticket(10m, 4);

        Assert.Throws<InvalidOperationException>(() =>
            _screening.AddTicket("01A", ticket)
        );
    }

    [Test]
    public void TicketsShouldMaintainBackReferenceToScreening()
    {
        foreach (var ticket in _screening.TicketsBySeatCode.Values)
        {
            Assert.That(ticket.Screening, Is.EqualTo(_screening));
        }
    }
}
