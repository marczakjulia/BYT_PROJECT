using System.Xml;
using System.Xml.Serialization;

namespace BYT_Entities.Models;

[Serializable]
public class HorrorMovie
{
    public int Id { get; set; }
    private int _brutalityRating;
    private List<string>? _jumpScares; // nullable
    private static List<HorrorMovie> HorrorMoviesList = new List<HorrorMovie>();

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
        AddHorrorMovie(this);
    }

    public HorrorMovie() { }
    
    private static void AddHorrorMovie(HorrorMovie horror)
    {
        if (horror == null)
        {
            throw new ArgumentException("horror cannot be null");
        }
        HorrorMoviesList.Add(horror);
    }

    public static void Save(string path = "horrormovies.xml")
    {
        StreamWriter file = File.CreateText(path);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<HorrorMovie>));
        using (XmlTextWriter writer = new XmlTextWriter(file))
        {
            xmlSerializer.Serialize(writer, HorrorMoviesList);
        }
    }

    public static bool Load(string path = "horrormovies.xml")
    {
        StreamReader file;
        try
        {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException)
        {
            HorrorMoviesList.Clear();
            return false;
        }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<HorrorMovie>));
        using (XmlTextReader reader = new XmlTextReader(file))
        {
            try
            {
                HorrorMoviesList = (List<HorrorMovie>)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException)
            {
                HorrorMoviesList.Clear();
                return false;
            }
            catch (Exception)
            {
                HorrorMoviesList.Clear();
                return false;
            }
        }
        return true;
    }
}