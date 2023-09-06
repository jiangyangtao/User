using Microsoft.EntityFrameworkCore;
using UserCenter.Core.Abstracts;
using UserCenter.Model;
using Yangtao.Hosting.Core.HttpErrorResult;
using Yangtao.Hosting.Extensions;
using Yangtao.Hosting.Repository.Abstractions;

namespace UserCenter.Core
{
    public class UserProvider : IUserProvider
    {
        private readonly IEntityRepositoryProvider<User> _userRepository;

        public UserProvider(IEntityRepositoryProvider<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task AddAsync(string userName, string password)
        {
            var exist = await ExistAsync(userName);
            if (exist) HttpErrorResult.ResponseBadRequest($"{userName} already exist");

            using var handler = PasswordHandler.CreateHandler(password);
            var encryptedPassword = handler.Encrypt();

            var user = new User
            {
                Username = userName,
                Password = encryptedPassword,
                Slat = handler.PasswordSlat,
                NeedChangePassword = false,
            };

            await _userRepository.AddAsync(user);
        }

        public async Task ChangePasswordAsync(UserPassword user)
        {
            var data = await _userRepository.Get(a => a.Username == user.Username).FirstOrDefaultAsync();
            if (data == null) HttpErrorResult.ResponseBadRequest($"{user.Username} not exist");

            using var handler = PasswordHandler.CreateHandler(data, user.OldPassword);
            var comparisonResult = handler.PasswordComparison();
            if (comparisonResult == false) HttpErrorResult.ResponseBadRequest($"{user.Username} user or password is incorrect.");

            var newPassword = handler.EncryptNewPassword(user.NewPassword);
            data.Password = newPassword;
            await _userRepository.UpdatePartAsync(data, a => a.Password);
        }

        public Task<bool> ExistAsync(string userName) => _userRepository.Get(a => a.Username == userName).AnyAsync();

        public async Task<UserBaseInfo?> GetByIdAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            return new UserBaseInfo
            {
                UserId = user.Id,
                Username = user.Username,
            };
        }

        public Task<long> GetUserCountAsync(UserQueryParams queryParams)
        {
            var query = queryParams.GetQueryable(_userRepository);
            return query.LongCountAsync();
        }

        public async Task<UserInfo[]> GetUsersAsync(UserQueryParams queryParams)
        {
            var query = queryParams.GetQueryable(_userRepository);
            var users = await query.OrderByDescending(a => a.CreateTime).Skip(queryParams.Start).Take(queryParams.Size).Select(a => new UserInfo
            {
                UserId = a.Id,
                Username = a.Username,
                Avatar = a.Avatar,
            }).ToArrayAsync();
            if (users.IsNullOrEmpty()) return Array.Empty<UserInfo>();

            return users;
        }

        public async Task<UserBaseInfo?> LoginAsync(string userName, string password)
        {
            var user = await _userRepository.Get(a => a.Username == userName).FirstOrDefaultAsync();
            if (user == null) return null;

            using var handler = PasswordHandler.CreateHandler(user, password);
            var comparisonResult = handler.PasswordComparison();
            if (comparisonResult == false) return null;

            return new UserBaseInfo()
            {
                UserId = user.Id,
                Username = userName,
            };
        }

        public async Task RemoveAsync(string userId) => await _userRepository.DeleteByIdAsync(userId);
    }
}