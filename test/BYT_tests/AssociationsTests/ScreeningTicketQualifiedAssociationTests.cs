using BYT_Entities.Enums;
using BYT_Entities.Models;

namespace TestByt;

public class ScreeningTicketQualifiedAssociationTests
{
    private Auditorium _auditorium;
    private Screening _screening;
    private Seat _seat1;
    private Seat _seat2;

    [SetUp]
    public void Setup()
    {
        _auditorium = new Auditorium("Aud1", AuditoriumScreenType.IMAX, AuditoriumSoundsSystem.DolbyAtmos, 2);
        _seat1 = new Seat("01A", SeatType.Normal, 1);
        _seat2 = new Seat("01B", SeatType.VIP, 2);

        _seat1.SetAuditorium(_auditorium);
        _seat2.SetAuditorium(_auditorium);

        var movie = new Movie(1, "Inception", "USA", 100, "Sci-Fi", "Nolan");
        _screening = new Screening(1, movie, _auditorium, DateTime.Now.AddDays(1), TimeSpan.FromHours(18),
            ScreeningFormat._2D, ScreeningVersion.Original);

        _screening.CreateTickets(10.0m);
    }

    [Test]
    public void TicketsBySeatCodeShouldContainAllSeats()
    {
        Assert.IsTrue(_screening.TicketsBySeatCode.ContainsKey("01A"));
        Assert.IsTrue(_screening.TicketsBySeatCode.ContainsKey("01B"));
    }

    [Test]
    public void AddTicketShouldAddTicketCorrectly()
    {
        var newSeatCode = "01C";
        var newSeat = new Seat(newSeatCode, SeatType.Accessible, 3);
        newSeat.SetAuditorium(_auditorium);

        var newTicket = new Ticket(15m, 3);
        _screening.AddTicket(newSeatCode, newTicket);

        Assert.IsTrue(_screening.TicketsBySeatCode.ContainsKey(newSeatCode));
        Assert.AreEqual(newTicket, _screening.TicketsBySeatCode[newSeatCode]);
    }

    [Test]
    public void RemoveTicketShouldRemoveTicketCorrectly()
    {
        _screening.RemoveTicket("01A");

        Assert.IsFalse(_screening.TicketsBySeatCode.ContainsKey("01A"));
    }

    [Test]
    public void AddingDuplicateTicketShouldThrowException()
    {
        var ticket = new Ticket(10m, 4);
        Assert.Throws<InvalidOperationException>(() => _screening.AddTicket("01A", ticket));
    }

    [Test]
    public void TicketsShouldMaintainBackReferenceToScreening()
    {
        foreach (var kvp in _screening.TicketsBySeatCode) Assert.AreEqual(_screening, kvp.Value.Screening);
    }
}