namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogoutController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LogoutController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("{uid}")]
        public IActionResult Post(string uid)
        {
            if (LoginController.loggedInUsers.ContainsKey(uid))
            { 
                LoginController.loggedInUsers.Remove(uid);
            }
            return Ok("Sikeresen kijelentkezett!");
        }

    }
}
