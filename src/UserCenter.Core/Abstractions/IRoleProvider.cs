using UserCenter.Model;

namespace UserCenter.Core.Abstractions
{
    public interface IRoleProvider
    {
        Task AddAsync(string roleName, string description);

        Task UpdatteAsync(RoleBaseInfo role);

        Task RemoveAsync(string roleId);

        Task<RoleInfo[]> GetRolesAsync(RoleQueryParams queryParams);

        Task<long> GetRoleCountAsync(RoleQueryParams queryParams);

        Task<RoleBaseInfo?> GetRoleAsync(string roleId);
    }
}
