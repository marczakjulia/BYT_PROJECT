using System.Xml;
using System.Xml.Serialization;

namespace BYT_Entities.Models;

[Serializable]
public class Manager
{
    public int Id { get; set; }
    private string _department;
    private double _baseSalary;
    private double _bonus;
    public static double MaxSalaryBonus { get; } = 0.35;
    private static List<Manager> managersList = new List<Manager>();
    
    public static List<Manager> GetManagers()
    {
        return new List<Manager>(managersList);
    }
    
    public static void ClearManagers()
    {
        managersList.Clear();
    }
    private static void AddManager(Manager manager)
    {
        if (manager == null)
            throw new ArgumentException("Manager cannot be null.");

        managersList.Add(manager);
    }


    public string Department
    {
        get => _department;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Department cannot be empty.");
            _department = value.Trim();
        }
    }

    public double BaseSalary
    {
        get => _baseSalary;
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(BaseSalary), "Base salary must be greater than 0.");
            _baseSalary = value;
        }
    }

    public double Bonus
    {
        get => _bonus;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(Bonus), "Bonus cannot be negative.");
            if (value > MaxSalaryBonus)
                throw new ArgumentOutOfRangeException(nameof(Bonus),
                    $"Bonus cannot exceed max bonus.");
            _bonus = value;
        }
    }

    public Manager(int id, string department, double baseSalary, double bonus)
    {
        Id = id;
        Department = department;
        BaseSalary = baseSalary;
        Bonus = bonus;
        AddManager(this);
    }

    public double CalculateBonusAmount()
    {
        return BaseSalary * Bonus;
    }
    
    public static void Save(string path = "manager.xml")
    {
        StreamWriter file = File.CreateText(path);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Manager>));
        using (XmlTextWriter writer = new XmlTextWriter(file))
        {
            xmlSerializer.Serialize(writer, managersList);
        }
    }

    public static bool Load(string path = "manager.xml")
    {
        StreamReader file;
        try
        {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException)
        {
            managersList.Clear();
            return false;
        }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Manager>));
        using (XmlTextReader reader = new XmlTextReader(file))
        {
            try
            {
                managersList = (List<Manager>)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException)
            {
                managersList.Clear();
                return false;
            }
            catch (Exception)
            {
                managersList.Clear();
                return false;
            }
        }
        return true;
    }

}