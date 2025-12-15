using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")] //adres naszego kontrolera - w nawiasach kwadratowych nazwa klasy bez "Controller"
    [ApiController] // oznaczamy nasz kontroler jako API
    public class ApiController : ControllerBase
    {
    }
}
