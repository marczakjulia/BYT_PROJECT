namespace BYT_Entities.Models;

public class NewRelease
{
    public int Id { get; set; }
    public bool IsExclusiveToCinema { get; set; }
    private DateTime _premiereDate;
    private string _distributor;
    public DateTime PremiereDate
    {
        get => _premiereDate;
        set
        {
            if (value == default)
                throw new ArgumentException("premiere date cannot be null");
            _premiereDate = value;
        }
    }
    public string Distributor
    {
        get => _distributor;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("cannot be null or whitespace");
            _distributor = value.Trim();
        }
    }

    public NewRelease(int id, bool isExclusiveToCinema, DateTime premiereDate, string distributor)
    {
        Id = id;
        IsExclusiveToCinema = isExclusiveToCinema;
        PremiereDate = premiereDate;
        Distributor = distributor;
    }

    public NewRelease() { }
}