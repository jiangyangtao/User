using Microsoft.EntityFrameworkCore;
using Yangtao.Hosting.Extensions;
using Yangtao.Hosting.Repository.Abstractions;

namespace UserCenter.Model
{
    public class User : BaseEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { set; get; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { set; get; }

        /// <summary>
        /// 密码盐
        /// </summary>
        public string Slat { set; get; }

        /// <summary>
        /// 是否需要修改密码
        /// </summary>
        public bool NeedChangePassword { set; get; }

        /// <summary>
        /// 头像
        /// </summary>
        public string? Avatar { set; get; }
    }

    public class UserPassword
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { set; get; }

        /// <summary>
        /// 密码
        /// </summary>
        public string OldPassword { set; get; }

        /// <summary>
        /// 密码
        /// </summary>
        public string NewPassword { set; get; }
    }

    public class UserBaseInfo
    {
        public string UserId { set; get; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { set; get; }
    }

    public class UserInfo : UserBaseInfo
    {
        /// <summary>
        /// 头像
        /// </summary>
        public string? Avatar { set; get; }

        public Role[] Roles { set; get; }
    }

    public class UserQueryParams : PaginationBase
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { set; get; }

        public IQueryable<User> GetQueryable(IEntityRepositoryProvider<User> entityRepository)
        {
            var query = entityRepository.Get();
            if (Username.NotNullAndEmpty()) query = query.Where(a => EF.Functions.Like(a.Username, $"%{Username}%"));

            return query;
        }
    }
}