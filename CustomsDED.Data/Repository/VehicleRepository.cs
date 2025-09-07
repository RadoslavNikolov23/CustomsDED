namespace CustomsDED.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CustomsDED.Common.Helpers;
    using CustomsDED.Data.Connection;
    using CustomsDED.Data.Models;
    using CustomsDED.Data.Repository.Contracts;

    public class VehicleRepository : BaseAsyncRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(CustomsDB dbContext) : base(dbContext)
        {
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
