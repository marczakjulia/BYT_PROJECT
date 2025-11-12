namespace BYT_Entities.Models;

public class HorrorMovie
{
    public int Id { get; set; }
    private int _brutalityRating;
    private List<string>? _jumpScares; // nullable

    public int BrutalityRating
    {
        get => _brutalityRating;
        set
        {
            if (value < 0 || value > 10)
                throw new ArgumentOutOfRangeException(nameof(value), "brutality rating must be between 0 and 10");
            _brutalityRating = value;
        }
    }

    public List<string>? JumpScares
    {
        get => _jumpScares;
        set
        {
            if (value != null && value.Any(js => string.IsNullOrWhiteSpace(js)))
                throw new ArgumentException("there cannot be an empty string in jump scares");
            _jumpScares = value?.Select(js => js.Trim()).ToList();
        }
    }
    public HorrorMovie(int id, int brutalityRating, List<string>? jumpScares = null)
    {
        Id = id;
        BrutalityRating = brutalityRating;
        JumpScares = jumpScares; // can be null cause [0..*] as in our uml diagram (there are some horror movies which do not have jump scares)
    }

    public HorrorMovie() { }
}