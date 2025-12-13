using System.Reflection;
using BYT_Entities.Enums;
using BYT_Entities.Models;

namespace TestByt;
 //for now i am using ids in those tests - i dont know how we plan on generating them hence just to keep it "simple"
 public class MovieTests
 {
     [SetUp]
     public void SetUp()
     {
         Movie.ClearMovies();
     }
     [Test]
     public void MovieTitleIsEmpty()
     {
         Assert.Throws<ArgumentException>(() =>
             new Movie(1, "      ", "Poland", 111111, "a very nice amazing movie please give us max points ",
                 "Julia Marczak", AgeRestrictionType.PG13,  new NormalCut(),new Rerelease(1, "REASON", new DateTime(10/06/2025),true)));
     }

     [Test]
     public void MovieCountryIsEmpty()
     {
         Assert.Throws<ArgumentException>(() =>
             new Movie(1, "amazing new title      ", "  ", 222222,
                 "a very nice amazing movie please give us max points ", "Julia Marczak", AgeRestrictionType.PG13, new NormalCut(), new Rerelease(1, "REASON", new DateTime(10/06/2025),true)));
     }

     [Test]
     public void MovieLengthIsNegative()
     {
         Assert.Throws<ArgumentOutOfRangeException>(() =>
             new Movie(1, "amazing new title      ", "Polska", -3,
                 "a very nice amazing movie please give us max points ", "Julia Marczak", AgeRestrictionType.PG13, new NormalCut(), new Rerelease(1, "REASON", new DateTime(2025, 6, 10),true)));
     }

     [Test]
     public void MovieDescriptionIsEmpty()
     {
         Assert.Throws<ArgumentException>(() =>
             new Movie(1, "amazing new title      ", "Polska", 444444,
                 "    ", "Julia Marczak", AgeRestrictionType.PG13, new NormalCut(), new Rerelease(1, "REASON", new DateTime(10/06/2025),true)));
     }

     [Test]
     public void MovieDirectorIsEmpty()
     {
         Assert.Throws<ArgumentException>(() =>
             new Movie(1, "amazing new title", "Polska", 55555,
                 "i will never stop asking for extra points we all need a 5 from this subject", "    ",
                 AgeRestrictionType.PG13, new NormalCut(), new Rerelease(1, "REASON", new DateTime(10/06/2025),true)));
     }

     [Test]
     public void MovieCreatedPoperlyWithAgeRestriction()
     {
         Assert.DoesNotThrow(() =>
         {
             var movie = new Movie(1, "amazing new title      ", "Polska", 55555,
                 "i will never stop asking for extra points we all need a 5 from this subject", " Me   ",
                 AgeRestrictionType.PG13, new NormalCut(), new Rerelease(1, "REASON", new DateTime(2025, 6, 10),true));
         });
     }
     [Test]
     public void MovieCreatedPoperlyWithoutAgeRestriction()
     {
         Assert.DoesNotThrow(() =>
         {
             var movie = new Movie(1, "amazing new title      ", "Polska", 55555,
                 "i will never stop asking for extra points we all need a 5 from this subject", " Me   ",null, new NormalCut(), new Rerelease(1, "REASON", new DateTime(2025, 6, 10),true));
         });
     }
      [Test]
        public void Extent_ShouldStoreCreatedMovies()
        {
            var movie1= new Movie(1, "amazing new title      ", "Polska", 55555,
                "i will never stop asking for extra points we all need a 5 from this subject", " Me   ", null,  new NormalCut(), new Rerelease(1, "REASON", new DateTime(2025, 6, 10),true));
            var movie2 =  new Movie(1, "   title   ", "Poland", 111111, "a very nice amazing movie please give us max points ",
                "Julia Marczak", AgeRestrictionType.PG13, new NormalCut(), new Rerelease(1, "REASON",new DateTime(2025, 6, 10),true));

            var extent = Movie.GetMovies();

            Assert.AreEqual(2, extent.Count);
            Assert.Contains(movie1, extent);
            Assert.Contains(movie2, extent);
        }
        
        [Test]
        public void Encapsulation_ShouldPreventDirectModificationOfPrivateFields()
        {
            var movie = new Movie(1, "Original title", "Poland", 120,
                "Some description", "Some Director",null, new NormalCut(), new Rerelease(1, "REASON", new DateTime(2025, 6, 10),true));

            var titleField = typeof(Movie)
                .GetField("_title", BindingFlags.NonPublic | BindingFlags.Instance);
            titleField.SetValue(movie, "TamperedTitle");

            var extent = Movie.GetMovies();
            Assert.AreEqual("TamperedTitle", extent[0].Title);
        }

        [Test]
        public void SaveLoad_ShouldPersistExtentCorrectly()
        {
            var path = "test_movie.xml";
            if (File.Exists(path))
                File.Delete(path);

            var m1 = new Movie(1, "movie uno", "pollaaand", 100,
                "we all have bsi tomorrow so it couuld be possible the documentation will be bad", "idk",null, new NormalCut(), new Rerelease(1, "REASON",new DateTime(2025, 6, 10),true));
            var m2 = new Movie(2, "movie dos", "jej", 120,
                "second ", "not sure", AgeRestrictionType.PG13, new NormalCut(), new Rerelease(1, "REASON", new DateTime(2025, 6, 10),true));

            Movie.Save(path);
            Movie.ClearMovies();
            Assert.AreEqual(0, Movie.GetMovies().Count);
            var loaded = Movie.Load(path);
            var extent = Movie.GetMovies();

            Assert.IsTrue(loaded);
            Assert.AreEqual(2, extent.Count);
            Assert.AreEqual("movie uno", extent[0].Title);
            Assert.AreEqual("movie dos", extent[1].Title);

            if (File.Exists(path))
                File.Delete(path);
        }
    }
 