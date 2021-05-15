using System.Collections.Generic;

#nullable disable

namespace WebApi
{
    public partial class UserState
    {
        public UserState()
        {
            Users = new HashSet<User>();
        }

        public int UserStateId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
