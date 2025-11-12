namespace BYT_Entities.Models;

public class ReRelease
{
    public int Id { get; set; }
    private string _reason;
    private DateTime _reReleaseDate;
    public bool? Remastered { get; set; }
    public string Reason
    {
        get => _reason;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("reason cannot be empty");
            _reason = value.Trim();
        }
    }
    public DateTime ReReleaseDate
    {
        get => _reReleaseDate;
        set
        {
            if (value == default)
                throw new ArgumentException("release date cannot be null");
            _reReleaseDate = value;
        }
    }
    
    public ReRelease(int id, string reason, DateTime reReleaseDate, bool? remastered)
    {
        Id = id;
        Reason = reason;
        ReReleaseDate = reReleaseDate;
        Remastered = remastered;
    }

    public ReRelease() { }
}