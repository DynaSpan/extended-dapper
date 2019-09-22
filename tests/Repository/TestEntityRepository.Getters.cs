using System;
using System.Collections.Generic;
using System.Linq;
using Extended.Dapper.Tests.Models;
using NUnit.Framework;

namespace Extended.Dapper.Tests.Repository
{
    [TestFixture]
    public class TestEntityRepositoryGetters : TestEntityRepository
    {
        #region Single items

        /// <summary>
        /// This tests if getting a single item from the DB
        /// (inserted by DatabaseHelper.PopulateDatabase())
        /// is working
        /// </summary>
        [Test]
        public void TestGetSingle()
        {
            var stephenHawking = AuthorRepository.Get(a => a.Name == "Stephen Hawking").Result;

            this.TestIfAuthorIsValid(stephenHawking, AuthorModelType.StephenHawking);
        }

        /// <summary>
        /// This tests if getting a single item from the db
        /// with their ManyToOne childrens works correctly
        /// </summary>
        [Test]
        public void TestGetSingleWithManyToOneChildren()
        {
            var briefHistoryBook = BookRepository.Get(
                b => b.ReleaseYear == 1988, 
                b => b.Author,
                b => b.Category
            ).Result;

            Assert.AreNotEqual(null, briefHistoryBook, "Could not retrieve the book");
            Assert.AreNotEqual(null, briefHistoryBook.Author, "Could not retrieve the ManyToOne Author");
            Assert.AreNotEqual(null, briefHistoryBook.Category, "Could not retrieve the ManyToOne Category");

            this.TestIfAuthorIsValid(briefHistoryBook.Author, AuthorModelType.StephenHawking);
            this.TestIfBookIsValid(briefHistoryBook, BookModelType.BriefHistoryOfTime, true, true);
        }

        /// <summary>
        /// This tests if getting a single item from the db
        /// with their OneToMany children works correctly
        /// </summary>
        [Test]
        public void TestGetSingleWithOneToManyChildren()
        {
            var stephenAuthor = AuthorRepository.Get(
                a => a.Name == "Stephen Hawking",
                a => a.Books
            ).Result;

            this.TestIfAuthorIsValid(stephenAuthor, AuthorModelType.StephenHawking);

            Assert.AreNotEqual(null, stephenAuthor.Books, "Could not retrieve the OneToMany Books");

            var briefHistoryBook = stephenAuthor.Books.Where(b => b.Name == "A Brief History of Time").SingleOrDefault();
            var briefAnswersBook = stephenAuthor.Books.Where(b => b.ReleaseYear == 2018).SingleOrDefault();

            this.TestIfBookIsValid(briefHistoryBook, BookModelType.BriefHistoryOfTime);
            this.TestIfBookIsValid(briefAnswersBook, BookModelType.BriefAnswers);
        }

        /// <summary>
        /// This tests if given a model which includes multiple children of the same object,
        /// the query and JOIN generation goes correctly (joins cant have the same table twice without aliassing)
        /// </summary>
        [Test]
        public void TestGetSingleWithMultipleJoinsFromSameTable()
        {
            var coBook = BookRepository.Get(b => b.Name == "Science questions answered", b => b.Author, b => b.CoAuthor).Result;

            this.TestIfBookIsValid(coBook, BookModelType.ScienceAnswered, false, true, true);

            this.TestIfAuthorIsValid(coBook.Author, AuthorModelType.StephenHawking);
            this.TestIfAuthorIsValid(coBook.CoAuthor, AuthorModelType.CarlSagan);
        }

        /// <summary>
        /// This test checks if you can search for entities while using another
        /// entity's ID as parameter. (e.g. retrieving all the books where the author is Stephen Hawking)
        /// </summary>
        [Test]
        public void TestSearchingWithForeignEntityIdAsParameter()
        {
            var stephenHawking = AuthorRepository.Get(a => a.Name == "Stephen Hawking").Result;

            this.TestIfAuthorIsValid(stephenHawking, AuthorModelType.StephenHawking);

            // Grab all books written by Stephen
            var books = BookRepository.GetAll(b => b.Author.Id == stephenHawking.Id).Result;

            Assert.AreNotEqual(null, books, "Could not retrieve books by Author Id");
            Assert.AreEqual(3, books.Count(), "Could not retrieve the correct books by Author Id");
        }

        /// <summary>
        /// This test checks if you can search for entities while using another
        /// entity as parameter. (e.g. retrieving all the books where the author is Stephen Hawking)
        /// </summary>
        [Test]
        public void TestSearchingWithForeignEntityAsParameter()
        {
            var stephenHawking = AuthorRepository.Get(a => a.Name == "Stephen Hawking").Result;

            this.TestIfAuthorIsValid(stephenHawking, AuthorModelType.StephenHawking);

            // Grab all books written by Stephen
            var books = BookRepository.GetAll(b => b.Author == stephenHawking).Result;

            Assert.AreNotEqual(null, books, "Could not retrieve books by Author Entity");
            Assert.AreEqual(3, books.Count(), "Could not retrieve the correct books by Author Entity");
        }

        /// <summary>
        /// This test checks if you can search for entities while using another
        /// entity's property as parameter. (e.g. retrieving all the books where the author is born in 1934)
        /// </summary>
        [Test]
        public void TestSearchingWithForeignEntityPropertyAsParameter()
        {
            // Grab all books written by Carl Sagan (born in 1934)
            var books = BookRepository.GetAll(b => b.Author.BirthYear == 1934, b => b.Author).Result;

            Assert.AreNotEqual(null, books, "Could not retrieve books by Author BirthYear property");
            Assert.AreEqual(2, books.Count(), "Could not retrieve the correct books by Author BirthYear property");
        }

        #endregion

        #region Multiple items

        /// <summary>
        /// This tests if getting a single item from the DB
        /// (inserted by DatabaseHelper.PopulateDatabase())
        /// is working
        /// </summary>
        [Test]
        public void TestGetAll()
        {
            var authors = AuthorRepository.GetAll(a => a.Books).Result;
            Assert.AreNotEqual(null, authors, "Could not retrieve Authors");

            var carlAuthor = authors.Where(a => a.Name == "Carl Sagan").SingleOrDefault();
            this.TestIfAuthorIsValid(carlAuthor, AuthorModelType.CarlSagan);
            Assert.AreNotEqual(null, carlAuthor.Books, "Could not retrieve Books of Author Carl Sagan");

            var cosmosVoyageBook = carlAuthor.Books.Where(b => b.Name == "Cosmos: A Personal Voyage").SingleOrDefault();
            var paleBlueDotBook  = carlAuthor.Books.Where(b => b.ReleaseYear == 1994).SingleOrDefault();
            this.TestIfBookIsValid(cosmosVoyageBook, BookModelType.Cosmos);
            this.TestIfBookIsValid(paleBlueDotBook, BookModelType.PaleBlueDot);

            var stephenAuthor = authors.Where(a => a.Name == "Stephen Hawking").SingleOrDefault();
            this.TestIfAuthorIsValid(stephenAuthor, AuthorModelType.StephenHawking);
            Assert.AreNotEqual(null, stephenAuthor.Books, "Could not retrieve Books of Author Stephen Hawking");

            var briefHistoryBook = stephenAuthor.Books.Where(b => b.Name == "A Brief History of Time").SingleOrDefault();
            var briefAnswersBook = stephenAuthor.Books.Where(b => b.ReleaseYear == 2018).SingleOrDefault();
            this.TestIfBookIsValid(briefHistoryBook, BookModelType.BriefHistoryOfTime);
            this.TestIfBookIsValid(briefAnswersBook, BookModelType.BriefAnswers);
        }

        /// <summary>
        /// This tests if the nullable ManyToOne Category
        /// on the Book is truely nullable
        /// </summary>
        [Test]
        public void TestNullableRelations() 
        {
            var authors = AuthorRepository.GetAll(a => a.Books).Result;
            Assert.AreNotEqual(null, authors, "Could not retrieve Authors");

            var carlAuthor = authors.Where(a => a.Name == "Carl Sagan").SingleOrDefault();
            this.TestIfAuthorIsValid(carlAuthor, AuthorModelType.CarlSagan);
            Assert.AreNotEqual(null, carlAuthor.Books, "Could not retrieve Books of Author");

            carlAuthor.Books = AuthorRepository.GetMany<Book>(carlAuthor, a => a.Books, b => b.Category).Result as ICollection<Book>;
            Assert.AreNotEqual(null, carlAuthor.Books, "Could not retrieve Many Books of Author");

            var nullableCategoryBook = carlAuthor.Books.Where(b => b.Name == "Cosmos: A Personal Voyage").SingleOrDefault();
            this.TestIfBookIsValid(nullableCategoryBook, BookModelType.Cosmos);
            Assert.AreEqual(null, nullableCategoryBook.Category, "The nullable Category is not null in Book");
        }

        #endregion
    }
}