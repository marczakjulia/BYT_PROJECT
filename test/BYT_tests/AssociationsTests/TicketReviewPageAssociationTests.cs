using BYT_Entities.Enums;
using BYT_Entities.Interfaces;
using BYT_Entities.Models;
using NUnit.Framework;

namespace TestByt;

public class TicketReviewPageAssociationTests
{
    private static Movie CreateMovie()
    {
        var genres = new IGenreType[]
        {
            new ComedyMovie("satire")
        };

        return new NormalCut(
            id: 1,
            title: "Test",
            countryOfOrigin: "USA",
            length: 1,
            description: "test",
            director: "test",
            ageRestriction: null,
            genres: genres,
            reRelease: new Rerelease(
                1,
                "REASON",
                new DateTime(2025, 6, 10),
                true
            )
        );
    }

    [SetUp]
    public void SetUp()
    {
        Ticket.ClearTickets();
    }

    [Test]
    public void AddReviewCreatesReverseConnection()
    {
        var movie = CreateMovie();
        var ticket = new Ticket(19.99m, 1);
        var review = new ReviewPage("Mazhar", "Altincay", 8, 1, movie, "Super actings!");

        ticket.AddReview(review);

        Assert.That(ticket.ReviewPage, Is.EqualTo(review));
        Assert.That(review.Ticket, Is.EqualTo(ticket));
    }

    [Test]
    public void AddReviewFromReviewSideCreatesReverseConnection()
    {
        var movie = CreateMovie();
        var ticket = new Ticket(19.99m, 1);
        var review = new ReviewPage("Mazhar", "Altincay", 8, 1, movie, "Super actings!");

        review.SetTicket(ticket);

        Assert.That(ticket.ReviewPage, Is.EqualTo(review));
        Assert.That(review.Ticket, Is.EqualTo(ticket));
    }

    [Test]
    public void RemoveReviewRemovesReverseConnection()
    {
        var movie = CreateMovie();
        var ticket = new Ticket(19.99m, 1);
        var review = new ReviewPage("Mazhar", "Altincay", 8, 1, movie, "Super actings!");

        ticket.AddReview(review);
        ticket.RemoveReview();

        Assert.That(ticket.ReviewPage, Is.Null);
        Assert.That(review.Ticket, Is.Null);
    }

    [Test]
    public void RemoveReviewFromReviewSideRemovesReverseConnection()
    {
        var movie = CreateMovie();
        var ticket = new Ticket(19.99m, 1);
        var review = new ReviewPage("Mazhar", "Altincay", 8, 1, movie, "Super actings!");

        ticket.AddReview(review);
        review.RemoveTicket();

        Assert.That(ticket.ReviewPage, Is.Null);
        Assert.That(review.Ticket, Is.Null);
    }

    [Test]
    public void AddReviewWhenTicketAlreadyHasReview()
    {
        var movie = CreateMovie();
        var ticket = new Ticket(19.99m, 1);
        var review1 = new ReviewPage("Mazhar", "Altincay", 8, 1, movie);
        var review2 = new ReviewPage("Gulcin", "Altincay", 9, 2, movie);

        ticket.AddReview(review1);

        Assert.Throws<InvalidOperationException>(() =>
            ticket.AddReview(review2));
    }

    [Test]
    public void SetTicketWhenReviewAlreadyHasTicket()
    {
        var movie = CreateMovie();
        var ticket1 = new Ticket(19.99m, 1);
        var ticket2 = new Ticket(19.99m, 2);
        var review = new ReviewPage("Gulcin", "Altincay", 9, 2, movie);

        review.SetTicket(ticket1);

        Assert.Throws<InvalidOperationException>(() =>
            review.SetTicket(ticket2));
    }

    [Test]
    public void AddReviewWithNull()
    {
        var ticket = new Ticket(19.99m, 1);

        Assert.Throws<ArgumentException>(() =>
            ticket.AddReview(null));
    }

    [Test]
    public void SetTicketWithNull()
    {
        var movie = CreateMovie();
        var review = new ReviewPage("Mazhar", "Altincay", 8, 1, movie);

        Assert.Throws<ArgumentException>(() =>
            review.SetTicket(null));
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
        var movie = CreateMovie();
        var review = new ReviewPage("Mazhar", "Altincay", 8, 1, movie);

        Assert.DoesNotThrow(() => review.RemoveTicket());
        Assert.That(review.Ticket, Is.Null);
    }

    [Test]
    public void ReverseConnection_WorksFromBothSides()
    {
        var movie = CreateMovie();
        var ticket1 = new Ticket(50.0m, 1);
        var ticket2 = new Ticket(60.0m, 2);

        var review1 = new ReviewPage("Mazhar", "Altincay", 8, 1, movie);
        var review2 = new ReviewPage("Gulcin", "Altincay", 5, 2, movie);

        ticket1.AddReview(review1);
        review2.SetTicket(ticket2);

        Assert.That(ticket1.ReviewPage, Is.EqualTo(review1));
        Assert.That(review1.Ticket, Is.EqualTo(ticket1));
        Assert.That(ticket2.ReviewPage, Is.EqualTo(review2));
        Assert.That(review2.Ticket, Is.EqualTo(ticket2));
    }

    [Test]
    public void UpdateReviewReplacesOldReviewAndUpdatesReverseConnections()
    {
        var movie = CreateMovie();
        var ticket = new Ticket(19.0m, 2);

        var oldReview = new ReviewPage("Mazhar", "Altincay", 8, 1, movie);
        var newReview = new ReviewPage("Gulcin", "Altincay", 5, 2, movie);

        ticket.AddReview(oldReview);
        ticket.UpdateReview(newReview);

        Assert.That(ticket.ReviewPage, Is.EqualTo(newReview));
        Assert.That(newReview.Ticket, Is.EqualTo(ticket));
        Assert.That(oldReview.Ticket, Is.Null);
    }

    [Test]
    public void UpdateTicketReplacesOldTicketAndUpdatesReverseConnections()
    {
        var movie = CreateMovie();
        var oldTicket = new Ticket(20.0m, 1);
        var newTicket = new Ticket(22.0m, 2);
        var review = new ReviewPage("Mazhar", "Altincay", 8, 1, movie);

        review.SetTicket(oldTicket);
        review.UpdateTicket(newTicket);

        Assert.That(review.Ticket, Is.EqualTo(newTicket));
        Assert.That(newTicket.ReviewPage, Is.EqualTo(review));
        Assert.That(oldTicket.ReviewPage, Is.Null);
    }
}
