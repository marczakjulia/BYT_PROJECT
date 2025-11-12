using BYT_Entities.Enums;
using BYT_Entities.Models;

namespace TestByt;

public class TicketTests
{
    [Test]
    public void TicketCreatedProperlyDefaultConstructor()
    {
        Assert.DoesNotThrow(() =>
        {
            var ticket = new Ticket(15.5m, 1);
            Assert.That(ticket.Price, Is.EqualTo(15.5m));
            Assert.That(ticket.Status, Is.EqualTo(TicketStatus.Available));
            Assert.That(ticket.PaymentType, Is.EqualTo(TicketPaymentType.None));
        });
    }

    [Test]
    public void TicketPriceZeroOrNegative_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Ticket(0, 1));
        Assert.Throws<ArgumentException>(() => new Ticket(-10, 1));
    }

    [Test]
    public void TicketRefundWithoutReason()
    {
        Assert.Throws<ArgumentException>(() =>
            new Ticket(20, TicketStatus.Refunded, TicketPaymentType.CreditCard, 1));
    }

    [Test]
    public void TicketRefundWithReason()
    {
        var ticket = new Ticket(20, TicketStatus.Refunded, TicketPaymentType.CreditCard, 1, "Customer request");
        Assert.That(ticket.ReasonOfRefundOrExpiration, Is.EqualTo("Customer request"));
    }

    [Test]
    public void SellTicketChangesStatusAndPaymentType()
    {
        var ticket = new Ticket(10, 1);
        ticket.SellTicket(TicketPaymentType.Blik);
        Assert.That(ticket.Status, Is.EqualTo(TicketStatus.Purchased));
        Assert.That(ticket.PaymentType, Is.EqualTo(TicketPaymentType.Blik));
    }

    [Test]
    public void RefundTicketUpdatesStatus()
    {
        var ticket = new Ticket(10, 1);
        ticket.SellTicket(TicketPaymentType.Cash);
        ticket.RefundTicket(DateTime.Now, DateTime.Now.AddDays(2), "Customer request");
        Assert.That(ticket.Status, Is.EqualTo(TicketStatus.Refunded));
    }

    [Test]
    public void ScanTicketInTime()
    {
        var ticket = new Ticket(10, 1);
        ticket.SellTicket(TicketPaymentType.Cash);
        ticket.ScanTicket(DateTime.Now, DateTime.Now.AddHours(1));
        Assert.That(ticket.Status, Is.EqualTo(TicketStatus.Scanned));
    }

    [Test]
    public void ScanTicketAfter30Minutes()
    {
        var ticket = new Ticket(10, 1);
        ticket.SellTicket(TicketPaymentType.Cash);
        ticket.ScanTicket(DateTime.Now, DateTime.Now.AddMinutes(-31));
        Assert.That(ticket.Status, Is.EqualTo(TicketStatus.Expired));
    }
}