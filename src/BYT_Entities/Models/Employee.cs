using System.Xml;
using System.Xml.Serialization;
using BYT_Entities.Complex;
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
    private Address _address;
    private static List<Employee> employeesList = new List<Employee>();
    private HashSet<Cinema> _cinemas = new HashSet<Cinema>();
    private IWorker _worker;
    private IManager _manager;

    public IWorker WorkerEmp => _worker;
    public IManager ManagerEmp => _manager;
    //Worker contructor
    public Employee(
        int id, string name, string surname, string pesel, string email,
        DateTime dayOfBirth, DateTime hireDate,
        Address address, EmployeeStatus status,
        IEnumerable<Cinema> cinemas,

        ShiftType shift,
        WorkType typeOfWork,
        double hourlyRate
    )
    {
        if (cinemas == null || !cinemas.Any())
            throw new ArgumentException("Employee must be assigned to at least one cinema.");

        Id = id;
        Name = name;
        Surname = surname;
        PESEL = pesel;
        Email = email;
        DayOfBirth = dayOfBirth;
        HireDate = hireDate;
        Status = status;
        Address = address;

        AddEmployee(this);
        foreach (var c in cinemas)
            AddCinema(c);

        _worker = new Worker(this, shift, typeOfWork, hourlyRate);
    }
    //Manager constructor
    public Employee(
        int id, string name, string surname, string pesel, string email,
        DateTime dayOfBirth, DateTime hireDate,
        Address address, EmployeeStatus status,
        IEnumerable<Cinema> cinemas,
        
        string department,
        double baseSalary,
        double bonusPercentage
    )
    {
        if (cinemas == null || !cinemas.Any())
            throw new ArgumentException("Employee must be assigned to at least one cinema.");

        Id = id;
        Name = name;
        Surname = surname;
        PESEL = pesel;
        Email = email;
        DayOfBirth = dayOfBirth;
        HireDate = hireDate;
        Status = status;
        Address = address;

        AddEmployee(this);
        foreach (var c in cinemas)
            AddCinema(c);

        _manager = new Manager(this, department, baseSalary, bonusPercentage);
    }
    
        private void SwitchToManager(
        string department,
        double baseSalary,
        double bonusPercentage)
    {
        _worker = null;
        _manager = new Manager(this, department, baseSalary, bonusPercentage);
    }

    private void SwitchToWorker(
        ShiftType shift,
        WorkType typeOfWork,
        double hourlyRate)
    {
        _manager = null;
        _worker = new Worker(this, shift, typeOfWork, hourlyRate);
    }
    
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

    public Address Address
    {
        get => _address;
        set
        {
            if (value == null)
                throw new ArgumentException("address cannot be null");

            _address = value;
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
    private DateTime? _fireDate;

    public DateTime? FireDate
    {
        get => _fireDate;
        set
        {
            if (value.HasValue && value.Value > DateTime.Now)
                throw new ArgumentException("Fire date cannot be in the future.");

            _fireDate = value;
        }
    }


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
    
    public HashSet<Cinema> GetCinemas()
    {
        return new HashSet<Cinema>(_cinemas);
    }

    public void AddCinema(Cinema cinema)
    {
        if (cinema == null)
            throw new ArgumentException("Cinema cannot be null.");

        if (_cinemas.Contains(cinema))
            return; //stop logic

        _cinemas.Add(cinema);

        cinema.AddEmployee(this);
    }

    public void RemoveCinema(Cinema cinema)
    {
        if (cinema == null)
            throw new ArgumentException("Cinema cannot be null.");

        if (!_cinemas.Contains(cinema))
            return;

        _cinemas.Remove(cinema);

        cinema.RemoveEmployee(this);
    }

    private Employee() { }
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
    
    private class Worker : IWorker
    {
        private readonly Employee _employee;

        private ShiftType _shift;
        private WorkType _typeOfWork;
        private int _hoursWorked;
        private double _hourlyRate;

        public ShiftType Shift
        {
            get => _shift;
            private set => _shift = value;
        }

        public WorkType TypeOfWork
        {
            get => _typeOfWork;
            private set => _typeOfWork = value;
        }

        public int HoursWorked
        {
            get => _hoursWorked;
            private set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(HoursWorked));
                _hoursWorked = value;
            }
        }

        public double HourlyRate
        {
            get => _hourlyRate;
            private set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(HourlyRate));
                _hourlyRate = value;
            }
        }

        public Worker(
            Employee employee,
            ShiftType shift,
            WorkType typeOfWork,
            double hourlyRate)
        {
            _employee = employee;
            Shift = shift;
            TypeOfWork = typeOfWork;
            HourlyRate = hourlyRate;
            HoursWorked = 0;
        }

        public void ChangeToManager(
            string department,
            double baseSalary,
            double bonusPercentage)
        {
            _employee.SwitchToManager(department, baseSalary, bonusPercentage);
        }
    }
    
    private class Manager:IManager
{
    private string _department;
    private double _baseSalary;
    private double _bonusPercentage;
    private Employee _employee;
    public static double MaxSalaryBonus { get; } = 0.35;
    
    private Manager _supervisor;
    private HashSet<Manager> _subordinates = new();  

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
    
    public IManager GetSupervisor()
    {
        return _supervisor;
    }

    public HashSet<IManager> GetSubordinates()
    {
        return new HashSet<IManager>(_subordinates);
    }
    public void SetSupervisor(IManager supervisor)
    {
        if (ReferenceEquals(supervisor, this))
            throw new InvalidOperationException("A manager cannot supervise themselves.");

        if (supervisor == null)
        {
            _supervisor?.RemoveSubordinateInternal(this);
            _supervisor = null;
            return;
        }

        if (supervisor is not Manager m)
            throw new InvalidOperationException("Supervisor must be a Manager.");

        if (_supervisor == m)
            throw new InvalidOperationException("This manager is already supervised by that manager.");

        _supervisor?.RemoveSubordinateInternal(this);
        _supervisor = m;
        m.AddSubordinateInternal(this);
    }
    
    public void AddSubordinate(IManager subordinate)
    {
        if (subordinate == null)
            throw new ArgumentException("Subordinate cannot be null.");

        if (ReferenceEquals(subordinate, this))
            throw new InvalidOperationException("A manager cannot be their own subordinate.");

        if (subordinate is not Manager m)
            throw new InvalidOperationException("Subordinate must be a Manager.");

        if (_subordinates.Contains(m))
            throw new InvalidOperationException("This manager already supervises the subordinate.");

        _subordinates.Add(m);
        m.SetSupervisorInternal(this);
    }

    public void RemoveSubordinate(IManager subordinate)
    {
        if (subordinate == null)
            throw new ArgumentException("Subordinate cannot be null.");

        if (subordinate is not Manager m)
            throw new InvalidOperationException("Subordinate must be a Manager.");

        if (!_subordinates.Contains(m))
            throw new InvalidOperationException("This subordinate is not supervised by this manager.");

        _subordinates.Remove(m);
        m.RemoveSupervisorInternal(this);
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
    
    public Manager(Employee employee, string department, double baseSalary, double bonusPercentage)
    {
        Department = department;
        BaseSalary = baseSalary;
        BonusPercentage = bonusPercentage;
        _employee = employee;
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
    private Manager(){}
    
    public void ChangeToWorker(
        ShiftType shift,
        WorkType typeOfWork,
        double hourlyRate)
    {
        _employee.SwitchToWorker(shift, typeOfWork, hourlyRate);
    }
}
}