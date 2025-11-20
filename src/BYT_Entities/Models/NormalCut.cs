using System.Xml;
using System.Xml.Serialization;

namespace BYT_Entities.Models;
[Serializable]
public class NormalCut
{
    public int Id { get; set; }
    private static List<NormalCut> NormalCuts = new List<NormalCut>();
    public NormalCut(int id)
    {
        Id = id;
        AddNormalCut(this);
    }
    
    private static void AddNormalCut(NormalCut normalCut)
    {
        if (normalCut == null)
        {
            throw new ArgumentException("normal cut cannot be null");
        }
        NormalCuts.Add(normalCut);
    }

    public static void Save(string path = "normalCut.xml")
    {
        StreamWriter file = File.CreateText(path);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<NormalCut>));
        using (XmlTextWriter writer = new XmlTextWriter(file))
        {
            xmlSerializer.Serialize(writer, NormalCuts);
        }
    }

    public static bool Load(string path = "normalCut.xml")
    {
        StreamReader file;
        try
        {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException)
        {
            NormalCuts.Clear();
            return false;
        }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<NormalCut>));
        using (XmlTextReader reader = new XmlTextReader(file))
        {
            try
            {
                NormalCuts = (List<NormalCut>)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException)
            {
                NormalCuts.Clear();
                return false;
            }
            catch (Exception)
            {
                NormalCuts.Clear();
                return false;
            }
        }
        return true;
    }
    public static List<NormalCut> GetNormalCutsMovies()
    {
        return new List<NormalCut>(NormalCuts);
    }
    public static void ClearNormalCutsMovies()
    {
        NormalCuts.Clear();
    }
}