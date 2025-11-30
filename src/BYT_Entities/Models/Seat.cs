using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using BYT_Entities.Enums;

namespace BYT_Entities.Models;

[Serializable]
public class Seat
{
    private static List<Seat> _seats = new();

    // 92A, 05J, 96Z
    private static readonly Regex SeatCodeSyntax = new(@"^[0-9]{2}[A-Z]$", RegexOptions.Compiled);

    [XmlIgnore] private Auditorium _auditorium;

    private string _code;
    private SeatType _type;

    public Seat(string code, SeatType type, int id)
    {
        Code = code;
        Type = type;
        _auditorium = null;
        Id = id;

        AddSeats(this);
    }

    public Seat()
    {
    }

    public int Id { get; set; }


    public string Code
    {
        get => _code;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Seat code can not be empty.");

            if (!SeatCodeSyntax.IsMatch(value))
                throw new ArgumentException(
                    "Seat code must have the pattern of two digits followed by one uppercase letter.");

            _code = value;
        }
    }

    public SeatType Type
    {
        get => _type;
        set
        {
            if (!Enum.IsDefined(typeof(SeatType), value))
                throw new ArgumentException("Invalid seat type.");
        }
    }

    [XmlIgnore] public Auditorium Auditorium => _auditorium;

    public void SetAuditorium(Auditorium auditorium)
    {
        if (auditorium == null)
            throw new ArgumentException("Auditorium can not be null.");

        if (_auditorium == auditorium)
            return;

        if (_auditorium != null)
            throw new InvalidOperationException("Seat is already assigned to an auditorium.");

        _auditorium = auditorium;
        auditorium.SetSeat(this);
    }

    public void RemoveAuditorium()
    {
        if (_auditorium == null)
            return;

        var auditoriumToRemove = _auditorium;
        _auditorium = null;

        auditoriumToRemove.RemoveSeat(this);
    }

    private static void AddSeats(Seat seat)
    {
        if (seat == null)
            throw new ArgumentException("Seat cannot be null.");

        _seats.Add(seat);
    }

    public static void Save(string path = "seat.xml")
    {
        var file = File.CreateText(path);
        var serializer = new XmlSerializer(typeof(List<Seat>));
        using (var writer = new XmlTextWriter(file))
        {
            serializer.Serialize(writer, _seats);
        }
    }

    public static bool Load(string path = "seat.xml")
    {
        StreamReader file;
        try
        {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException)
        {
            _seats.Clear();
            return false;
        }

        var serializer = new XmlSerializer(typeof(List<Seat>));
        using (var reader = new XmlTextReader(file))
        {
            try
            {
                _seats = (List<Seat>)serializer.Deserialize(reader);
            }
            catch (InvalidCastException)
            {
                _seats.Clear();
                return false;
            }
        }

        return true;
    }

    public static Seat? GetSeatByCode(List<Seat?> seats, string code)
    {
        foreach (var seat in seats)
            if (seat.Code.Equals(code, StringComparison.Ordinal))
                return seat;
        return null;
    }
}