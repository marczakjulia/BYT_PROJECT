using System.Xml;
using System.Xml.Serialization;
using BYT_Entities.Enums;

namespace BYT_Entities.Models;

[Serializable]
public class Movie
{
    public int Id { get; set; }
    private string _title;
    private string _countryOfOrigin;
    private int _length;
    private string _description;
    private string _director;
    private static List<Movie> MoviesList = new List<Movie>();
    public AgeRestrictionType? AgeRestriction { get; set; }
    private HashSet<ReviewPage> _reviews = new HashSet<ReviewPage>();
    
    
    [XmlIgnore]
    private HashSet<Screening> _screenings = new();

    [XmlIgnore]
    public HashSet<Screening> Screenings => new(_screenings);
    
    public static List<Movie> GetMovies()
    {
        return new List<Movie>(MoviesList);
    }
    public static void ClearMovies()
    {
        MoviesList.Clear();
    }
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
        AddMovie(this);
    }

    public Movie() { }
    private static void AddMovie(Movie movie)
    {
        if (movie == null)
        {
            throw new ArgumentException("movie cannot be null");
        }
        MoviesList.Add(movie);
    }

    public static void Save(string path = "movie.xml")
    {
        StreamWriter file = File.CreateText(path);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Movie>));
        using (XmlTextWriter writer = new XmlTextWriter(file))
        {
            xmlSerializer.Serialize(writer, MoviesList);
        }
    }

    public static bool Load(string path = "movie.xml")
    {
        StreamReader file;
        try
        {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException)
        {
            MoviesList.Clear();
            return false;
        }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Movie>));
        using (XmlTextReader reader = new XmlTextReader(file))
        {
            try
            {
                MoviesList = (List<Movie>)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException)
            {
                MoviesList.Clear();
                return false;
            }
            catch (Exception)
            {
                MoviesList.Clear();
                return false;
            }
        }
        return true;
    }
    public HashSet<ReviewPage> GetReviews()
    {
        return new HashSet<ReviewPage>(_reviews); // Return COPY (slides rule)
    }
    public void AddReview(ReviewPage review)
    {
        if (review == null)
            throw new ArgumentException("Review cannot be null.");

        if (_reviews.Contains(review))
            throw new InvalidOperationException("This review is already linked with the movie.");

        _reviews.Add(review);
        review.SetMovieInternal(this);
    }
    public void RemoveReview(ReviewPage review)
    {
        if (review == null)
            throw new ArgumentException("Review cannot be null.");

        if (!_reviews.Contains(review))
            throw new InvalidOperationException("This review is not associated with the movie.");

        _reviews.Remove(review);

        // Reverse removal
        review.RemoveMovieInternal(this);
    }
    internal void AddReviewInternal(ReviewPage review)
    {
        _reviews.Add(review);
    }

    internal void RemoveReviewInternal(ReviewPage review)
    {
        _reviews.Remove(review);
    }

    public void AddScreening(Screening screening)
    {
        if (screening == null)
            throw new ArgumentException("Screening cannot be null.");

        _screenings.Add(screening);
        if (_screenings.Add(screening))
        {
            if (screening.Movie != this)
                screening.SetMovie(this);
        }
    }

    public void RemoveScreening(Screening screening)
    {
        if (screening == null)
            throw new ArgumentException("Screening cannot be null.");

        if (_screenings.Remove(screening))
        {
            if (screening.Movie == this)
                screening.RemoveMovie();
        }
    }

    internal void AddScreeningInternal(Screening screening)
    {
        _screenings.Add(screening);
    }

    internal void RemoveScreeningInternal(Screening screening)
    {
        _screenings.Remove(screening);
    }


    /*
    for later to implement when we do associations, since they both need to connect to review 
    public decimal GetAverageRate(int id)
    so like var reviews = _reviews.GetByMovieId(id)
    return Math.Round reviews.average....
    (this one gets all reviews connected with the id of the movie and takes their avg. value)

*/
    
}