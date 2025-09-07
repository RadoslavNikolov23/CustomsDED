namespace CustomsDED.Data.Connection
{
    using SQLite;
    using CustomsDED.Common.Helpers;
    using CustomsDED.Data.Models;

    using static CustomsDED.Data.Connection.DatabaseConfiguration;

    public class CustomsDB
    {
        private SQLiteAsyncConnection? database;

        public async Task Init()
        {
            try
            {
                if (database is not null)
                    return;

                database = new SQLiteAsyncConnection(DatabasePath, Flags);

                //---------For Deleting the tables and starting over for test purpose only!!-----------
                //await database.DropTableAsync<DbSchedule>();
                // await database.DropTableAsync<DbShift>();

                await database.CreateTableAsync<Person>();
                await database.CreateTableAsync<Vehicle>();

                //For FK support for future use
                //await database.ExecuteAsync("PRAGMA foreign_keys = ON;");
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in Init, in the WorkScheduleDBClass ");
                throw;
            }
        }

        public SQLiteAsyncConnection Database => database!;
    }
}
