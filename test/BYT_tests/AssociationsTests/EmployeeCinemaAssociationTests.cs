using BYT_Entities.Models;
using BYT_Entities.Enums;
using BYT_Entities.Complex;

namespace TestByt;

public class EmployeeCinemaAssociationTests
{
    private Cinema c1;
    private Cinema c2;
    private Employee e1;

    [SetUp]
    public void Setup()
    {
        Employee.ClearEmployees();
        Cinema.ClearCinemas();

        c1 = new Cinema(1, "C1", "A", "1", "a@a.com", "10-20");
        c2 = new Cinema(2, "C2", "B", "2", "b@b.com", "10-20");

        e1 = new Employee(
            id: 10,
            name: "Anna",
            surname: "Boss",
            pesel: "12345678901",
            email: "boss@test.com",
            dayOfBirth: DateTime.Now.AddYears(-25),
            hireDate: DateTime.Now.AddYears(-2),
            address: new Address("s", "1", "c", "00-000", "PL"),
            status: EmployeeStatus.Working,
            cinemas: new List<Cinema> { c1 },

            // Worker-specific
            shift: ShiftType.Morning,
            typeOfWork: WorkType.Cashier,
            hourlyRate: 25.0
        );
    }

    [Test]
    public void AddCinema_ShouldCreateReverseConnection()
    {
        e1.AddCinema(c2);

        Assert.IsTrue(e1.GetCinemas().Contains(c2));
        Assert.IsTrue(c2.GetEmployees().Contains(e1));
    }

    [Test]
    public void RemoveCinema_ShouldRemoveReverseConnection()
    {
        Assert.IsTrue(e1.GetCinemas().Contains(c1));
        Assert.IsTrue(c1.GetEmployees().Contains(e1));

        e1.RemoveCinema(c1);

        Assert.IsFalse(e1.GetCinemas().Contains(c1));
        Assert.IsFalse(c1.GetEmployees().Contains(e1));
    }

    [Test]
    public void ModifyCinema_ShouldRemoveOldAndAddNew_WithReverseConnections()
    {
        e1.RemoveCinema(c1);
        e1.AddCinema(c2);

        Assert.IsFalse(e1.GetCinemas().Contains(c1));
        Assert.IsTrue(e1.GetCinemas().Contains(c2));

        Assert.IsFalse(c1.GetEmployees().Contains(e1));
        Assert.IsTrue(c2.GetEmployees().Contains(e1));
    }

    [Test]
    public void AddCinema_ShouldThrow_WhenNull()
    {
        Assert.Throws<ArgumentException>(() => e1.AddCinema(null!));
    }

    [Test]
    public void RemoveCinema_ShouldThrow_WhenNull()
    {
        Assert.Throws<ArgumentException>(() => e1.RemoveCinema(null!));
    }

    [Test]
    public void AddSameCinemaTwice_ShouldNotDuplicate()
    {
        int before = e1.GetCinemas().Count;

        e1.AddCinema(c1);

        int after = e1.GetCinemas().Count;

        Assert.AreEqual(before, after);
    }

    [Test]
    public void RemoveCinema_NotAssigned_ShouldDoNothing()
    {
        Assert.DoesNotThrow(() => e1.RemoveCinema(c2));
    }
}
