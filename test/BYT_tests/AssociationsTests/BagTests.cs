using BYT_Entities.Enums;
using BYT_Entities.Models;

namespace TestByt;

public class BagTests
{
    [SetUp]
    public void Setup()
    {
        Movie.ClearMovies();
        Auditorium.ClearAuditorium();

        var newRelease = new NewRelease(1, false, DateTime.Now.AddDays(7), "Pixar");
        _movie = new Movie(1, "X", "TR", 100, "animation", "Mazhar", AgeRestrictionType.PG13, newRelease);

        _auditorium = new Auditorium("Hall 1", AuditoriumScreenType._4DX, AuditoriumSoundsSystem.DolbyAtmos, 1);

        for (var i = 0; i < 12; i++)
        {
            var seat = new Seat($"{i:D2}A", SeatType.Normal, i);
            _auditorium.SetSeat(seat);
        }
    }

    private Movie _movie;
    private Auditorium _auditorium;

    
    [Test]
    public void RemoveOneScreeningShouldKeepOthersIntact()
    {
        var screening1 = new Screening(1, _movie, _auditorium, DateTime.Now.AddDays(1), new TimeSpan(14, 0, 0), ScreeningFormat.IMAX, ScreeningVersion.Original);
        var screening2 = new Screening(2, _movie, _auditorium, DateTime.Now.AddDays(2), new TimeSpan(18, 0, 0), ScreeningFormat.IMAX, ScreeningVersion.Dubbing);
        var screening3 = new Screening(3, _movie, _auditorium, DateTime.Now.AddDays(3), new TimeSpan(20, 30, 0), ScreeningFormat.IMAX, ScreeningVersion.Original);

        screening2.RemoveCompletely();

        Assert.That(_movie.Screenings.Count, Is.EqualTo(2));
        Assert.That(_auditorium.Screenings.Count, Is.EqualTo(2));
        Assert.That(_movie.Screenings.Contains(screening1), Is.True);
        Assert.That(_movie.Screenings.Contains(screening2), Is.False);
        Assert.That(_movie.Screenings.Contains(screening3), Is.True);
    }
    

    [Test]
    public void SetAuditoriumShouldUpdateConnectionsProperly()
    {
        var auditorium2 = new Auditorium("Hall 2", AuditoriumScreenType.IMAX, AuditoriumSoundsSystem.DolbyAtmos, 2);
        for (var i = 0; i < 12; i++)
        {
            var seat = new Seat($"{i:D2}A", SeatType.Normal, i);
            auditorium2.SetSeat(seat);
        }

        var screening = new Screening(1, _movie, _auditorium, DateTime.Now.AddDays(1), new TimeSpan(14, 0, 0), ScreeningFormat.IMAX, ScreeningVersion.Original);

        screening.SetAuditorium(auditorium2);

        Assert.That(_auditorium.Screenings.Contains(screening), Is.False);
        Assert.That(auditorium2.Screenings.Contains(screening), Is.True);
        Assert.That(screening.Auditorium, Is.EqualTo(auditorium2));
    }


    [Test]
    public void MultipleScreeningsSameDayDifferentTimesShouldAllExist()
    {
        var sameDay = DateTime.Now.AddDays(1);

        var morning = new Screening(1, _movie, _auditorium, sameDay, new TimeSpan(10, 0, 0), ScreeningFormat.IMAX, ScreeningVersion.Original);
        var afternoon = new Screening(2, _movie, _auditorium, sameDay, new TimeSpan(14, 30, 0), ScreeningFormat.IMAX, ScreeningVersion.Dubbing);
        var evening = new Screening(3, _movie, _auditorium, sameDay, new TimeSpan(19, 0, 0), ScreeningFormat.IMAX, ScreeningVersion.Original);

        Assert.That(_movie.Screenings.Count, Is.EqualTo(3));
        Assert.That(_auditorium.Screenings.Count, Is.EqualTo(3));
        Assert.That(morning.StartTime.Hours, Is.EqualTo(10));
        Assert.That(afternoon.StartTime.Hours, Is.EqualTo(14));
        Assert.That(evening.StartTime.Hours, Is.EqualTo(19));
    }

    [Test]
    public void BagAssociationSameMovieSameAuditoriumMultipleTimesAllInstancesShouldExist()
    {
        var s1 = new Screening(1, _movie, _auditorium, DateTime.Now.AddDays(1), new TimeSpan(10, 0, 0), ScreeningFormat.IMAX, ScreeningVersion.Original);
        var s2 = new Screening(2, _movie, _auditorium, DateTime.Now.AddDays(1), new TimeSpan(14, 0, 0), ScreeningFormat._3D, ScreeningVersion.Dubbing);
        var s3 = new Screening(3, _movie, _auditorium, DateTime.Now.AddDays(1), new TimeSpan(18, 0, 0), ScreeningFormat.IMAX, ScreeningVersion.Lector);

        Assert.That(_movie.Screenings.Count, Is.EqualTo(3));
        Assert.That(_auditorium.Screenings.Count, Is.EqualTo(3));
    }

    [Test]
    public void BagAssociationEachScreeningHasUniqueAttributesCombination()
    {
        var s1 = new Screening(1, _movie, _auditorium, DateTime.Now.AddDays(1), new TimeSpan(10, 0, 0), ScreeningFormat.IMAX, ScreeningVersion.Original);
        var s2 = new Screening(2, _movie, _auditorium, DateTime.Now.AddDays(2), new TimeSpan(10, 0, 0), ScreeningFormat._3D, ScreeningVersion.Dubbing);

        Assert.That(s1.Date, Is.Not.EqualTo(s2.Date));
        Assert.That(s1.Format, Is.Not.EqualTo(s2.Format));
        Assert.That(s1.Version, Is.Not.EqualTo(s2.Version));
    }

    [Test]
    public void BagAssociationRemovingOneInstanceDoesNotAffectOtherInstances()
    {
        var s1 = new Screening(1, _movie, _auditorium, DateTime.Now.AddDays(1), new TimeSpan(10, 0, 0), ScreeningFormat.IMAX, ScreeningVersion.Original);
        var s2 = new Screening(2, _movie, _auditorium, DateTime.Now.AddDays(1), new TimeSpan(14, 0, 0), ScreeningFormat._3D, ScreeningVersion.Dubbing);
        var s3 = new Screening(3, _movie, _auditorium, DateTime.Now.AddDays(1), new TimeSpan(18, 0, 0), ScreeningFormat.IMAX, ScreeningVersion.Lector);

        s2.RemoveCompletely();

        Assert.That(_movie.Screenings.Count, Is.EqualTo(2));
        Assert.That(_movie.Screenings.Contains(s1), Is.True);
        Assert.That(_movie.Screenings.Contains(s3), Is.True);
        Assert.That(s1.Movie, Is.EqualTo(_movie));
        Assert.That(s3.Movie, Is.EqualTo(_movie));
    }
}