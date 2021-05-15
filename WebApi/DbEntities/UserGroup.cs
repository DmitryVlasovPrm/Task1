using System.Collections.Generic;

#nullable disable

namespace WebApi
{
    public partial class UserGroup
    {
        public UserGroup()
        {
            Users = new HashSet<User>();
        }

        public int UserGroupId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
