namespace TestByt;

using BYT_Entities.Models;
using NUnit.Framework;


public class MovieReviewAssociationTests
{
    private Movie _movie;

    [SetUp]
    public void Setup()
    {
        Movie.ClearMovies();
        _movie = new Movie(1, "TestMovie", "USA", 120, "desc", "dir", null,
            new Rerelease(1, "rr", DateTime.Now, true));
    }
    [Test]
    public void ReviewConstructor_ShouldCreateBidirectionalLink()
    {
        var review = new ReviewPage("A", "B", 8, 1, _movie);

        Assert.That(_movie.GetReviews().Contains(review));
        Assert.That(review.Movie, Is.EqualTo(_movie));
    }
    [Test]
    public void AddReview_ShouldAddToMovieAndSetReverseConnection()
    {
        var review = new ReviewPage("A", "B", 7, 1, _movie);
        Assert.That(_movie.GetReviews().Contains(review));
        Assert.That(review.Movie, Is.EqualTo(_movie));
    }
    
    [Test]
    public void RemoveReview_ShouldRemoveFromBothSides()
    {
        var review = new ReviewPage("A", "B", 8, 1, _movie);

        _movie.RemoveReview(review);

        Assert.That(_movie.GetReviews().Contains(review), Is.False);
        Assert.That(review.Movie, Is.Null);
    }
    [Test]
    public void SetMovie_ShouldSwitchMovieProperly()
    {
        var movie2 = new Movie(2, "Other", "UK", 90, "d", "x", null,
            new Rerelease(2, "r", DateTime.Now, false));

        var review = new ReviewPage("A", "B", 8, 1, _movie);

        review.SetMovie(movie2);

        Assert.That(_movie.GetReviews().Contains(review), Is.False);
        Assert.That(movie2.GetReviews().Contains(review));
        Assert.That(review.Movie, Is.EqualTo(movie2));
    }

    [Test]
    public void Movie_ShouldAllowMultipleReviews()
    {
        var r1 = new ReviewPage("A", "B", 5, 1, _movie);
        var r2 = new ReviewPage("C", "D", 7, 2, _movie);
        var r3 = new ReviewPage("E", "F", 9, 3, _movie);

        Assert.That(_movie.GetReviews().Count, Is.EqualTo(3));
        Assert.That(_movie.GetReviews().Contains(r1));
        Assert.That(_movie.GetReviews().Contains(r2));
        Assert.That(_movie.GetReviews().Contains(r3));
    }
    
    [Test]
    public void AddReview_ShouldThrow_WhenNull()
    {
        Assert.Throws<ArgumentException>(() => _movie.AddReview(null));
    }
    
    [Test]
    public void ReviewConstructor_ShouldThrow_WhenMovieIsNull()
    {
        Assert.Throws<ArgumentException>(() =>
            new ReviewPage("A", "B", 8, 1, null));
    }
    [Test]
    public void RemoveReview_ShouldThrow_WhenNotAssociated()
    {
        var r = new ReviewPage("A", "B", 8, 1, _movie);
        _movie.RemoveReview(r); // first remove succeeds

        Assert.Throws<InvalidOperationException>(() => _movie.RemoveReview(r));
    }
    [Test]
    public void GetReviews_ShouldReturnCopy_NotActualCollection()
    {
        var r = new ReviewPage("A", "B", 8, 1, _movie);

        var copy = _movie.GetReviews();
        copy.Clear(); 

        Assert.That(_movie.GetReviews().Count, Is.EqualTo(1));
    }
}
