

using Yangtao.Hosting.Repository.Abstractions;

namespace UserCenter.Model
{
    public class UserRole : BaseEntity
    {
        public string UserId { set; get; }

        public string RoleId { set; get; }
    }
}
