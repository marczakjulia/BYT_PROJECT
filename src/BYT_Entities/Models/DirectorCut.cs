using System.Xml;
using System.Xml.Serialization;
using BYT_Entities.Interfaces;

namespace BYT_Entities.Models;

[Serializable]
public class DirectorCut : ICutType
{
    private static List<DirectorCut> DirectorsCutList = new();
    private string? _alternativeEnding;
    private string _changesDescription;

    private int _extraMinutes;

    public DirectorCut(int id, int extraMinutes, string changesDescription, string? alternativeEnding = null)
    {
        Id = id;
        ExtraMinutes = extraMinutes;
        ChangesDescription = changesDescription;
        AlternativeEnding = alternativeEnding;
        AddDirectorCut(this);
    }

    public DirectorCut()
    {
    }

    public int Id { get; set; }

    public int ExtraMinutes
    {
        get => _extraMinutes;
        set
        {
            if (value < 1)
                throw new ArgumentOutOfRangeException(nameof(value), "must be greater than or equal to 1");
            _extraMinutes = value;
        }
    }

    public string? AlternativeEnding
    {
        get => _alternativeEnding;
        set
        {
            if (value != null && string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("cannot be an empty string");
            _alternativeEnding = value?.Trim();
        }
    }

    public string ChangesDescription
    {
        get => _changesDescription;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("cannot be null");
            _changesDescription = value.Trim();
        }
    }

    public static List<DirectorCut> GetDirectorCutsMovies()
    {
        return new List<DirectorCut>(DirectorsCutList);
    }

    public static void ClearDirectorCutsMovies()
    {
        DirectorsCutList.Clear();
    }

    private static void AddDirectorCut(DirectorCut directorCut)
    {
        if (directorCut == null) throw new ArgumentException("directorCut cannot be null");
        DirectorsCutList.Add(directorCut);
    }

    public static void Save(string path = "directorcutmovies.xml")
    {
        var file = File.CreateText(path);
        var xmlSerializer = new XmlSerializer(typeof(List<DirectorCut>));
        using (var writer = new XmlTextWriter(file))
        {
            xmlSerializer.Serialize(writer, DirectorsCutList);
        }
    }

    public static bool Load(string path = "directorcutmovies.xml")
    {
        StreamReader file;
        try
        {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException)
        {
            DirectorsCutList.Clear();
            return false;
        }

        var xmlSerializer = new XmlSerializer(typeof(List<DirectorCut>));
        using (var reader = new XmlTextReader(file))
        {
            try
            {
                DirectorsCutList = (List<DirectorCut>)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException)
            {
                DirectorsCutList.Clear();
                return false;
            }
            catch (Exception)
            {
                DirectorsCutList.Clear();
                return false;
            }
        }

        return true;
    }

    public string GetCutTypeName()
    {
        return "Director Cut";
    }

    public int GetExtraMinutes()
    {
        return ExtraMinutes;
    }

    public bool HasAlternativeEnding()
    {
        return AlternativeEnding != null;
    }
}