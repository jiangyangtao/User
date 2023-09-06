using Microsoft.EntityFrameworkCore;
using Yangtao.Hosting.Extensions;
using Yangtao.Hosting.Repository.Abstractions;

namespace UserCenter.Model
{
    public class Role : BaseEntity
    {
        public string RoleName { set; get; }

        public string Description { set; get; }
    }

    public class RoleBaseInfo
    {
        public string RoleId { set; get; }

        public string RoleName { set; get; }

        public string Description { set; get; }
    }

    public class RoleInfo : RoleBaseInfo
    {
        public User[] Users { set; get; }
    }

    public class RoleQueryParams : PaginationBase
    {
        public string RoleName { set; get; }

        public IQueryable<Role> GetQueryable(IEntityRepositoryProvider<Role> entityRepository)
        {
            var query = entityRepository.Get();
            if (RoleName.NotNullAndEmpty()) query = query.Where(a => EF.Functions.Like(a.RoleName, $"{RoleName}"));

            return query;
        }
    }
}
