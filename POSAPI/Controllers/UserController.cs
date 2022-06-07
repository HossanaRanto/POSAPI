using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSAPI.Data;
using POSAPI.Dto;
using POSAPI.Helper;
using POSAPI.Model;
using System.Security.Cryptography;
using System.Text;

namespace POSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext context;
        private readonly JwtHelper jwt;
        private string key = "zip hackaton key";

        

        public UserController(DataContext context,JwtHelper jwt)
        {
            this.context = context;
            context.UserProfils.ToList();
            this.jwt = jwt;
        }
        [HttpGet]
        public async Task<ActionResult<List<UserClient>>> Get()
        {
            return Ok(await context.UserClients.ToListAsync());
        } 
        [HttpPost("client")]
        public async Task<ActionResult<List<UserClient>>> Create(UserDto userdto)
        {
            await context.UserProfils.ToListAsync();
            var userp = await context.Clients.FindAsync(userdto.UserPersonId);
            if (userp == null)
            {
                return BadRequest("User Profil doesn't exisit in the database");
            }
            CreatePasswordHash(userdto.Password, out byte[] passwordhash);
            var user = new UserClient()
            {
                UserName = userdto.UserName,
                Password = System.Text.Encoding.UTF8.GetString(passwordhash),
                Client = userp
            };
            context.UserClients.Add(user);
            await context.SaveChangesAsync();
            return Ok(await context.UserClients.ToListAsync());
        }
        [HttpPost("admin")]
        public async Task<ActionResult<List<UserClient>>> Createadmin(UserDto userdto)
        {
            await context.UserProfils.ToListAsync();
            var userp = await context.Employees.FindAsync(userdto.UserPersonId);
            if (userp == null)
            {
                return BadRequest("User Profil doesn't exisit in the database");
            }
            CreatePasswordHash(userdto.Password, out byte[] passwordhash);
            var user = new UserEmployee()
            {
                UserName = userdto.UserName,
                Password = System.Text.Encoding.UTF8.GetString(passwordhash),
                Employee = userp
            };
            context.UserEmployees.Add(user);
            await context.SaveChangesAsync();
            return Ok(await context.UserEmployees.ToListAsync());
        }

        [HttpPut("client")]
        public async Task<ActionResult<List<UserClient>>> UpdateClient(UserDto userdto)
        {
            var userp = await context.UserClients.FindAsync(userdto.Id);
            if (userp == null)
            {
                return BadRequest("User Profil doesn't exisit in the database");
            }
            CreatePasswordHash(userdto.Password, out byte[] passwordhash);
            userp.UserName = userdto.UserName;
            userp.Password = System.Text.Encoding.UTF8.GetString(passwordhash);
            await context.SaveChangesAsync();
            return Ok(await context.UserClients.ToListAsync());
        }
        [HttpPut("admin")]
        public async Task<ActionResult<List<UserClient>>> UpdateEmployee(UserDto userdto)
        {
            var userp = await context.UserEmployees.FindAsync(userdto.Id);
            if (userp == null)
            {
                return BadRequest("User Profil doesn't exisit in the database");
            }
            CreatePasswordHash(userdto.Password, out byte[] passwordhash);
            userp.UserName = userdto.UserName;
            userp.Password = System.Text.Encoding.UTF8.GetString(passwordhash);
            await context.SaveChangesAsync();
            return Ok(await context.UserClients.ToListAsync());
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto userdto)
        {
            var userclient = await context.UserClients.FirstOrDefaultAsync(u => u.UserName == userdto.UserName);
            var useremployee = await context.UserEmployees.FirstOrDefaultAsync(u => u.UserName == userdto.UserName);
            if (userclient == null && useremployee==null)
            {
                return BadRequest("Verify your User Name");
            }
            CreatePasswordHash(userdto.Password, out byte[] passwordhash);
            string passworddto = Encoding.UTF8.GetString(passwordhash);

            //Comparer client si nulle ou employée nulle
            dynamic pwd = null;int userid = 0;
            if (useremployee == null)
            {
                pwd = await context.UserClients.FirstOrDefaultAsync(u => u.Password == passworddto && u.UserName == userdto.UserName);
            }
            else
            {
                pwd = await context.UserEmployees.FirstOrDefaultAsync(u => u.Password == passworddto && u.UserName == userdto.UserName);
            }
            if (pwd == null)
            {
                return BadRequest("Password does not match any account");
            }

            //Prendre l'id (peut importe si c'est un employée ou client)
            userid = pwd.Id;
            var jwt = this.jwt.Generate(userid);

            Response.Cookies.Append("zip", jwt, new CookieOptions
            {
                HttpOnly = true
            });

            return Ok(new
            {
                message = "Token added successfully"
            });
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("zip");
            return Ok(new
            {
                message = "logout executed"
            });
        }
        [HttpGet("user")]
        public async Task<ActionResult<UserClient>> GetUser()
        {
            var jwt = Request.Cookies["zip"];
            if (jwt!=null)
            {
                var token = this.jwt.Verify(jwt);
                int userid = int.Parse(token.Issuer);
                UserEmployee empl = GetByIdEmployee(userid);
                UserClient client = GetById(userid);
                return Ok(empl == null ? client : client);
            }
            return BadRequest("No connected user found");
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash)
        {
            var key_password = System.Text.Encoding.UTF8.GetBytes(key);
            using (var hmac = new HMACSHA512(key_password))
            {
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash)
        {
            var key_password = System.Text.Encoding.UTF8.GetBytes(key);
            using (var hmac = new HMACSHA512(key_password))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }

        }
        private UserClient GetById(int id)
        {
            var user = context.UserClients.FirstOrDefault(u => u.Id == id);
            return user;
        }
        private UserEmployee GetByIdEmployee(int id)
        {
            var user = context.UserEmployees.FirstOrDefault(u => u.Id == id);
            return user;
        }
    }
}
