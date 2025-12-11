using System.Xml;
using System.Xml.Serialization;

namespace BYT_Entities.Models;
[Serializable]
public class NewRelease
{
    public int Id { get; set; }
    private static List<NewRelease> NewReleaseList = new List<NewRelease>();
    public bool IsExclusiveToCinema { get; set; }
    private DateTime _premiereDate;
    private string _distributor;
    [XmlIgnore]
    public Movie? Movie { get; private set; }

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
        AddNewRelease(this);
    }

    public NewRelease() { }
    
    private static void AddNewRelease(NewRelease newRelease)
    {
        if (newRelease == null)
        {
            throw new ArgumentException("newRelease cannot be null");
        }
        NewReleaseList.Add(newRelease);
    }
    public static List<NewRelease> GetNewReleasesMovies()
    {
        return new List<NewRelease>(NewReleaseList);
    }
    public static void ClearNewRleaseMovies()
    {
        NewReleaseList.Clear();
    }
    public static void Save(string path = "newrelease.xml")
    {
        StreamWriter file = File.CreateText(path);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<NewRelease>));
        using (XmlTextWriter writer = new XmlTextWriter(file))
        {
            xmlSerializer.Serialize(writer, NewReleaseList);
        }
    }

    public static bool Load(string path = "newrelease.xml")
    {
        StreamReader file;
        try
        {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException)
        {
            NewReleaseList.Clear();
            return false;
        }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<NewRelease>));
        using (XmlTextReader reader = new XmlTextReader(file))
        {
            try
            {
                NewReleaseList = (List<NewRelease>)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException)
            {
                NewReleaseList.Clear();
                return false;
            }
            catch (Exception)
            {
                NewReleaseList.Clear();
                return false;
            }
        }
        return true;
    }
    public void SetMovie(Movie movie)
    {
        if (movie == null)
            throw new ArgumentException("movie cannot be null.");

        if (Movie == movie)
            return;

        if (Movie != null)
            throw new InvalidOperationException("this newrelease already is connected to a movie.");

        if (movie.NewRelease != null && movie.NewRelease != this)
            throw new InvalidOperationException("this movie is already connected with another new release.");

        Movie = movie;

        if (movie.NewRelease != this)
            movie.SetNewRelease(this);
    }

    public void RemoveMovie()
    {
        if (Movie == null)
            return;

        var movieToRemove = Movie;
        Movie = null;

        if (movieToRemove.NewRelease == this)
            movieToRemove.RemoveNewRelease();
    }

    public void UpdateMovie(Movie newMovie)
    {
        if (newMovie == null)
            throw new ArgumentException("new movie cannot be null.");

        if (Movie == newMovie)
            return;

        if (newMovie.NewRelease != null && newMovie.NewRelease != this)
            throw new InvalidOperationException("this movie is already connected with another new release.");

        var oldMovie = Movie;
        if (oldMovie != null)
        {
            Movie = null;
            if (oldMovie.NewRelease == this)
                oldMovie.RemoveNewRelease();
        }

        Movie = newMovie;
        if (newMovie.NewRelease != this)
            newMovie.SetNewRelease(this);
    }

}