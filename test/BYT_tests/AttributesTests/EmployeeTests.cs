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
                new DateTime(1995, 5, 5), DateTime.Now.AddDays(-30), 4000,
                new Address("street", "buildingNumber", "city", "postalCode", "country"),
                EmployeeStatus.Working,
                new List<Cinema> { _defaultCinema }
            )
        );
    }

    [Test]
    public void ShouldThrowException_WhenBirthDateInFuture()
    {
        Assert.Throws<ArgumentException>(() =>
            new Employee(
                2, "Anna", "Smuga", "12345678901", "aaaa@aaa.pl",
                DateTime.Now.AddDays(10), DateTime.Now, 4000,
                new Address("street", "buildingNumber", "city", "postalCode", "country"),
                EmployeeStatus.Working,
                new List<Cinema> { _defaultCinema }
            )
        );
    }
    
    [Test]
    public void ShouldThrowException_WhenEmailInvalid()
    {
        Assert.Throws<ArgumentException>(() =>
            new Employee(
                3, "Ania", "Smuga", "12345678901", "aaaania",
                new DateTime(1990, 1, 1), DateTime.Now, 4000,
                new Address("street", "buildingNumber", "city", "postalCode", "country"),
                EmployeeStatus.Working,
                new List<Cinema> { _defaultCinema }
            )
        );
    }
    
    [Test]
    public void ShouldCreateEmployee_WhenAllDataValid()
    {
        Assert.DoesNotThrow(() =>
            new Employee(
                4, "Anna", "Smuga", "98765432109", "anna@success.com",
                new DateTime(1992, 3, 12), DateTime.Now.AddDays(-100), 5000,
                new Address("street", "buildingNumber", "city", "postalCode", "country"),
                EmployeeStatus.Working,
                new List<Cinema> { _defaultCinema }
            )
        );
    }
    
    [Test]
    public void Extent_ShouldStoreCreatedEmployees()
    {
        var e1 = new Employee( 
            1, "John", "Doe", "12345678901", "john@test.com",
            new DateTime(1990, 1, 1), new DateTime(2020, 1, 1), 3000,
            new Address("street", "buildingNumber", "city", "postalCode", "country"),
            EmployeeStatus.Working,
            new List<Cinema> { _defaultCinema }
        );

        var e2 = new Employee(
            2, "Jane", "Smith", "98765432109", "jane@test.com",
            new DateTime(1992, 5, 5), new DateTime(2021, 2, 2), 3500,
            new Address("street", "buildingNumber", "city", "postalCode", "country"),
            EmployeeStatus.Working,
            new List<Cinema> { _defaultCinema }
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
            new DateTime(1990, 1, 1), new DateTime(2020, 1, 1), 3000,
            new Address("street", "buildingNumber", "city", "postalCode", "country"),
            EmployeeStatus.Working,
            new List<Cinema> { _defaultCinema }
        );

        var nameField = typeof(Employee).GetField("_name",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance);

        nameField.SetValue(employee, "TamperedName");

        var extent = Employee.GetEmployees();

        Assert.AreEqual("TamperedName", extent[0].Name);
    }
}
