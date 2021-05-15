using System;

#nullable disable

namespace WebApi
{
    public partial class User
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int UserGroupId { get; set; }
        public int UserStateId { get; set; }

        public virtual UserGroup UserGroup { get; set; }
        public virtual UserState UserState { get; set; }
    }
}
