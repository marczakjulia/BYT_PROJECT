namespace TestByt;

using BYT_Entities.Enums;
using BYT_Entities.Interfaces;
using BYT_Entities.Models;
using NUnit.Framework;

public class ScreeningAssociationTests
{
    private Movie _movie;
    private Auditorium _auditorium;
    private Cinema c1;

    [SetUp]
    public void Setup()
    {
        Movie.ClearMovies();

        c1 = new Cinema(1, "C1", "A", "1", "a@a.com", "10-20");

        var genres = new IGenreType[]
        {
            new ComedyMovie("Satire")
        };

        _movie = new NormalCut(
            id: 1,
            title: "Test",
            countryOfOrigin: "PL",
            length: 120,
            description: "Desc",
            director: "Dir",
            ageRestriction: null,
            genres: genres,
            reRelease: new Rerelease(1, "reas", new DateTime(2006, 10, 6), true)
        );

        _auditorium = new Auditorium(
            "A1",
            AuditoriumScreenType._2D,
            AuditoriumSoundsSystem.Stereo,
            1,
            c1
        );
    }

    [Test]
    public void ScreeningConstructor_ShouldCreateBidirectionalLinks()
    {
        var screening = new Screening(
            10,
            _movie,
            _auditorium,
            DateTime.Now.AddDays(2),
            TimeSpan.FromHours(18),
            ScreeningFormat._2D,
            ScreeningVersion.Original
        );

        Assert.That(_movie.Screenings.Contains(screening));
        Assert.That(_auditorium.Screenings.Contains(screening));
        Assert.That(screening.Movie, Is.EqualTo(_movie));
        Assert.That(screening.Auditorium, Is.EqualTo(_auditorium));
    }

    [Test]
    public void RemoveScreening_ShouldRemoveFromBothSides()
    {
        var screening = new Screening(
            10,
            _movie,
            _auditorium,
            DateTime.Now.AddDays(2),
            TimeSpan.FromHours(18),
            ScreeningFormat._2D,
            ScreeningVersion.Original
        );

        screening.RemoveCompletely();

        Assert.That(_movie.Screenings.Contains(screening), Is.False);
        Assert.That(_auditorium.Screenings.Contains(screening), Is.False);
        Assert.That(screening.Movie, Is.Null);
        Assert.That(screening.Auditorium, Is.Null);
    }

    [Test]
    public void SetMovie_ShouldSwitchMovieProperly()
    {
        var genres = new IGenreType[]
        {
            new HorrorMovie(7)
        };

        var movie2 = new NormalCut(
            id: 2,
            title: "Other",
            countryOfOrigin: "PL",
            length: 90,
            description: "x",
            director: "y",
            ageRestriction: AgeRestrictionType.PG13,
            genres: genres,
            newRelease: new NewRelease(1, true, new DateTime(2006, 10, 6), "d")
        );

        var screening = new Screening(
            10,
            _movie,
            _auditorium,
            DateTime.Now.AddDays(1),
            TimeSpan.FromHours(18),
            ScreeningFormat._2D,
            ScreeningVersion.Original
        );

        screening.SetMovie(movie2);

        Assert.That(_movie.Screenings.Contains(screening), Is.False);
        Assert.That(movie2.Screenings.Contains(screening));
        Assert.That(screening.Movie, Is.EqualTo(movie2));
    }

    [Test]
    public void SetAuditorium_ShouldSwitchAuditoriumProperly()
    {
        var aud2 = new Auditorium(
            "A2",
            AuditoriumScreenType._3D,
            AuditoriumSoundsSystem.DolbyAtmos,
            2,
            c1
        );

        var screening = new Screening(
            10,
            _movie,
            _auditorium,
            DateTime.Now.AddDays(1),
            TimeSpan.FromHours(18),
            ScreeningFormat._2D,
            ScreeningVersion.Original
        );

        screening.SetAuditorium(aud2);

        Assert.That(_auditorium.Screenings.Contains(screening), Is.False);
        Assert.That(aud2.Screenings.Contains(screening));
        Assert.That(screening.Auditorium, Is.EqualTo(aud2));
    }

    [Test]
    public void Constructor_ShouldThrow_WhenMovieIsNull()
    {
        Assert.Throws<ArgumentException>(() =>
            new Screening(
                1,
                null,
                _auditorium,
                DateTime.Now.AddDays(1),
                TimeSpan.FromHours(12),
                ScreeningFormat._2D,
                ScreeningVersion.Original
            )
        );
    }

    [Test]
    public void Constructor_ShouldThrow_WhenAuditoriumIsNull()
    {
        Assert.Throws<ArgumentException>(() =>
            new Screening(
                1,
                _movie,
                null,
                DateTime.Now.AddDays(1),
                TimeSpan.FromHours(12),
                ScreeningFormat._2D,
                ScreeningVersion.Original
            )
        );
    }

    [Test]
    public void MovieAndAuditorium_ShouldAllowMultipleScreenings()
    {
        var s1 = new Screening(
            11,
            _movie,
            _auditorium,
            DateTime.Now.AddDays(1),
            TimeSpan.FromHours(10),
            ScreeningFormat._2D,
            ScreeningVersion.Original
        );

        var s2 = new Screening(
            12,
            _movie,
            _auditorium,
            DateTime.Now.AddDays(1),
            TimeSpan.FromHours(13),
            ScreeningFormat._3D,
            ScreeningVersion.Lector
        );

        var s3 = new Screening(
            13,
            _movie,
            _auditorium,
            DateTime.Now.AddDays(1),
            TimeSpan.FromHours(16),
            ScreeningFormat._2D,
            ScreeningVersion.Original
        );

        Assert.That(_movie.Screenings.Count, Is.EqualTo(3));
        Assert.That(_auditorium.Screenings.Count, Is.EqualTo(3));
    }
}
