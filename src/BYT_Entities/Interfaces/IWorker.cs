using BYT_Entities.Enums;

namespace BYT_Entities.Models;

public interface IWorker
{
    ShiftType Shift { get; }
    WorkType TypeOfWork { get; }
    int HoursWorked { get; }
    double HourlyRate { get; }

    void ChangeToManager(string department, double baseSalary, double bonusPercentage);
}
