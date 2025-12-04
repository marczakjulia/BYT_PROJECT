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
    
    [XmlIgnore]
    public Screening Screening { get; private set; }
    [XmlIgnore]
    public string SeatCode { get; private set; }

    public Ticket(decimal price, int id)
    {
        Price = price;
        Id = id;
        Status = TicketStatus.Available;
        PaymentType = TicketPaymentType.None;
        ReasonOfRefundOrExpiration = null;

        AddTicket(this);
    }

    public Ticket()
    {
    }

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

    [XmlIgnore]
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

    [XmlIgnore]
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

    [XmlIgnore]
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

    [XmlIgnore] public ReviewPage? ReviewPage { get; private set; }

    public static List<Ticket> GetTickets()
    {
        return new List<Ticket>(_tickets);
    }

    public static void ClearTickets()
    {
        _tickets.Clear();
    }

    private static void AddTicket(Ticket ticket)
    {
        if (ticket == null)
            throw new ArgumentException("Ticket cannot be null.");

        _tickets.Add(ticket);
    }
    
    internal void SetScreeningInternal(Screening screening, string seatCode)
    {
        Screening = screening ?? throw new ArgumentException("Screening cannot be null.");
        SeatCode = seatCode ?? throw new ArgumentException("SeatCode cannot be null.");
    }

    internal void RemoveScreeningInternal()
    {
        Screening = null;
        SeatCode = null;
    }


    public static void Save(string path = "ticket.xml")
    {
        var file = File.CreateText(path);
        var serializer = new XmlSerializer(typeof(List<Ticket>));
        using (var writer = new XmlTextWriter(file))
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

        var serializer = new XmlSerializer(typeof(List<Ticket>));
        using (var reader = new XmlTextReader(file))
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

    public void AddReview(ReviewPage reviewPage)
    {
        if (reviewPage == null)
            throw new ArgumentException("Review page cannot be null.");

        if (ReviewPage == reviewPage)
            return;

        if (ReviewPage != null)
            throw new InvalidOperationException("Ticket has already has a review page.");

        if (reviewPage.Ticket != null && reviewPage.Ticket != this)
            throw new InvalidOperationException("This review is already associated with another ticket.");

        ReviewPage = reviewPage;

        //creating reverse connection
        if (reviewPage.Ticket != this)
            reviewPage.SetTicket(this);
    }

    public void RemoveReview()
    {
        if (ReviewPage == null)
            return;
        var reviewPageToRemove = ReviewPage;
        ReviewPage = null;

        if (reviewPageToRemove.Ticket == this)
            reviewPageToRemove.RemoveTicket();
    }

    public void UpdateReview(ReviewPage newReview)
    {
        if (newReview == null)
            throw new ArgumentException("New review cannot be null.");

        if (ReviewPage == newReview)
            return;

        if (newReview.Ticket != null && newReview.Ticket != this)
            throw new InvalidOperationException("The new review is already associated with another ticket.");

        var oldReview = ReviewPage;
        if (oldReview != null)
        {
            ReviewPage = null;
            if (oldReview.Ticket == this)
                oldReview.RemoveTicket();
        }

        ReviewPage = newReview;
        if (newReview.Ticket != this)
            newReview.SetTicket(this);
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