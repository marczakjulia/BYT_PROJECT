using BYT_Entities.Enums;
using BYT_Entities.Models;

namespace TestByt;

public class XORTest
{
    [SetUp]
        public void SetUp()
        {
            Movie.ClearMovies();
            NewRelease.ClearNewRleaseMovies();
            Rerelease.ClearReleaseMovies();
        }

        [Test]
        public void CreateMovie_WithNewRelease_ShouldSetReverseConnection()
        {
            var nr = new NewRelease(1, true, DateTime.Now, "Disney");
            var movie = new Movie(1, "Title", "USA", 100, "Desc", "Dir", AgeRestrictionType.PG13, nr);

            Assert.AreEqual(nr, movie.NewRelease);
            Assert.AreEqual(movie, nr.Movie);
        }

        [Test]
        public void CreateMovie_WithRerelease_ShouldSetReverseConnection()
        {
            var rr = new Rerelease(1, "Reason", DateTime.Now, false);
            var movie = new Movie(1, "Title", "USA", 100, "Desc", "Dir", AgeRestrictionType.PG13, rr);

            Assert.AreEqual(rr, movie.Rerelease);
            Assert.AreEqual(movie, rr.Movie);
        }
        
        [Test]
        public void CannotAssignNewRelease_WhenRereleaseAlreadyAssigned()
        {
            var rr = new Rerelease(1, "Reason", DateTime.Now, false);
            var nr = new NewRelease(2, true, DateTime.Now, "Warner");

            var movie = new Movie(1, "T", "C", 90, "D", "X", null, rr);

            Assert.Throws<InvalidOperationException>(() => movie.SetNewRelease(nr));
        }

        [Test]
        public void CannotAssignRerelease_WhenNewReleaseAlreadyAssigned()
        {
            var nr = new NewRelease(1, true, DateTime.Now, "Sony");
            var rr = new Rerelease(2, "Reason", DateTime.Now, false);

            var movie = new Movie(1, "T", "C", 100, "D", "X", null, nr);

            Assert.Throws<InvalidOperationException>(() => movie.SetRerelease(rr));
        }

        [Test]
        public void RemoveNewRelease_ShouldBreakReverseConnection()
        {
            var nr = new NewRelease(1, true, DateTime.Now, "Universal");
            var movie = new Movie(1, "Movie", "USA", 100, "D", "X", null, nr);

            movie.RemoveNewRelease();

            Assert.IsNull(movie.NewRelease);
            Assert.IsNull(nr.Movie);
        }

        [Test]
        public void RemoveRerelease_ShouldBreakReverseConnection()
        {
            var rr = new Rerelease(1, "Reason", DateTime.Now, true);
            var movie = new Movie(1, "Movie", "USA", 100, "D", "X", null, rr);

            movie.RemoveRerelease();

            Assert.IsNull(movie.Rerelease);
            Assert.IsNull(rr.Movie);
        }


        [Test]
        public void UpdateNewRelease_ShouldReplaceReferenceAndKeepXor()
        {
            var nr1 = new NewRelease(1, true, DateTime.Now, "A24");
            var nr2 = new NewRelease(2, false, DateTime.Now, "Netflix");

            var movie = new Movie(1, "T", "C", 100, "D", "X", null, nr1);

            movie.UpdateNewRelease(nr2);

            Assert.AreEqual(nr2, movie.NewRelease);
            Assert.IsNull(nr1.Movie);  
            Assert.AreEqual(movie, nr2.Movie);
        }

        [Test]
        public void UpdateRerelease_ShouldReplaceReferenceAndKeepXor()
        {
            var rr1 = new Rerelease(1, "Old", DateTime.Now, false);
            var rr2 = new Rerelease(2, "New", DateTime.Now, true);

            var movie = new Movie(1, "T", "C", 100, "D", "X", null, rr1);

            movie.UpdateRerelease(rr2);

            Assert.AreEqual(rr2, movie.Rerelease);
            Assert.IsNull(rr1.Movie);
            Assert.AreEqual(movie, rr2.Movie);
        }
        

        [Test]
        public void SetNewRelease_WithNull_ShouldThrow()
        {
            var movie = new Movie(1, "T", "C", 80, "D", "X", null, new NewRelease());
            Assert.Throws<ArgumentException>(() => movie.SetNewRelease(null));
        }

        [Test]
        public void SetRerelease_WithNull_ShouldThrow()
        {
            var movie = new Movie(1, "T", "C", 80, "D", "X",null, new Rerelease());
            Assert.Throws<ArgumentException>(() => movie.SetRerelease(null));
        }

        [Test]
        public void UpdateNewRelease_WhenMovieIsRerelease_ShouldThrow()
        {
            var rr = new Rerelease(1, "R", DateTime.Now, false);
            var nr = new NewRelease(2, true, DateTime.Now, "Disney");

            var movie = new Movie(1, "T", "C", 80, "D", "X", null, rr);

            Assert.Throws<InvalidOperationException>(() => movie.UpdateNewRelease(nr));
        }

        [Test]
        public void UpdateRerelease_WhenMovieIsNewRelease_ShouldThrow()
        {
            var rr = new Rerelease(1, "R", DateTime.Now, false);
            var nr = new NewRelease(2, true, DateTime.Now, "Disney");

            var movie = new Movie(1, "T", "C", 80, "D", "X", null, nr);

            Assert.Throws<InvalidOperationException>(() => movie.UpdateRerelease(rr));
        }
    }

