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
        private readonly IEntityRepositoryProvider<Model.User> _userRepository;

        public UserProvider(IEntityRepositoryProvider<Model.User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task AddAsync(string userName, string password)
        {
            var exist = await _userRepository.Get(a => a.Username == userName).AnyAsync();
            if (exist) HttpErrorResult.ResponseBadRequest($"{userName} already exist");

           
        }

        public Task ChangePasswordAsync(UserPassword user)
        {
            throw new NotImplementedException();
        }

        public Task<long> GetUserCountAsync(UserQueryParams queryParams)
        {
            throw new NotImplementedException();
        }

        public async Task<UserRole[]> GetUsersAsync(UserQueryParams queryParams)
        {
            var query = queryParams.GetQueryable(_userRepository);
            var users = await query.OrderByDescending(a => a.CreateTime).Skip(queryParams.Start).Take(queryParams.Size).Select(a => new UserRole
            {
                Username = a.Username,
                Avatar = a.Avatar,
            }).ToArrayAsync();
            if (users.IsNullOrEmpty()) return Array.Empty<UserRole>();

            return users;
        }

        public Task RemoveAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}