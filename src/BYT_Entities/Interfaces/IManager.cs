using BYT_Entities.Enums;

namespace BYT_Entities.Models;

public interface IManager
{
    string Department { get; }
    double BaseSalary { get; }
    double BonusPercentage { get; }

    double GetBonus();
    void ChangeToWorker(ShiftType shift, WorkType type, double hourlyRate);
    //added but not in the diagram 
    IManager GetSupervisor();
    HashSet<IManager> GetSubordinates();

    void SetSupervisor(IManager supervisor);
    void AddSubordinate(IManager subordinate);
    void RemoveSubordinate(IManager subordinate);
}
