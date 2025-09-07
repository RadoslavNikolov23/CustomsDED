namespace CustomsDED.Data.Connection
{
    public static class DatabaseConfiguration
    {
        public static string? TestOverridePath { get; set; }

        private const string DatabaseFilename = "CustomsDatabase.db3";

        public const SQLite.SQLiteOpenFlags Flags =
                                        SQLite.SQLiteOpenFlags.ReadWrite |
                                        SQLite.SQLiteOpenFlags.Create |
                                        SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
           TestOverridePath ?? Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    }
}
