using BYT_Entities.Models;
using BYT_Entities.Enums;
using BYT_Entities.Complex;

namespace TestByt;

public class ManagerTests
{
    private Employee boss;
    private Employee lead;
    private Employee worker;
    private Cinema cinema;

    [SetUp]
    public void Setup()
    {
        Employee.ClearEmployees();
        Cinema.ClearCinemas();

        cinema = new Cinema(1, "C1", "A", "1", "a@a.com", "10-20");

        boss = new Employee(
            id: 1,
            name: "Big",
            surname: "Boss",
            pesel: "11111111111",
            email: "boss@test.com",
            dayOfBirth: DateTime.Now.AddYears(-45),
            hireDate: DateTime.Now.AddYears(-15),
            address: new Address("Street", "1", "City", "00-000", "PL"),
            status: EmployeeStatus.Working,
            cinemas: new[] { cinema },

            department: "HQ",
            baseSalary: 9000,
            bonusPercentage: 0.2
        );

        lead = new Employee(
            id: 2,
            name: "Team",
            surname: "Lead",
            pesel: "22222222222",
            email: "lead@test.com",
            dayOfBirth: DateTime.Now.AddYears(-35),
            hireDate: DateTime.Now.AddYears(-7),
            address: new Address("Street", "2", "City", "00-000", "PL"),
            status: EmployeeStatus.Working,
            cinemas: new[] { cinema },

            department: "IT",
            baseSalary: 6000,
            bonusPercentage: 0.1
        );

        worker = new Employee(
            id: 3,
            name: "Junior",
            surname: "Dev",
            pesel: "33333333333",
            email: "worker@test.com",
            dayOfBirth: DateTime.Now.AddYears(-25),
            hireDate: DateTime.Now.AddYears(-2),
            address: new Address("Street", "3", "City", "00-000", "PL"),
            status: EmployeeStatus.Working,
            cinemas: new[] { cinema },

            department: "IT",
            baseSalary: 4000,
            bonusPercentage: 0.05
        );
    }

    [Test]
    public void SetSupervisor_ShouldCreateReverseConnection()
    {
        lead.ManagerEmp.SetSupervisor(boss.ManagerEmp);

        Assert.AreEqual(boss.ManagerEmp, lead.ManagerEmp.GetSupervisor());
        Assert.IsTrue(boss.ManagerEmp.GetSubordinates().Contains(lead.ManagerEmp));
    }

    [Test]
    public void RemoveSupervisor_ShouldRemoveReverseConnection()
    {
        lead.ManagerEmp.SetSupervisor(boss.ManagerEmp);

        lead.ManagerEmp.SetSupervisor(null);

        Assert.IsNull(lead.ManagerEmp.GetSupervisor());
        Assert.IsFalse(boss.ManagerEmp.GetSubordinates().Contains(lead.ManagerEmp));
    }

    [Test]
    public void AddSubordinate_ShouldCreateReverseConnection()
    {
        boss.ManagerEmp.AddSubordinate(lead.ManagerEmp);

        Assert.AreEqual(boss.ManagerEmp, lead.ManagerEmp.GetSupervisor());
        Assert.IsTrue(boss.ManagerEmp.GetSubordinates().Contains(lead.ManagerEmp));
    }

    [Test]
    public void RemoveSubordinate_ShouldRemoveReverseConnection()
    {
        boss.ManagerEmp.AddSubordinate(lead.ManagerEmp);

        boss.ManagerEmp.RemoveSubordinate(lead.ManagerEmp);

        Assert.IsNull(lead.ManagerEmp.GetSupervisor());
        Assert.IsFalse(boss.ManagerEmp.GetSubordinates().Contains(lead.ManagerEmp));
    }

    [Test]
    public void ChangingSupervisor_ShouldUpdateLinks()
    {
        lead.ManagerEmp.SetSupervisor(boss.ManagerEmp);
        lead.ManagerEmp.SetSupervisor(worker.ManagerEmp);

        Assert.AreEqual(worker.ManagerEmp, lead.ManagerEmp.GetSupervisor());
        Assert.IsFalse(boss.ManagerEmp.GetSubordinates().Contains(lead.ManagerEmp));
        Assert.IsTrue(worker.ManagerEmp.GetSubordinates().Contains(lead.ManagerEmp));
    }

    [Test]
    public void SetSupervisor_ShouldThrow_WhenSelf()
    {
        Assert.Throws<InvalidOperationException>(() =>
            boss.ManagerEmp.SetSupervisor(boss.ManagerEmp));
    }

    [Test]
    public void AddSubordinate_ShouldThrow_WhenSelf()
    {
        Assert.Throws<InvalidOperationException>(() =>
            boss.ManagerEmp.AddSubordinate(boss.ManagerEmp));
    }

    [Test]
    public void AddSubordinateTwice_ShouldThrow()
    {
        boss.ManagerEmp.AddSubordinate(lead.ManagerEmp);

        Assert.Throws<InvalidOperationException>(() =>
            boss.ManagerEmp.AddSubordinate(lead.ManagerEmp));
    }

    [Test]
    public void RemoveNonExistingSubordinate_ShouldThrow()
    {
        Assert.Throws<InvalidOperationException>(() =>
            boss.ManagerEmp.RemoveSubordinate(lead.ManagerEmp));
    }

    [Test]
    public void AddSubordinate_ShouldThrow_WhenNull()
    {
        Assert.Throws<ArgumentException>(() =>
            boss.ManagerEmp.AddSubordinate(null!));
    }

    [Test]
    public void RemoveSubordinate_ShouldThrow_WhenNull()
    {
        Assert.Throws<ArgumentException>(() =>
            boss.ManagerEmp.RemoveSubordinate(null!));
    }
}
