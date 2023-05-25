using Levi9.CommerceSync.ConnectionServices;
using Levi9.CommerceSync.Worker.Options;
using Microsoft.Extensions.Options;
using Quartz;

namespace Levi9.CommerceSync.Worker.Jobs
{
    [DisallowConcurrentExecution]
    public class SyncDataJob : IJob
    {
        private readonly IErpConnectionService _erpConnectionService;
        private readonly IPosConnectionService _posConnectionService;

        public SyncDataJob(IErpConnectionService erpConnectionService, IPosConnectionService posConnectionService, IOptions<SyncDataJobOptions> options)
        {
            _erpConnectionService = erpConnectionService;
            _posConnectionService = posConnectionService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var responseMessages = new List<string>();
            var productResponse = await _erpConnectionService.SyncProducts();
            var clientResponse = await _erpConnectionService.SyncClients();
            responseMessages.Add(productResponse.Message);
            responseMessages.Add(clientResponse.Message);

            if (productResponse.IsSuccess && clientResponse.IsSuccess)
            {
                var documentResponse = await _posConnectionService.SyncDocuments();
                responseMessages.Add(documentResponse.Message);
                if (documentResponse.IsSuccess)
                {
                    Console.WriteLine(string.Join("\n", responseMessages));
                }
                else
                {
                    Console.WriteLine(string.Join("\n", responseMessages));
                }
            }
            else
            {
                Console.WriteLine(string.Join("\n", responseMessages));
            }
        }

    }
}
