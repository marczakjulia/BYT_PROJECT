using BYT_Entities.Enums;
using BYT_Entities.Interfaces;
using BYT_Entities.Models;
using NUnit.Framework;

namespace TestByt;

public class XorTest
{
    [SetUp]
    public void SetUp()
    {
        Movie.ClearMovies();
        NewRelease.ClearNewRleaseMovies();
        Rerelease.ClearReleaseMovies();
    }

    [Test]
    public void CreateMovie_WithNewRelease_ShouldSetReverseConnection()
    {
        var nr = new NewRelease(1, true, DateTime.Now, "Disney");
        var movie = CreateMovieWithNewRelease(nr);

        Assert.That(movie.NewRelease, Is.EqualTo(nr));
        Assert.That(nr.Movie, Is.EqualTo(movie));
        Assert.That(movie.Rerelease, Is.Null);
    }

    [Test]
    public void CreateMovie_WithRerelease_ShouldSetReverseConnection()
    {
        var rr = new Rerelease(1, "Reason", DateTime.Now, false);
        var movie = CreateMovieWithRerelease(rr);

        Assert.That(movie.Rerelease, Is.EqualTo(rr));
        Assert.That(rr.Movie, Is.EqualTo(movie));
        Assert.That(movie.NewRelease, Is.Null);
    }

    [Test]
    public void CannotAssignNewRelease_WhenRereleaseAlreadyAssigned()
    {
        var rr = new Rerelease(1, "Reason", DateTime.Now, false);
        var nr = new NewRelease(2, true, DateTime.Now, "Warner");

        var movie = CreateMovieWithRerelease(rr);

        Assert.Throws<InvalidOperationException>(() =>
            movie.SetNewRelease(nr));
    }

    [Test]
    public void CannotAssignRerelease_WhenNewReleaseAlreadyAssigned()
    {
        var nr = new NewRelease(1, true, DateTime.Now, "Sony");
        var rr = new Rerelease(2, "Reason", DateTime.Now, false);

        var movie = CreateMovieWithNewRelease(nr);

        Assert.Throws<InvalidOperationException>(() =>
            movie.SetRerelease(rr));
    }

    [Test]
    public void RemoveNewRelease_ShouldBreakReverseConnection()
    {
        var nr = new NewRelease(1, true, DateTime.Now, "Universal");
        var movie = CreateMovieWithNewRelease(nr);

        movie.RemoveNewRelease();

        Assert.That(movie.NewRelease, Is.Null);
        Assert.That(nr.Movie, Is.Null);
    }

    [Test]
    public void RemoveRerelease_ShouldBreakReverseConnection()
    {
        var rr = new Rerelease(1, "Reason", DateTime.Now, true);
        var movie = CreateMovieWithRerelease(rr);

        movie.RemoveRerelease();

        Assert.That(movie.Rerelease, Is.Null);
        Assert.That(rr.Movie, Is.Null);
    }

    [Test]
    public void UpdateNewRelease_ShouldReplaceReferenceAndKeepXor()
    {
        var nr1 = new NewRelease(1, true, DateTime.Now, "A24");
        var nr2 = new NewRelease(2, false, DateTime.Now, "Netflix");

        var movie = CreateMovieWithNewRelease(nr1);

        movie.UpdateNewRelease(nr2);

        Assert.That(movie.NewRelease, Is.EqualTo(nr2));
        Assert.That(nr2.Movie, Is.EqualTo(movie));
        Assert.That(nr1.Movie, Is.Null);
        Assert.That(movie.Rerelease, Is.Null);
    }

    [Test]
    public void UpdateRerelease_ShouldReplaceReferenceAndKeepXor()
    {
        var rr1 = new Rerelease(1, "Old", DateTime.Now, false);
        var rr2 = new Rerelease(2, "New", DateTime.Now, true);

        var movie = CreateMovieWithRerelease(rr1);

        movie.UpdateRerelease(rr2);

        Assert.That(movie.Rerelease, Is.EqualTo(rr2));
        Assert.That(rr2.Movie, Is.EqualTo(movie));
        Assert.That(rr1.Movie, Is.Null);
        Assert.That(movie.NewRelease, Is.Null);
    }

    [Test]
    public void SetNewRelease_WithNull_ShouldThrow()
    {
        var movie = CreateMovieWithNewRelease(
            new NewRelease(1, true, DateTime.Now, "Pixar"));

        Assert.Throws<ArgumentException>(() => movie.SetNewRelease(null));
    }

    [Test]
    public void SetRerelease_WithNull_ShouldThrow()
    {
        var movie = CreateMovieWithRerelease(
            new Rerelease(1, "Reason", DateTime.Now, true));

        Assert.Throws<ArgumentException>(() => movie.SetRerelease(null));
    }

    [Test]
    public void UpdateNewRelease_WhenMovieIsRerelease_ShouldThrow()
    {
        var movie = CreateMovieWithRerelease(
            new Rerelease(1, "R", DateTime.Now, false));

        var nr = new NewRelease(2, true, DateTime.Now, "Disney");

        Assert.Throws<InvalidOperationException>(() =>
            movie.UpdateNewRelease(nr));
    }

    [Test]
    public void UpdateRerelease_WhenMovieIsNewRelease_ShouldThrow()
    {
        var movie = CreateMovieWithNewRelease(
            new NewRelease(1, true, DateTime.Now, "Disney"));

        var rr = new Rerelease(2, "R", DateTime.Now, false);

        Assert.Throws<InvalidOperationException>(() =>
            movie.UpdateRerelease(rr));
    }
    

    private static Movie CreateMovieWithNewRelease(NewRelease nr)
        => new NormalCut(
            1, "Title", "USA", 100, "Desc", "Dir",
            AgeRestrictionType.PG13,
            new IGenreType[] { new ComedyMovie("satire") },
            nr
        );

    private static Movie CreateMovieWithRerelease(Rerelease rr)
        => new NormalCut(
            1, "Title", "USA", 100, "Desc", "Dir",
            AgeRestrictionType.PG13,
            new IGenreType[] { new ComedyMovie("satire") },
            rr
        );
}
