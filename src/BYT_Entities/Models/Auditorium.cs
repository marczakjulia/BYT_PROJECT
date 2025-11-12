using BYT_Entities.Enums;

namespace BYT_Entities.Models;

public class Auditorium
{
    public int Id { get; set; }
    private string _name;
    private ScreenType _screenType;
    private AuditoriumSoundsSystem _soundSystem;
    private List<Seat> _seats;

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

    public ScreenType ScreenType
    {
        get => _screenType;
        set
        {
            if (!Enum.IsDefined(typeof(ScreenType), value))
                throw new ArgumentException("Invalid screen type.");
        }
    }

    public AuditoriumSoundsSystem SoundSystem
    {
        get => _soundSystem;
        set
        {
            if (!Enum.IsDefined(typeof(AuditoriumSoundsSystem), value))
                throw new ArgumentException("Invalid sound system type.");
        }
    }
    
    public List<Seat> Seats
    {
        get => _seats;
        set
        {
            if(value == null)
                throw new ArgumentException("Seats cannot be null.");
            if(value.Count < 12)
                throw new ArgumentException("There must be at least 12 seats");
            _seats = value;
        }
    }
    
    public Auditorium(string name, ScreenType screenType, AuditoriumSoundsSystem soundSystem, List<Seat> seats, int id)
    {
        Name = name;
        ScreenType = screenType;
        SoundSystem = soundSystem;
        Seats = seats;
        Id = id;
    }

    // public bool IsAvailable(DateTime date, TimeSpan startTime, TimeSpan movieDuration, List<Screening> screenings)
    // {
    //     if (screenings == null)
    //         throw new ArgumentException("Screenings list cannot be null.");
    //
    //     return !screenings.Any(s =>
    //         s.Auditorium == this &&
    //         s.Date.Date == date.Date &&
    //         s.Status != ScreeningStatus.Canceled &&
    //         s.Status != ScreeningStatus.Finished &&
    //         (startTime < s.StartTime + s.Movie.TotalLength && s.StartTime < startTime + movieDuration)
    //     );
    // }
    
    // public static List<Auditorium> GetAvailableAuditoriums(List<Auditorium> auditoriums, DateTime date, TimeSpan startTime, List<Screening> screenings)
    // {
    //     if (auditoriums == null)
    //         throw new ArgumentException("Auditoriums list cannot be null.");
    //
    //     return auditoriums.Where(a => a.IsAvailable(date, startTime, screenings)).ToList();
    // }
    
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