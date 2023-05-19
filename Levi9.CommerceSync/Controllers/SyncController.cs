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
            if(response == true)
                return Ok("The products have been successfully synchronized.");
            return BadRequest("Something went wrong !");
        }
    }
}
