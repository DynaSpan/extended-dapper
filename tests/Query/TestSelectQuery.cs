using System;
using System.Linq;
using Extended.Dapper.Core.Database;
using Extended.Dapper.Core.Repository;
using Extended.Dapper.Core.Reflection;
using Extended.Dapper.Core.Sql;
using Extended.Dapper.Core.Sql.QueryProviders;
using Extended.Dapper.Tests.Models;
using NUnit.Framework;

namespace Extended.Dapper.Tests.Query
{
    [TestFixture]
    public class TestSelectQuery
    {
        private EntityRepository<Book> BookRepository { get; set; }
        private EntityRepository<Author> AuthorRepository { get; set; }

        [SetUp]
        public void Setup()
        {
            SqlQueryProviderHelper.SetProvider(DatabaseProvider.MSSQL);
            var sqlGenerator = new SqlGenerator(DatabaseProvider.MSSQL);

            var databaseSettings = new DatabaseSettings()
            {
                Host = "172.20.0.10",
                User = "dapper",
                Password = "extended-dapper-sql-password",
                Database = "dapper",
                DatabaseProvider = DatabaseProvider.MSSQL
            };
            var databaseFactory = new DatabaseFactory(databaseSettings);
            
            BookRepository = new EntityRepository<Book>(databaseFactory);
            AuthorRepository = new EntityRepository<Author>(databaseFactory);
        }

        /// <summary>
        /// Currently used for literal testing, not unittesting
        /// </summary>
        [Test]
        public void TestModelMapping()
        {
            var books = (BookRepository.Get(b => b.ReleaseYear == 1988, b => b.Author).Result);

            foreach (var book in books)
            {
                Console.WriteLine(book);
            }

            Console.WriteLine("==============");

            var authors = (AuthorRepository.Get(a => a.Country == "United Kingdom", a => a.Books).Result);

            foreach (var author in authors)
            {
                Console.WriteLine(author);
            }

            Console.WriteLine("==============");

            // Get other author by ID
            var otherAuthor = AuthorRepository.GetById(new Guid("6ba27ef2-fb90-4f85-ba23-6934cf5a04ec"), a => a.Books).Result;

            Console.WriteLine(otherAuthor);
        }

        [Test]
        public void TestInsert()
        {
            var newAuthor = new Author(){
                Name = "Spees Kees",
                BirthYear = 2652,
                Country = "Republic of Earth Citizens, Mars, Solar System, Milky Way Galaxy"
            };
            var newBook = new Book() {
                Author = newAuthor,
                Name = "The birth of Spees",
                ReleaseYear = 2687
            };

            Console.WriteLine(BookRepository.Insert(newBook).Result);
        }
    }
}