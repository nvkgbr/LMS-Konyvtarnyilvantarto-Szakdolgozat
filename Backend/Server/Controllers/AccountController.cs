namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IMemberService _service;

        public AccountController(ITokenService tokenService, IMemberService memberService)
        {
            _tokenService = tokenService;
            _service = memberService;
        }

        [HttpPost("login")]
        public IActionResult Login(Member memb)
        {
            Console.WriteLine("Bejöttem a loginba");
            var member = _service.GetMemberByEmail(memb.Email);

            if (member == null)
            {
                return Unauthorized("Hibás felhasználónév");
            }

            if (memb.ReadersCode != member.ReadersCode)
            {
                return Unauthorized("Hibás jelszó!");
            }

            member.Token = _tokenService.CreateToken(memb);

            return Ok(member);
        }

    }
}
