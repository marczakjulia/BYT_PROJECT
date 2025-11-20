using System.Reflection;
using BYT_Entities.Enums;
using BYT_Entities.Models;

namespace TestByt;

public class TicketTests
{
    [SetUp]
    public void SetUp()
    {
     Ticket.ClearTickets();   
    }
    
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
    
    //Extent checks
    [Test]
    public void ExtentShouldStoreCreatedTicket()
    {
        var ticket = new Ticket(10, 1);
        var ticket2 = new Ticket(10, 2);

        var extent = Ticket.GetTickets();
        
        Assert.AreEqual(2, extent.Count);
        Assert.Contains(ticket, extent);
        Assert.Contains(ticket2, extent);
    }

    [Test]
    public void Encapsulation_ShouldPreventExternalModification()
    {
        var ticket = new Ticket(10, 1);
        var extent = Ticket.GetTickets();
        extent = Ticket.GetTickets();
        extent.Clear();
        
        Assert.AreEqual(1, Ticket.GetTickets().Count);
        Assert.AreEqual(ticket, Ticket.GetTickets()[0]);
    }
    
    [Test]
    public void Encapsulation_ShouldPreventDirectModificationOfPrivateFields()
    {
        var ticket = new Ticket(10, 1);

        var priceField = typeof(Ticket)
            .GetField("_price", BindingFlags.NonPublic | BindingFlags.Instance);

        priceField.SetValue(ticket, 59m); 

        var extent = Ticket.GetTickets();

        //  stores the same instance - tampered value should be visible
        Assert.AreEqual(59m, extent[0].Price);
    }

    
    [Test]
    public void SaveLoad_ShouldPersistExtentCorrectly()
    {
        var path = "test_tickets.xml";

        var ticket = new Ticket(10, 1);
        var ticket2 = new Ticket(20, 2);

        Ticket.Save(path);

        Ticket.ClearTickets();
        Assert.AreEqual(0, Ticket.GetTickets().Count);

        var loaded = Ticket.Load(path);
        var extent = Ticket.GetTickets();

        Assert.IsTrue(loaded);
        Assert.AreEqual(2, extent.Count);

        Assert.AreEqual(10, extent[0].Price);
        Assert.AreEqual(20, extent[1].Price);

        if (File.Exists(path))
            File.Delete(path);
    }

}