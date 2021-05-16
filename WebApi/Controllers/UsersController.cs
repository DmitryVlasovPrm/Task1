using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private ServerContext _dbContext;

        public UsersController(ServerContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetailedUser>>> Get()
        {
            var users = _dbContext.Users.Include(x => x.UserGroup).Include(x => x.UserState);
            return await users.Select(x => DetailedUser.ToDetailed(x)).ToListAsync();
        }

        // GET api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DetailedUser>> Get(int id)
        {
            var users = _dbContext.Users.Include(x => x.UserGroup).Include(x => x.UserState);
            var user = await users.FirstOrDefaultAsync(x => x.UserId == id);
            if (user == null)
                return NotFound();

            return new ObjectResult(DetailedUser.ToDetailed(user));
        }

        // POST api/users
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<User>> Post(NewUser newUser)
        {
            if (newUser == null)
                return BadRequest();

            if (newUser.UserGroupId == 1)
            {
                var admin = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserGroupId == 1 && x.UserStateId != 2);
                if (admin != null)
                    return BadRequest(new { errorText = "Administrator already exists" });
            }

            var existUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Login == newUser.Login);
            if (existUser != null)
            {
                // Если такой пользователь существует
                if (existUser.UserStateId != 2)
                {
                    return BadRequest(new { errorText = "This user login already exists" });
                }
                // Если такой был удален, то можем создать нового с таким же login
                else
                {
                    existUser.Password = newUser.Password;
                    existUser.UserGroupId = newUser.UserGroupId;
                    existUser.UserStateId = 1;
                }
            }
            else
			{
                _dbContext.Users.Add(NewUser.ToUser(newUser));
            }

            await _dbContext.SaveChangesAsync();
            await Task.Delay(5000);
            return Ok();
        }

        // DELETE api/users/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            user.UserStateId = 2; // Blocked
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}