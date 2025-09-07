namespace CustomsDED.Services.VehicleServices
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CustomsDED.Data.Models;
    using CustomsDED.Data.Repository.Contracts;
    using CustomsDED.DTOs.VehicleDTOs;
    using CustomsDED.Services.VehicleServices.Contract;
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository vehicleRepo;

        public VehicleService(IVehicleRepository vehicleRepo)
        {
            this.vehicleRepo = vehicleRepo;
        }

        public async Task<bool> AddVehicleAsync(VehicleAddDTO vehicleDTO)
        {
            bool isAdded = false;

            if (vehicleDTO != null)
            {
                Vehicle vehicle = new Vehicle()
                {
                    LicensePlate = vehicleDTO.LicensePlate,
                    AddictionInfo = vehicleDTO.AddictionInfo,
                    DateOfInspection = DateTime.UtcNow,
                };

                isAdded = await vehicleRepo.AddAsync(vehicle);

            }

            return isAdded;
        }

        public async Task<ICollection<VehicleGetPlateDTO>> GetVehiclesByTextAsync(string plateInput)
        {
            IEnumerable<Vehicle> vehiclesEntitiesList =
                                   await this.vehicleRepo.GetAllVehiclesByPlateInput(plateInput.ToLower());

            ICollection<VehicleGetPlateDTO> vehiclesGetPlateDTOsList = new List<VehicleGetPlateDTO>();

            foreach (Vehicle vehicle in vehiclesEntitiesList)
            {
                vehiclesGetPlateDTOsList.Add(new VehicleGetPlateDTO()
                {
                    LicensePlate = vehicle.LicensePlate,
                    AddictionInfo = vehicle.AddictionInfo,
                    DateOfInspection = vehicle.DateOfInspection,

                });
            }

            return vehiclesGetPlateDTOsList;
        }

        public async Task<ICollection<VehicleGetDateDTO>> GetVehiclesByDateAsync(DateTime pickedDate)
        {

            IEnumerable<Vehicle> vehiclesEntitiesList =
                                   await this.vehicleRepo.GetAllVehiclesByDate(pickedDate);

            ICollection<VehicleGetDateDTO> vehiclesGetDateDTOsList = new List<VehicleGetDateDTO>();

            foreach (Vehicle vehicle in vehiclesEntitiesList)
            {
                vehiclesGetDateDTOsList.Add(new VehicleGetDateDTO()
                {
                    LicensePlate = vehicle.LicensePlate,
                    AddictionInfo = vehicle.AddictionInfo,
                    DateOfInspection = vehicle.DateOfInspection,

                });
            }

            return vehiclesGetDateDTOsList;
        }
    }
}
