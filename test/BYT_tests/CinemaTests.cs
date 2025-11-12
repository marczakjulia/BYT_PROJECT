using BYT_Entities.Models;

namespace TestByt;

public class CinemaTests
{
    [Test]
    public void Name_ShouldThrow_WhenEmpty()
    {
        Assert.Throws<ArgumentException>(() => new Cinema(1, " ", "Street 123", "+48657363638","cinema@test.pl", "9-18"));
    }
    
    [Test]
    public void ShouldThrowException_WhenEmailInvalid()
    {
        Assert.Throws<ArgumentException>(() => new Cinema(2, "Cinema","Street 5", "+4812345678901", "cinema", "8-21"));
    }
    
    [Test]
    public void ShouldCreateCinema_WhenAllDataValid()
    {
        Assert.DoesNotThrow(() => new Cinema(3, "Cinema","Street 555", "+4812345678901", "cinema@cinema", "8-21"));    
    }
    
}