using System.ComponentModel.DataAnnotations;

namespace UserCenter.Application.Dto
{

    public class LoginDto : UserNameDtoBase
    {
        [Required]
        public string Password { set; get; }
    }

    public class LoginResult : UserNameDtoBase
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserId { set; get; }
    }
}
