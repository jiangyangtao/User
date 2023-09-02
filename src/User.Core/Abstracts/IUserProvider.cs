using User.Model;

namespace User.Core.Abstracts
{
    public interface IUserProvider
    {
        Task AddAsync(string userName, string password);

        Task RemoveAsync(string userId);

        Task ChangePasswordAsync(UserPassword user);

        Task<UserRole[]> GetUsersAsync(UserQueryParams queryParams);

        Task<long> GetUserCountAsync(UserQueryParams queryParams);
    }
}
