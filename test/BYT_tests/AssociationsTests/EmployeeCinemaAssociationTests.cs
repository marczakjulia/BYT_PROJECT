using BYT_Entities.Complex;
using BYT_Entities.Enums;

namespace TestByt;
using BYT_Entities.Models;

public class EmployeeCinemaAssociationTests
{
    [SetUp]
    public void SetUp()
    {
        Cinema.ClearCinemas();
        Employee.ClearEmployees();
    }

    [Test]
    public void AddEmployeeToCinema_CreatesReverseConnection()
    {
        var cinema = new Cinema(1, "Galaxy", "Address", "123", "mail@mail.com", "10-22");
        var emp = new Employee(1, "Anna", "Nowak", "123", "a@mail.com",
            DateTime.Now.AddYears(-20), DateTime.Now.AddYears(-2), 3000,
            new Address("s","1","c","00-000","pl"), EmployeeStatus.Working);

        cinema.AddEmployee(emp);

        Assert.That(cinema.GetEmployees().Contains(emp));
        Assert.That(emp.GetCinemas().Contains(cinema));
    }

    [Test]
    public void AddCinemaFromEmployeeSide_CreatesReverseConnection()
    {
        var cinema = new Cinema(1, "Galaxy", "Address", "123", "mail@mail.com", "10-22");
        var emp = new Employee(1, "Anna", "Nowak", "123", "a@mail.com",
            DateTime.Now.AddYears(-20), DateTime.Now.AddYears(-2), 3000,
            new Address("s","1","c","00-000","pl"), EmployeeStatus.Working);

        emp.AddCinema(cinema);

        Assert.That(emp.GetCinemas().Contains(cinema));
        Assert.That(cinema.GetEmployees().Contains(emp));
    }

    [Test]
    public void RemoveEmployeeFromCinema_RemovesReverseConnection()
    {
        var cinema = new Cinema(1, "Galaxy", "Address", "123", "mail@mail.com", "10-22");
        var emp = new Employee(1, "Anna", "Nowak", "123", "a@mail.com",
            DateTime.Now.AddYears(-20), DateTime.Now.AddYears(-2), 3000,
            new Address("s","1","c","00-000","pl"), EmployeeStatus.Working);

        cinema.AddEmployee(emp);
        cinema.RemoveEmployee(emp);

        Assert.That(!cinema.GetEmployees().Contains(emp));
        Assert.That(!emp.GetCinemas().Contains(cinema));
    }

    [Test]
    public void RemoveCinemaFromEmployee_RemovesReverseConnection()
    {
        var cinema = new Cinema(1, "Galaxy", "Address", "123", "mail@mail.com", "10-22");
        var emp = new Employee(1, "Anna", "Nowak", "123", "a@mail.com",
            DateTime.Now.AddYears(-20), DateTime.Now.AddYears(-2), 3000,
            new Address("s","1","c","00-000","pl"), EmployeeStatus.Working);

        emp.AddCinema(cinema);
        emp.RemoveCinema(cinema);

        Assert.That(!cinema.GetEmployees().Contains(emp));
        Assert.That(!emp.GetCinemas().Contains(cinema));
    }

    [Test]
    public void AddEmployee_Null_Throws()
    {
        var cinema = new Cinema(1, "Galaxy", "Address", "123", "mail@mail.com", "10-22");
        Assert.Throws<ArgumentException>(() => cinema.AddEmployee(null));
    }

    [Test]
    public void AddCinema_Null_Throws()
    {
        var emp = new Employee(1, "Anna", "Nowak", "123", "a@mail.com",
            DateTime.Now.AddYears(-20), DateTime.Now.AddYears(-2), 3000,
            new Address("s","1","c","00-000","pl"), EmployeeStatus.Working);

        Assert.Throws<ArgumentException>(() => emp.AddCinema(null));
    }

    [Test]
    public void AddSameEmployeeTwice_Throws()
    {
        var cinema = new Cinema(1, "Galaxy", "Address", "123", "mail@mail.com", "10-22");
        var emp = new Employee(1, "Anna", "Nowak", "123", "a@mail.com",
            DateTime.Now.AddYears(-20), DateTime.Now.AddYears(-2), 3000,
            new Address("s","1","c","00-000","pl"), EmployeeStatus.Working);

        cinema.AddEmployee(emp);

        var ex = Assert.Throws<InvalidOperationException>(() => cinema.AddEmployee(emp));
        Assert.That(ex.Message, Does.Contain("already linked"));
    }

    [Test]
    public void AddSameCinemaTwice_Throws()
    {
        var cinema = new Cinema(1, "Galaxy", "Address", "123", "mail@mail.com", "10-22");
        var emp = new Employee(1, "Anna", "Nowak", "123", "a@mail.com",
            DateTime.Now.AddYears(-20), DateTime.Now.AddYears(-2), 3000,
            new Address("s","1","c","00-000","pl"), EmployeeStatus.Working);

        emp.AddCinema(cinema);

        var ex = Assert.Throws<InvalidOperationException>(() => emp.AddCinema(cinema));
        Assert.That(ex.Message, Does.Contain("already linked"));
    } 

    [Test]
    public void ManyToMany_AssociationWorksBothWays()
    {
        var c1 = new Cinema(1, "Galaxy", "Address", "123", "mail@mail.com", "10-22");
        var c2 = new Cinema(2, "Silver", "Address2", "321", "silver@mail.com", "9-23");

        var e1 = new Employee(1, "Anna", "Nowak", "123", "a@mail.com",
            DateTime.Now.AddYears(-20), DateTime.Now.AddYears(-2), 3000,
            new Address("s","1","c","00-000","pl"), EmployeeStatus.Working);

        var e2 = new Employee(2, "Pawel", "Kowal", "456", "p@mail.com",
            DateTime.Now.AddYears(-30), DateTime.Now.AddYears(-5), 4000,
            new Address("s","1","c","00-000","pl"), EmployeeStatus.Working);

        c1.AddEmployee(e1);
        c1.AddEmployee(e2);
        c2.AddEmployee(e1);

        Assert.That(c1.GetEmployees().SequenceEqual(new[] { e1, e2 }));
        Assert.That(c2.GetEmployees().SequenceEqual(new[] { e1 }));
        Assert.That(e1.GetCinemas().Contains(c1) && e1.GetCinemas().Contains(c2));
        Assert.That(e2.GetCinemas().Contains(c1));
    }
}
