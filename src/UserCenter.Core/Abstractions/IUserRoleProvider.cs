using UserCenter.Model;

namespace UserCenter.Core.Abstractions
{
    public interface IUserRoleProvider
    {
        Task GrantAsync(string userId, string roleId);

        Task UnGrantAsync(string userId, string roleId);

        Task<bool> ExistAsync(string userId, string roleId);

        Task<UserInfo[]> FillRoleAsync(UserInfo[] userInfos);

        Task<RoleInfo[]> FillUserAsync(RoleInfo[] roles);
    }
}
