namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public static Dictionary<string, string> loggedInUsers = new Dictionary<string, string>();
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("GetSalt/{username}")]
        public IActionResult GetSalt(string username)
        {
            string query = $@"SELECT salt FROM user WHERE username='{username}'";
            string sqlDataSource = _configuration.GetConnectionString("Default");
            string? salt = "";
            using (MySqlConnection conn = new MySqlConnection(sqlDataSource))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (cmd.ExecuteScalar() == null)
                        {
                            return BadRequest("Nincs ilyen felhasználónév az adatbázisban.");
                        }
                        else
                        {
                            salt = cmd.ExecuteScalar().ToString();
                        }
                        conn.Close();
                    }
                }
                catch (Exception e) { return BadRequest(e.Message); }

            }
            return Ok(salt);
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            if (loggedInUsers.ContainsValue(user.Username))
            {
                foreach (var data in loggedInUsers.ToList())
                {
                    if (data.Value == user.Username)
                    {
                        lock (loggedInUsers) { loggedInUsers.Remove(data.Key); }
                    }
                }

                Console.WriteLine("A felhasználó be volt jelentkezve egy másik eszközön!");
            }

            Console.WriteLine(user.Password);

            string query = $@"SELECT COUNT(*) FROM user WHERE username='{user.Username}' AND password=MD5('{user.Password}')";
            string sqlDataSource = _configuration.GetConnectionString("Default");
            string? uid = "";
            int rows = 0;
            using (MySqlConnection conn = new MySqlConnection(sqlDataSource))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        result = (result == DBNull.Value) ? null : result;
                        rows = Convert.ToInt32(result);

                        if (rows == 0)
                        {
                            return BadRequest("Hibás felhasználónév/jelszó.");
                        }
                        else
                        {
                            uid = Guid.NewGuid().ToString();
                            loggedInUsers.Add(uid, user.Username);
                        }
                        conn.Close();
                    }
                }
                catch (Exception e) { return BadRequest(e.Message); }

            }
            return Ok(uid);
        }
    }
}
