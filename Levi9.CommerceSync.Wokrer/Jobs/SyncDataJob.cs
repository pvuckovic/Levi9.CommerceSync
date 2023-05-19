using Levi9.CommerceSync.Worker.Options;
using Microsoft.Extensions.Options;
using Quartz;

namespace Levi9.CommerceSync.Worker.Jobs
{
    [DisallowConcurrentExecution]
    public class SyncDataJob : IJob
    {
        private readonly IErpConnectionService _helloService;

        public SyncDataJob(IErpConnectionService helloService ,IOptions<SyncDataJobOptions> options)
        {
            _helloService = helloService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            //var response = await _helloService.SyncProducts();
            Console.Write("Sync");
        }
    }
}
