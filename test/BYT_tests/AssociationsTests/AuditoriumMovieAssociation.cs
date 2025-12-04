using BYT_Entities.Enums;
using BYT_Entities.Models;

namespace TestByt;

public class AuditoriumMovieAssociation
{
    [Test]
    public void CreatingScreening_AddsToMovieAndAuditorium()
    {
        var movie = new Movie(1, "Inception", "USA", 148, "Sci-Fi", "Nolan");
        var auditorium = new Auditorium("Aud1", AuditoriumScreenType._2D, AuditoriumSoundsSystem.Stereo, 1);

        var screening = new Screening(1, movie, auditorium, DateTime.Now.AddDays(1), TimeSpan.FromHours(18),
            ScreeningFormat._2D, ScreeningVersion.Original);

        Assert.Contains(screening, movie.Screenings.ToList());
        Assert.Contains(screening, auditorium.Screenings.ToList());
        Assert.AreEqual(movie, screening.Movie);
        Assert.AreEqual(auditorium, screening.Auditorium);
    }

    [Test]
    public void RemovingScreeningRemovesFromMovieAndAuditorium()
    {
        var movie = new Movie(2, "Interstellar", "USA", 169, "Space", "Nolan");
        var auditorium = new Auditorium("Aud2", AuditoriumScreenType.IMAX, AuditoriumSoundsSystem.DolbyAtmos, 2);
        var screening = new Screening(2, movie, auditorium, DateTime.Now.AddDays(2), TimeSpan.FromHours(20), ScreeningFormat._3D, ScreeningVersion.Dubbing);

        screening.RemoveScreeningFromAssociations();

        Assert.IsFalse(movie.Screenings.Contains(screening));
        Assert.IsFalse(auditorium.Screenings.Contains(screening));
    }


    [Test]
    public void AddingScreeningTwice_DoesNotDuplicate()
    {
        var movie = new Movie(3, "Dunkirk", "UK", 106, "War", "Nolan");
        var auditorium = new Auditorium("Aud3", AuditoriumScreenType._3D, AuditoriumSoundsSystem.DolbyAtmos, 3);
        var screening = new Screening(3, movie, auditorium, DateTime.Now.AddDays(3), TimeSpan.FromHours(19),
            ScreeningFormat._2D, ScreeningVersion.Original);

        movie.AddScreening(screening);
        auditorium.AddScreening(screening);

        Assert.AreEqual(1, movie.Screenings.Count);
        Assert.AreEqual(1, auditorium.Screenings.Count);
    }

    [Test]
    public void Screening_AssociationIsBidirectional()
    {
        var movie = new Movie(4, "Tenet", "USA", 150, "Action", "Nolan");
        var auditorium = new Auditorium("Aud4", AuditoriumScreenType.IMAX, AuditoriumSoundsSystem.DolbyAtmos, 4);

        var screening = new Screening(4, movie, auditorium, DateTime.Now.AddDays(1), TimeSpan.FromHours(21), ScreeningFormat._3D, ScreeningVersion.Original);

        Assert.Contains(screening, movie.Screenings.ToList());
        Assert.Contains(screening, auditorium.Screenings.ToList());
        Assert.AreEqual(movie, screening.Movie);
        Assert.AreEqual(auditorium, screening.Auditorium);
    }
}
