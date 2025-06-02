using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Models;

namespace UMS.DAL.Shared
{
    public interface ICloudServiceRepository : IRepository<CloudService>
    {
        Task<IEnumerable<CloudService>> GetByTypeAsync(ServiceType type);
        Task<IEnumerable<CloudService>> GetByProviderAsync(string provider);
        Task<IEnumerable<CloudService>> SearchAsync(string searchTerm);
    }
}
