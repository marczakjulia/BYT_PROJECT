using BYT_Entities.Enums;
using BYT_Entities.Models;

namespace TestByt;

public class ScreeningTests
{
    private Auditorium _auditorium;
    private Movie _movie;
    
    [SetUp]
    public void Setup()
    {
        var tempSeatsList = new List<Seat>();
        for (int i = 0; i < 15; i++)
        {
            tempSeatsList.Add(null);
        }
        
        _auditorium = new Auditorium("Salon 1", AuditoriumScreenType._2D, AuditoriumSoundsSystem.Stereo, tempSeatsList, 1);
        _auditorium.Seats.Clear();
        
        for (int i = 0; i < 15; i++)
        {
            // 00A,01A,...,14A
            _auditorium.Seats.Add(new Seat($"{i:D2}A", SeatType.Normal, _auditorium, i + 1));
        }
        _movie = new Movie(1, "Movie 1", "Turkey", 105, "Agree to Julia", "Mazhar Altincay", AgeRestrictionType.PG13);
    }

    [Test]
    public void MovieIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new Screening(1, null, _auditorium, DateTime.Now.Date.AddDays(1), new TimeSpan(14, 30, 0), 
                ScreeningFormat._2D, ScreeningVersion.Subtitles));
    }

    [Test]
    public void AuditoriumIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new Screening(1, _movie, null, DateTime.Now.Date.AddDays(1), new TimeSpan(14, 30, 0), 
                ScreeningFormat._2D, ScreeningVersion.Original));
    }

    [Test]
    public void DateIsInPast()
    {
        Assert.Throws<ArgumentException>(() =>
            new Screening(1, _movie, _auditorium, DateTime.Now.Date.AddDays(-1), new TimeSpan(14, 30, 0), 
                ScreeningFormat._2D, ScreeningVersion.Original));
    }

    [Test]
    public void StartTimeIsInPast()
    {
        var pastTime = DateTime.Now.TimeOfDay.Subtract(TimeSpan.FromHours(1));
        Assert.Throws<ArgumentException>(() =>
            new Screening(1, _movie, _auditorium, DateTime.Now.Date, pastTime, 
                ScreeningFormat._2D, ScreeningVersion.Original));
    }
    
    [Test]
    public void AuditoriumHasLessThan12Seats()
    {
        var seats = new List<Seat>();
        var tempAuditorium = new Auditorium("Temp", AuditoriumScreenType.IMAX, AuditoriumSoundsSystem.DolbyAtmos, new List<Seat>(new Seat[12]), 1);

        for (int i = 0; i < 10; i++)
        {
            seats.Add(new Seat($"{i:D2}A", SeatType.Normal, tempAuditorium, i + 1));
        }

        Assert.Throws<ArgumentException>(() =>
        {
            new Auditorium("Mini Salon", AuditoriumScreenType._2D, AuditoriumSoundsSystem.Stereo, seats, 2);
        });
    }
}