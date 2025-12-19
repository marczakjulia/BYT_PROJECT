using BYT_Entities.Interfaces;
using BYT_Entities.Models;

namespace TestByt.InheritanceTests;

public class CutTypesTests
{
    [Test]
    public void Movie_ShouldBeAbstract_AndNotInstantiable()
    {
        Assert.That(typeof(Movie).IsAbstract, Is.True);
    }

    [Test]
    public void Cut_ShouldBeExactlyOneConcreteType()
    {
        Movie movie = new NormalCut(
            1,
            "Test",
            "USA",
            100,
            "Desc",
            "Dir",
            null,
            new IGenreType[] { new ComedyMovie("satire") },
            new Rerelease(1,"r",DateTime.Now,true)
        );

        Assert.That(movie, Is.InstanceOf<NormalCut>());
        Assert.That(movie, Is.Not.InstanceOf<DirectorCut>());
        Assert.That(movie, Is.Not.InstanceOf<ExtendedCut>());
    }
    [Test]
    public void DifferentCuts_ShouldHaveDifferentTotalRuntime()
    {
        var genres = new IGenreType[] { new HorrorMovie(5) };

        Movie normal = new NormalCut(
            1, "Normal", "USA", 100, "Desc", "Dir", null,
            genres,
            new Rerelease(1,"r",DateTime.Now,true)
        );

        Movie director = new DirectorCut(
            2, "Director", "USA", 100, "Desc", "Dir", null,
            genres,
            extraMinutes: 20,
            changesDescription: "Extra scenes",
            rerelease: new Rerelease(2,"r",DateTime.Now,true)
        );

        Assert.That(normal.GetTotalRuntime(), Is.EqualTo(100));
        Assert.That(director.GetTotalRuntime(), Is.EqualTo(120));
    }
    [Test]
    public void MovieReference_ShouldCallCorrectCutImplementation()
    {
        Movie movie = new DirectorCut(
            1,
            "Test",
            "USA",
            90,
            "Desc",
            "Dir",
            null,
            new IGenreType[] { new ComedyMovie("parody") },
            extraMinutes: 15,
            changesDescription: "Extended ending",
            rerelease: new Rerelease(1,"r",DateTime.Now,true)
        );

        Assert.That(movie.GetCutName(), Is.EqualTo("Director Cut"));
        Assert.That(movie.GetTotalRuntime(), Is.EqualTo(105));
    }
    [Test]
    public void AllMoviesInExtent_ShouldBeConcreteCutTypes()
    {
        Movie.ClearMovies();

        var genres = new IGenreType[] { new ComedyMovie("satire") };

        new NormalCut(1, "A", "USA", 100, "D", "X", null, genres,
            new Rerelease(1,"r",DateTime.Now,true));

        new DirectorCut(2, "B", "USA", 100, "D", "X", null, genres,
            10, "Changes", "f",
            new Rerelease(2,"r", DateTime.Now,true));

        var movies = Movie.GetMovies();

        Assert.That(movies.All(m =>
                m is NormalCut || m is DirectorCut || m is ExtendedCut),
            Is.True);
    }




}