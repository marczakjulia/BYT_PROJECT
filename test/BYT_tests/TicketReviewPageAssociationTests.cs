using BYT_Entities.Models;

namespace TestByt;

public class TicketReviewPageAssociationTests
{
    [SetUp]
    public void SetUp()
    {
        Ticket.ClearTickets();
    }

    [Test]
    public void AddReviewCreatesReverseConnection()
    {
        var ticket = new Ticket(19.99m, 1);
        var review = new ReviewPage("Mazhar", "Altincay", 8, 1, "Super actings!");

        ticket.AddReview(review);

        Assert.That(ticket.ReviewPage, Is.EqualTo(review));
        Assert.That(review.Ticket, Is.EqualTo(ticket));
    }

    [Test]
    public void AddReviewFromReviewSideCreatesReverseConnection()
    {
        var ticket = new Ticket(19.99m, 1);
        var review = new ReviewPage("Mazhar", "Altincay", 8, 1, "Super actings!");

        review.SetTicket(ticket);

        Assert.That(ticket.ReviewPage, Is.EqualTo(review));
        Assert.That(review.Ticket, Is.EqualTo(ticket));
    }

    [Test]
    public void RemoveReviewRemovesReverseConnection()
    {
        var ticket = new Ticket(19.99m, 1);
        var review = new ReviewPage("Mazhar", "Altincay", 8, 1, "Super actings!");
        ticket.AddReview(review);

        ticket.RemoveReview();

        Assert.That(ticket.ReviewPage, Is.Null);
        Assert.That(review.Ticket, Is.Null);
    }

    [Test]
    public void RemoveReviewFromReviewSideRemovesReverseConnection()
    {
        var ticket = new Ticket(19.99m, 1);
        var review = new ReviewPage("Mazhar", "Altincay", 8, 1, "Super actings!");
        ticket.AddReview(review);

        review.RemoveTicket();

        Assert.That(ticket.ReviewPage, Is.Null);
        Assert.That(review.Ticket, Is.Null);
    }

    [Test]
    public void AddReviewWhenTicketAlreadyHasReview()
    {
        var ticket = new Ticket(19.99m, 1);
        var review1 = new ReviewPage("Mazhar", "Altincay", 8, 1, "Super actings!");
        var review2 = new ReviewPage("Gulcin", "Altincay", 9, 2);
        ticket.AddReview(review1);

        var exception = Assert.Throws<InvalidOperationException>(() => ticket.AddReview(review2));
        Assert.That(exception.Message, Does.Contain("already has a review"));
    }

    [Test]
    public void SetTicketWhenReviewAlreadyHasTicket()
    {
        var ticket1 = new Ticket(19.99m, 1);
        var ticket2 = new Ticket(19.99m, 1);
        var review = new ReviewPage("Gulcin", "Altincay", 9, 2);
        review.SetTicket(ticket1);

        var exception = Assert.Throws<InvalidOperationException>(() => review.SetTicket(ticket2));
        Assert.That(exception.Message, Does.Contain("associated with another ticket"));
    }

    [Test]
    public void AddReviewWithNull()
    {
        var ticket = new Ticket(19.99m, 1);
        Assert.Throws<ArgumentException>(() => ticket.AddReview(null));
    }

    [Test]
    public void SetTicketWithNull()
    {
        var review = new ReviewPage("Mazhar", "Altincay", 8, 1, "Super actings!");
        Assert.Throws<ArgumentException>(() => review.SetTicket(null));
    }

    [Test]
    public void RemoveReviewWhenNoReviewExists()
    {
        var ticket = new Ticket(19.99m, 1);
        Assert.DoesNotThrow(() => ticket.RemoveReview());
        Assert.That(ticket.ReviewPage, Is.Null);
    }

    [Test]
    public void RemoveTicketWhenNoTicketExists()
    {
        var review = new ReviewPage("Mazhar", "Altincay", 8, 1, "Super actings!");
        Assert.DoesNotThrow(() => review.RemoveTicket());
        Assert.That(review.Ticket, Is.Null);
    }

    [Test]
    public void ReverseConnection_WorksFromBothSides()
    {
        var ticket1 = new Ticket(50.0m, 1);
        var ticket2 = new Ticket(60.0m, 2);
        var review1 = new ReviewPage("Mazhar", "Altincay", 8, 1, "Super actings!");
        var review2 = new ReviewPage("Gulcin", "Altincay", 5, 2);

        ticket1.AddReview(review1); // Connect from ticket side
        review2.SetTicket(ticket2); // Connect from review side

        Assert.That(ticket1.ReviewPage, Is.EqualTo(review1));
        Assert.That(review1.Ticket, Is.EqualTo(ticket1));
        Assert.That(ticket2.ReviewPage, Is.EqualTo(review2));
        Assert.That(review2.Ticket, Is.EqualTo(ticket2));
    }

    [Test]
    public void UpdateReviewReplacesOldReviewWithNewAndUpdatesReverseConnections()
    {
        var ticket = new Ticket(19.0m, 2);
        var oldReview = new ReviewPage("Mazhar", "Altincay", 8, 1, "Super actings!");
        var newReview = new ReviewPage("Gulcin", "Altincay", 5, 2);
        ticket.AddReview(oldReview);

        ticket.UpdateReview(newReview);

        Assert.That(ticket.ReviewPage, Is.EqualTo(newReview));
        Assert.That(newReview.Ticket, Is.EqualTo(ticket));
        Assert.That(oldReview.Ticket, Is.Null);
    }

    [Test]
    public void UpdateTicketReplacesOldTicketWithNewAndUpdatesReverseConnections()
    {
        var oldTicket = new Ticket(20.0m, 1);
        var newTicket = new Ticket(22.0m, 2);
        var review = new ReviewPage("Mazhar", "Altincay", 8, 1, "Super actings!");
        review.SetTicket(oldTicket);

        review.UpdateTicket(newTicket);

        Assert.That(review.Ticket, Is.EqualTo(newTicket));
        Assert.That(newTicket.ReviewPage, Is.EqualTo(review));
        Assert.That(oldTicket.ReviewPage, Is.Null);
    }
}