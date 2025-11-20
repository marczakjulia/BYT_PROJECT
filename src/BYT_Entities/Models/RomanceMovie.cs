using System.Xml;
using System.Xml.Serialization;

namespace BYT_Entities.Models;
[Serializable]
public class RomanceMovie
{
    public int Id { get; set; }
    private int _intensity;
    private List<string>? _inappropriateScenes;
    private static List<RomanceMovie> RomanceMovies = new List<RomanceMovie>();
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
        AddRomanceMovie(this);
    }

    public RomanceMovie() { }
       
    private static void AddRomanceMovie(RomanceMovie romance)
    {
        if (romance == null)
        {
            throw new ArgumentException("Romance cannot be null");
        }
        RomanceMovies.Add(romance);
    }

    public static void Save(string path = "romance.xml")
    {
        StreamWriter file = File.CreateText(path);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<RomanceMovie>));
        using (XmlTextWriter writer = new XmlTextWriter(file))
        {
            xmlSerializer.Serialize(writer, RomanceMovies);
        }
    }

    public static bool Load(string path = "romance.xml")
    {
        StreamReader file;
        try
        {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException)
        {
            RomanceMovies.Clear();
            return false;
        }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<RomanceMovie>));
        using (XmlTextReader reader = new XmlTextReader(file))
        {
            try
            {
                RomanceMovies = (List<RomanceMovie>)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException)
            {
                RomanceMovies.Clear();
                return false;
            }
            catch (Exception)
            {
                RomanceMovies.Clear();
                return false;
            }
        }
        return true;
    }
    public static List<RomanceMovie> GetRomanceMovies()
    {
        return new List<RomanceMovie>(RomanceMovies);
    }
    public static void ClearRomanceMovies()
    {
        RomanceMovies.Clear();
    }
}