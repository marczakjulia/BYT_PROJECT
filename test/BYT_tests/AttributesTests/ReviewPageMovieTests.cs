using BYT_Entities.Interfaces;
using BYT_Entities.Models;
using NUnit.Framework;

namespace TestByt;

public class ReviewPageMovieTests
{
    private static IEnumerable<IGenreType> Genres() =>
        new IGenreType[] { new ComedyMovie("satire") };

    [Test]
    public void CreatingReviewAutomaticallyAddsItToMovie()
    {
        var movie = new NormalCut(
            1, "Test", "USA", 1, "test", "test", null,
            Genres(),
            new Rerelease(1, "REASON", new DateTime(2025, 6, 10), true)
        );

        var review = new ReviewPage("John", "Doe", 8, 1, movie, "Nice");

        Assert.That(movie.GetReviews().Count, Is.EqualTo(1));
        Assert.That(movie.GetReviews().Contains(review), Is.True);
    }

    [Test]
    public void AddReview_AddsReviewAndUpdatesReverseLink()
    {
        var movie = new NormalCut(
            1, "Test", "USA", 1, "test", "test", null,
            Genres(),
            new Rerelease(1, "REASON", new DateTime(2025, 6, 10), true)
        );

        var review = new ReviewPage("John", "Doe", 8, 1, movie);

        Assert.That(review.Movie, Is.EqualTo(movie));
        Assert.That(movie.GetReviews(), Contains.Item(review));
    }

    [Test]
    public void AddReview_Duplicate_Throws()
    {
        var movie = new NormalCut(
            1, "Test", "USA", 1, "test", "test", null,
            Genres(),
            new Rerelease(1, "REASON", new DateTime(2025, 6, 10), true)
        );

        var review = new ReviewPage("John", "Doe", 8, 1, movie);

        Assert.Throws<InvalidOperationException>(() => movie.AddReview(review));
    }

    [Test]
    public void RemoveReview_RemovesBothSides()
    {
        var movie = new NormalCut(
            1, "Test", "USA", 1, "test", "test", null,
            Genres(),
            new Rerelease(1, "REASON", new DateTime(2025, 6, 10), true)
        );

        var review = new ReviewPage("John", "Doe", 8, 1, movie);

        movie.RemoveReview(review);

        Assert.That(movie.GetReviews().Count, Is.EqualTo(0));
        Assert.That(review.Movie, Is.Null);
    }

    [Test]
    public void RemoveReview_NotAssociated_Throws()
    {
        var movie1 = new NormalCut(
            1, "Test", "USA", 1, "test", "test", null,
            Genres(),
            new Rerelease(1, "REASON", new DateTime(2025, 6, 10), true)
        );

        var movie2 = new NormalCut(
            2, "Other", "USA", 100, "Desc", "Dir", null,
            Genres(),
            new Rerelease(2, "REASON", new DateTime(2025, 6, 10), true)
        );

        var review = new ReviewPage("John", "Doe", 8, 1, movie1);

        Assert.Throws<InvalidOperationException>(() => movie2.RemoveReview(review));
    }

    [Test]
    public void MovieCanHaveZeroReviews()
    {
        var movie = new NormalCut(
            1, "Test", "USA", 1, "test", "test", null,
            Genres(),
            new Rerelease(1, "REASON", new DateTime(2025, 6, 10), true)
        );

        Assert.That(movie.GetReviews().Count, Is.EqualTo(0));
    }

    [Test]
    public void ReviewMustAlwaysHaveMovie()
    {
        var movie = new NormalCut(
            1, "Test", "USA", 1, "test", "test", null,
            Genres(),
            new Rerelease(1, "REASON", new DateTime(2025, 6, 10), true)
        );

        var review = new ReviewPage("John", "Doe", 8, 1, movie);

        Assert.That(review.Movie, Is.Not.Null);
    }

    [Test]
    public void ChangeMovie_RemovesFromOldMovieAndAddsToNew()
    {
        var movie1 = new NormalCut(
            1, "OldMovie", "USA", 100, "Desc", "Dir", null,
            Genres(),
            new Rerelease(1, "REASON", new DateTime(2025, 6, 10), true)
        );

        var movie2 = new NormalCut(
            2, "NewMovie", "UK", 90, "D", "Dir", null,
            Genres(),
            new Rerelease(2, "REASON", new DateTime(2025, 6, 10), true)
        );

        var review = new ReviewPage("John", "Doe", 8, 1, movie1);

        review.SetMovie(movie2);

        Assert.That(review.Movie, Is.EqualTo(movie2));
        Assert.That(movie2.GetReviews(), Contains.Item(review));
        Assert.That(movie1.GetReviews(), Does.Not.Contain(review));
    }

    [Test]
    public void CreatingReview_WithNullMovie_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            new ReviewPage("John", "Doe", 8, 1, null!));
    }

    [Test]
    public void RemoveReview_CalledTwice_SecondCallThrows()
    {
        var movie = new NormalCut(
            1, "Test", "USA", 100, "Desc", "Dir", null,
            Genres(),
            new Rerelease(1, "REASON", new DateTime(2025, 6, 10), true)
        );

        var review = new ReviewPage("John", "Doe", 8, 1, movie);

        movie.RemoveReview(review);

        Assert.Throws<InvalidOperationException>(() => movie.RemoveReview(review));
    }

    [Test]
    public void SetMovie_WithNullMovie_Throws()
    {
        var movie = new NormalCut(
            1, "Test", "USA", 1, "test", "test", null,
            Genres(),
            new Rerelease(1, "REASON", new DateTime(2025, 6, 10), true)
        );

        var review = new ReviewPage("John", "Doe", 8, 1, movie);

        Assert.Throws<ArgumentException>(() => review.SetMovie(null!));
    }

    [Test]
    public void SetMovie_SameMovie_DoesNotDuplicate()
    {
        var movie = new NormalCut(
            1, "Test", "USA", 100, "Desc", "Dir", null,
            Genres(),
            new Rerelease(1, "REASON", new DateTime(2025, 6, 10), true)
        );

        var review = new ReviewPage("John", "Doe", 8, 1, movie);

        review.SetMovie(movie);

        Assert.That(movie.GetReviews().Count, Is.EqualTo(1));
    }
}
