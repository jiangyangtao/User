using UserCenter.GrpcServices;

namespace UserCenter.GrpcClient
{
    public interface IUserGrpcClientProvider
    {
        Task<UserResponse> GetUserByIdAsync(string userId);

        Task<UserResponse[]> GetUsersAsync(string[] userIds);
    }
}
