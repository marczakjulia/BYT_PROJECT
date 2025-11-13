using System.Xml;
using System.Xml.Serialization;
using BYT_Entities.Enums;

namespace BYT_Entities.Models;

[Serializable]
public class Ticket
{
    private static List<Ticket> _tickets = new();
    private TicketPaymentType _paymentType;
    private decimal _price;
    private string? _reasonOfRefundOrExpiration;
    private TicketStatus _status;

    public Ticket(decimal price, int id)
    {
        Price = price;
        Id = id;
        Status = TicketStatus.Available;
        PaymentType = TicketPaymentType.None;
        ReasonOfRefundOrExpiration = null;
    }
    
    public Ticket(){}

    public Ticket(decimal price, TicketStatus status, TicketPaymentType paymentType, int id, string? reason = null)
    {
        Price = price;
        Status = status;

        if ((status == TicketStatus.Refunded || status == TicketStatus.Expired)
            && string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reason must be provided when ticket is refunded or expired.");

        ReasonOfRefundOrExpiration = reason;
        PaymentType = paymentType;
        Id = id;
        
        AddTicket(this);
    }


    private static void AddTicket(Ticket ticket)
    {
        if (ticket == null)
            throw new ArgumentException("Ticket cannot be null.");

        _tickets.Add(ticket);
    }

    public static void Save(string path = "ticket.xml")
    {
        StreamWriter file = File.CreateText(path);
        XmlSerializer serializer = new XmlSerializer(typeof(List<Ticket>));
        using (XmlTextWriter writer = new XmlTextWriter(file))
        {
            serializer.Serialize(writer, _tickets);
        }
    }

    public static bool Load(string path = "ticket.xml")
    {
        StreamReader file;
        try
        {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException)
        {
            _tickets.Clear();
            return false;
        }

        XmlSerializer serializer = new XmlSerializer(typeof(List<Ticket>));
        using (XmlTextReader reader = new XmlTextReader(file))
        {
            try
            {
                _tickets = (List<Ticket>)serializer.Deserialize(reader);
            }
            catch (InvalidCastException)
            {
                _tickets.Clear();
                return false;
            }
        }

        return true;
    }

    public int Id { get; set; }

    public decimal Price
    {
        get => _price;
        set
        {
            if (value <= 0)
                throw new ArgumentException("Price cannot be less or equal to zero");
            _price = value;
        }
    }

    public TicketStatus Status
    {
        get => _status;
        // By default, when the screening is set, tickets status is available
        private set
        {
            if (!Enum.IsDefined(typeof(TicketStatus), value))
                throw new ArgumentException("Invalid ticket status.");
            _status = value;
        }
    }

    public string? ReasonOfRefundOrExpiration
    {
        get => _reasonOfRefundOrExpiration;
        private set
        {
            if ((Status == TicketStatus.Refunded || Status == TicketStatus.Expired)
                && string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Reason must be provided as refunded or expired");
            _reasonOfRefundOrExpiration = value;
        }
    }

    public TicketPaymentType PaymentType
    {
        get => _paymentType;
        private set
        {
            if (!Enum.IsDefined(typeof(TicketPaymentType), value))
                throw new ArgumentException("Invalid payment type.");
            _paymentType = value;
        }
    }
    

    public void SellTicket(TicketPaymentType paymentType)
    {
        // Can another customer buy a ticket when its status is Refunded?
        if (Status != TicketStatus.Available)
            throw new ArgumentException("Ticket can only be sold if its status is available.");

        PaymentType = paymentType;
        Status = TicketStatus.Purchased;
    }

    public void RefundTicket(DateTime currentTime, DateTime screeningTime, string reason)
    {
        if (Status != TicketStatus.Purchased)
            throw new ArgumentException("Ticket can only be refunded if it was purchased.");

        //checking 24h rule
        var difference = screeningTime - currentTime;
        if (difference.TotalHours >= 24)
        {
            Status = TicketStatus.Refunded;
            ReasonOfRefundOrExpiration = reason;
        }
        else
        {
            {
                Status = TicketStatus.Expired;
                ReasonOfRefundOrExpiration =
                    "Ticket expired due to not refunding on time(should refund earlier than 24h)";
            }
        }
    }

    public void ScanTicket(DateTime currentTime, DateTime screeningTime)
    {
        if (Status != TicketStatus.Purchased)
            throw new ArgumentException("Ticket can only be scanned if it was purchased.");

        if (currentTime <= screeningTime)
        {
            Status = TicketStatus.Scanned;
        }
        else
        {
            var minsAfterStart = currentTime - screeningTime;
            if (minsAfterStart.TotalMinutes <= 30)
            {
                Status = TicketStatus.Scanned;
            }
            else
            {
                Status = TicketStatus.Expired;
                ReasonOfRefundOrExpiration =
                    "Ticket is not scanned within the first 30 minutes after screening started";
            }
        }
    }
}