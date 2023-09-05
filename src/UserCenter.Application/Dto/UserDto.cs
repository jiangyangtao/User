using System.ComponentModel.DataAnnotations;
using UserCenter.Model;
using Yangtao.Hosting.Mvc.FormatResult;

namespace UserCenter.Application.Dto
{
    public class UserDtoBase
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string UserId { set; get; }
    }

    public class UserNameDtoBase
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string Username { set; get; }
    }

    public class UserDto : LoginDto
    {

    }

    public class ChangePasswordDto : UserNameDtoBase
    {
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string OldPassword { set; get; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string NewPassword { set; get; }

        public UserPassword GetUserPassword() => new()
        {
            Username = Username,
            OldPassword = OldPassword,
            NewPassword = NewPassword,
        };
    }


    public class UserQueryDto : PagingParameter
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string username { set; get; }

        public UserQueryParams GetUserQueryParams()
        {
            return new UserQueryParams
            {
                Username = username,
                Start = startIndex,
                Size = size,
            };
        }
    }

    public class UserRoleResult : UserNameDtoBase
    {
        public UserRoleResult(UserRole user)
        {
            UserId = user.UserId;
            Username = user.Username;
            Avatar = user.Avatar;
        }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string UserId { set; get; }

        /// <summary>
        /// 头像
        /// </summary>
        public string? Avatar { set; get; }
    }

    public class UserRolePaginationResult : PaginationResult<UserRoleResult>
    {
        public UserRolePaginationResult(UserRole[] users, long count) : base(count)
        {
            List = users.Select(a => new UserRoleResult(a)).ToArray();
        }
    }
}
