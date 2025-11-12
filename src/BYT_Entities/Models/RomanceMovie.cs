namespace BYT_Entities.Models;

public class RomanceMovie
{
    public int Id { get; set; }
    private int _intensity;
    private List<string>? _inappropriateScenes;
    public int Intensity
    {
        get => _intensity;
        set
        {
            if (value < 1 || value > 5)
                throw new ArgumentOutOfRangeException(nameof(value), "intensity must be between 1 and 5.");
            _intensity = value;
        }
    }
    public List<string>? InappropriateScenes
    {
        get => _inappropriateScenes;
        set
        {
            if (value != null && value.Any(s => string.IsNullOrWhiteSpace(s)))
                throw new ArgumentException("inappropriate scenes cannot be empty");

            _inappropriateScenes = value?.Select(s => s.Trim()).ToList();
        }
    }
    public RomanceMovie(int id, int intensity, List<string>? inappropriateScenes = null)
    {
        Id = id;
        Intensity = intensity;
        InappropriateScenes = inappropriateScenes;
    }

    public RomanceMovie() { }
}