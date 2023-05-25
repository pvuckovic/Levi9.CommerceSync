using Levi9.CommerceSync.ConnectionServices;
using Levi9.CommerceSync.Datas.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Levi9.CommerceSync.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly IErpConnectionService _erpConnectionService;
        private readonly IPosConnectionService _posConnectionService;

        public SyncController(IErpConnectionService erpConnectionService, IPosConnectionService posConnectionService)
        {
            _erpConnectionService = erpConnectionService;
            _posConnectionService = posConnectionService;
        }

        [HttpGet]
        public async Task<IActionResult> SynchronizeData()
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
                    return Ok(responseMessages);
                }
                else return BadRequest(responseMessages);
            }
            return BadRequest(responseMessages);
        }
    }
}
