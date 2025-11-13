using System.Xml;
using System.Xml.Serialization;

namespace BYT_Entities.Models;

[Serializable]
public class ExtendedCut
{
    public int Id { get; set; }
    private static List<ExtendedCut> ExtendedCutsList = new List<ExtendedCut>();
    private  string _extraScenesDescription;
    private List<string> _addedScenes;
    private int _extraMinutes;
    public required string ExtraScenesDescription
    {
        get => _extraScenesDescription;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("cannot be null");
            _extraScenesDescription = value.Trim();
        }
    }
    public required List<string> AddedScenes
    {
        get => _addedScenes;
        set
        {
            if (value == null)
                throw new ArgumentException("cannot be null");
            if (value.Any(s => string.IsNullOrWhiteSpace(s)))
                throw new ArgumentException("cannot be empty string");

            _addedScenes = value.Select(s => s.Trim()).ToList();
        }
    }
    public required int ExtraMinutes
    {
        get => _extraMinutes;
        set
        {
            if (value < 1)
                throw new ArgumentOutOfRangeException(nameof(value), "must be greater than 0");
            _extraMinutes = value;
        }
    }

    public ExtendedCut(int id, string extraScenesDescription, int extraMinutes, List<string> addedScenes)
    {
        Id = id;
        ExtraScenesDescription = extraScenesDescription;
        ExtraMinutes = extraMinutes;
        AddedScenes = addedScenes;
        AddExtendedCutMovie(this);
    }

    public ExtendedCut() { }
    private static void AddExtendedCutMovie(ExtendedCut extendedCut)
    {
        if (extendedCut == null)
        {
            throw new ArgumentException("extended cut cannot be null");
        }
        ExtendedCutsList.Add(extendedCut);
    }

    public static void Save(string path = "extendedcut.xml")
    {
        StreamWriter file = File.CreateText(path);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ExtendedCut>));
        using (XmlTextWriter writer = new XmlTextWriter(file))
        {
            xmlSerializer.Serialize(writer, ExtendedCutsList);
        }
    }

    public static bool Load(string path = "extendedcut.xml")
    {
        StreamReader file;
        try
        {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException)
        {
            ExtendedCutsList.Clear();
            return false;
        }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ExtendedCut>));
        using (XmlTextReader reader = new XmlTextReader(file))
        {
            try
            {
                ExtendedCutsList = (List<ExtendedCut>)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException)
            {
                ExtendedCutsList.Clear();
                return false;
            }
            catch (Exception)
            {
                ExtendedCutsList.Clear();
                return false;
            }
        }
        return true;
    }
}