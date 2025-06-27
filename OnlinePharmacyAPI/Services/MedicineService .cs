using OnlinePharmacyAppAPI.Models;
using OnlinePharmacyAppAPI.Services.Interfaces;

namespace OnlinePharmacyAppAPI.Services
{
    public class MedicineService : IMedicineService
    {
        private readonly IRepository<Medicine> _medicineRepository;
        private readonly IRepository<AlternativeMedicine> _altMedicineRepository;

        public MedicineService(
            IRepository<Medicine> medicineRepository,
            IRepository<AlternativeMedicine> altMedicineRepository)
        {
            _medicineRepository = medicineRepository;
            _altMedicineRepository = altMedicineRepository;
        }

        public async Task<List<Medicine>> GetAllMedicinesAsync()
        {
            return await _medicineRepository.GetAll()
                .Where(m => m.StockQuantity > 0)
                .ToListAsync();
        }

        public async Task<List<Medicine>> GetAlternativeMedicinesAsync(int originalMedicineId)
        {
            return await _altMedicineRepository.GetAll()
                .Where(a => a.OriginalMedicineId == originalMedicineId)
                .Select(a => a.SubstituteMedicine)
                .Where(m => m.StockQuantity > 0)
                .ToListAsync();
        }

        public async Task ReplenishStockAsync(int medicineId, int quantity)
        {
            var medicine = await _medicineRepository.GetByIdAsync(medicineId);
            if (medicine != null)
            {
                medicine.StockQuantity += quantity;
                await _medicineRepository.UpdateAsync(medicine);
            }
        }
    }
}
