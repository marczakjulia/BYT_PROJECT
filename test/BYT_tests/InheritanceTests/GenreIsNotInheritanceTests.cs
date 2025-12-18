using BYT_Entities.Interfaces;

namespace TestByt.InheritanceTests;

using BYT_Entities.Models;
using NUnit.Framework;

public class GenreIsNotInheritanceTests
{
    [Test]
    public void Genre_ShouldNotBe_Movie()
    {
        Assert.That(typeof(ComedyMovie).IsSubclassOf(typeof(Movie)), Is.False);
        Assert.That(typeof(HorrorMovie).IsSubclassOf(typeof(Movie)), Is.False);
        Assert.That(typeof(RomanceMovie).IsSubclassOf(typeof(Movie)), Is.False);
    }

    [Test]
    public void Genre_ShouldImplement_IGenreType()
    {
        Assert.That(typeof(ComedyMovie).GetInterfaces(), Contains.Item(typeof(IGenreType)));
        Assert.That(typeof(HorrorMovie).GetInterfaces(), Contains.Item(typeof(IGenreType)));
        Assert.That(typeof(RomanceMovie).GetInterfaces(), Contains.Item(typeof(IGenreType)));
    }
    
}
