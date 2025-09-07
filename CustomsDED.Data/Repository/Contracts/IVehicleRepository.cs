namespace CustomsDED.Data.Repository.Contracts
{
    using CustomsDED.Data.Models;

    public interface IVehicleRepository : IBaseAsyncRepository<Vehicle>
    {
        Task<IEnumerable<Vehicle>> GetAllVehiclesByDate(DateTime pickedDate);
    }
}
