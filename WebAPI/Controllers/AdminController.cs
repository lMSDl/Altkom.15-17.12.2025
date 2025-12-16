using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)] //uktywamy kontoler dla dokumentacji OpenAPI
    public class AdminController : ApiController
    {
        [HttpGet]
        //[ApiExplorerSettings(IgnoreApi = true)] //możemy też stosować na poziomie akcji
        public ActionResult<string> GetSecret()
        {
            return "This is a secret message for admins only.";
        }
    }
}
