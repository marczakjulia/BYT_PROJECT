using System.Xml;
using System.Xml.Serialization;
using BYT_Entities.Interfaces;

namespace BYT_Entities.Models;

[Serializable]
public class ExtendedCut : ICutType
{
    private static List<ExtendedCut> ExtendedCutsList = new();
    private List<string> _addedScenes;
    private int _extraMinutes;
    private string _extraScenesDescription;

    public ExtendedCut(int id, string extraScenesDescription, int extraMinutes, List<string> addedScenes)
    {
        Id = id;
        ExtraScenesDescription = extraScenesDescription;
        ExtraMinutes = extraMinutes;
        AddedScenes = addedScenes;
        AddExtendedCutMovie(this);
    }

    public ExtendedCut()
    {
    }

    public int Id { get; set; }

    public string ExtraScenesDescription
    {
        get => _extraScenesDescription;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("cannot be null");
            _extraScenesDescription = value.Trim();
        }
    }

    public List<string> AddedScenes
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

    public int ExtraMinutes
    {
        get => _extraMinutes;
        set
        {
            if (value < 1)
                throw new ArgumentOutOfRangeException(nameof(value), "must be greater than 0");
            _extraMinutes = value;
        }
    }

    private static void AddExtendedCutMovie(ExtendedCut extendedCut)
    {
        if (extendedCut == null) throw new ArgumentException("extended cut cannot be null");
        ExtendedCutsList.Add(extendedCut);
    }

    public static void Save(string path = "extendedcut.xml")
    {
        var file = File.CreateText(path);
        var xmlSerializer = new XmlSerializer(typeof(List<ExtendedCut>));
        using (var writer = new XmlTextWriter(file))
        {
            xmlSerializer.Serialize(writer, ExtendedCutsList);
        }
    }

    public static List<ExtendedCut> GetExtendedCutMovies()
    {
        return new List<ExtendedCut>(ExtendedCutsList);
    }

    public static void ClearExtendedCutMovies()
    {
        ExtendedCutsList.Clear();
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

        var xmlSerializer = new XmlSerializer(typeof(List<ExtendedCut>));
        using (var reader = new XmlTextReader(file))
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

    public string GetCutTypeName()
    {
        return "Extended Cut";
    }

    public int GetExtraMinutes()
    {
        return ExtraMinutes;
    }

    public int GetAddedScenesCount()
    {
        return AddedScenes.Count;
    }
    
}