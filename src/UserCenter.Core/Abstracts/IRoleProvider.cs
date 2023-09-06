using UserCenter.Model;

namespace UserCenter.Core.Abstracts
{
    public interface IRoleProvider
    {
        Task AddAsync(string roleName, string description);

        Task UpdatteAsync(RoleInfo role);

        Task RemoveAsync(string roleId);

        Task<RoleUserInfo[]> GetRolesAsync(RoleQueryParams queryParams);

        Task<long> GetRoleCountAsync(RoleQueryParams queryParams);

        Task<RoleInfo?> GetRoleAsync(string roleId);
    }
}
