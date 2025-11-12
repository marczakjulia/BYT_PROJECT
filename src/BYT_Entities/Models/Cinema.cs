namespace BYT_Entities.Models;

public class Cinema
{
    public int Id { get; set; }
    private string _name;
    private string _address;
    private string _phone;
    private string _email;
    private string _openingHours;
    
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Cinema name cannot be empty.");
            _name = value.Trim();
        }
    }

    public string Address
    {
        get => _address;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Address cannot be empty.");
            _address = value.Trim();
        }
    }

    public string Phone
    {
        get => _phone;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Phone cannot be empty.");
            _phone = value.Trim();
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                throw new ArgumentException("Email must contain '@'.");
            _email = value.Trim();
        }
    }

    public string OpeningHours
    {
        get => _openingHours;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Opening hours cannot be empty.");
            _openingHours = value.Trim();
        }
    }
    
    public Cinema(int id, string name, string address, string phone, string email, string openingHours)
    {
        Id = id;
        Name = name;
        Address = address;
        Phone = phone;
        Email = email;
        OpeningHours = openingHours;
    }
    
    public void CalculateTotalEarnings(){}
    
}