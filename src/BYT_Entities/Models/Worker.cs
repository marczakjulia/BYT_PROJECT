using BYT_Entities.Enums;

namespace BYT_Entities.Models;

public class Worker
{
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
        set => _hourlyRate = value;
    }
    
    public Worker(ShiftType shift, WorkType typeOfWork, double hourlyRate)
    {
        Shift = shift;
        TypeOfWork = typeOfWork;
        HoursWorked = 0;
        HourlyRate = hourlyRate;
    }
    
}