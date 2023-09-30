using Microsoft.EntityFrameworkCore;
using UserCenter.Core.Abstractions;
using UserCenter.Model;
using Yangtao.Hosting.Core.HttpErrorResult;
using Yangtao.Hosting.Extensions;
using Yangtao.Hosting.Repository.Abstractions;
using Yangtao.Hosting.Repository.Core;

namespace UserCenter.Core
{
    internal class RoleProvider : IRoleProvider
    {
        private readonly IEntityRepositoryProvider<Role> _roleRepository;
        private readonly IUserRoleProvider _userRoleProvider;

        public RoleProvider(
            IEntityRepositoryProvider<Role> roleRepository, IUserRoleProvider userRoleProvider)
        {
            _roleRepository = roleRepository;
            _userRoleProvider = userRoleProvider;
        }

        public async Task AddAsync(string roleName, string description)
        {
            var exist = await _roleRepository.Get(a => a.RoleName == roleName).AnyAsync();
            if (exist) HttpErrorResult.ResponseConflict($"{roleName} already exist.");

            await _roleRepository.AddAsync(new Role { RoleName = roleName, Description = description });
        }

        public Task<RoleBaseInfo?> GetRoleAsync(string roleId) =>
            _roleRepository.Get(a => a.Id == roleId).Select(a => new RoleBaseInfo
            {
                RoleId = a.Id,
                RoleName = a.RoleName,
                Description = a.Description,
            }).FirstOrDefaultAsync();



        public Task<long> GetRoleCountAsync(RoleQueryParams queryParams)
        {
            var query = queryParams.GetQueryable(_roleRepository);
            return query.LongCountAsync();
        }

        public async Task<RoleInfo[]> GetRolesAsync(RoleQueryParams queryParams)
        {
            var query = queryParams.GetQueryable(_roleRepository);
            var list = await query.DefaultOrderPagination(queryParams).Select(a => new RoleInfo
            {
                RoleId = a.Id,
                RoleName = a.RoleName,
                Description = a.Description,
            }).ToArrayAsync();
            if (list.IsNullOrEmpty()) return Array.Empty<RoleInfo>();


            return await _userRoleProvider.FillUserAsync(list);
        }

        public Task RemoveAsync(string roleId) => _roleRepository.DeleteByIdAsync(roleId);

        public Task UpdatteAsync(RoleBaseInfo role) => _roleRepository.UpdateIfExistByIdAsync(role.RoleId, a =>
            {
                a.RoleName = role.RoleName;
                a.Description = role.Description;
            });
    }
}
