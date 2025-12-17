using System.Xml;
using System.Xml.Serialization;
using BYT_Entities.Interfaces;

namespace BYT_Entities.Models;

[Serializable]
public class ComedyMovie: IGenreType
{
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
    
    //THIS IS FOR THE NEW ASSIGMENT 
    public string GetGenreName() => "Comedy";
    public ComedyMovie(string humorType)
    {
        HumorType = humorType;
    }
    public ComedyMovie() { }
    
}