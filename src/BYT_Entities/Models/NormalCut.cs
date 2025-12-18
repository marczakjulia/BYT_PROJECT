using System.Xml;
using System.Xml.Serialization;
using BYT_Entities.Enums;
using BYT_Entities.Interfaces;

namespace BYT_Entities.Models;

[Serializable]
public class NormalCut : Movie
{
    public NormalCut() { }
    public NormalCut(
        int id,
        string title,
        string countryOfOrigin,
        int length,
        string description,
        string director,
        AgeRestrictionType? ageRestriction,
        IEnumerable<IGenreType> genres,
        NewRelease? newRelease = null
    )
        : base(id, title, countryOfOrigin, length, description,
            director, ageRestriction, newRelease, genres)
    {
    }
    public NormalCut(
        int id,
        string title,
        string countryOfOrigin,
        int length,
        string description,
        string director,
        AgeRestrictionType? ageRestriction,
        IEnumerable<IGenreType> genres,
        Rerelease? reRelease = null
    )
        : base(id, title, countryOfOrigin, length, description,
            director, ageRestriction, reRelease, genres)
    {
    }
    
    public override int GetTotalRuntime() => Length;
    public override string GetCutName() => "Normal Cut";
}