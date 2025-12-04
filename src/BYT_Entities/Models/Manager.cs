using System.Xml;
using System.Xml.Serialization;

namespace BYT_Entities.Models;

[Serializable]
public class Manager
{
    public int Id { get; set; }
    private string _department;
    private double _baseSalary;
    private double _bonusPercentage;
    public static double MaxSalaryBonus { get; } = 0.35;
    private static List<Manager> managersList = new List<Manager>();
    
    private Manager _supervisor;
    private HashSet<Manager> _subordinates = new();  
    
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
    
    public double BonusPercentage
    {
        get => _bonusPercentage;
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(BonusPercentage), "Bonus percentage must be greater than 0.");
            _baseSalary = value;
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

    public double GetBonus()
    {
        double bonus;

        if (_bonusPercentage > MaxSalaryBonus)
        {
            Console.WriteLine("The bonus percentage is too large, it's reduced to the maximum.");
            bonus = BaseSalary * MaxSalaryBonus;
        }
        else
        {
            bonus = BaseSalary * _bonusPercentage;
        }

        if (bonus < 0)
            throw new ArgumentOutOfRangeException(nameof(bonus), "Bonus cannot be negative.");

        return bonus;
    }
    
    public Manager GetSupervisor()
    {
        return _supervisor;
    }
    public HashSet<Manager> GetSubordinates()
    {
        return new HashSet<Manager>(_subordinates);
    }
    public void SetSupervisor(Manager supervisor)
    {
        if (supervisor == this)
            throw new InvalidOperationException("A manager cannot supervise themselves.");

        if (supervisor == null)
        {
            if (_supervisor != null)
                _supervisor.RemoveSubordinateInternal(this);

            _supervisor = null;
            return;
        }
        
        if (_supervisor == supervisor)
            throw new InvalidOperationException("This manager is already supervised by that manager.");
        
        if (_supervisor != null)
            _supervisor.RemoveSubordinateInternal(this);

        _supervisor = supervisor;
        supervisor.AddSubordinateInternal(this);
    }
    
    public void AddSubordinate(Manager subordinate)
    {
        if (subordinate == null)
            throw new ArgumentException("Subordinate cannot be null.");

        if (subordinate == this)
            throw new InvalidOperationException("A manager cannot be their own subordinate.");

        if (_subordinates.Contains(subordinate))
            throw new InvalidOperationException("This manager already supervises the subordinate.");

        _subordinates.Add(subordinate);

        subordinate.SetSupervisorInternal(this);
    }
    public void RemoveSubordinate(Manager subordinate)
    {
        if (subordinate == null)
            throw new ArgumentException("Subordinate cannot be null.");

        if (!_subordinates.Contains(subordinate))
            throw new InvalidOperationException("This subordinate is not supervised by this manager.");

        _subordinates.Remove(subordinate);

        subordinate.RemoveSupervisorInternal(this);
    }
    internal void AddSubordinateInternal(Manager subordinate)
    {
        _subordinates.Add(subordinate);
    }

    internal void RemoveSubordinateInternal(Manager subordinate)
    {
        _subordinates.Remove(subordinate);
    }

    internal void SetSupervisorInternal(Manager supervisor)
    {
        _supervisor = supervisor;
    }

    internal void RemoveSupervisorInternal(Manager supervisor)
    {
        if (_supervisor == supervisor)
            _supervisor = null;
    }
    
    public Manager(int id, string department, double baseSalary, double bonusPercentage)
    {
        Id = id;
        Department = department;
        BaseSalary = baseSalary;
        BonusPercentage = bonusPercentage;
        AddManager(this);
    }
    public Manager Supervisor
    {
        get => _supervisor;
        set => _supervisor = value;
    }
    
    public List<Manager> Subordinates
    {
        get => _subordinates.ToList();
        set => _subordinates = value != null ? new HashSet<Manager>(value) : new HashSet<Manager>();
    }
    public Manager(){}
    
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
                //restore relationship because they would be lost with xml file
                foreach (var manager in managersList)
                {
                    if (manager._supervisor != null)
                    {
                        manager._supervisor.AddSubordinateInternal(manager);
                    }

                    foreach (var subordinate in manager._subordinates)
                    {
                        subordinate.SetSupervisorInternal(manager);
                    }
                }
            }
            catch
            {
                managersList.Clear();
                return false;
            }
        }
        return true;
    }


}