namespace CustomsDED.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CustomsDED.Common.Helpers;
    using CustomsDED.Data.Connection;
    using CustomsDED.Data.Models;
    using CustomsDED.Data.Repository.Contracts;

    public class PersonRepository : BaseAsyncRepository<Person>, IPersonRepository
    {
        public PersonRepository(CustomsDB dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Person>> GetAllPersonsByDate(DateTime pickedDate)
        {
            await EnsureInitialized();

            IEnumerable<Person> entities = new List<Person>();

            try
            {

                entities = await this.dbContext.Database
                                        .Table<Person>()
                                        .Where(v => v.DateOfInspection == pickedDate.Date)
                                        .ToListAsync();
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in GetAllVehiclesByDate, in the VehicleRepository class.");
                throw;
            }

            return entities;
        }
    }
}
