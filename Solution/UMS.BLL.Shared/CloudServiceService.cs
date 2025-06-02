using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.DAL.Shared;
using UMS.Models;

namespace UMS.BLL.Shared
{
    public class CloudServiceService : ICloudServiceService
    {
        private readonly ICloudServiceRepository _cloudServiceRepository;

        public CloudServiceService(ICloudServiceRepository cloudServiceRepository)
        {
            _cloudServiceRepository = cloudServiceRepository;
        }

        public async Task<IEnumerable<CloudService>> GetAllCloudServicesAsync()
        {
            return await _cloudServiceRepository.GetAllAsync();
        }

        public async Task<CloudService> GetCloudServiceByIdAsync(int id)
        {
            return await _cloudServiceRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<CloudService>> GetCloudServicesByTypeAsync(ServiceType type)
        {
            return await _cloudServiceRepository.GetByTypeAsync(type);
        }

        public async Task<IEnumerable<CloudService>> GetCloudServicesByProviderAsync(string provider)
        {
            return await _cloudServiceRepository.GetByProviderAsync(provider);
        }

        public async Task<IEnumerable<CloudService>> SearchCloudServicesAsync(string searchTerm)
        {
            return await _cloudServiceRepository.SearchAsync(searchTerm);
        }

        public async Task<bool> AddCloudServiceAsync(CloudService cloudService)
        {
            try
            {
                cloudService.DateAdded = DateTime.Now;
                await _cloudServiceRepository.AddAsync(cloudService);
                await _cloudServiceRepository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateCloudServiceAsync(CloudService cloudService)
        {
            try
            {
                var existingService = await _cloudServiceRepository.GetByIdAsync(cloudService.Id);
                if (existingService == null)
                    return false;

                cloudService.DateAdded = existingService.DateAdded;
                cloudService.LastUpdated = DateTime.Now;

                await _cloudServiceRepository.UpdateAsync(cloudService);
                await _cloudServiceRepository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteCloudServiceAsync(int id)
        {
            try
            {
                var cloudService = await _cloudServiceRepository.GetByIdAsync(id);
                if (cloudService == null)
                    return false;

                await _cloudServiceRepository.DeleteAsync(cloudService);
                await _cloudServiceRepository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
