using System.Xml;
using System.Xml.Serialization;
using BYT_Entities.Enums;

namespace BYT_Entities.Models;

[Serializable]
public class Screening
{
    private static List<Screening> _screenings = new();
    private Auditorium _auditorium;
    private DateTime _date;
    private ScreeningFormat _format;
    private Movie _movie;
    private TimeSpan _startTime;
    private ScreeningStatus _status;
    private ScreeningVersion _version;
    [XmlIgnore]
    public Movie Movie => _movie;
    [XmlIgnore]
    public Auditorium Auditorium => _auditorium;
    
    private readonly Dictionary<string, Ticket> _ticketsBySeatCode = new();
    public IReadOnlyDictionary<string, Ticket> TicketsBySeatCode => _ticketsBySeatCode;

    public Screening(int id, Movie movie, Auditorium auditorium, DateTime date, TimeSpan startTime,
        ScreeningFormat format, ScreeningVersion version)
    {
        Id = id;
        Date = date;
        StartTime = startTime;
        Format = format;
        Version = version;
        Status = ScreeningStatus.Planned;
        SetMovie(movie ?? throw new ArgumentException("Movie cannot be null."));
        SetAuditorium(auditorium ?? throw new ArgumentException("Auditorium cannot be null."));
        AddScreening(this);
    }


    public Screening()
    {
    }

    public int Id { get; set; }
    
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
    
    public void CreateTickets(decimal price)
    {
        int ticketId = 1;

        foreach (var seat in Auditorium.Seats)
        {
            var ticket = new Ticket(price, ticketId++);
            AddTicket(seat.Code, ticket);
        }
    }

    public void AddTicket(string seatCode, Ticket ticket)
    {
        if (ticket == null) 
            throw new ArgumentException( "Ticket cannot be null.");
        if (_ticketsBySeatCode.ContainsKey(seatCode))
            throw new InvalidOperationException($"A ticket for seat {seatCode} already exists.");

        if (ticket.Screening != null && ticket.Screening != this)
            throw new InvalidOperationException("Ticket belongs to another screening.");

        _ticketsBySeatCode.Add(seatCode, ticket);

        // reverse update
        if (ticket.Screening != this || ticket.SeatCode != seatCode)
            ticket.SetScreening(this, seatCode);
    }


    public void RemoveTicket(string seatCode)
    {
        if (_ticketsBySeatCode.Remove(seatCode, out var ticket))
        {
            if (ticket.Screening == this)
                ticket.RemoveScreening();
        }
    }
    
    public void SetMovie(Movie movie)
    {
        if (movie == null)
            throw new ArgumentException("Movie cannot be null.");

        if (_movie == movie)
            return;

        if (_movie != null)
            _movie.RemoveScreeningInternal(this);

        _movie = movie;
        _movie.AddScreeningInternal(this);
    }

    public void RemoveMovie()
    {
        if (_movie == null)
            return;

        var oldMovie = _movie;
        _movie = null;
        oldMovie.RemoveScreeningInternal(this);
    }
    
    public void SetAuditorium(Auditorium auditorium)
    {
        if (auditorium == null)
            throw new ArgumentException("Auditorium cannot be null.");

        if (_auditorium == auditorium)
            return;

        if (_auditorium != null)
            _auditorium.RemoveScreeningInternal(this);

        _auditorium = auditorium;
        _auditorium.AddScreeningInternal(this);
    }

    public void RemoveAuditorium()
    {
        if (_auditorium == null)
            return;

        var oldAuditorium = _auditorium;
        _auditorium = null;
        oldAuditorium.RemoveScreeningInternal(this);
    }


    
    public void RemoveCompletely()
    {
        RemoveMovie();
        RemoveAuditorium();
    }

    private static void AddScreening(Screening screening)
    {
        if (screening == null)
            throw new ArgumentException("Screening cannot be null.");

        _screenings.Add(screening);
    }

    public static void Save(string path = "screening.xml")
    {
        var file = File.CreateText(path);
        var serializer = new XmlSerializer(typeof(List<Screening>));
        using (var writer = new XmlTextWriter(file))
        {
            serializer.Serialize(writer, _screenings);
        }
    }

    public static bool Load(string path = "screening.xml")
    {
        StreamReader file;
        try
        {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException)
        {
            _screenings.Clear();
            return false;
        }

        var serializer = new XmlSerializer(typeof(List<Screening>));
        using (var reader = new XmlTextReader(file))
        {
            try
            {
                _screenings = (List<Screening>)serializer.Deserialize(reader);
            }
            catch (InvalidCastException)
            {
                _screenings.Clear();
                return false;
            }
        }

        return true;
    }

    public void CreateScreening(decimal price)
    {
        if (_ticketsBySeatCode.Count > 0)
            throw new InvalidOperationException("Tickets have already been created");

        var ticketId = 1;

        foreach (var seat in Auditorium.Seats)
        {
            var ticket = new Ticket(price, ticketId++);
            _ticketsBySeatCode.Add(seat.Code, ticket);
        }
        Status = ScreeningStatus.Planned;
    }


    public void StartScreening()
    {
        if (Status != ScreeningStatus.Planned)
            throw new InvalidOperationException("Screening can only be started if it is planned.");

        Status = ScreeningStatus.Running;
    }

    public void CancelScreening()
    {
        if (Status != ScreeningStatus.Planned && Status != ScreeningStatus.Running)
            throw new InvalidOperationException("Screening can only be cancelled if it is planned or running.");

        Status = ScreeningStatus.Canceled;

        foreach (var ticket in TicketsBySeatCode.Values)
            if (ticket.Status == TicketStatus.Purchased || ticket.Status == TicketStatus.Scanned)
                ticket.RefundTicket(DateTime.Now, Date, "Screening cancelled by employee");
    }

    public void FinishScreening()
    {
        if (Status != ScreeningStatus.Running)
            throw new InvalidOperationException("Screening can only be finished if it is Running.");

        Status = ScreeningStatus.Finished;
    }

    public bool IsSeatAvailable(string seatCode)
    {
        if (!TicketsBySeatCode.ContainsKey(seatCode))
            throw new ArgumentException($"Seat code {seatCode} does not exist in this screening.");

        var ticket = TicketsBySeatCode[seatCode];
        return ticket.Status != TicketStatus.Purchased && ticket.Status != TicketStatus.Scanned;
    }

    public List<Seat> GetAvailableSeats()
    {
        return TicketsBySeatCode
            .Where(dict => dict.Value.Status != TicketStatus.Scanned && dict.Value.Status != TicketStatus.Purchased)
            .Select(dict => _auditorium.Seats.First(seat => seat.Code == dict.Key))
            .ToList();
    }


    public static List<Screening> GetAllScreeningsForAMovie(List<Screening> screenings, Movie movie)
    {
        if (movie == null)
            throw new ArgumentException("Movie cannot be null.");

        return screenings.Where(s => s.Movie == movie).ToList();
    }
}