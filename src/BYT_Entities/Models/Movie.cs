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
    private HashSet<ReviewPage> _reviews = new();
    [XmlIgnore]
    public HashSet<ReviewPage> Reviews => new(_reviews); 
    
    [XmlIgnore]
    private HashSet<Screening> _screenings = new();

    [XmlIgnore]
    public HashSet<Screening> Screenings => new(_screenings);
    
    [XmlIgnore]
    public NewRelease? NewRelease { get; private set; }
    [XmlIgnore]
    public Rerelease? Rerelease { get; private set; }

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
    
    public Movie(int id, string title, string countryOfOrigin, int length, string description, string director, AgeRestrictionType? ageRestriction, NewRelease newRelease)
    {
        Id = id;
        Title = title;
        CountryOfOrigin = countryOfOrigin;
        Length = length;
        Description = description;
        Director = director;
        AgeRestriction = ageRestriction;
        AddMovie(this);
        SetNewRelease(newRelease);
    }
    public Movie(int id, string title, string countryOfOrigin, int length, string description, string director, AgeRestrictionType? ageRestriction, Rerelease rerelease)
    {
        Id = id;
        Title = title;
        CountryOfOrigin = countryOfOrigin;
        Length = length;
        Description = description;
        Director = director;
        AgeRestriction = ageRestriction;
        AddMovie(this);
        SetRerelease(rerelease);
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
        return new HashSet<ReviewPage>(_reviews); 
    }
    public void AddReview(ReviewPage review)
    {
        if (review == null)
            throw new ArgumentException("Review cannot be null.");
        if (_reviews.Contains(review))
            throw new InvalidOperationException("This review is already linked with the movie.");
        _reviews.Add(review);
        if (review.Movie != this)
            review.SetMovie(this);
    }

    public void RemoveReview(ReviewPage review)
    {
        if (review == null)
            throw new ArgumentException("Review cannot be null.");

        if (!_reviews.Contains(review))
            throw new InvalidOperationException("This review is not associated with this movie.");

        _reviews.Remove(review);
        if (review.Movie == this)
            review.RemoveMovie();
    }
    
    public void AddScreening(Screening screening)
    {
        if (screening == null)
            throw new ArgumentException("Screening cannot be null.");

        if (_screenings.Contains(screening))
            return;

        _screenings.Add(screening);

        if (screening.Movie != this)
            screening.SetMovie(this);
    }


    public void RemoveScreening(Screening screening)
    {
        if (screening == null)
            throw new ArgumentException("Screening cannot be null.");

        if (!_screenings.Contains(screening))
            return;

        _screenings.Remove(screening);

        if (screening.Movie == this)
            screening.RemoveMovie();
    }

    
    public void SetNewRelease(NewRelease newRelease)
    {
        if (newRelease == null)
            throw new ArgumentException("NewRelease cannot be null.");

        if (NewRelease == newRelease)
            return;
        if (NewRelease != null)
            throw new InvalidOperationException("This movie is already associated with a new release.");
        if (Rerelease != null)
            throw new InvalidOperationException("Movie already has a rerelease. Cannot assign a new release.");
        if (newRelease.Movie != null && newRelease.Movie != this)
            throw new InvalidOperationException("This new release is already associated with another movie.");

        NewRelease = newRelease;
        if (newRelease.Movie != this)
            newRelease.SetMovie(this);
    }

    public void SetRerelease(Rerelease rerelease)
    {
        if (rerelease == null)
            throw new ArgumentException("Rerelease cannot be null.");

        if (Rerelease == rerelease)
            return;

        if (Rerelease != null)
            throw new InvalidOperationException("This movie is already associated with a rerelease.");

        if (NewRelease != null)
            throw new InvalidOperationException("Movie already has a new release. Cannot assign a new release.");

        if (rerelease.Movie != null && rerelease.Movie != this)
            throw new InvalidOperationException("This rerelease is already associated with another movie.");

        Rerelease = rerelease;

        if (rerelease.Movie != this)
            rerelease.SetMovie(this);
    }

    public void RemoveNewRelease()
    {
        if (NewRelease == null)
            return;

        var newReleaseToRemove = NewRelease;
        NewRelease = null;

        if (newReleaseToRemove.Movie == this)
            newReleaseToRemove.RemoveMovie();
    }
    public void RemoveRerelease()
    {
        if (Rerelease == null)
            return;

        var rereleaseToRemove = Rerelease;
        Rerelease = null;

        if (rereleaseToRemove.Movie == this)
            rereleaseToRemove.RemoveMovie();
    }
    public void UpdateRerelease(Rerelease newRerelease)
    {
        if (newRerelease == null)
            throw new ArgumentException("rerelease cannot be null");

        if (Rerelease == newRerelease)
            return;
        if (NewRelease != null)
            throw new InvalidOperationException("movie is a new release. It cannot be updated to a rerelease");

        if (newRerelease.Movie != null && newRerelease.Movie != this)
            throw new InvalidOperationException("this rerelease is already associated with another movie");

        var oldRerelease = Rerelease;
        if (oldRerelease != null)
        {
            Rerelease = null;
            if (oldRerelease.Movie == this)
                oldRerelease.RemoveMovie();
        }

        Rerelease = newRerelease;

        if (newRerelease.Movie != this)
            newRerelease.SetMovie(this);
    }


    public void UpdateNewRelease(NewRelease newNewRelease)
    {
        if (newNewRelease == null)
            throw new ArgumentException("new release cannot be null");

        if (NewRelease == newNewRelease)
            return;
        if (Rerelease != null)
            throw new InvalidOperationException("movie is a new release. it cannot be updated to a rerelease");
        if (newNewRelease.Movie != null && newNewRelease.Movie != this)
            throw new InvalidOperationException("the new release is already associated with another movie.");

        var oldNewRelease = NewRelease;
        if (oldNewRelease != null)
        {
            NewRelease = null;
            if (oldNewRelease.Movie == this)
                oldNewRelease.RemoveMovie();
        }

        NewRelease = newNewRelease;
        if (newNewRelease.Movie != this)
            newNewRelease.SetMovie(this);
    }


    /*
    for later to implement when we do associations, since they both need to connect to review 
    public decimal GetAverageRate(int id)
    so like var reviews = _reviews.GetByMovieId(id)
    return Math.Round reviews.average....
    (this one gets all reviews connected with the id of the movie and takes their avg. value)

*/
    
}