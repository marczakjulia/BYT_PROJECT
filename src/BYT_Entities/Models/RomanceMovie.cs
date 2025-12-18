using System.Xml;
using System.Xml.Serialization;
using BYT_Entities.Interfaces;

namespace BYT_Entities.Models;
[Serializable]
public class RomanceMovie: IGenreType
{
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
    //FOR NEW ASSIGMENT
    public string GetGenreName() => "Romance";
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
    public RomanceMovie(int intensity, List<string>? inappropriateScenes = null)
    {
        Intensity = intensity;
        InappropriateScenes = inappropriateScenes;
    }

    public RomanceMovie() { }
    
}