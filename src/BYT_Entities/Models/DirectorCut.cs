namespace BYT_Entities.Models;

public class DirectorCut
{
    public int Id { get; set; }

    private int _extraMinutes;
    private string? _alternativeEnding;
    private string _changesDescription;
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
    public DirectorCut(int id, int extraMinutes, string changesDescription, string? alternativeEnding = null)
    {
        Id = id;
        ExtraMinutes = extraMinutes;
        ChangesDescription = changesDescription;
        AlternativeEnding = alternativeEnding;
    }
    public DirectorCut() { }
}