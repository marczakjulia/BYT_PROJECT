using BYT_Entities.Enums;

namespace BYT_Entities.Models;

public class Worker
{
    public int Id { get; set; }
    private ShiftType _shift;
    private WorkType _typeOfWork;
    private int _hoursWorked;
    private double _hourlyRate;

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
    }
    
}