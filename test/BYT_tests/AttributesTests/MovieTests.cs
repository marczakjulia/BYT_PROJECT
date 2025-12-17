using System.Reflection;
using BYT_Entities.Enums;
using BYT_Entities.Interfaces;
using BYT_Entities.Models;
using NUnit.Framework;

namespace TestByt;

public class MovieTests
{
    [SetUp]
    public void SetUp()
    {
        Movie.ClearMovies();
    }

    private static IEnumerable<IGenreType> DefaultGenres() =>
        new IGenreType[] { new ComedyMovie("satire") };

    [Test]
    public void MovieTitleIsEmpty()
    {
        Assert.Throws<ArgumentException>(() =>
            new NormalCut(
                1,
                "      ",
                "Poland",
                100,
                "desc",
                "Dir",
                AgeRestrictionType.PG13,
                DefaultGenres(),
                new Rerelease(1, "REASON", new DateTime(2025, 6, 10), true)
            ));
    }

    [Test]
    public void MovieCountryIsEmpty()
    {
        Assert.Throws<ArgumentException>(() =>
            new NormalCut(
                1,
                "Title",
                "   ",
                100,
                "desc",
                "Dir",
                AgeRestrictionType.PG13,
                DefaultGenres(),
                new Rerelease(1, "REASON", new DateTime(2025, 6, 10), true)
            ));
    }

    [Test]
    public void MovieLengthIsNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new NormalCut(
                1,
                "Title",
                "Poland",
                -10,
                "desc",
                "Dir",
                AgeRestrictionType.PG13,
                DefaultGenres(),
                new Rerelease(1, "REASON", new DateTime(2025, 6, 10), true)
            ));
    }

    [Test]
    public void MovieDescriptionIsEmpty()
    {
        Assert.Throws<ArgumentException>(() =>
            new NormalCut(
                1,
                "Title",
                "Poland",
                100,
                "   ",
                "Dir",
                AgeRestrictionType.PG13,
                DefaultGenres(),
                new Rerelease(1, "REASON", new DateTime(2025, 6, 10), true)
            ));
    }

    [Test]
    public void MovieDirectorIsEmpty()
    {
        Assert.Throws<ArgumentException>(() =>
            new NormalCut(
                1,
                "Title",
                "Poland",
                100,
                "desc",
                "   ",
                AgeRestrictionType.PG13,
                DefaultGenres(),
                new Rerelease(1, "REASON", new DateTime(2025, 6, 10), true)
            ));
    }

    [Test]
    public void MovieCreatedProperlyWithAgeRestriction()
    {
        Assert.DoesNotThrow(() =>
            new NormalCut(
                1,
                "Title",
                "Poland",
                100,
                "desc",
                "Dir",
                AgeRestrictionType.PG13,
                DefaultGenres(),
                new Rerelease(1, "REASON", new DateTime(2025, 6, 10), true)
            ));
    }

    [Test]
    public void MovieCreatedProperlyWithoutAgeRestriction()
    {
        Assert.DoesNotThrow(() =>
            new NormalCut(
                1,
                "Title",
                "Poland",
                100,
                "desc",
                "Dir",
                null,
                DefaultGenres(),
                new Rerelease(1, "REASON", new DateTime(2025, 6, 10), true)
            ));
    }

    [Test]
    public void Extent_ShouldStoreCreatedMovies()
    {
        var m1 = new NormalCut(
            1,
            "Movie 1",
            "Poland",
            100,
            "desc",
            "Dir",
            null,
            DefaultGenres(),
            new Rerelease(1, "REASON", new DateTime(2025, 6, 10), true)
        );

        var m2 = new NormalCut(
            2,
            "Movie 2",
            "Poland",
            120,
            "desc",
            "Dir",
            AgeRestrictionType.PG13,
            DefaultGenres(),
            new Rerelease(2, "REASON", new DateTime(2025, 6, 10), true)
        );

        var extent = Movie.GetMovies();

        Assert.AreEqual(2, extent.Count);
        Assert.Contains(m1, extent);
        Assert.Contains(m2, extent);
    }

    [Test]
    public void Encapsulation_ShouldPreventDirectModificationOfPrivateFields()
    {
        var movie = new NormalCut(
            1,
            "Original title",
            "Poland",
            120,
            "desc",
            "Dir",
            null,
            DefaultGenres(),
            new Rerelease(1, "REASON", new DateTime(2025, 6, 10), true)
        );

        var titleField = typeof(Movie)
            .GetField("_title", BindingFlags.NonPublic | BindingFlags.Instance);

        titleField!.SetValue(movie, "TamperedTitle");

        Assert.AreEqual("TamperedTitle", movie.Title);
    }
    
}
