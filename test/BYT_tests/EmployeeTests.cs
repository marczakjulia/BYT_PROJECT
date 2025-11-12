using BYT_Entities.Models;

namespace TestByt;

public class EmployeeTests
{
    [Test]
    public void ShouldThrowException_WhenNameIsEmpty()
    {
        Assert.Throws<ArgumentException>(() => new Employee(1, "   ", "Smith", "12345678901", "john@cinema.com", new DateTime(1995, 5, 5), DateTime.Now.AddDays(-30), 4000));
    }
    [Test]
    public void ShouldThrowException_WhenBirthDateInFuture()
    {
        Assert.Throws<ArgumentException>(() => new Employee(2,"Anna", "Smuga", "12345678901", "aaaa@aaa.pl", DateTime.Now.AddDays(10), DateTime.Now, 4000));
    }
    
    [Test]
    public void ShouldThrowException_WhenEmailInvalid()
    {
        Assert.Throws<ArgumentException>(() => new Employee(3, "Ania", "Smuga", "12345678901", "aaaania", new DateTime(1990, 1, 1), DateTime.Now, 4000));
    }
    
    [Test]
    public void ShouldCreateEmployee_WhenAllDataValid()
    {
        Assert.DoesNotThrow(() => new Employee(4, "Anna", "Smuga", "98765432109", "anna@success.com", new DateTime(1992, 3, 12), DateTime.Now.AddDays(-100), 5000));
    }
    
}