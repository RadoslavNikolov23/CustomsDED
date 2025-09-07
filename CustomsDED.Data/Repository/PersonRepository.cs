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

        public async Task<IEnumerable<Person>> GetAllPersonsByTextInput(string textInput)
        {
            await EnsureInitialized();

            IEnumerable<Person> entities = new List<Person>();

            try
            {

                entities = await this.dbContext.Database
                                        .Table<Person>()
                                        .Where(p => p.FirstName.ToLower().Contains(textInput) ||
                                                    p.LastName.ToLower().Contains(textInput) ||
                                                    (p.PersonalNumber != null ? p.PersonalNumber.ToLower().Contains(textInput) : false))
                                        .ToListAsync();
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in GetAllPersonsByTextInput, in the PersonRepository class.");
                throw;
            }

            return entities;
        }

        public async Task<IEnumerable<Person>> GetAllPersonsByDate(DateTime pickedDate)
        {
            await EnsureInitialized();

            IEnumerable<Person> entities = new List<Person>();

            try
            {

                entities = await this.dbContext.Database
                                        .Table<Person>()
                                        .Where(p => p.DateOfInspection == pickedDate.Date)
                                        .ToListAsync();
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in GetAllVehiclesByDate, in the PersonRepository class.");
                throw;
            }

            return entities;
        }
    }
}
