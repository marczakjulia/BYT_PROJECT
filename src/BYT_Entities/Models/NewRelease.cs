using System.Xml;
using System.Xml.Serialization;

namespace BYT_Entities.Models;
[Serializable]
public class NewRelease
{
    public int Id { get; set; }
    private static List<NewRelease> NewReleaseList = new List<NewRelease>();
    public bool IsExclusiveToCinema { get; set; }
    private DateTime _premiereDate;
    private string _distributor;
    public DateTime PremiereDate
    {
        get => _premiereDate;
        set
        {
            if (value == default)
                throw new ArgumentException("premiere date cannot be null");
            _premiereDate = value;
        }
    }
    public string Distributor
    {
        get => _distributor;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("cannot be null or whitespace");
            _distributor = value.Trim();
        }
    }

    public NewRelease(int id, bool isExclusiveToCinema, DateTime premiereDate, string distributor)
    {
        Id = id;
        IsExclusiveToCinema = isExclusiveToCinema;
        PremiereDate = premiereDate;
        Distributor = distributor;
        AddNewRelease(this);
    }

    public NewRelease() { }
    
    private static void AddNewRelease(NewRelease newRelease)
    {
        if (newRelease == null)
        {
            throw new ArgumentException("newRelease cannot be null");
        }
        NewReleaseList.Add(newRelease);
    }

    public static void Save(string path = "newrelease.xml")
    {
        StreamWriter file = File.CreateText(path);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<NewRelease>));
        using (XmlTextWriter writer = new XmlTextWriter(file))
        {
            xmlSerializer.Serialize(writer, NewReleaseList);
        }
    }

    public static bool Load(string path = "newrelease.xml")
    {
        StreamReader file;
        try
        {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException)
        {
            NewReleaseList.Clear();
            return false;
        }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<NewRelease>));
        using (XmlTextReader reader = new XmlTextReader(file))
        {
            try
            {
                NewReleaseList = (List<NewRelease>)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException)
            {
                NewReleaseList.Clear();
                return false;
            }
            catch (Exception)
            {
                NewReleaseList.Clear();
                return false;
            }
        }
        return true;
    }
}