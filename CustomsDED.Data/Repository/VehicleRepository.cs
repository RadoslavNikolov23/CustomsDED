namespace CustomsDED.Data.Repository
{
    using CustomsDED.Common.Helpers;
    using CustomsDED.Data.Connection;
    using CustomsDED.Data.Models;
    using CustomsDED.Data.Repository.Contracts;
    using Microsoft.Maui;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class VehicleRepository : BaseAsyncRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(CustomsDB dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesByPlateInput(string plateInput)
        {
            await EnsureInitialized();

            string textToSearch = plateInput.ToUpper().Trim();

            IEnumerable<Vehicle> entities = new List<Vehicle>();

            try
            {
                //entities = await this.dbContext.Database
                //.Table<Vehicle>()
                //.Where(v => v.LicensePlate.ToUpper().Contains(plateInput) ||
                //            (v.AdditionalInfo != null && v.AdditionalInfo.Contains(plateInput)))
                //.ToListAsync();

                entities = await this.dbContext.Database
                               .QueryAsync<Vehicle>(
   "SELECT * FROM Vehicle WHERE LicensePlate LIKE ? OR AdditionalInfo LIKE ?", $"%{textToSearch}%", $"%{textToSearch}%");
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in GetAllVehiclesByPlateInput, in the VehicleRepository class.");
                throw;
            }

            return entities;
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesByDate(DateTime pickedDate)
        {
            await EnsureInitialized();

            IEnumerable<Vehicle> entities = new List<Vehicle>();

            DateTime nextDay = pickedDate.AddDays(1);

            try
            {
                entities = await this.dbContext.Database
                                        .Table<Vehicle>()
                                        .Where(p => p.DateOfInspection >= pickedDate
                                            && p.DateOfInspection < nextDay)
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
