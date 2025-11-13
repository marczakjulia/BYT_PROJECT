using System.Xml;
using System.Xml.Serialization;

namespace BYT_Entities.Models;

[Serializable]
public class ComedyMovie
{
    private static List<ComedyMovie> ComedyMoviesList = new List<ComedyMovie>();
    public int Id { get; set; }
    private string _humorType;
    public string HumorType
    {
        get => _humorType;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("humor type cannot be null or empty");
            _humorType = value.Trim();
        }
    }
    public ComedyMovie(int id, string humorType)
    {
        Id = id;
        HumorType = humorType;
        AddComedyMovie(this);
    }

    private static void AddComedyMovie(ComedyMovie comedyMovie)
    {
        if (comedyMovie == null)
        {
            throw new ArgumentException("comedyMovie cannot be null");
        }
        ComedyMoviesList.Add(comedyMovie);
    }
    public ComedyMovie() { }

    public static void Save(string path = "comedymovies.xml")
    {
        StreamWriter file = File.CreateText(path);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ComedyMovie>));
        using (XmlTextWriter writer = new XmlTextWriter(file))
        {
            xmlSerializer.Serialize(writer, ComedyMoviesList);
        }
    }

    public static bool Load(string path = "comedymovies.xml")
    {
        StreamReader file;
        try
        {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException)
        {
            ComedyMoviesList.Clear();
            return false;
        }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ComedyMovie>));
        using (XmlTextReader reader = new XmlTextReader(file))
        {
            try
            {
                ComedyMoviesList = (List<ComedyMovie>)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException)
            {
                ComedyMoviesList.Clear();
                return false;
            }
            catch (Exception)
            {
                ComedyMoviesList.Clear();
                return false;
            }
        }
        return true;
    }
}