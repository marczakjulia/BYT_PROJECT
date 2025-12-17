using BYT_Entities.Interfaces;
using BYT_Entities.Models;
using NUnit.Framework;

namespace TestByt;

public class ReviewPageTests
{
    private static IEnumerable<IGenreType> Genres() =>
        new IGenreType[] { new ComedyMovie("satire") };

    [Test]
    public void ReviewPageCreatedProperly()
    {
        var movie = new NormalCut(
            1,
            "Test",
            "USA",
            1,
            "test",
            "test",
            null,
            Genres(),
            new Rerelease(1, "REASON", new DateTime(2025, 6, 10), true)
        );

        var review = new ReviewPage("Mazhar", "Altincay", 9, 1, movie, "Nice movie");

        Assert.That(review.Name, Is.EqualTo("Mazhar"));
        Assert.That(review.Surname, Is.EqualTo("Altincay"));
        Assert.That(review.Rate, Is.EqualTo(9));
        Assert.That(review.Comment, Is.EqualTo("Nice movie"));
    }

    [Test]
    public void ReviewPageEmptyNameOrSurname()
    {
        var movie = new NormalCut(
            1,
            "Test",
            "USA",
            1,
            "test",
            "test",
            null,
            Genres(),
            new Rerelease(1, "REASON", new DateTime(2025, 6, 10), true)
        );

        Assert.Throws<ArgumentException>(() =>
            new ReviewPage("", "Altincay", 7, 1, movie));

        Assert.Throws<ArgumentException>(() =>
            new ReviewPage("Mazhar", "", 7, 1, movie));
    }

    [Test]
    public void ReviewPageInvalidRate_Throws()
    {
        var movie = new NormalCut(
            1,
            "Test",
            "USA",
            1,
            "test",
            "test",
            null,
            Genres(),
            new Rerelease(1, "REASON", new DateTime(2025, 6, 10), true)
        );

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ReviewPage("Mazhar", "Altincay", 0, 1, movie));

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ReviewPage("Mazhar", "Altincay", 11, 1, movie));
    }
}
