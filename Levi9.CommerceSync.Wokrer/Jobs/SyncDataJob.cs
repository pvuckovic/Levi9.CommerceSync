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
            var productResponse = await _erpConnectionService.SyncProducts();
            //var clientResponse = await _erpConnectionService.SyncClients();

            if (productResponse.IsSuccess)
            {
                Console.WriteLine(productResponse.Message);
                //Console.WriteLine(clientResponse.Message);
            }
            else
            {
                if (!productResponse.IsSuccess)
                    Console.WriteLine(productResponse.Message);

                //if (!clientResponse.IsSuccess)
                //    Console.WriteLine(clientResponse.Message);
            }
        }
    }
}
