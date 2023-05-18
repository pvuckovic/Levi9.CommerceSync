using Levi9.CommerceSync.Domain.Model;

namespace Levi9.CommerceSync.Domain.Repositories
{
    public interface ISyncRepository
    {
        Task<SyncStatus> GetLastUpdate(string resourceType);
    }
}
