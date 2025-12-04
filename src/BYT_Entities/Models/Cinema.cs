using System.Xml;
using System.Xml.Serialization;

namespace BYT_Entities.Models;

[Serializable]
public class Cinema
{
    public int Id { get; set; }
    private string _name;
    private string _address;
    private string _phone;
    private string _email;
    private string _openingHours;
    private static List<Cinema> cinemasList = new List<Cinema>();
    [XmlIgnore]
    private HashSet<Employee> _employees = new HashSet<Employee>();
    [XmlIgnore]
    private HashSet<Auditorium> _auditoriums = new HashSet<Auditorium>();

    public static List<Cinema> GetCinemas()
    {
        return new List<Cinema>(cinemasList);
    }
    
    public static void ClearCinemas()
    {
        cinemasList.Clear();
    }
    private static void AddCinema(Cinema cinema)
    {
        if (cinema == null)
            throw new ArgumentException("Cinema cannot be null.");

        cinemasList.Add(cinema);
    }
    
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
    
    public HashSet<Employee> GetEmployees()
    {
        return new HashSet<Employee>(_employees);
    }

    public void AddEmployee(Employee employee)
    {
        if (employee == null)
            throw new ArgumentException("Employee cannot be null.");

        if (_employees.Contains(employee))
            throw new InvalidOperationException("This employee is already linked to this cinema.");

        _employees.Add(employee);

        employee.AddCinemaInternal(this);
    }

    public void RemoveEmployee(Employee employee)
    {
        if (employee == null)
            throw new ArgumentException("Employee cannot be null.");

        if (!_employees.Contains(employee))
            throw new InvalidOperationException("This employee is not linked to this cinema.");

        _employees.Remove(employee);

        employee.RemoveCinemaInternal(this);
    }

    internal void AddEmployeeInternal(Employee employee)
    {
        _employees.Add(employee);
    }

    internal void RemoveEmployeeInternal(Employee employee)
    {
        _employees.Remove(employee);
    }
    
    public HashSet<Auditorium> GetAuditoriums()
    {
        return new HashSet<Auditorium>(_auditoriums); // slide rule: return copy
    }
    
    public void AddAuditorium(Auditorium auditorium)
    {
        if (auditorium == null)
            throw new ArgumentException("Auditorium cannot be null.");

        if (_auditoriums.Contains(auditorium))
            throw new InvalidOperationException("This auditorium already belongs to the cinema.");

        if (auditorium.Cinema != null)
            throw new InvalidOperationException("This auditorium already belongs to another cinema.");

        _auditoriums.Add(auditorium);

        auditorium.SetCinemaInternal(this);
    }
    public void RemoveAuditorium(Auditorium auditorium)
    {
        if (auditorium == null)
            throw new ArgumentException("Auditorium cannot be null.");

        if (!_auditoriums.Contains(auditorium))
            throw new InvalidOperationException("Auditorium does not belong to this cinema.");

        _auditoriums.Remove(auditorium);

        auditorium.RemoveCinemaInternal(this);
    }
    public void Destroy()
    {
        foreach (var a in _auditoriums.ToList())
            RemoveAuditorium(a);

        cinemasList.Remove(this);
    }
    
    public List<Employee> Employees
    {
        get => _employees.ToList();
        set => _employees = value != null ? new HashSet<Employee>(value) : new HashSet<Employee>();
    }

    public List<Auditorium> Auditoriums
    {
        get => _auditoriums.ToList();
        set => _auditoriums = value != null ? new HashSet<Auditorium>(value) : new HashSet<Auditorium>();
    }

    
    public Cinema(int id, string name, string address, string phone, string email, string openingHours)
    {
        Id = id;
        Name = name;
        Address = address;
        Phone = phone;
        Email = email;
        OpeningHours = openingHours;
        AddCinema(this);
    }
    
    public void CalculateTotalEarnings(){}
    public Cinema() { }
    
    public static void Save(string path = "cinema.xml")
    {
        foreach (var cinema in cinemasList)
        {
            if (cinema._auditoriums.Count < 1)
            {
                //throw new InvalidOperationException($"Cinema '{cinema.Name}' must contain at least one auditorium before saving.");
            }
        }
        StreamWriter file = File.CreateText(path);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Cinema>));
        using (XmlTextWriter writer = new XmlTextWriter(file))
        {
            xmlSerializer.Serialize(writer, cinemasList);
        }

    }
    public void RestoreRelations()
    {
        _employees.Clear();
        _auditoriums.Clear();

        foreach (var emp in Employees)
        {
            _employees.Add(emp);
            emp.AddCinemaInternal(this);
        }

        foreach (var aud in Auditoriums)
        {
            _auditoriums.Add(aud);
            aud.SetCinemaInternal(this);
        }
    }

    public static bool Load(string path = "cinema.xml")
    {
        StreamReader file;
        try
        {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException)
        {
            cinemasList.Clear();
            return false;
        }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Cinema>));
        using (XmlTextReader reader = new XmlTextReader(file))
        {
            try
            {
                cinemasList = (List<Cinema>)xmlSerializer.Deserialize(reader);
                
            }
            
            catch (InvalidCastException)
            {
                cinemasList.Clear();
                return false;
            }
            catch (Exception)
            {
                cinemasList.Clear();
                return false;
            }
        }
        return true;
    }
    
}