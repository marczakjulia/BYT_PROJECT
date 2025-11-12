using BYT_Entities.Models;

namespace TestByt;

public class ReviewPageTests
{
    [Test]
    public void ReviewPageCreatedProperly()
    {
        var review = new ReviewPage("Mazhar", "Altincay", 9, 1, "Nice movie");
        Assert.That(review.Name, Is.EqualTo("Mazhar"));
        Assert.That(review.Surname, Is.EqualTo("Altincay"));
        Assert.That(review.Rate, Is.EqualTo(9));
        Assert.That(review.Comment, Is.EqualTo("Nice movie"));
    }

    [Test]
    public void ReviewPageEmptyNameOrSurname()
    {
        Assert.Throws<ArgumentException>(() => new ReviewPage("", "Altincay", 7, 1));
        Assert.Throws<ArgumentException>(() => new ReviewPage("Mazhar", "", 7, 1));
    }

    [Test]
    public void ReviewPageInvalidRate_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new ReviewPage("Mazhar", "Altincay", 0, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new ReviewPage("Mazhar", "Altincay", 11, 1));
    }
}