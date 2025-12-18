using System.Xml;
using System.Xml.Serialization;
using BYT_Entities.Enums;
using BYT_Entities.Interfaces;

namespace BYT_Entities.Models;

[Serializable]
public class DirectorCut : Movie
{
    private int _extraMinutes;
    private string _changesDescription;
    private string? _alternativeEnding;
    public DirectorCut() { }

    public DirectorCut(
        int id,
        string title,
        string countryOfOrigin,
        int length,
        string description,
        string director,
        AgeRestrictionType? ageRestriction,
        IEnumerable<IGenreType> genres,
        int extraMinutes,
        string changesDescription,
        string? alternativeEnding = null,
        NewRelease? newRelease = null
    )
        : base(id, title, countryOfOrigin, length, description,
               director, ageRestriction, newRelease, genres)
    {
        ExtraMinutes = extraMinutes;
        ChangesDescription = changesDescription;
        AlternativeEnding = alternativeEnding;
    }

    public DirectorCut(
        int id,
        string title,
        string countryOfOrigin,
        int length,
        string description,
        string director,
        AgeRestrictionType? ageRestriction,
        IEnumerable<IGenreType> genres,
        int extraMinutes,
        string changesDescription,
        string? alternativeEnding = null,
        Rerelease? rerelease = null
    )
        : base(id, title, countryOfOrigin, length, description,
               director, ageRestriction, rerelease, genres)
    {
        ExtraMinutes = extraMinutes;
        ChangesDescription = changesDescription;
        AlternativeEnding = alternativeEnding;
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

    public string ChangesDescription
    {
        get => _changesDescription;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Changes description cannot be empty.");
            _changesDescription = value.Trim();
        }
    }

    public string? AlternativeEnding
    {
        get => _alternativeEnding;
        private set
        {
            if (value != null && string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Alternative ending cannot be empty.");
            _alternativeEnding = value?.Trim();
        }
    }

    public bool HasAlternativeEnding() => AlternativeEnding != null;

    public override int GetTotalRuntime() => Length + ExtraMinutes;

    public override string GetCutName() => "Director Cut";
}
