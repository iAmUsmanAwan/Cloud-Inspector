using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Models;

namespace UMS.BLL.Shared
{
    public interface ICloudServiceService
    {
        Task<IEnumerable<CloudService>> GetAllCloudServicesAsync();
        Task<CloudService> GetCloudServiceByIdAsync(int id);
        Task<IEnumerable<CloudService>> GetCloudServicesByTypeAsync(ServiceType type);
        Task<IEnumerable<CloudService>> GetCloudServicesByProviderAsync(string provider);
        Task<IEnumerable<CloudService>> SearchCloudServicesAsync(string searchTerm);
        Task<bool> AddCloudServiceAsync(CloudService cloudService);
        Task<bool> UpdateCloudServiceAsync(CloudService cloudService);
        Task<bool> DeleteCloudServiceAsync(int id);
    }
}
