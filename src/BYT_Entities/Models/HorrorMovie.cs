using System.Xml;
using System.Xml.Serialization;
using BYT_Entities.Interfaces;

namespace BYT_Entities.Models;

[Serializable]
public class HorrorMovie : IGenreType
{
    private int _brutalityRating;
    private List<string>? _jumpScares; // nullable
    
    //FOR NEW ASSIGMENT 
    
    public string GetGenreName() => "Horror";
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
    public HorrorMovie(int brutalityRating, List<string>? jumpScares = null)
    {
        BrutalityRating = brutalityRating;
        JumpScares = jumpScares; // can be null cause [0..*] as in our uml diagram (there are some horror movies which do not have jump scares)
    }

    public HorrorMovie() { }
    
}