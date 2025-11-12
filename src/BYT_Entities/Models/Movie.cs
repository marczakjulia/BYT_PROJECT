using BYT_Entities.Enums;

namespace BYT_Entities.Models;

public class Movie
{
    public int Id { get; set; }
    private string _title;
    private string _countryOfOrigin;
    private int _length;
    private string _description;
    private string _director;
    public AgeRestrictionType? AgeRestriction { get; set; }

    public string Title
    {
        get => _title;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("title cannot be empty");
            _title = value.Trim();
        }
    }
    public string CountryOfOrigin
    {
        get => _countryOfOrigin;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("countryOfOrigin cannot be empty");
            _countryOfOrigin = value.Trim();
        }
    }
    public string Description
    {
        get => _description;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("description cannot be empty");
            _description = value.Trim();
        }
    }
    public string Director
    {
        get => _director;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("director cannot be empty");
            _director = value.Trim();
        }
    }
    public int Length
    {
        get => _length;
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), "length must be greater than 0");
            _length = value;
        }
    }
    
    public Movie(int id, string title, string countryOfOrigin, int length, string description, string director, AgeRestrictionType? ageRestriction = null)
    {
        Id = id;
        Title = title;
        CountryOfOrigin = countryOfOrigin;
        Length = length;
        Description = description;
        Director = director;
        AgeRestriction = ageRestriction;
    }

    public Movie() { }
    
    /*
    for later to implement when we do associations, since they both need to connect to review 
    public decimal GetAverageRate(int id)
    so like var reviews = _reviews.GetByMovieId(id)
    return Math.Round reviews.average....
    (this one gets all reviews connected with the id of the movie and takes their avg. value)

*/
    
}