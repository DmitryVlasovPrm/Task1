using System;

#nullable disable

namespace WebApi
{
    public class DetailedUser
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UserGroup { get; set; }
        public string GroupDescription { get; set; }
        public string UserState { get; set; }
        public string StateDescription { get; set; }

        public static DetailedUser ToDetailed(User user)
        {
            return new DetailedUser
            {
                UserId = user.UserId,
                Login = user.Login,
                CreatedDate = user.CreatedDate,
                UserGroup = user.UserGroup.Code,
                GroupDescription = user.UserGroup.Description,
                UserState = user.UserState.Code,
                StateDescription = user.UserState.Description
            };
        }
    }
}
