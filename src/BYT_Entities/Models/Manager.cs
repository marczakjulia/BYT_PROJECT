namespace BYT_Entities.Models;

public class Manager
{
    private string _department;
    private double _baseSalary;
    private double _bonus;
    public static double MaxSalaryBonus { get; } = 0.35;

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

    public Manager(string department, double baseSalary, double bonus)
    {
        Department = department;
        BaseSalary = baseSalary;
        Bonus = bonus;
    }

    public double CalculateBonusAmount()
    {
        return BaseSalary * Bonus;
    }

}