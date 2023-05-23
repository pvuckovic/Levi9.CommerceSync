using Levi9.CommerceSync.Datas.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Levi9.CommerceSync.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly IErpConnectionService _erpConnectionService;

        public SyncController(IErpConnectionService erpConnectionService)
        {
            _erpConnectionService = erpConnectionService;
        }

        [HttpGet]
        public async Task<IActionResult> SynchronizeData()
        {
            var productResponse = await _erpConnectionService.SyncProducts();
            //var clientResponse = await _erpConnectionService.SyncClients();
            //if (productResponse.IsSuccess == true && clientResponse.IsSuccess == true)
                if (productResponse.IsSuccess == true)
                return Ok(productResponse.Message);
            //return Ok(productResponse.Message + "\n" + clientResponse.Message);

            if (!productResponse.IsSuccess)
                return BadRequest(productResponse.Message);

            //if (!clientResponse.IsSuccess)
            //    return BadRequest(clientResponse.Message);

            return BadRequest("Something went wrong!");
        }
    }
}
