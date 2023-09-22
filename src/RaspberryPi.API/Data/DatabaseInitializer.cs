using Dapper;

namespace RaspberryPi.API.Data
{
    public class DatabaseInitializer
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DatabaseInitializer(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void Initialize()
        {
            // https://www.sqlite.org/stricttables.html
            // SQLite supports a strict typing mode, as of version 3.37.0 (2021-11-27)
            // The current nuget package for .NET targets version 3.35.5
            // SELECT sqlite_version()
            // Do not use async in SQLite

            using var connection = _connectionFactory.CreateConnection();

            connection.Execute(@"CREATE TABLE IF NOT EXISTS SqliteNativeDataTypes 
                                            (              
	                                            MyInteger INTEGER PRIMARY KEY,
	                                            MyReal REAL NOT NULL,
	                                            MyText TEXT NOT NULL
                                            )");

            connection.Execute(@"CREATE TABLE IF NOT EXISTS SqliteSupportedNetTypes 
                                            (
                                                MyBool INTERGER NOT NULL,
                                                MyByte INTERGER NOT NULL,
                                                MyChar TEXT NOT NULL,
                                                MyDateOnly TEXT NOT NULL,
                                                MyDateTime TEXT NOT NULL,
                                                MyDateTimeOffset TEXT NOT NULL,
                                                MyDecimal TEXT NOT NULL,
                                                MyDouble REAL NOT NULL,
                                                MyGuid TEXT PRIMARY KEY,
	                                            MyInt INTEGER NOT NULL,
	                                            MyString TEXT NOT NULL,
                                                MyTimeOnly TEXT NOT NULL,
                                                MyTimeSpan TEXT NOT NULL
                                            )");

            connection.Execute(@"CREATE TABLE IF NOT EXISTS SqlLiteKeyValue (
                                            Id INTEGER PRIMARY KEY,
                                            Value TEXT NOT NULL,
                                            DateModified TEXT NOT NULL
                                           )");
        }
    }
}