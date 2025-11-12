namespace BYT_Entities.Models;

public class ExtendedCut
{
    public int Id { get; set; }

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
    }

    public ExtendedCut() { }
}