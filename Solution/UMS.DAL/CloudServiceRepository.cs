using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Models;

namespace UMS.DAL
{
    public class CloudServiceRepository : Repository<CloudService>, ICloudServiceRepository
    {
        public CloudServiceRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CloudService>> GetByTypeAsync(ServiceType type)
        {
            return await _dbSet.Where(s => s.Type == type).ToListAsync();
        }

        public async Task<IEnumerable<CloudService>> GetByProviderAsync(string provider)
        {
            return await _dbSet.Where(s => s.Provider == provider).ToListAsync();
        }

        public async Task<IEnumerable<CloudService>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return await GetAllAsync();

            return await _dbSet
                .Where(s => s.Name.Contains(searchTerm) ||
                            s.Description.Contains(searchTerm) ||
                            s.Provider.Contains(searchTerm))
                .ToListAsync();
        }
    }
}
