using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using WebApi;

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

            // Проверка логинов, которые регистрируются в данный момент
            var newLogin = newUser.Login;
            if (Program.LoginsInProcess.Contains(newLogin))
                return BadRequest(new { errorText = "This user login already exists" });
            else
                Program.LoginsInProcess.Add(newLogin);
            await Task.Delay(5000);

            // Занят ли текущий login
            var existUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Login == newUser.Login);
            if (existUser != null)
            {
                // Если такой пользователь существует
                if (existUser.UserStateId != 2)
                {
                    Program.LoginsInProcess.Remove(newLogin);
                    return BadRequest(new { errorText = "This user login already exists" });
                }
                // Если такой был удален, то можем создать нового с таким же login
                else
                {
                    existUser.Password = newUser.Password;
                    existUser.UserStateId = 1;
                }
            }
            else
            {
                // Такого пользователя нет
                _dbContext.Users.Add(NewUser.ToUser(newUser));
            }

            await _dbContext.SaveChangesAsync();
            Program.LoginsInProcess.Remove(newLogin);
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

            if (user.UserGroupId == 1)
                // Администратор не может удалить сам себя
                return BadRequest(new { errorText = "Administrator can't be blocked" });
            else
                // Blocked
                user.UserStateId = 2;

            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}