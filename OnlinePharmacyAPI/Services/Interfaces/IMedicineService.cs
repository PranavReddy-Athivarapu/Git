using OnlinePharmacyAppAPI.Models;

namespace OnlinePharmacyAppAPI.Services.Interfaces
{
    public interface IMedicineService
    {
        Task<List<Medicine>> GetAllMedicinesAsync();
        Task<Medicine> GetMedicineByIdAsync(int id);
        Task<List<Medicine>> GetAlternativeMedicinesAsync(int originalMedicineId);
        Task ReplenishStockAsync(int medicineId, int quantity);
        Task<bool> CheckStockAvailabilityAsync(int medicineId, int requestedQuantity);
    }
}
