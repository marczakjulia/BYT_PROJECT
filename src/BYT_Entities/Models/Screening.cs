using BYT_Entities.Enums;

namespace BYT_Entities.Models;

public class Screening
{
    // private Movie _movie;
    public int Id { get; set; }
    private Auditorium _auditorium;
    private DateTime _date;
    private TimeSpan _startTime;
    private ScreeningFormat _format;
    private ScreeningVersion _version;
    private ScreeningStatus _status;
    private Dictionary<string, Ticket> _ticketsBySeatCode;

    // public Movie Movie
    // {
    //     get => _movie;
    //     set
    //     {
    //         if (value == null)
    //             throw new ArgumentNullException(nameof(Movie), "Movie cannot be null.");
    //         _movie = value;
    //     }
    // }

    public Auditorium Auditorium
    {
        get => _auditorium;
        set
        {
            if (value == null)
                throw new ArgumentNullException(nameof(Auditorium), "Auditorium cannot be null.");
            _auditorium = value;
        }
    }

    public DateTime Date
    {
        get => _date;
        set
        {
            // we cannot set a screening date in the past
            if (value < DateTime.Now.Date)
                throw new ArgumentException("Screening date cannot be set in the past.");

            _date = value;
        }
    }

    public TimeSpan StartTime
    {
        get => _startTime;
        set
        {
            //same start time should be in the future
            if (Date.Date == DateTime.Now.Date && value < DateTime.Now.TimeOfDay)
                throw new ArgumentException("Start time cannot be in the past for today's date.");

            _startTime = value;
        }
    }

    public ScreeningFormat Format
    {
        get => _format;
        set
        {
            if (!Enum.IsDefined(typeof(ScreeningFormat), value))
                throw new ArgumentException("Invalid screening format.");
            _format = value;
        }
    }

    public ScreeningVersion Version
    {
        get => _version;
        set
        {
            if (!Enum.IsDefined(typeof(ScreeningVersion), value))
                throw new ArgumentException("Invalid screening version.");
            _version = value;
        }
    }
    
    public ScreeningStatus Status
    {
        get => _status;
        private set
        {
            if (!Enum.IsDefined(typeof(ScreeningStatus), value))
                throw new ArgumentException("Invalid screening status.");
            _status = value;
        }
    }

    //read-only
    public Dictionary<string, Ticket> TicketsBySeatCode => _ticketsBySeatCode;

    // public Screening(int id, Movie movie, Auditorium auditorium, DateTime date, TimeSpan startTime, ScreeningFormat format, ScreeningVersion version)
    // {
    //     Id = id;
    //     Movie = movie;
    //     Auditorium = auditorium;
    //     Date = date;
    //     StartTime = startTime;
    //     Format = format;
    //     Version = version;
    //     Status = ScreeningStatus.Planned;
    //     _ticketsBySeatCode = new Dictionary<string, Ticket>();
    // }

    public void CreateScreening(decimal price)
    {
        if (_ticketsBySeatCode.Count > 0)
            throw new InvalidOperationException("Tickets have already been created");

        int ticketId = 1;

        foreach (var seat in Auditorium.Seats)
        { 
            _ticketsBySeatCode.Add(seat.Code, new Ticket(price, ticketId));
            ticketId++;
        }

        
        Status = ScreeningStatus.Planned;
    }

    public void StartScreening()
    {
        if(Status != ScreeningStatus.Planned)
            throw new InvalidOperationException("Screening can only be started if it is planned.");

        Status = ScreeningStatus.Running;
    }

    public void CancelScreening()
    {
        if(Status != ScreeningStatus.Planned && Status != ScreeningStatus.Running)
            throw new InvalidOperationException("Screening can only be cancelled if it is planned or running.");
        
        Status = ScreeningStatus.Canceled;
        
        foreach (var ticket in _ticketsBySeatCode.Values)
        {
            if (ticket.Status == TicketStatus.Purchased || ticket.Status == TicketStatus.Scanned)
                ticket.RefundTicket(DateTime.Now, Date, "Screening cancelled by employee");
        }
    }
    
    public void FinishScreening()
    {
        if (Status != ScreeningStatus.Running)
            throw new InvalidOperationException("Screening can only be finished if it is Running.");

        Status = ScreeningStatus.Finished;
    }

    public bool IsSeatAvailable(string seatCode)
    {
        if(!_ticketsBySeatCode.ContainsKey(seatCode))
            throw new ArgumentException($"Seat code {seatCode} does not exist in this screening.");
        
        var ticket = _ticketsBySeatCode[seatCode];
        return ticket.Status != TicketStatus.Purchased && ticket.Status != TicketStatus.Scanned;
    }

    public List<Seat> GetAvailableSeats()
    {
        return _ticketsBySeatCode
            .Where(dict => dict.Value.Status != TicketStatus.Scanned && dict.Value.Status != TicketStatus.Purchased)
            .Select(dict => _auditorium.Seats.First(seat => seat.Code == dict.Key))
            .ToList();
    }

    
    // public static List<Screening> GetAllScreeningsForAMovie(List<Screening> screenings, Movie movie)
    // {
    //     if (movie == null)
    //         throw new ArgumentException("Movie cannot be null.");
    //
    //     return screenings.Where(s => s.Movie == movie).ToList();
    // }
}