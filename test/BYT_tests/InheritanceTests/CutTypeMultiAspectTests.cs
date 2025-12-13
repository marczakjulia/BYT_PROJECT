using BYT_Entities.Interfaces;

namespace TestByt.InheritanceTests;

using NUnit.Framework;
using BYT_Entities.Models;
using BYT_Entities.Enums;

public class MovieCutTypeTests
{
    [SetUp]
    public void Setup()
    {
        Movie.ClearMovies();
        NormalCut.ClearNormalCutsMovies();
        DirectorCut.ClearDirectorCutsMovies();
        ExtendedCut.ClearExtendedCutMovies();
    }
    
    [Test]
    public void MultiAspect_CutType_IsDisjoint()
    {
        var normalCut = new NormalCut(1);
        var newRelease = new NewRelease(1, true,DateTime.Now, "Pixar");
        var movie = new Movie(1, "Test Movie", "USA", 120, "Description", 
            "Director", AgeRestrictionType.PG13, normalCut, newRelease);

        int typeCount = 0;
        if (movie.IsNormalCut()) typeCount++;
        if (movie.IsDirectorCut()) typeCount++;
        if (movie.IsExtendedCut()) typeCount++;
        
        Assert.That(typeCount, Is.EqualTo(1));
    }

    [Test]
    public void MultiAspect_CutType_IsComplete()
    {
        var normalCut = new NormalCut(1);
        var newRelease = new NewRelease(1, true,DateTime.Now, "Pixar");
        var movie = new Movie(1, "Test Movie", "USA", 120, "Description", 
            "Director", AgeRestrictionType.PG13, normalCut, newRelease);

        Assert.That(movie.CutType, Is.Not.Null);
        
        bool hasType = movie.IsNormalCut() || movie.IsDirectorCut() || movie.IsExtendedCut();
        Assert.That(hasType, Is.True);
    }

    [Test]
    public void MultiAspect_CutType_IsStatic()
    {
        var normalCut = new NormalCut(1);
        var newRelease = new NewRelease(1, true,DateTime.Now, "Pixar");
        var movie = new Movie(1, "Test Movie", "USA", 120, "Description", 
            "Director", AgeRestrictionType.PG13, normalCut, newRelease);

        var initialType = movie.GetCutTypeName();
        var initialIsNormal = movie.IsNormalCut();

        Assert.That(initialType, Is.EqualTo("Normal Cut"));
        Assert.That(initialIsNormal, Is.True);
    }

    [Test]
    public void MultiAspect_DifferentMovies_CanHaveDifferentCutTypes()
    {
        var movie1 = new Movie(1, "Movie 1", "USA", 120, "Desc", "Dir", 
            AgeRestrictionType.PG13, new NormalCut(1), new NewRelease(1, true,DateTime.Now, "Pixar"));
        
        var movie2 = new Movie(2, "Movie 2", "USA", 130, "Desc", "Dir", AgeRestrictionType.PG13,new DirectorCut(1, 14, "character's family is also included"), new NewRelease(1, true,DateTime.Now, "Pixar"));
        
        var movie3 = new Movie(3, "Movie 3", "USA", 140, "Desc", "Dir", 
            AgeRestrictionType.PG13, new ExtendedCut(1, "Additional character development and world-building", 30, new List<string> { "Extended Lothlorien", "More Council of Elrond" }), new NewRelease(1, true,DateTime.Now, "Pixar"));

        
        // Each movie has independent cut type 
        Assert.That(movie1.IsNormalCut(), Is.True);
        Assert.That(movie2.IsDirectorCut(), Is.True);
        Assert.That(movie3.IsExtendedCut(), Is.True);
        
        // All three cut types can coexist in different movie instances
        Assert.That(Movie.GetMovies().Count, Is.EqualTo(3));
    }

    
    [Test]
    public void Inheritance_CutType_IsNonOverlapping()
    {
        var directorCut = new DirectorCut(1, 20, "Changes");
        var newRelease = new NewRelease(1, true,DateTime.Now, "Pixar");
        var movie = new Movie(1, "Test Movie", "USA", 120, "Description", 
            "Director", AgeRestrictionType.PG18, directorCut, newRelease);

        Assert.That(movie.IsDirectorCut(), Is.True);
        Assert.That(movie.IsExtendedCut(), Is.False);
        Assert.That(movie.IsNormalCut(), Is.False);
        
        // Only can be 1 of the instances
        int trueCount = new[] { 
            movie.IsNormalCut(), 
            movie.IsDirectorCut(), 
            movie.IsExtendedCut() 
        }.Count(x => x);
        
        Assert.That(trueCount, Is.EqualTo(1));
    }

    
    [Test]
    public void Composition_Movie_HasACutType()
    {
        var normalCut = new NormalCut(1);
        var newRelease = new NewRelease(1, true,DateTime.Now, "Pixar");
        var movie = new Movie(1, "Test Movie", "USA", 120, "Description", 
            "Director", AgeRestrictionType.PG13, normalCut, newRelease);

        // Movie is NOT derived from CutType
        Assert.That(movie, Is.Not.InstanceOf<NormalCut>());
        Assert.That(movie, Is.Not.InstanceOf<DirectorCut>());
        Assert.That(movie, Is.Not.InstanceOf<ExtendedCut>());
        Assert.That(movie, Is.Not.InstanceOf<ICutType>());
        
        // Movie HAS a CutType
        Assert.That(movie.CutType, Is.InstanceOf<ICutType>());
        Assert.That(movie.CutType, Is.InstanceOf<NormalCut>());
    }


    [Test]
    public void Composition_Movie_DelegatesBehaviorToCutType()
    {
        var directorCut = new DirectorCut(1, 25, "Changes");
        var newRelease = new NewRelease(1, true,DateTime.Now, "Pixar");
        var movie = new Movie(1, "Test Movie", "USA", 120, "Description", 
            "Director", AgeRestrictionType.PG18, directorCut, newRelease);

        // Act & Assert - Movie delegates to CutType component
        Assert.That(movie.GetCutTypeName(), Is.EqualTo(directorCut.GetCutTypeName()));
        Assert.That(movie.CutType.GetExtraMinutes(), Is.EqualTo(directorCut.GetExtraMinutes()));
        Assert.That(movie.GetTotalRuntime(), Is.EqualTo(120 + 25));
    }
    

    [Test]
    public void Integration_NullCutType_DefaultsToNormalCut()
    {
        var newRelease = new NewRelease(1, true, DateTime.Now, "Pixar");
        var movie = new Movie(1, "Test Movie", "USA", 120, "Description", 
            "Director", AgeRestrictionType.PG13, null, newRelease);

        // Default cut is NormalCut
        Assert.That(movie.CutType, Is.Not.Null);
        Assert.That(movie.IsNormalCut(), Is.True);
        Assert.That(movie.GetCutTypeName(), Is.EqualTo("Normal Cut"));
    }

}