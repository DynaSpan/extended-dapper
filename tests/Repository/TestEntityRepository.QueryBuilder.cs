using System;
using System.Collections.Generic;
using System.Linq;
using Extended.Dapper.Core.Sql.QueryBuilder;
using Extended.Dapper.Tests.Helpers;
using Extended.Dapper.Tests.Models;
using NUnit.Framework;

namespace Extended.Dapper.Tests.Repository
{
    [TestFixture]
    public class TestEntityRepositoryQueryBuilder : TestEntityRepository
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();
            DatabaseHelper.PopulateDatabase().Wait();
        }

        /// <summary>
        /// Tests if executing the QueryBuilder with a search works
        /// as expected
        /// </summary>
        [Test]
        public void TestWhere()
        {
            var carlSagan = this.AuthorRepository.GetQueryBuilder()
                .Where(a => a.BirthYear == 1934)
                .GetResults()
                .Result;

            Assert.AreEqual(1, carlSagan.Count(), "Search was not executed correctly");

            this.TestIfAuthorIsValid(carlSagan.First(), AuthorModelType.CarlSagan);
        }

        /// <summary>
        /// Tests if limiting works correctly
        /// </summary>
        [Test]
        public void TestLimit()
        {
            var twoAuthors = this.AuthorRepository.GetQueryBuilder()
                .Limit(2)
                .GetResults()
                .Result;

            Assert.AreEqual(2, twoAuthors.Count(), "Query was not limited correctly");
        }

        /// <summary>
        /// Tests if limiting with an order by works as expected
        /// </summary>
        [Test]
        public void TestLimitWithOrderBy()
        {
            var twoAuthors = this.AuthorRepository.GetQueryBuilder()
                .OrderBy(a => a.Name)
                .Limit(2)
                .GetResults()
                .Result;

            Assert.AreEqual(2, twoAuthors.Count(), "Query was not limited correctly");

            var authorWoBooks = twoAuthors.First();
            var carlSagan = twoAuthors.ElementAt(1);

            this.TestIfAuthorIsValid(authorWoBooks, AuthorModelType.AuthorWithoutBooks);
            this.TestIfAuthorIsValid(carlSagan, AuthorModelType.CarlSagan);
        }

        /// <summary>
        /// Tests if limiting with an order by and search
        /// works as expected
        /// </summary>
        [Test]
        public void TestSearchLimitedWithOrderBy()
        {
            var twoAuthors = this.AuthorRepository.GetQueryBuilder()
                .Where(a => a.BirthYear > 1910) // exclude Author w/o Books
                .OrderBy(a => a.Name)
                .Limit(2)
                .GetResults()
                .Result;

            Assert.AreEqual(2, twoAuthors.Count(), "Query was not limited correctly");

            var carlSagan = twoAuthors.ElementAt(0);
            var stephenHawking = twoAuthors.ElementAt(1);

            this.TestIfAuthorIsValid(stephenHawking, AuthorModelType.StephenHawking);
            this.TestIfAuthorIsValid(carlSagan, AuthorModelType.CarlSagan);
        }

        /// <summary>
        /// Tests if limiting with an order by and search
        /// works as expected
        /// </summary>
        // [Test]
        // public void TestSearchLimitedWithOrderByAndChildren()
        // {
        //     var twoAuthors = this.AuthorRepository.GetQueryBuilder()
        //         .Where(a => a.BirthYear > 1910) // exclude Author w/o Books
        //         .IncludeChildren(a => a.Books)
        //         .OrderBy(a => a.Name)
        //         .Limit(2)
        //         .GetResults()
        //         .Result;

        //     Assert.AreEqual(2, twoAuthors.Count(), "Query was not limited correctly");

        //     var carlSagan = twoAuthors.ElementAt(0);
        //     var stephenHawking = twoAuthors.ElementAt(1);

        //     Assert.AreEqual(2, carlSagan.Books.Count, "Children count is incorrect");
        //     Assert.AreEqual(3, stephenHawking.Books, "Children count is incorrect");

        //     this.TestIfAuthorIsValid(stephenHawking, AuthorModelType.StephenHawking);
        //     this.TestIfAuthorIsValid(carlSagan, AuthorModelType.CarlSagan);
        // }
    }
}