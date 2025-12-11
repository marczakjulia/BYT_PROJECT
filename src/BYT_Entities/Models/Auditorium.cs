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
    public Cinema Cinema { get; set; }

    public void AddCinema(Cinema cinema)
    {
        if (cinema == null)
            throw new ArgumentException("Cinema cannot be null.");

        if (this.Cinema == cinema)
            return;

        if (this.Cinema != null && this.Cinema != cinema)
            throw new InvalidOperationException("Auditorium already belongs to another cinema.");

        this.Cinema = cinema;

        cinema.AddAuditorium(this);
    }
    //needed for compisition
    public static void RemoveFromGlobalList(Auditorium auditorium)
    {
        if (auditorium == null) throw new ArgumentException("Auditorium cannot be null.");
        _auditorium.Remove(auditorium);
    }

    public void RemoveCinema(Cinema cinema)
    {
        if (cinema == null)
            throw new ArgumentException("Cinema cannot be null.");

        if (this.Cinema != cinema)
            return;

        this.Cinema = null;

        cinema.RemoveAuditorium(this);
    }


    public Auditorium()
    {
        _seats = new HashSet<Seat>();
    }

    public Auditorium(string name, AuditoriumScreenType auditoriumScreenType,
        AuditoriumSoundsSystem soundSystem, int id)
    {
        Name = name;
        AuditoriumScreenType = auditoriumScreenType;
        SoundSystem = soundSystem;
        Id = id;
        _seats = new HashSet<Seat>();

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

    [XmlIgnore] public HashSet<Seat> Seats => new(_seats); // Return copy

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

    public void RemoveSeat(Seat seat)
    {
        if (seat == null)
            throw new ArgumentException("Seat cannot be null.");

        if (_seats.Count <= 12)
            throw new InvalidOperationException("Cannot remove seat. Auditorium must have at least 12 seats.");

        if (!_seats.Contains(seat))
            throw new InvalidOperationException("Seat not found in this auditorium.");

        _seats.Remove(seat);

        if (seat.Auditorium == this)
            seat.RemoveAuditorium();
    }

    public void ClearAuditorium()
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

    public void SetSeats(HashSet<Seat> seats)
    {
        if (seats == null)
            throw new ArgumentException("Seats cannot be null.");

        if (seats.Count < 12)
            throw new ArgumentException("There must be at least 12 seats");

        foreach (var seat in _seats.ToList()) RemoveSeat(seat);

        foreach (var seat in seats) SetSeat(seat);
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

    public void AddSeat(Seat seat)
    {
        if (seat == null)
            throw new ArgumentException("Cannot add null seat.");

        _seats.Add(seat);
    }
}