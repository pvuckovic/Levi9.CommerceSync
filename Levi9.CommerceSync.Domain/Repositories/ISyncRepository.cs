using Levi9.CommerceSync.Domain.Model;

namespace Levi9.CommerceSync.Domain.Repositories
{
    public interface ISyncRepository
    {
        Task<string> GetLastUpdate(string resourceType);

        Task<bool> UpdateLastUpdate(string resourceType, string lastUpdate);
    }
}
