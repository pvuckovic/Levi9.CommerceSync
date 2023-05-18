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
        public async Task<bool> SynchronizeData()
        {
            var response = await _helloService.SyncProducts();
            if(response == true)
                return true;
            return false;
        }
    }
}
