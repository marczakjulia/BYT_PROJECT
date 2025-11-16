using System.Xml;
using System.Xml.Serialization;
using BYT_Entities.Enums;

namespace BYT_Entities.Models;

[Serializable]
public class Employee
{
    public int Id { get; set; }
    private string _name;
    private string _surname;
    private string _pesel;
    private string _email;
    private double _salary;
    private DateTime _dayOfBirth;
    private DateTime _hireDate;
    private static List<Employee> employeesList = new List<Employee>();
    
    public static List<Employee> GetEmployees()
    {
        return new List<Employee>(employeesList);
    }
    
    public static void ClearEmployees()
    {
        employeesList.Clear();
    }
    private static void AddEmployee(Employee employee)
    {
        if (employee == null)
            throw new ArgumentException("Employee cannot be null.");

        employeesList.Add(employee);
    }
    
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Name cannot be empty.");
            _name = value.Trim();
        }
    }

    public string Surname
    {
        get => _surname;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Surname cannot be empty.");
            _surname = value.Trim();
        }
    }

    public string PESEL
    {
        get => _pesel;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("PESEL cannot be empty.");
            _pesel = value.Trim();
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

    public DateTime DayOfBirth
    {
        get => _dayOfBirth;
        set
        {
            if (value > DateTime.Now)
                throw new ArgumentException("Birth date cannot be in the future.");
            _dayOfBirth = value;
        }
    }

    public DateTime HireDate
    {
        get => _hireDate;
        set
        {
            if (value > DateTime.Now)
                throw new ArgumentException("Hire date cannot be in the future.");
            _hireDate = value;
        }
    }
    public DateTime? FireDate { get; set; }

    public EmployeeStatus Status { get; set; } = EmployeeStatus.Working;

    public double Salary
    {
        get => _salary;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(Salary), "Base salary cannot be negative.");
            _salary = value;
        }
    }

    public Employee(int id, string name, string surname, string pesel, string email,
        DateTime dayOfBirth, DateTime hireDate, double salary)
    {
        Id = id;
        Name = name;
        Surname = surname;
        PESEL = pesel;
        Email = email;
        DayOfBirth = dayOfBirth;
        HireDate = hireDate;
        Salary = salary;
        AddEmployee(this);
    }
    public Employee() { }
    public static void Save(string path = "employee.xml")
    {
        StreamWriter file = File.CreateText(path);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Employee>));
        using (XmlTextWriter writer = new XmlTextWriter(file))
        {
            xmlSerializer.Serialize(writer, employeesList);
        }
    }

    public static bool Load(string path = "employee.xml")
    {
        StreamReader file;
        try
        {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException)
        {
            employeesList.Clear();
            return false;
        }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Employee>));
        using (XmlTextReader reader = new XmlTextReader(file))
        {
            try
            {
                employeesList = (List<Employee>)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException)
            {
                employeesList.Clear();
                return false;
            }
            catch (Exception)
            {
                employeesList.Clear();
                return false;
            }
        }
        return true;
    }
    

}