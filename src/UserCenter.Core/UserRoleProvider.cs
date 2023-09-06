using Microsoft.EntityFrameworkCore;
using UserCenter.Core.Abstracts;
using UserCenter.Model;
using Yangtao.Hosting.Extensions;
using Yangtao.Hosting.Repository.Abstractions;

namespace UserCenter.Core
{
    internal class UserRoleProvider : IUserRoleProvider
    {
        private readonly IEntityRepositoryProvider<User> _userRepository;
        private readonly IEntityRepositoryProvider<Role> _roleRepository;
        private readonly IEntityRepositoryProvider<UserRole> _userRoleRepository;

        public UserRoleProvider(
            IEntityRepositoryProvider<User> userRepository,
            IEntityRepositoryProvider<Role> roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public Task<bool> ExistAsync(string userId, string roleId) => _userRoleRepository.Get(a => a.RoleId == roleId && a.UserId == userId).AnyAsync();

        public async Task<UserInfo[]> FillRoleAsync(UserInfo[] userInfos)
        {
            if (userInfos.IsNullOrEmpty()) return userInfos;

            var userIds = userInfos.Select(a => a.UserId).ToArray();
            var userRoles = await _userRoleRepository.Get(a => userIds.Contains(a.UserId)).ToArrayAsync();
            if (userRoles.IsNullOrEmpty()) return userInfos;

            var roleIds = userRoles.Select(a => a.RoleId).Distinct().ToArray();
            var roles = await _roleRepository.Get(a => roleIds.Contains(a.Id)).ToArrayAsync();
            if (roles.IsNullOrEmpty()) return userInfos;

            foreach (var userInfo in userInfos)
            {
                var _userRoles = userRoles.Where(a => a.UserId == userInfo.UserId).ToArray();
                if (_userRoles.IsNullOrEmpty()) continue;

                var _roleIds = _userRoles.Select(a => a.RoleId).Distinct().ToArray();
                var _roles = roles.Where(a => _roleIds.Contains(a.Id)).ToArray();
                if (_roles.IsNullOrEmpty()) continue;

                userInfo.Roles = _roles;
            }

            return userInfos;
        }

        public async Task<RoleInfo[]> FillUserAsync(RoleInfo[] roles)
        {
            if (roles.IsNullOrEmpty()) return roles;

            var roleIds = roles.Select(a => a.RoleId).ToArray();
            var userRoles = await _userRoleRepository.Get(a => roleIds.Contains(a.RoleId)).ToArrayAsync();
            if (userRoles.IsNullOrEmpty()) return roles;

            var userIds = userRoles.Select(a => a.UserId).ToArray();
            var users = await _userRepository.Get(a => userIds.Contains(a.Id)).ToArrayAsync();
            if (users.IsNullOrEmpty()) return roles;

            foreach (var role in roles)
            {
                var _userRoles = userRoles.Where(a => a.RoleId == role.RoleId).ToArray();
                if (_userRoles.IsNullOrEmpty()) continue;

                var _userIds = _userRoles.Select(a => a.UserId).Distinct().ToArray();
                var _users = users.Where(a => _userIds.Contains(a.Id)).ToArray();
                if (_users.IsNullOrEmpty()) continue;

                role.Users = _users;
            }

            return roles;
        }

        public async Task GrantAsync(string userId, string roleId)
        {
            var exist = await ExistAsync(userId, roleId);
            if (exist) return;

            await _userRoleRepository.AddAsync(new UserRole
            {
                UserId = userId,
                RoleId = roleId,
            });
        }

        public Task UnGrantAsync(string userId, string roleId) => _userRoleRepository.DeleteIfExistAsync(a => a.RoleId == roleId && a.UserId == userId);

    }
}
