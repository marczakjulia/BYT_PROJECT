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
    private List<Seat> _seats;
    private AuditoriumSoundsSystem _soundSystem;

    public Auditorium()
    {
    }

    public Auditorium(string name, AuditoriumScreenType auditoriumScreenType, AuditoriumSoundsSystem soundSystem,
        List<Seat> seats, int id)
    {
        Name = name;
        AuditoriumScreenType = auditoriumScreenType;
        SoundSystem = soundSystem;
        Seats = seats;
        Id = id;
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

    public List<Seat> Seats
    {
        get => new(_seats);
        set
        {
            if (value == null)
                throw new ArgumentException("Seats cannot be null.");
            if (value.Count < 12)
                throw new ArgumentException("There must be at least 12 seats");
            _seats = value;
        }
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

    public void RemoveSeat(Seat seat)
    {
        if (seat == null)
            throw new ArgumentException("Cannot remove null seat.");

        if (_seats.Count <= 12)
            throw new InvalidOperationException("Cannot remove seat: auditorium must have at least 12 seats.");

        _seats.Remove(seat);
    }
}