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

            var productResponse = await _erpConnectionService.SyncProducts();
            var clientResponse = await _erpConnectionService.SyncClients();
            var documentResponse = await _posConnectionService.SyncDocuments();


            if (productResponse.IsSuccess == true && clientResponse.IsSuccess == true)
            {
                return Ok(productResponse.Message + "\n" + clientResponse.Message + "\n");
            }
            else if (productResponse.IsSuccess && !clientResponse.IsSuccess)
            {
                return Ok(productResponse.Message + "\n" + clientResponse.Message + "\n");
            }
            else if (!productResponse.IsSuccess && clientResponse.IsSuccess)
            {
                return Ok(productResponse.Message + "\n" + clientResponse.Message + "\n");
            }
            if (!productResponse.IsSuccess && !clientResponse.IsSuccess)
                return Ok(productResponse.Message + "\n" + clientResponse.Message + "\n");

            return BadRequest("Something went wrong!");
        }
    }
}
