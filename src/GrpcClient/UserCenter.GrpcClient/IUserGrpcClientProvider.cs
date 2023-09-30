using UserCenter.GrpcServices;

namespace UserCenter.GrpcClient
{
    public interface IUserGrpcClientProvider
    {
        Task<UserResponse> LoginAsync(string userName, string password);

        Task<UserResponse> ValidationAsync(string userId);
    }
}
