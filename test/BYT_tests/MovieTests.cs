using BYT_Entities.Enums;
using BYT_Entities.Models;

namespace TestByt;
 //for now i am using ids in those tests - i dont know how we plan on generating them hence just to keep it "simple"
 public class MovieTests
 {
     [Test]
     public void MovieTitleIsEmpty()
     {
         Assert.Throws<ArgumentException>(() =>
             new Movie(1, "      ", "Poland", 111111, "a very nice amazing movie please give us max points ",
                 "Julia Marczak", AgeRestrictionType.PG13));
     }

     [Test]
     public void MovieCountryIsEmpty()
     {
         Assert.Throws<ArgumentException>(() =>
             new Movie(1, "amazing new title      ", "  ", 222222,
                 "a very nice amazing movie please give us max points ", "Julia Marczak", AgeRestrictionType.PG13));
     }

     [Test]
     public void MovieLengthIsNegative()
     {
         Assert.Throws<ArgumentOutOfRangeException>(() =>
             new Movie(1, "amazing new title      ", "Polska", -3,
                 "a very nice amazing movie please give us max points ", "Julia Marczak", AgeRestrictionType.PG13));
     }

     [Test]
     public void MovieDescriptionIsEmpty()
     {
         Assert.Throws<ArgumentException>(() =>
             new Movie(1, "amazing new title      ", "Polska", 444444,
                 "    ", "Julia Marczak", AgeRestrictionType.PG13));
     }

     [Test]
     public void MovieDirectorIsEmpty()
     {
         Assert.Throws<ArgumentException>(() =>
             new Movie(1, "amazing new title", "Polska", 55555,
                 "i will never stop asking for extra points we all need a 5 from this subject", "    ",
                 AgeRestrictionType.PG13));
     }

     [Test]
     public void MovieCreatedPoperlyWithAgeRestriction()
     {
         Assert.DoesNotThrow(() =>
         {
             var movie = new Movie(1, "amazing new title      ", "Polska", 55555,
                 "i will never stop asking for extra points we all need a 5 from this subject", " Me   ",
                 AgeRestrictionType.PG13);
         });
     }
     [Test]
     public void MovieCreatedPoperlyWithoutAgeRestriction()
     {
         Assert.DoesNotThrow(() =>
         {
             var movie = new Movie(1, "amazing new title      ", "Polska", 55555,
                 "i will never stop asking for extra points we all need a 5 from this subject", " Me   ");
         });
     }
 }