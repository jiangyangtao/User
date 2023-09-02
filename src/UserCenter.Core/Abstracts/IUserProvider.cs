using UserCenter.Model;

namespace UserCenter.Core.Abstracts
{
    public interface IUserProvider
    {
        Task AddAsync(string userName, string password);

        Task RemoveAsync(string userId);

        Task ChangePasswordAsync(UserPassword user);

        Task<bool> ExistAsync(string userName);

        Task<UserRole[]> GetUsersAsync(UserQueryParams queryParams);

        Task<long> GetUserCountAsync(UserQueryParams queryParams);

        Task<UserInfo?> LoginAsync(string userName, string password);
    }
}
