using BYT_Entities.Models;

namespace TestByt;

public class CinemaTests
{
    [SetUp]
    public void Setup()
    {
        Cinema.ClearCinemas();
    }
    
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
    
    //Extent tests
    [Test]
    public void Extent_ShouldStoreCreatedCinemas()
    {
        var c1 = new Cinema(1, "Cine1", "Addr1", "123", "c1@test.pl", "9-18");
        var c2 = new Cinema(2, "Cine2", "Addr2", "456", "c2@test.pl", "10-20");

        var extent = Cinema.GetCinemas();

        Assert.AreEqual(2, extent.Count);
        Assert.Contains(c1, extent);
        Assert.Contains(c2, extent);
    }
    
    [Test]
    public void Encapsulation_ShouldPreventDirectModificationOfExtentFields()
    {
        var cinema = new Cinema(1, "OriginalName", "Addr", "123", "test@test.pl", "9-18");

        var nameField = typeof(Cinema).GetField("_name", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        nameField.SetValue(cinema, "TamperedName");

        var extent = Cinema.GetCinemas();

        Assert.AreEqual("TamperedName", extent[0].Name);
    }
    
    [Test]
    public void SaveLoad_ShouldPersistExtentCorrectly()
    {
        var path = "test_cinema.xml";
        var c1 = new Cinema(1, "Cine1", "Addr1", "123", "c1@test.pl", "9-18");
        var c2 = new Cinema(2, "Cine2", "Addr2", "456", "c2@test.pl", "10-20");

        Cinema.Save(path);

        Cinema.ClearCinemas();
        Assert.AreEqual(0, Cinema.GetCinemas().Count);

        var loaded = Cinema.Load(path);
        var extent = Cinema.GetCinemas();

        Assert.IsTrue(loaded);
        Assert.AreEqual(2, extent.Count);
        Assert.AreEqual("Cine1", extent[0].Name);
        Assert.AreEqual("Cine2", extent[1].Name);
        Assert.AreEqual("10-20", extent[1].OpeningHours);
        
        if (File.Exists(path))
            File.Delete(path);
    }
}
    
