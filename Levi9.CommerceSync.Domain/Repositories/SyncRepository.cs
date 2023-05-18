using Levi9.CommerceSync.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Levi9.CommerceSync.Domain.Repositories
{
    public class SyncRepository : ISyncRepository
    {
        private readonly SyncDbContext _context;

        public SyncRepository(SyncDbContext context)
        {
            _context = context;
        }

        public async Task<SyncStatus> GetLastUpdate(string resourceType)
        {
            var lastUpdate = await _context.SyncStatuses.FirstOrDefaultAsync(p => p.ResourceType == resourceType);
            return lastUpdate;
        }

    }
}
