namespace BYT_Entities.Models;

public class ComedyMovie
{
    public int Id { get; set; }
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
    public ComedyMovie(int id, string humorType)
    {
        Id = id;
        HumorType = humorType;
    }

    public ComedyMovie() { }
}