using BYT_Entities.Complex;
using BYT_Entities.Enums;
using BYT_Entities.Models;

namespace TestByt;

public class EmployeeInheritanceTests
{
    private Cinema _cinema;

    [SetUp]
    public void Setup()
    {
        Employee.ClearEmployees();
        Cinema.ClearCinemas();

        _cinema = new Cinema(
            1, "TestCinema", "Street", "1", "mail@test.com", "8-20"
        );
    }

    private Employee CreateWorkerEmployee()
    {
        return new Employee(
            1, "John", "Doe", "12345678901", "john@test.com",
            new DateTime(1990, 1, 1),
            DateTime.Now.AddDays(-10),
            new Address("street", "1", "city", "00-000", "PL"),
            EmployeeStatus.Working,
            new List<Cinema> { _cinema },

            ShiftType.Morning,
            WorkType.Ticket,
            40.0
        );
    }

    private Employee CreateManagerEmployee()
    {
        return new Employee(
            2, "Anna", "Boss", "98765432109", "anna@test.com",
            new DateTime(1985, 5, 5),
            DateTime.Now.AddDays(-100),
            new Address("street", "2", "city", "00-000", "PL"),
            EmployeeStatus.Working,
            new List<Cinema> { _cinema },

            "IT",
            8000,
            0.2
        );
    }

    [Test]
    public void Employee_ShouldStartAsWorker()
    {
        var employee = CreateWorkerEmployee();

        Assert.NotNull(employee.WorkerEmp);
        Assert.IsNull(employee.ManagerEmp);
    }

    [Test]
    public void Employee_ShouldStartAsManager()
    {
        var employee = CreateManagerEmployee();

        Assert.NotNull(employee.ManagerEmp);
        Assert.IsNull(employee.WorkerEmp);
    }

    [Test]
    public void Worker_ShouldChangeToManager()
    {
        var employee = CreateWorkerEmployee();

        employee.WorkerEmp.ChangeToManager(
            "HR",
            7000,
            0.15
        );

        Assert.IsNull(employee.WorkerEmp);
        Assert.NotNull(employee.ManagerEmp);
    }

    [Test]
    public void Manager_ShouldChangeToWorker()
    {
        var employee = CreateManagerEmployee();

        employee.ManagerEmp.ChangeToWorker(
            ShiftType.Evening,
            WorkType.Validator,
            30.0
        );

        Assert.IsNull(employee.ManagerEmp);
        Assert.NotNull(employee.WorkerEmp);
    }

    [Test]
    public void Employee_ShouldNeverHaveBothRoles()
    {
        var employee = CreateWorkerEmployee();

        employee.WorkerEmp.ChangeToManager("Sales", 6000, 0.1);

        Assert.False(
            employee.WorkerEmp != null && employee.ManagerEmp != null
        );
    }

    [Test]
    public void Employee_ShouldHandleMultipleRoleChanges()
    {
        var employee = CreateWorkerEmployee();

        employee.WorkerEmp.ChangeToManager("IT", 9000, 0.25);
        employee.ManagerEmp.ChangeToWorker(ShiftType.Morning, WorkType.Ticket, 45);
        employee.WorkerEmp.ChangeToManager("Finance", 10000, 0.3);

        Assert.NotNull(employee.ManagerEmp);
        Assert.IsNull(employee.WorkerEmp);
    }
}



