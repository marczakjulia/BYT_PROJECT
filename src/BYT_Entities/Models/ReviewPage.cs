namespace BYT_Entities.Models;

public class ReviewPage
{
    public int Id { get; set; }
    private string _name;
    private string _surname;
    private int _rate;
    private string? _comment;
    
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

    public string? Comment
    {
        get => _comment;
        set => _comment = value; 
    }
    
    public ReviewPage(string name, string surname, int rate, int id, string? comment = null)
    {
        Name = name;
        Surname = surname;
        Rate = rate;
        Id = id;
        Comment = comment;
    }

    public void AddReview()
    {
        //TODO: I do not think we reallyneed that function, instead of that we can create additional helper class like ReviewManager that has dictionary<movieTitle, List<reviews>> and we can implement methods such as GetReviewsByMovieTtile, getAverageRating,AddReview.
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