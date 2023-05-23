using Levi9.CommerceSync.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Levi9.CommerceSync.Domain.Repositories
{
    public class SyncRepository : ISyncRepository
    {
        private readonly SyncDbContext _context;

        public SyncRepository(SyncDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetLastUpdate(string resourceType)
        {
            var syncStatus = await _context.SyncStatuses.FirstOrDefaultAsync(p => p.ResourceType == resourceType);
            return syncStatus.LastUpdate;
        }

        public async Task<SyncResult<bool>> UpdateLastUpdate(string resourceType, string lastUpdate)
        {
            var syncStatus = await _context.SyncStatuses.FirstOrDefaultAsync(p => p.ResourceType == resourceType);
            if (syncStatus != null)
            {
                syncStatus.LastUpdate = lastUpdate;
                await _context.SaveChangesAsync();
                return new SyncResult<bool> { Message = "SYNC: Last update of" + resourceType + "updated succesfully.", IsSuccess = true};
            }
            return new SyncResult<bool> { Message = "SYNC: Somehing went wrong whit updating last update of" + resourceType + ".", IsSuccess = false };
        }
    }
   
}
