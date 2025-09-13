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

            string textToSearch = textInput.ToUpper().Trim();

            IEnumerable<Person> entities = new List<Person>();

            try
            {

                //entities = await this.dbContext.Database
                //                    .Table<Person>()
                //                    .Where(p => p.FirstName.Contains(textInput) ||
                //                                p.LastName.Contains(textInput) ||
                //                                (p.PersonalId != null && p.PersonalId.Contains(textInput)) ||
                //                                (p.AdditionInfo != null && p.AdditionInfo.Contains(textInput)))
                //                    .ToListAsync();
                entities = await this.dbContext.Database
                                    .QueryAsync<Person>(
        "SELECT * FROM Person WHERE FirstName LIKE ? OR LastName LIKE ? OR PersonalId LIKE ? OR AdditionInfo LIKE ?",$"%{textToSearch}%", $"%{textToSearch}%", $"%{textToSearch}%", $"%{textToSearch}%");

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

            DateTime nextDay = pickedDate.AddDays(1);

            try
            {

                entities = await this.dbContext.Database
                                        .Table<Person>()
                                        .Where(p => p.DateOfInspection >= pickedDate
                                            && p.DateOfInspection < nextDay)
                                        .ToListAsync();
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in GetAllPersonsByDate, in the PersonRepository class.");
                throw;
            }

            return entities;
        }
    }
}
