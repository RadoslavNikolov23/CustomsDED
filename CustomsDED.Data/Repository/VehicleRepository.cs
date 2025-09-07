namespace CustomsDED.Data.Repository
{
    using CustomsDED.Common.Helpers;
    using CustomsDED.Data.Connection;
    using CustomsDED.Data.Models;
    using CustomsDED.Data.Repository.Contracts;

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

            IEnumerable<Vehicle> entities = new List<Vehicle>();

            try
            {

                entities = await this.dbContext.Database
                                        .Table<Vehicle>()
                                        .Where(v => v.LicensePlate.ToLower().Contains(plateInput) ||
                                                    (v.AddictionInfo != null ? v.AddictionInfo.ToLower().Contains(plateInput) : false))
                                        .ToListAsync();
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

            try
            {

                entities = await this.dbContext.Database
                                        .Table<Vehicle>()
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
