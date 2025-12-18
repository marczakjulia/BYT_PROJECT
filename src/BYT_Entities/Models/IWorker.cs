using BYT_Entities.Enums;

namespace BYT_Entities.Models;

public interface IWorker
{
    ShiftType Shift { get; }
    WorkType TypeOfWork { get; }
    int HoursWorked { get; }
    double HourlyRate { get; }

    double CalculateSalary();
    void ChangeToManager(double baseSalary, double bonusPercentage);
}
