using Levi9.CommerceSync.Worker.Options;
using Microsoft.Extensions.Options;
using Quartz;

namespace Levi9.CommerceSync.Worker.Jobs
{
    [DisallowConcurrentExecution]
    public class SyncDataJob : IJob
    {
        private readonly IErpConnectionService _erpConnectionService;

        public SyncDataJob(IErpConnectionService erpConnectionService, IOptions<SyncDataJobOptions> options)
        {
            _erpConnectionService = erpConnectionService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var response = await _erpConnectionService.SyncProducts();
            if(response.IsSuccess)
                Console.Write(response.Message + "\n");
            else
            {
                Console.Write(response.Message + "\n");
            }
        }
    }
}
