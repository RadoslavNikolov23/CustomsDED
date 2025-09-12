namespace CustomsDED.Services.VehicleServices
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CustomsDED.Common.Helpers;
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
            try
            {
                bool isAdded = false;

                if (vehicleDTO != null)
                {
                    Vehicle vehicle = new Vehicle()
                    {
                        LicensePlate = vehicleDTO.LicensePlate!.ToUpper(),
                        AdditionalInfo = vehicleDTO.AdditionalInfo,
                        DateOfInspection = DateTime.Now,
                    };

                    isAdded = await vehicleRepo.AddAsync(vehicle);

                }

                return isAdded;
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in AddVehicleAsync, in the VehicleService class.");
                throw;
            }
        }

        public async Task<ICollection<VehicleGetPlateDTO>> GetVehiclesByTextAsync(string plateInput)
        {
            try
            {
                IEnumerable<Vehicle> vehiclesEntitiesList =
                                       await this.vehicleRepo.GetAllVehiclesByPlateInput(plateInput.ToLower());

                ICollection<VehicleGetPlateDTO> vehiclesGetPlateDTOsList = new List<VehicleGetPlateDTO>();

                foreach (Vehicle vehicle in vehiclesEntitiesList)
                {
                    vehiclesGetPlateDTOsList.Add(new VehicleGetPlateDTO()
                    {
                        LicensePlate = vehicle.LicensePlate,
                        AdditionalInfo = vehicle.AdditionalInfo,
                        DateOfInspection = vehicle.DateOfInspection,

                    });
                }

                return vehiclesGetPlateDTOsList;
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in GetVehiclesByTextAsync, in the VehicleService class.");
                throw;
            }
        }

        public async Task<ICollection<VehicleGetDateDTO>> GetVehiclesByDateAsync(DateTime pickedDate)
        {
            try
            {
                IEnumerable<Vehicle> vehiclesEntitiesList =
                                       await this.vehicleRepo.GetAllVehiclesByDate(pickedDate);

                ICollection<VehicleGetDateDTO> vehiclesGetDateDTOsList = new List<VehicleGetDateDTO>();

                foreach (Vehicle vehicle in vehiclesEntitiesList)
                {
                    vehiclesGetDateDTOsList.Add(new VehicleGetDateDTO()
                    {
                        LicensePlate = vehicle.LicensePlate,
                        AdditionalInfo = vehicle.AdditionalInfo,
                        DateOfInspection = vehicle.DateOfInspection,

                    });
                }

                return vehiclesGetDateDTOsList;
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in GetVehiclesByDateAsync, in the VehicleService class.");
                throw;
            }
        }
    }
}
