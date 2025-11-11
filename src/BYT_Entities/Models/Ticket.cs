using BYT_Entities.Enums;
using Microsoft.VisualBasic.CompilerServices;

namespace BYT_Entities.Models;

public class Ticket
{
    private decimal _price;
    private TicketStatus _status;
    private string? _reasonOfRefundOrExpiration;
    private TicketPaymentType _paymentType;

    public decimal Price
    {
        get => _price;
        set
        {
            if(value <= 0)
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
    
    //initial step
    public Ticket(decimal price)
    {
        Price = price;
        Status = TicketStatus.Available;
        PaymentType = TicketPaymentType.None;
        ReasonOfRefundOrExpiration = null;
    }

    public Ticket(decimal price, TicketStatus status, TicketPaymentType paymentType, string? reason = null)
    {
        Price = price;
        Status = status;

        if ((status == TicketStatus.Refunded || status == TicketStatus.Expired) 
            && string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reason must be provided when ticket is refunded or expired.");

        ReasonOfRefundOrExpiration = reason;
        PaymentType = paymentType;
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
        if(Status != TicketStatus.Purchased)
            throw new ArgumentException("Ticket can only be refunded if it was purchased.");
        
        //checking 24h rule
        TimeSpan difference = screeningTime - currentTime;
        if (difference.TotalHours >= 24)
        {
            Status = TicketStatus.Refunded;
            ReasonOfRefundOrExpiration = reason;
        }
        else
        {
            {
                Status = TicketStatus.Expired;
                ReasonOfRefundOrExpiration = "Ticket expired due to not refunding on time(should refund earlier than 24h)";
            }
        }
    }

    public void ScanTicket(DateTime currentTime, DateTime screeningTime)
    {
        if(Status != TicketStatus.Purchased)
            throw new ArgumentException("Ticket can only be scanned if it was purchased.");

        if (currentTime <= screeningTime)
            Status = TicketStatus.Scanned;
        else
        {
            TimeSpan minsAfterStart = currentTime - screeningTime;
            if (minsAfterStart.TotalMinutes <= 30)
                Status = TicketStatus.Scanned;
            else
            {
                Status = TicketStatus.Expired;
                ReasonOfRefundOrExpiration = "Ticket is not scanned within the first 30 minutes after screening started";
            }
        }
            
    }
}