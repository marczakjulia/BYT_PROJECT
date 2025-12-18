using BYT_Entities.Interfaces;

namespace TestByt.InheritanceTests;

using BYT_Entities.Models;
using NUnit.Framework;

public class MovieGenreCompositionTests
{
    [Test]
    public void Movie_ShouldRequire_AtLeastOneGenre()
    {
        Assert.Throws<ArgumentException>(() =>
            new NormalCut(
                1,
                "Test",
                "USA",
                100,
                "Desc",
                "Dir",
                null,
                Enumerable.Empty<IGenreType>(),
                new Rerelease(1,"b",DateTime.Now,true)
            )
        );
    }

    [Test]
    public void Movie_ShouldContain_Genres_AsComposition()
    {
        var comedy = new ComedyMovie("satire");
        var horror = new HorrorMovie(7);

        var movie = new NormalCut(
            1,
            "Test",
            "USA",
            100,
            "Desc",
            "Dir",
            null,
            new IGenreType[] { comedy, horror },
            new Rerelease(1,"b",DateTime.Now,true)
        );

        Assert.That(movie.Genres.Count, Is.EqualTo(2));
        Assert.That(movie.IsComedy(), Is.True);
        Assert.That(movie.IsHorror(), Is.True);
    }

    [Test]
    public void Movie_ShouldAllow_OverlappingGenres()
    {
        var movie = new NormalCut(
            1,
            "Mixed",
            "USA",
            100,
            "Desc",
            "Dir",
            null,
            new IGenreType[]
            {
                new ComedyMovie("dark"),
                new RomanceMovie(4)
            },
            new Rerelease(1,"b",DateTime.Now,true)
        );

        Assert.That(movie.IsComedy(), Is.True);
        Assert.That(movie.IsRomance(), Is.True);
    }

    [Test]
    public void Movie_ShouldReturn_SpecificGenreInstance()
    {
        var comedy = new ComedyMovie("parody");

        var movie = new NormalCut(
            1,
            "Test",
            "USA",
            100,
            "Desc",
            "Dir",
            null,
            new IGenreType[] { comedy },
            new Rerelease(1,"b",DateTime.Now,true)
        );

        Assert.That(movie.GetComedy(), Is.EqualTo(comedy));
    }
}
