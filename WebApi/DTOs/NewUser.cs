#nullable disable

namespace WebApi
{
    public class NewUser
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public static User ToUser(NewUser newUser)
        {
            return new User
            {
                Login = newUser.Login,
                Password = newUser.Password,
                UserGroupId = 2,
                UserStateId = 1,
            };
        }
    }
}
