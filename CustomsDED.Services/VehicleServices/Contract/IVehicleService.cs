namespace CustomsDED.Services.VehicleServices.Contract
{
    using CustomsDED.DTOs.VehicleDTOs;

    public interface IVehicleService
    {
        Task<bool> AddVehicleAsync(VehicleAddDTO vehicleDTO);

        Task<ICollection<VehicleGetPlateDTO>> GetVehiclesByTextAsync(string textInput);

        Task<ICollection<VehicleGetDateDTO>> GetVehiclesByDateAsync(DateTime pickedDate);
    }
}
