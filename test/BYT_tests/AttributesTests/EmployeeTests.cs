using BYT_Entities.Complex;
using BYT_Entities.Enums;
using BYT_Entities.Models;

namespace TestByt;

public class EmployeeTests
{
    private Cinema _defaultCinema;

    [SetUp]
    public void Setup()
    {
        Employee.ClearEmployees();
        Cinema.ClearCinemas();

        _defaultCinema = new Cinema(
            100, "TestCinema", "Street 1", "123", "mail@test.com", "08-22"
        );
    }

    [Test]
    public void ShouldThrowException_WhenNameIsEmpty()
    {
        Assert.Throws<ArgumentException>(() =>
            new Employee(
                1, "   ", "Smith", "12345678901", "john@cinema.com",
                new DateTime(1995, 5, 5), DateTime.Now.AddDays(-30),
                new Address("street", "1", "city", "00-000", "PL"),
                EmployeeStatus.Working,
                new List<Cinema> { _defaultCinema },
                ShiftType.Evening,
                WorkType.Cashier,
                25
            )
        );
    }

    [Test]
    public void ShouldThrowException_WhenBirthDateInFuture()
    {
        Assert.Throws<ArgumentException>(() =>
            new Employee(
                2, "Anna", "Smuga", "12345678901", "anna@test.com",
                DateTime.Now.AddDays(10), DateTime.Now,
                new Address("street", "1", "city", "00-000", "PL"),
                EmployeeStatus.Working,
                new List<Cinema> { _defaultCinema },
                ShiftType.Evening,
                WorkType.Cashier,
                30
            )
        );
    }

    [Test]
    public void ShouldThrowException_WhenEmailInvalid()
    {
        Assert.Throws<ArgumentException>(() =>
            new Employee(
                3, "Ania", "Smuga", "12345678901", "invalidEmail",
                new DateTime(1990, 1, 1), DateTime.Now,
                new Address("street", "1", "city", "00-000", "PL"),
                EmployeeStatus.Working,
                new List<Cinema> { _defaultCinema },
                ShiftType.Both,
                WorkType.Cashier,
                22
            )
        );
    }

    [Test]
    public void ShouldCreateEmployee_WhenAllDataValid()
    {
        Assert.DoesNotThrow(() =>
            new Employee(
                4, "Anna", "Smuga", "98765432109", "anna@success.com",
                new DateTime(1992, 3, 12), DateTime.Now.AddDays(-100),
                new Address("street", "1", "city", "00-000", "PL"),
                EmployeeStatus.Working,
                new List<Cinema> { _defaultCinema },
                ShiftType.Evening,
                WorkType.Cashier,
                35
            )
        );
    }

    [Test]
    public void Extent_ShouldStoreCreatedEmployees()
    {
        var e1 = new Employee(
            1, "John", "Doe", "12345678901", "john@test.com",
            new DateTime(1990, 1, 1), new DateTime(2020, 1, 1),
            new Address("street", "1", "city", "00-000", "PL"),
            EmployeeStatus.Working,
            new List<Cinema> { _defaultCinema },
            ShiftType.Evening,
            WorkType.Cashier,
            25
        );

        var e2 = new Employee(
            2, "Jane", "Smith", "98765432109", "jane@test.com",
            new DateTime(1992, 5, 5), new DateTime(2021, 2, 2),
            new Address("street", "1", "city", "00-000", "PL"),
            EmployeeStatus.Working,
            new List<Cinema> { _defaultCinema },
            ShiftType.Evening,
            WorkType.Cashier,
            30
        );

        var extent = Employee.GetEmployees();

        Assert.AreEqual(2, extent.Count);
        Assert.Contains(e1, extent);
        Assert.Contains(e2, extent);
    }

    [Test]
    public void Encapsulation_ShouldPreventDirectModificationOfPrivateFields()
    {
        var employee = new Employee(
            1, "John", "Doe", "12345678901", "john@test.com",
            new DateTime(1990, 1, 1), new DateTime(2020, 1, 1),
            new Address("street", "1", "city", "00-000", "PL"),
            EmployeeStatus.Working,
            new List<Cinema> { _defaultCinema },
            ShiftType.Evening,
            WorkType.Cashier,
            25
        );

        var nameField = typeof(Employee)
            .GetField("_name",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);

        nameField!.SetValue(employee, "TamperedName");

        var extent = Employee.GetEmployees();

        Assert.AreEqual("TamperedName", extent[0].Name);
    }
}
