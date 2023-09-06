using Microsoft.EntityFrameworkCore;
using UserCenter.Core.Abstracts;
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

        public RoleProvider(
            IEntityRepositoryProvider<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task AddAsync(string roleName, string description)
        {
            var exist = await _roleRepository.Get(a => a.RoleName == roleName).AnyAsync();
            if (exist) HttpErrorResult.ResponseConflict($"{roleName} already exist.");

            await _roleRepository.AddAsync(new Role { RoleName = roleName, Description = description });
        }

        public Task<RoleInfo?> GetRoleAsync(string roleId) =>
            _roleRepository.Get(a => a.Id == roleId).Select(a => new RoleInfo
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

        public async Task<RoleUserInfo[]> GetRolesAsync(RoleQueryParams queryParams)
        {
            var query = queryParams.GetQueryable(_roleRepository);
            var list = await query.DefaultOrderPagination(queryParams).Select(a => new RoleUserInfo
            {
                RoleId = a.Id,
                RoleName = a.RoleName,
                Description = a.Description,
            }).ToArrayAsync();
            if (list.IsNullOrEmpty()) return Array.Empty<RoleUserInfo>();

            return list;
        }

        public Task RemoveAsync(string roleId) => _roleRepository.DeleteByIdAsync(roleId);

        public Task UpdatteAsync(RoleInfo role) => _roleRepository.UpdateIfExistByIdAsync(role.RoleId, a =>
            {
                a.RoleName = role.RoleName;
                a.Description = role.Description;
            });
    }
}
