using System.Xml;
using System.Xml.Serialization;
using BYT_Entities.Enums;

namespace BYT_Entities.Models;

[Serializable]  
public class Worker
{
    public int Id { get; set; }
    private ShiftType _shift;
    private WorkType _typeOfWork;
    private int _hoursWorked;
    private double _hourlyRate;
    private static List<Worker> workersList = new List<Worker>();
    
    public static List<Worker> GetWorkers()
    {
        return new List<Worker>(workersList);
    }
    
    public static void ClearWorkers()
    {
        workersList.Clear();
    }
    private static void AddWorker(Worker worker)
    {
        if (worker == null)
            throw new ArgumentException("Worker cannot be null.");

        workersList.Add(worker);
    }

    public ShiftType Shift
    {
        get => _shift;
        set => _shift = value;
    }

    public WorkType TypeOfWork
    {
        get => _typeOfWork;
        set => _typeOfWork = value;
    }

    public int HoursWorked
    {
        get => _hoursWorked;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(HoursWorked), "Hours worked cannot be negative.");
            _hoursWorked = value;
        }
    }
    
    public double HourlyRate
    {
        get => _hourlyRate;
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(HourlyRate), "Hourly rate must be greater than zero.");
            _hourlyRate = value;
        }
    }
    
    public Worker(int id, ShiftType shift, WorkType typeOfWork, double hourlyRate)
    {
        Id = id;
        Shift = shift;
        TypeOfWork = typeOfWork;
        HoursWorked = 0;
        HourlyRate = hourlyRate;
        AddWorker(this);
    }
    public Worker() { }
    public static void Save(string path = "worker.xml")
    {
        StreamWriter file = File.CreateText(path);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Worker>));
        using (XmlTextWriter writer = new XmlTextWriter(file))
        {
            xmlSerializer.Serialize(writer, workersList);
        }
    }

    public static bool Load(string path = "worker.xml")
    {
        StreamReader file;
        try
        {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException)
        {
            workersList.Clear();
            return false;
        }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Worker>));
        using (XmlTextReader reader = new XmlTextReader(file))
        {
            try
            {
                workersList = (List<Worker>)xmlSerializer.Deserialize(reader);
            }
            catch (InvalidCastException)
            {
                workersList.Clear();
                return false;
            }
            catch (Exception)
            {
                workersList.Clear();
                return false;
            }
        }
        return true;
    }
    
}