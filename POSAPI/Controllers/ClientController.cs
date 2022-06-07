using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSAPI.Data;
using POSAPI.Dto;
using POSAPI.Model;

namespace POSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly DataContext context;

        public ClientController(DataContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<ActionResult<Client>> GetList()
        {
            await context.UserProfils.ToListAsync();
            return Ok(await context.Clients.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfil>> Get(int id)
        {
            var userp = await context.UserProfils.FindAsync(id);
            if (userp == null)
            {
                return BadRequest("User Not Found");
            }
            return Ok(userp);
        }

        [HttpPost]
        public async Task<ActionResult<List<Client>>> CreateUserProfil(ClientDto clientdto)
        {
            UserProfil profil = new UserProfil
            {
                Name = clientdto.Name,
                LastName = clientdto.LastName,
                BirthDate = clientdto.BirthDate,
                Gender = clientdto.Gender,
                Email = clientdto.Email,
                PhoneNumber = clientdto.PhoneNumber
            };
            Client client = new Client(profil)
            {
                Subscribe = clientdto.Subscribe
            };
            context.UserProfils.Add(profil);
            context.Clients.Add(client);
            await context.SaveChangesAsync();
            return Ok(await context.UserProfils.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<Client>>> Update(ClientDto clientdto)
        {
            var profil = context.UserProfils.Find(clientdto.UserProfilId);
            var client = context.Clients.Find(clientdto.Id);
            if (profil == null || client==null)
            {
                return BadRequest("User Not Found");
            }
            profil.Name = clientdto.Name;
            profil.LastName = clientdto.LastName;
            profil.Gender = clientdto.Gender;
            profil.BirthDate = clientdto.BirthDate;
            profil.Email = profil.Email;
            profil.PhoneNumber = clientdto.PhoneNumber;

            client.Profil = profil;
            client.Subscribe = clientdto.Subscribe;

            await context.SaveChangesAsync();
            return Ok(await context.UserProfils.ToListAsync());
        }

    }
}
