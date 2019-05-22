using System;
using Extended.Dapper.Core.Database;

namespace Extended.Dapper.Core.Sql.QueryProviders
{
    public static class SqlQueryProviderHelper
    {
        public static bool Verbose { get; set; } = false;

        private static SqlQueryProvider sqlQueryProvider;

        /// <summary>
        /// Sets the database provider, so the correct SqlQueryProvider is
        /// returned when GetProvider() is called
        /// </summary>
        /// <param name="databaseProvider"></param>
        /// <param name="databaseSettings"></param>
        public static void SetProvider(DatabaseProvider databaseProvider, DatabaseSettings databaseSettings)
        {
            switch (databaseProvider)
            {
                case DatabaseProvider.MSSQL:
                    sqlQueryProvider = new MsSqlQueryProvider(databaseSettings);
                    break;
                case DatabaseProvider.MySQL:
                    sqlQueryProvider = new MySqlQueryProvider(databaseSettings);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Sets the database provider, so the correct SqlQueryProvider is
        /// returned when GetProvider() is called
        /// </summary>
        /// <param name="databaseProvider"></param>
        /// <param name="connectionString"></param>
        public static void SetProvider(DatabaseProvider databaseProvider, string connectionString)
        {
            switch (databaseProvider)
            {
                case DatabaseProvider.MSSQL:
                    sqlQueryProvider = new MsSqlQueryProvider(connectionString);
                    break;
                case DatabaseProvider.MySQL:
                    sqlQueryProvider = new MySqlQueryProvider(connectionString);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Returns an instance of the correct ISqlProvider
        /// </summary>
        /// <returns>Instance of ISqlProvider according to the databaseProvider; 
        /// or null if not implemented</returns>
        public static ISqlQueryProvider GetProvider()
        {
            return sqlQueryProvider;
        }
    }
}