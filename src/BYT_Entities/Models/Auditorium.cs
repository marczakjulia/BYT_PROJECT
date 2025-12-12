using System.Xml;
using System.Xml.Serialization;
using BYT_Entities.Enums;

namespace BYT_Entities.Models;

[Serializable]
public class Auditorium
{
    private static List<Auditorium> _auditorium = new();
    private AuditoriumScreenType _auditoriumScreenType;
    private string _name;
    private HashSet<Seat> _seats = new();
    private AuditoriumSoundsSystem _soundSystem;
    private Cinema _cinema;

    public Cinema Cinema
    {
        get => _cinema;
        private set => _cinema = value;
    }

    
    [XmlIgnore]
    private HashSet<Screening> _screenings = new();

    [XmlIgnore]
    public IReadOnlyCollection<Screening> Screenings => _screenings;

    internal void AddScreeningInternal(Screening screening)
    {
        _screenings.Add(screening);
    }

    internal void RemoveScreeningInternal(Screening screening)
    {
        _screenings.Remove(screening);
    }

    public void AddCinema(Cinema cinema)
    {
        if (cinema == null)
            throw new ArgumentException("Cinema cannot be null.");

        if (_cinema == cinema)
            return;

        if (_cinema != null && _cinema != cinema)
            throw new InvalidOperationException("Auditorium already belongs to another cinema.");

        _cinema = cinema;

        cinema.AddAuditorium(this);
        
    }
    public static List<Auditorium> GetAll()
    {
        return new List<Auditorium>(_auditorium);
    }

    public static void RemoveFromGlobalList(Auditorium auditorium)
    {
        if (auditorium == null) throw new ArgumentException("Auditorium cannot be null.");
        _auditorium.Remove(auditorium);
    }

    public void RemoveCinema(Cinema cinema)
    {
        if (cinema == null)
            throw new ArgumentException("Cinema cannot be null.");

        if (_cinema != cinema)
            return;

        _cinema = null;

        cinema.RemoveAuditorium(this);
    }

    public Auditorium()
    {
        _seats = new HashSet<Seat>();
    }

    public Auditorium(string name, AuditoriumScreenType auditoriumScreenType,
        AuditoriumSoundsSystem soundSystem, int id, Cinema cinema)
    {
        Name = name;
        AuditoriumScreenType = auditoriumScreenType;
        SoundSystem = soundSystem;
        Id = id;
        _seats = new HashSet<Seat>();
        AddCinema(cinema);

        AddAuditorium(this);
    }

    public int Id { get; set; }

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Auditorium name cannot be empty.");
            _name = value;
        }
    }

    public AuditoriumScreenType AuditoriumScreenType
    {
        get => _auditoriumScreenType;
        set
        {
            if (!Enum.IsDefined(typeof(AuditoriumScreenType), value))
                throw new ArgumentException("Invalid screen type.");
            _auditoriumScreenType = value;
        }
    }

    public AuditoriumSoundsSystem SoundSystem
    {
        get => _soundSystem;
        set
        {
            if (!Enum.IsDefined(typeof(AuditoriumSoundsSystem), value))
                throw new ArgumentException("Invalid sound system type.");
            _soundSystem = value;
        }
    }

    [XmlIgnore] public HashSet<Seat> Seats => new(_seats);

    public void SetSeat(Seat seat)
    {
        if (seat == null)
            throw new ArgumentException("Seat cannot be null.");

        if (_seats.Contains(seat))
            return;

        _seats.Add(seat);

        if (seat.Auditorium != this)
            seat.SetAuditorium(this);
    }

    public static void ClearAuditorium()
    {
        _auditorium.Clear();
    }

    public void ValidateMinimumSeats()
    {
        if (_seats.Count < 12)
            throw new InvalidOperationException("Auditorium cannot have less than 12 seats.");
    }

    public int GetSeatCount()
    {
        return _seats.Count;
    }
    
    public void AddSeat(Seat seat)
    {
        if (seat == null)
            throw new ArgumentException("Cannot add null seat.");

        if (_seats.Contains(seat))
            return;

        _seats.Add(seat);

        // Ensure bidirectional link
        if (seat.Auditorium != this)
            seat.SetAuditorium(this);
    }

    public void RemoveSeat(Seat seat)
    {
        if (seat == null)
            throw new ArgumentException("Seat cannot be null.");

        if (!_seats.Contains(seat))
            throw new InvalidOperationException("Seat not found in this auditorium.");

        if (_seats.Count <= 12)
            throw new InvalidOperationException("Auditorium must have at least 12 seats.");

        _seats.Remove(seat);

        if (seat.Auditorium == this)
            seat.RemoveAuditorium();
    }

    public void SetSeats(IEnumerable<Seat> seats)
    {
        if (seats == null)
            throw new ArgumentException("Seats cannot be null.");

        var seatList = seats.ToList();
        if (seatList.Count < 12)
            throw new ArgumentException("There must be at least 12 seats.");

        foreach (var seat in _seats.ToList())
            RemoveSeat(seat);

        foreach (var seat in seatList)
            AddSeat(seat);
    }

    private static void AddAuditorium(Auditorium auditorium)
    {
        if (auditorium == null)
            throw new ArgumentException("Auditorium cannot be null.");

        _auditorium.Add(auditorium);
    }

    public static void Save(string path = "auditorium.xml")
    {
        var file = File.CreateText(path);
        var serializer = new XmlSerializer(typeof(List<Auditorium>));
        using (var writer = new XmlTextWriter(file))
        {
            serializer.Serialize(writer, _auditorium);
        }
    }

    public static bool Load(string path = "auditorium.xml")
    {
        StreamReader file;
        try
        {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException)
        {
            _auditorium.Clear();
            return false;
        }

        var serializer = new XmlSerializer(typeof(List<Auditorium>));
        using (var reader = new XmlTextReader(file))
        {
            try
            {
                _auditorium = (List<Auditorium>)serializer.Deserialize(reader);
            }
            catch (InvalidCastException)
            {
                _auditorium.Clear();
                return false;
            }
        }

        return true;
    }
}