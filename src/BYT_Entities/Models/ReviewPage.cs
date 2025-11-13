using System.Xml;
using System.Xml.Serialization;

namespace BYT_Entities.Models;

[Serializable]
public class ReviewPage
{
    private static List<ReviewPage> _reviewPages = new();
    private string _name;
    private int _rate;
    private string _surname;

    public ReviewPage(string name, string surname, int rate, int id, string? comment = null)
    {
        Name = name;
        Surname = surname;
        Rate = rate;
        Id = id;
        Comment = comment;
        
        AddReviewPage(this);
    }
    public ReviewPage() { }
    
    private static void AddReviewPage(ReviewPage reviewPage)
    {
        if (reviewPage == null)
            throw new ArgumentException("ReviewPage cannot be null.");

        _reviewPages.Add(reviewPage);
    }

    public static void Save(string path = "reviewpage.xml")
    {
        var file = File.CreateText(path);
        var serializer = new XmlSerializer(typeof(List<ReviewPage>));
        using (var writer = new XmlTextWriter(file))
        {
            serializer.Serialize(writer, _reviewPages);
        }
    }

    public static bool Load(string path = "reviewpage.xml")
    {
        StreamReader file;
        try
        {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException)
        {
            _reviewPages.Clear();
            return false;
        }

        var serializer = new XmlSerializer(typeof(List<ReviewPage>));
        using (var reader = new XmlTextReader(file))
        {
            try
            {
                _reviewPages = (List<ReviewPage>)serializer.Deserialize(reader);
            }
            catch (InvalidCastException)
            {
                _reviewPages.Clear();
                return false;
            }
        }

        return true;
    }
    public int Id { get; set; }

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Name cannot be empty.");
            _name = value;
        }
    }

    public string Surname
    {
        get => _surname;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Surname cannot be empty.");
            _surname = value;
        }
    }

    public int Rate
    {
        get => _rate;
        set
        {
            if (value < 1 || value > 10)
                throw new ArgumentOutOfRangeException(nameof(Rate), "Rate must be between 1 and 10.");
            _rate = value;
        }
    }

    public string? Comment { get; set; }

    public void AddReview()
    {
        //TODO: I do not think we reallyneed that function, instead of that we can create additional helper class like ReviewManager that has dictionary<movieTitle, List<reviews>> and we can implement methods such as GetReviewsByMovieTtile, getAverageRating,AddReview.
        //i like it, we'll see how we will have to implement the serialization and the associations -julia
    }

    // public static List<ReviewPage> GetAllReviewsByNameAndSurname(List<ReviewPage> reviews, string name, string surname)
    // {
    //     if (reviews == null)
    //         throw new ArgumentException("Reviews list cannot be null.");
    //     if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname))
    //         throw new ArgumentException("Name and surname cannot be empty.");
    //
    //     return reviews
    //         .Where(r =>
    //             r.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
    //             r.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase))
    //         .ToList();
    // }
}