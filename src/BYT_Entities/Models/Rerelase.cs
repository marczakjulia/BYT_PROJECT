using System.Xml;
using System.Xml.Serialization;

namespace BYT_Entities.Models;

[Serializable]
public class Rerelease
{
    public int Id { get; set; }
    private string _reason;
    private DateTime _reReleaseDate;
    public bool? Remastered { get; set; }
    private static List<Rerelease> Rereleases = new List<Rerelease>();
    public string Reason
    {
        get => _reason;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("reason cannot be empty");
            _reason = value.Trim();
        }
    }
    public DateTime ReReleaseDate
    {
        get => _reReleaseDate;
        set
        {
            if (value == default)
                throw new ArgumentException("release date cannot be null");
            _reReleaseDate = value;
        }
    }
    
    public Rerelease(int id, string reason, DateTime reReleaseDate, bool? remastered)
    {
        Id = id;
        Reason = reason;
        ReReleaseDate = reReleaseDate;
        Remastered = remastered;
        AddRerelease(this);
    }

    public Rerelease() { }
    
    private static void AddRerelease(Rerelease newRelease)
    {
        if (newRelease == null)
        {
            throw new ArgumentException("Rerelease cannot be null");
        }
        Rereleases.Add(newRelease);
    }

    public static void Save(string path = "rerelease.xml")
    {
        StreamWriter file = File.CreateText(path);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Rerelease>));
        using (XmlTextWriter writer = new XmlTextWriter(file))
        {
            xmlSerializer.Serialize(writer, Rereleases);
        }
    }

    public static bool Load(string path = "rerelease.xml")
    {
        StreamReader file;
        try
        {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException)
        {
            Rereleases.Clear();
            return false;
        }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Rerelease>));
        using (XmlTextReader reader = new XmlTextReader(file))
        {
            try
            {
                Rereleases = (List<Rerelease>)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException)
            {
                Rereleases.Clear();
                return false;
            }
            catch (Exception)
            {
                Rereleases.Clear();
                return false;
            }
        }
        return true;
    }
}