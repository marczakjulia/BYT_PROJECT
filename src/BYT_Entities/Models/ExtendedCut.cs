using System.Xml;
using System.Xml.Serialization;
using BYT_Entities.Enums;
using BYT_Entities.Interfaces;

namespace BYT_Entities.Models;

[Serializable]
public class ExtendedCut : Movie
{
    private int _extraMinutes;
    private string _extraScenesDescription;
    private List<string> _addedScenes;
    public ExtendedCut() { }

    public ExtendedCut(
        int id,
        string title,
        string countryOfOrigin,
        int length,
        string description,
        string director,
        AgeRestrictionType? ageRestriction,
        IEnumerable<IGenreType> genres,
        int extraMinutes,
        string extraScenesDescription,
        List<string> addedScenes,
        NewRelease? newRelease = null
    )
        : base(id, title, countryOfOrigin, length, description,
               director, ageRestriction, newRelease, genres)
    {
        ExtraMinutes = extraMinutes;
        ExtraScenesDescription = extraScenesDescription;
        AddedScenes = addedScenes;
    }

    public ExtendedCut(
        int id,
        string title,
        string countryOfOrigin,
        int length,
        string description,
        string director,
        AgeRestrictionType? ageRestriction,
        IEnumerable<IGenreType> genres,
        int extraMinutes,
        string extraScenesDescription,
        List<string> addedScenes,
        Rerelease? rerelease = null
    )
        : base(id, title, countryOfOrigin, length, description,
               director, ageRestriction, rerelease, genres)
    {
        ExtraMinutes = extraMinutes;
        ExtraScenesDescription = extraScenesDescription;
        AddedScenes = addedScenes;
    }

    public int ExtraMinutes
    {
        get => _extraMinutes;
        private set
        {
            if (value < 1)
                throw new ArgumentOutOfRangeException(nameof(value),
                    "Extra minutes must be at least 1.");
            _extraMinutes = value;
        }
    }

    public string ExtraScenesDescription
    {
        get => _extraScenesDescription;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Extra scenes description cannot be empty.");
            _extraScenesDescription = value.Trim();
        }
    }

    public List<string> AddedScenes
    {
        get => _addedScenes;
        private set
        {
            if (value == null)
                throw new ArgumentException("Added scenes cannot be null.");
            if (value.Any(s => string.IsNullOrWhiteSpace(s)))
                throw new ArgumentException("Added scenes cannot contain empty values.");

            _addedScenes = value.Select(s => s.Trim()).ToList();
        }
    }

    public int GetAddedScenesCount() => AddedScenes.Count;

    public override int GetTotalRuntime() => Length + ExtraMinutes;

    public override string GetCutName() => "Extended Cut";
}
