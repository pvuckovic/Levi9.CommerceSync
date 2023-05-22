using Microsoft.AspNetCore.Mvc;

namespace Levi9.CommerceSync.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly IErpConnectionService _helloService;

        public SyncController(IErpConnectionService helloService)
        {
            _helloService = helloService;
        }

        [HttpGet]
        public async Task<IActionResult> SynchronizeData()
        {
            var response = await _helloService.SyncProducts();
            if(response.IsSuccess == true)
                return Ok(response.Message);
            return BadRequest(response.Message);
        }
    }
}
